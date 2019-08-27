using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Opc.Ua;
using Opc.Ua.Client;
using BytonOpcService.DataModel;
using BytonOpcService.DBOperator;
using System.Reflection;
using ApplicationLog;





namespace BytonOpcService.UaClient
{
    public class UaClientManager
    {
       
        public OpcUaHelper.OpcUaClient opcUaClient;
        public static bool OpcConnected = false;//与OPC Server的连接状态
        public static List<byte[]> ProductionRecorBuffList = new List<byte[]>();
        public static byte[] IOLinkRecorBuff = new byte[0];
        public static byte[] SystemWatchBuff = new byte[0];
        public static DateTime localCacheModifiedTime = new DateTime();//记录数据更新的时间戳
        public static byte SetPLC = 0; //设置PLC读取成功位
        public static string SignalQuality = "";
        public static int ReConnectCouter =0;//OPC重连异常计数
        public static int PLCException = 0;//OPC信号质量异常计数
        public static int SystemStatusRecordCount = 61;//系统更新计数，用于决定是否插入系统历史表
        public static bool CouldDealWithSystemTable = false;//用于决定是否可以开始操作系统表
        public static string IPCReceiveResultNodeId = "";

        public UaClientManager()
        {
            opcUaClient = new OpcUaHelper.OpcUaClient();
        }

        #region 无用户名密码登陆，前提服务器登陆不需要用户名密码
        /// <summary>
        /// 无用户名密码登陆，前提服务器登陆不需要用户名密码
        /// </summary>
        /// <param name="opcServerURL"></param>
        /// <returns></returns>
        public bool ConnectOpcServerWithNoPwd(string opcServerURL)
        {
            bool conStatus = false;
            opcUaClient.UserIdentity = new UserIdentity();
            Task t1 = opcUaClient.ConnectServer(opcServerURL);
            Task.WaitAll(t1);
            if (t1.Status == TaskStatus.RanToCompletion)
            {
                conStatus = true;
            }
            else
            {
                conStatus = false;
            }

            return conStatus;
        }
        #endregion

        #region 用户名密码登陆，前提服务器已设置用户名密码
        /// <summary>
        /// 用户名密码登陆，前提服务器已设置用户名密码
        /// </summary>
        /// <param name="opcServerURL"></param>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public bool ConnectOpcServerWithPwd(string opcServerURL,string userName,string pwd)
        {
            bool conStatus = false;
            try
            {
                opcUaClient.UserIdentity = new UserIdentity(userName, pwd);
                Task t1 = opcUaClient.ConnectServer(opcServerURL);
                Task.WaitAll(t1);
                if (t1.Status == TaskStatus.RanToCompletion)
                {
                    conStatus = true;
                }
                else
                {
                    conStatus = false;
                }
                return conStatus;
            }
            catch(Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
                return conStatus;
            }
        }
        #endregion

        #region 根据提供的nodeID读取相应的OPC节点值
        /// <summary>
        /// 根据提供的nodeID读取相应的OPC节点值
        /// </summary>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        public string ReadDataFromOpcServer(string nodeID)
        {
            try
            {
                string ret = "";
                NodeId mNodeID = new NodeId(nodeID, 2);
                DataValue dv = opcUaClient.ReadNode(mNodeID);
                ret = dv.WrappedValue.Value.ToString();
                return ret;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
                return "ReadFail:" + ex.Message;
            }
        }
        #endregion

