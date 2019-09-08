using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BytonOpcService.UaClient;
using BytonOpcService.DBOperator;
using System.Threading;
using ApplicationLog;
using WebApiSelfHost;

namespace BytonOpcService
{
    public partial class Service1 : ServiceBase
    {

        #region 全局变量定义区
        private UaClientManager mUaClient = new UaClientManager();
        private XMLSettings mXml = new XMLSettings();

        Thread Th_HeartBeat;
        Thread Th_SystemWatcher;
        Thread Th_ReadProduction;
        #endregion
        public Service1()
        {
            InitializeComponent();

            mXml.ReadXMLSettings();
            UaClientManager.IPCReceiveResultNodeId = mXml.IPCReceiveResult;
            SetProductionRecorBuffListNotNull();//初始化时填充缓存

        }

        private void SetProductionRecorBuffListNotNull()
        {
            for (int i = 0; i < mXml.NodeIdList.Length; i++)
            {
                byte[] buff = new byte[0];
                UaClientManager.ProductionRecorBuffList.Add(buff);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                OPCWeAPi.StartPushClient();

                Th_SystemWatcher = new Thread(Func_SystemWatcher);
                Th_SystemWatcher.IsBackground = true;
                Th_SystemWatcher.Start();

                Th_HeartBeat = new Thread(Func_HeartBeat);
                Th_HeartBeat.IsBackground = true;
                Th_HeartBeat.Start();


            }
            catch (Exception ex)
            { }
        }

        protected override void OnStop()
        {
        }

        private void Func_SystemWatcher()
        {
            bool InsertHistory = false;//决定是否插入历史表
            while (true)
            {
                try
                {
                    if (UaClientManager.SystemStatusRecordCount > 60)
                    {
                        InsertHistory = true;
                        UaClientManager.SystemStatusRecordCount = 0;
                    }
                    else
                    {
                        InsertHistory = false;
                    }
                    if (UaClientManager.CouldDealWithSystemTable)
                    {
                        DBOperator.DBOperator.UpdateSystemStatusTable(UaClientManager.OpcConnected, UaClientManager.SignalQuality, InsertHistory);
                    }
                    if (UaClientManager.OpcConnected)
                    {
                        DateTime nowTime = DateTime.Now;
                        byte[] writeTimestamp = new byte[4];
                        writeTimestamp[0] = (byte)nowTime.Minute;
                        writeTimestamp[1] = (byte)nowTime.Second;
                        writeTimestamp[2] = (byte)(nowTime.Millisecond / 100);
                        writeTimestamp[3] = (byte)(nowTime.Millisecond % 100);
                        mUaClient.opcUaClient.WriteNode<byte[]>("ns=2;s=Byton.RFIDPro.Timestamp", writeTimestamp);
                    }

                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogFile.Trace, ex.Message);
                }
                Thread.Sleep(400);
            }
        }

        /// <summary>
        /// 判断程序与OPC Server连接状态的心跳
        /// </summary>
        private void Func_HeartBeat()
        {
            while (true)
            {
                if (!UaClientManager.OpcConnected)
                {
                    if (mUaClient.opcUaClient != null && mUaClient.opcUaClient.Connected)
                    {
                        mUaClient.opcUaClient.Disconnect();
                        DBOperator.DBOperator.InsertBigEventTable("断开与OPC Server的连接");
                        LogManager.WriteLog(LogFile.Trace, "断开与OPC Server的连接");
                    }
                    UaClientManager.OpcConnected = mUaClient.ConnectOpcServerWithPwd(mXml.URL, mXml.UserName, mXml.Password);
                    DBOperator.DBOperator.InsertBigEventTable("尝试与OPC Server建立连接");
                    LogManager.WriteLog(LogFile.Trace, "尝试与OPC Server建立连接");
                    if (UaClientManager.OpcConnected)
                    {
                        //开启线程，读取OPC数据，分析，比对
                        //MessageBox.Show("Conected to OPCserver");
                        DBOperator.DBOperator.InsertBigEventTable("与OPC Server建立连接成功");
                        LogManager.WriteLog(LogFile.Trace, "与OPC Server建立连接成功");
                        mUaClient.StartLoopReadProductionRecord(mXml.NodeIdList);
                        mUaClient.StartLoopReadIOLinkRecord(mXml.IOLinkList);
                        mUaClient.StartLoopReadSystemRecord(mXml.SystemNodeId);
                    }
                    else
                    {
                        DBOperator.DBOperator.InsertBigEventTable("与OPC Server建立连接失败");
                        LogManager.WriteLog(LogFile.Trace, "与OPC Server建立连接失败");
                    }

                }
                Thread.Sleep(5000);
            }
        }


    }
}