        #region 根据提供的nodeID写入对应值到OPC Server
        /// <summary>
        /// 根据提供的nodeID写入对应值到OPC Server
        /// </summary>
        /// <param name="nodeID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool WriteDataToOpcServer(string nodeID, byte[] data)
        {
            bool retValue = false;
            int writeSuccessCount = 0;
            try
            {
                retValue = opcUaClient.WriteNode<byte[]>(nodeID, data);
                if (retValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
                return false;
            }
        }
        #endregion

        #region 根据提供的nodeIDArray写入对应值到OPC Server
        /// <summary>
        /// 根据提供的nodeIDArray写入对应值到OPC Server
        /// </summary>
        /// <param name="nodeIDArray"></param>
        /// <param name="dataArray"></param>
        /// <returns></returns>
        public bool WriteDataListToOpcServer(string[] nodeIDArray, List<byte>[] dataArray)
        {
            bool retValue = false;
            int writeSuccessCount = 0;
            try
            {
                for (int i = 0; i < nodeIDArray.Length; i++)
                {
                    retValue = opcUaClient.WriteNode<byte[]>(nodeIDArray[i], (byte[])dataArray[i].ToArray());
                    if (retValue)
                        writeSuccessCount++;
                }
                if (writeSuccessCount == nodeIDArray.Length)
                {
                    return true;
                }
                else
                {
                    //只有全部写入成功才会返回True。
                    return false;
                }
                //retValue = opcUaClient.WriteNodes(nodeIDArray, dataArray);
                //此为批量写入数据，连接正常的情况下数据可以写入成功。但是会报BadSecure异常，这里使用循环单次写入方式
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
                return false;
            }
        }
        #endregion

        #region 开启线程，间隔读取OPC Server数据
        /// <summary>
        /// 开启循环读取ProductionRecord
        /// </summary>
        public void StartLoopReadProductionRecord(string[] nodeID)
        {
            try
            {
                Thread th_LoopReadProduction = new Thread(ReadProductionIntervallyRefreshCache);
                th_LoopReadProduction.IsBackground = true;
                th_LoopReadProduction.Start(nodeID);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
            }
        }

        /// <summary>
        /// 循环读取节点并判断缓存是否更新
        /// </summary>
        private void ReadProductionIntervallyRefreshCache(object nodeID)
        {
            string[] NodeID = nodeID as string[];
            byte[] curBuff = new byte[0];
            while (OpcConnected)
            {
                try
                {
                    for (int i = 0; i < NodeID.Length; i++)
                    {
                        curBuff = opcUaClient.ReadNode<byte[]>(NodeID[i]);
                        bool diffBuff = !Enumerable.SequenceEqual(curBuff, ProductionRecorBuffList[i]);
                        if (JudgeWhetherDo(curBuff[1]) && diffBuff)
                        {
                            //设置PLC读取成功标志位
                            SetBit(true, i);
                            opcUaClient.WriteNode<byte>(IPCReceiveResultNodeId, SetPLC);
                            //当前curBuff替换为缓存Buff
                            ProductionRecorBuffList[i] = curBuff;
                            //将当前buff提供给数据分析函数，解析后插入数据库
                            ProductionData PD = DealProductionData(curBuff, i);
                            if (PD != null)
                            {
                                DBOperator.DBOperator.InsertProductionTable(PD, i);
                            }
                        }
                        else if (curBuff[1] == 0)
                        {
                            //复位读取成功标志位
                            SetBit(false, i);
                            opcUaClient.WriteNode<byte>(IPCReceiveResultNodeId, SetPLC);
                        }
                    }
                    ReConnectCouter = 0;
                }
                catch (Exception ex)
                {
                    ReConnectCouter++;
                    if (ReConnectCouter > 5)
                    {
                        ReConnectCouter = 0;
                        SystemStatusRecordCount = 0;
                        OpcConnected = false;
                    }
                    LogManager.WriteLog(LogFile.Trace,ex.Message);
                }
                Thread.Sleep(500);
            }
        }

        private bool JudgeWhetherDo(byte v)
        {
            if (v == (byte)4 || v == (byte)5 || v == (byte)6 || v == (byte)7 || v == (byte)12)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// 处理实时料框数据表
        /// 状态字 1-OK 0-NG
        /// </summary>
        /// <param name="curBuff"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private ProductionData DealProductionData(byte[] curBuff,int i)
        {
            try
            {
                ProductionData PD = new ProductionData();
                PD.RFIDStationName = "RFIDStation" + (i+1);
                PD.ASRSPLCReadSignal = curBuff[0] == 1 ? "On" : "Off";
                byte[] newByte = new byte[1];
                newByte[0] = curBuff[1];
                PD.TagReadResult = BitConverter.ToString(newByte);//待定
                PD.ASRSPLCResult = curBuff[2]==1?"OK":"NG";
                PD.ManualWriteBoxRequest = curBuff[3].ToString();
                PD.RFIDReaderConSatus = curBuff[4] == 1 ? "OK" : "NG";
                PD.R200ConStatus = curBuff[5] == 1 ? "OK" : "NG";
                //料框ID及类型点表暂未确定 前料框
                PD.F_BoxID = Encoding.Default.GetString(curBuff.Skip(10).Take(4).ToArray());
                PD.F_BoxType =((char)curBuff[16]).ToString();
                byte[] idByte = new byte[2];
                idByte[0] = curBuff[19];
                idByte[1] = curBuff[18];
                PD.F_TagID = BitConverter.ToUInt16(idByte, 0).ToString();
                //标签写入日期格式未定
                PD.F_R200Distance = BitConverter.ToUInt16(curBuff, 38).ToString();

                //后料框料框ID及类型点表暂未确定
                PD.B_BoxID = Encoding.Default.GetString(curBuff.Skip(40).Take(4).ToArray());
                PD.B_BoxType = ((char)curBuff[46]).ToString();
                idByte[0] = curBuff[49];
                idByte[1] = curBuff[48];
                PD.B_TagID = BitConverter.ToUInt16(idByte, 0).ToString();
                PD.B_R200Distance = BitConverter.ToUInt16(curBuff, 68).ToString();

                return PD;
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
                return null;
            }
            
        }

        /// <summary>
        /// 开启读取IOLink线程
        /// </summary>
        /// <param name="nodeID"></param>
        public void StartLoopReadIOLinkRecord(string[] nodeID)
        {
            try
            {
                Thread th_LoopReadSystem = new Thread(ReadIOLinkIntervallyRefreshCache);
                th_LoopReadSystem.IsBackground = true;
                th_LoopReadSystem.Start(nodeID);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
            }
        }

        private void ReadIOLinkIntervallyRefreshCache( object nodeID)
        {
            string[] NodeID = nodeID as string[];
            while (OpcConnected)
            {
                try
                {
                    byte[] curBuff = new byte[6594];
                    for (int i = 0; i < NodeID.Length; i++)
                    {
                        byte[] regionBuff = opcUaClient.ReadNode<byte[]>(NodeID[i]);
                        System.Buffer.BlockCopy(regionBuff, 0, curBuff, 800 * i, regionBuff.Length);
                    }
                    DealIOLinkData(curBuff);
                    Thread.Sleep(6000);
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogFile.Trace, ex.Message);
                }
            }
        }

        /// <summary>
        /// 解析IOLink数据并维持缓存
        /// </summary>
        /// <param name="curBuff"></param>
        private void DealIOLinkData(byte[] curBuff)
        {
            if (curBuff.Length > 6593)
            {
                try
                {
                    if (Enumerable.SequenceEqual(IOLinkRecorBuff, curBuff))
                    { }
                    else
                    {
                        IOLinkData.ClearAll();
                        IOLinkRecorBuff = curBuff;
                        int index = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            RFID mRFID = new RFID();
                            mRFID.StationName = "RFIDStation" + (i+1);
                            mRFID.VendorName= Encoding.Default.GetString(curBuff.Skip(index + 0).Take(64).ToArray()).Replace("\0", "");
                            mRFID.ProductName = Encoding.Default.GetString(curBuff.Skip(index + 64).Take(64).ToArray()).Replace("\0","");
                            mRFID.SerialNumber = Encoding.Default.GetString(curBuff.Skip(index + 128).Take(16).ToArray()).Replace("\0", "");
                            mRFID.HardwareVersion = Encoding.Default.GetString(curBuff.Skip(index + 144).Take(64).ToArray()).Replace("\0", "");
                            mRFID.FirmwareVersion = Encoding.Default.GetString(curBuff.Skip(index + 208).Take(64).ToArray()).Replace("\0", "");
                            mRFID.DeviceStatus = curBuff[index + 272].ToString();
                            mRFID.Mode = curBuff[index + 273] == 0x80 ? "Easy Mode" : "Expert Mode";
                            mRFID.MemoryArea = curBuff[index + 274] == 0x80 ? "UID" : "User Memory";
                            mRFID.NumberOfBytes = curBuff[index + 275].ToString();
                            mRFID.StartAddress = BitConverter.ToUInt16(curBuff, index + 276).ToString();
                            mRFID.AutoStart= curBuff[index + 278] == 0x80 ? "On" : "Off";
                            mRFID.TagType = curBuff[index + 279].ToString();
                            IOLinkData.RFIDList.Add(mRFID);

                            R200 mR200 = new R200();
                            mR200.VendorName = Encoding.Default.GetString(curBuff.Skip(index + 280).Take(64).ToArray()).Replace("\0", "");
                            mR200.ProductName= Encoding.Default.GetString(curBuff.Skip(index + 344).Take(64).ToArray()).Replace("\0", "");
                            mR200.SerialNumber = Encoding.Default.GetString(curBuff.Skip(index + 408).Take(16).ToArray()).Replace("\0", "");
                            mR200.HardwareVersion = Encoding.Default.GetString(curBuff.Skip(index + 424).Take(64).ToArray()).Replace("\0", "");
                            mR200.FirmwareVersion = Encoding.Default.GetString(curBuff.Skip(index + 488).Take(64).ToArray()).Replace("\0", "");
                            mR200.DeviceStatus = curBuff[index + 552].ToString();
                            mR200.TemperatureIndicate = curBuff[index + 553].ToString();
                            mR200.OperationHours = BitConverter.ToUInt32(curBuff, index + 554).ToString();
                            IOLinkData.R200List.Add(mR200);
                            index += 800;
                        }
                        index = 6400;
                        for (int i = 0; i < 4; i++)
                        {
                            ICE mICE = new ICE();
                            mICE.OrderID = Encoding.Default.GetString(curBuff.Skip(index + 0).Take(20).ToArray()).Replace("\0", "");
                            mICE.SerialNumber = Encoding.Default.GetString(curBuff.Skip(index + 20).Take(16).ToArray()).Replace("\0", "");
                            mICE.HardwareVersion = BitConverter.ToUInt16(curBuff, (index + 36)).ToString();
                            mICE.SoftwareVersion = BitConverter.ToUInt32(curBuff, (index + 38)).ToString();
                            mICE.IMVersion = curBuff[index+42].ToString() +"."+ curBuff[index + 43].ToString();
                            IOLinkData.ICEList.Add(mICE);
                            index += 50;
                        }
                        //Update DiagnoseTable
                        DBOperator.DBOperator.UpdateDiagnoseTable();

                    }
                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogFile.Trace, ex.Message);
                }
            }
        }

        /// <summary>
        /// 开启读取系统监控数据线程
        /// </summary>
        /// <param name="nodeID"></param>
        public void StartLoopReadSystemRecord(string nodeID)
        {
            try
            {
                Thread th_LoopReadSystem = new Thread(ReadSystemIntervallyRefreshCache);
                th_LoopReadSystem.IsBackground = true;
                th_LoopReadSystem.Start(nodeID);
            }
            catch (Exception ex)
            { }
        }

        private void ReadSystemIntervallyRefreshCache(object nodeID)
        {
            string NodeID = nodeID as string;
            OpcUaHelper.OpcNodeAttribute[] opcNodeAttri;
            byte[] curBuff = new byte[0];
            while (OpcConnected)
            {
                try
                {
                    //byte[] curBuff = opcUaClient.ReadNode<byte[]>(NodeID);
                    opcNodeAttri = opcUaClient.ReadNoteAttributes(NodeID);
                    curBuff = (byte[])opcNodeAttri.ToList().FirstOrDefault(p => p.Name.ToUpper() == "VALUE").Value;
                    SignalQuality = opcNodeAttri.ToList().FirstOrDefault(p => p.Name.ToUpper() == "VALUE").StatusCode.ToString();
                    if (SignalQuality.ToUpper() == "GOOD")
                    {
                        DealSystemData(curBuff);
                        PLCException = 0;
                        SystemStatusRecordCount++;
                    }
                    else
                    {
                        PLCException++;
                        if (PLCException > 2)
                        {
                            PLCException = 0;
                            SystemStatusRecordCount = 0;
                            DBOperator.DBOperator.InsertBigEventTable("OPC数据信号质量差，PLC异常或网络状态不佳");
                        }
                    }
                    ReConnectCouter = 0;
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    ReConnectCouter ++;
                    if (ReConnectCouter > 5)
                    {
                        ReConnectCouter = 0;
                        SystemStatusRecordCount = 0;
                        OpcConnected = false;
                    }
                    LogManager.WriteLog(LogFile.Trace, ex.Message);
                }
            }
        }

        /// <summary>
        /// 处理系统状态数据
        /// 系统表中状态字 0-OK 1-NG
        /// </summary>
        /// <param name="curBuff"></param>
        private void DealSystemData(byte[] curBuff)
        {
            try
            {
                if (Enumerable.SequenceEqual(curBuff, SystemWatchBuff))
                { }
                else
                {
                    #region 赋值
                    SystemWatchData.PnBusDevEx = GetBitFromByteBuff(curBuff, 0, 3);
                    SystemWatchData.AsrsPlcConEx = GetBitFromByteBuff(curBuff, 0, 5);
                    SystemWatchData.IpcConEx = GetBitFromByteBuff(curBuff, 0, 6);
                    SystemWatchData.PfPlcEx = GetBitFromByteBuff(curBuff, 0, 7);

                    SystemWatchData.Ice1Ex = GetBitFromByteBuff(curBuff, 1, 0);
                    SystemWatchData.Ice2Ex = GetBitFromByteBuff(curBuff, 1, 1);
                    SystemWatchData.Ice3Ex = GetBitFromByteBuff(curBuff, 1, 2);
                    SystemWatchData.Ice4Ex = GetBitFromByteBuff(curBuff, 1, 3);
                    SystemWatchData.DPCouplerEx = GetBitFromByteBuff(curBuff, 1, 4);

                    SystemWatchData.RFIDStation1Ex = GetBitFromByteBuff(curBuff, 2, 0);
                    SystemWatchData.RFIDStation2Ex = GetBitFromByteBuff(curBuff, 2, 1);
                    SystemWatchData.RFIDStation3Ex = GetBitFromByteBuff(curBuff, 2, 2);
                    SystemWatchData.RFIDStation4Ex = GetBitFromByteBuff(curBuff, 2, 3);
                    SystemWatchData.RFIDStation5Ex = GetBitFromByteBuff(curBuff, 2, 4);
                    SystemWatchData.RFIDStation6Ex = GetBitFromByteBuff(curBuff, 2, 5);
                    SystemWatchData.RFIDStation7Ex = GetBitFromByteBuff(curBuff, 2, 6);
                    SystemWatchData.RFIDStation8Ex = GetBitFromByteBuff(curBuff, 2, 7);

                    SystemWatchData.RFIDStation1R200Ex = GetBitFromByteBuff(curBuff, 3, 0);
                    SystemWatchData.RFIDStation2R200Ex = GetBitFromByteBuff(curBuff, 3, 1);
                    SystemWatchData.RFIDStation3R200Ex = GetBitFromByteBuff(curBuff, 3, 2);
                    SystemWatchData.RFIDStation4R200Ex = GetBitFromByteBuff(curBuff, 3, 3);
                    SystemWatchData.RFIDStation5R200Ex = GetBitFromByteBuff(curBuff, 3, 4);
                    SystemWatchData.RFIDStation6R200Ex = GetBitFromByteBuff(curBuff, 3, 5);
                    SystemWatchData.RFIDStation7R200Ex = GetBitFromByteBuff(curBuff, 3, 6);
                    SystemWatchData.RFIDStation8R200Ex = GetBitFromByteBuff(curBuff, 3, 7);

                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 0);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 1);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 2);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 3);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 4);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 5);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 6);
                    SystemWatchData.RFIDStation1ReadEx = GetBitFromByteBuff(curBuff, 4, 7);
                    #endregion
                    SystemWatchBuff = curBuff;
                    //update SystemStatusTable
                    //可以赋值
                    CouldDealWithSystemTable = true;
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, ex.Message);
            }
        }

        /// <summary>
        /// 根据传入的buff及index得到要提取的第几号字节的第几个bit值,并返回状态 0-OK 1-NG（index起始为0）
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="byteIndex"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public string GetBitFromByteBuff(byte[] buff, int byteIndex, int bitIndex)
        {
            return ((buff[byteIndex] & (byte)Math.Pow(2, bitIndex))>>bitIndex)==0?"OK":"NG";
        }

        /// <summary>
        /// 根据传入的索引和bool值,将PLC复位标志的某个bit设置为1或0 (true |)  (false &)
        /// </summary>
        /// <param name="setOrClear"></param>
        /// <param name="bitIndex"></param>
        public void SetBit(bool setOrClear, int bitIndex)
        {
            if (setOrClear)
            {
                SetPLC |= (byte)Math.Pow(2, bitIndex);
            }
            else
            {
                SetPLC&=(byte)(255- Math.Pow(2, bitIndex));
            }
        }

        #endregion


    }

}
