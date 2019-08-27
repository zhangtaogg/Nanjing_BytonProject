using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BytonOpcService.DataModel;
using System.Data.Entity;
using ApplicationLog;

namespace BytonOpcService.DBOperator
{
    //实体数据库操作类
    public class DBOperator
    {

        /// <summary>
        /// 向实时信息表中添加记录
        /// </summary>
        /// <param name="PDA"></param>
        public static void InsertProductionTable(ProductionData PDA,int IOLinkIndex)
        {
            try
            {
                using (BytonEntities bn = new BytonEntities())
                {
                    #region 添加Production记录
                    dev_productionRecord F_PR = new dev_productionRecord();
                    F_PR.StationName = PDA.RFIDStationName;
                    F_PR.TagID = PDA.F_TagID;
                    F_PR.BoxID = PDA.F_BoxID;
                    F_PR.BoxType = PDA.F_BoxType;
                    F_PR.ASRSReadSignal = PDA.ASRSPLCReadSignal;
                    F_PR.ASRSReturnCode = PDA.ASRSPLCResult;
                    F_PR.RFIDConnectionStatus = PDA.RFIDReaderConSatus;
                    F_PR.R200ConnectionStatus = PDA.R200ConStatus;
                    F_PR.Distance = PDA.F_R200Distance;
                    F_PR.TagPositon = "F";
                    F_PR.RFID_ReturnCode = PDA.TagReadResult;
                    F_PR.CreateDate = DateTime.Now;
                    F_PR.ModifiedDate = DateTime.Now;
                    F_PR.ModifiedBy = "HOST";
                    bn.dev_productionRecord.Add(F_PR);

                    dev_productionRecord B_PR = new dev_productionRecord();
                    B_PR.StationName = PDA.RFIDStationName;
                    B_PR.TagID = PDA.B_TagID;
                    B_PR.BoxID = PDA.B_BoxID;
                    B_PR.BoxType = PDA.B_BoxType;
                    B_PR.ASRSReadSignal = PDA.ASRSPLCReadSignal;
                    B_PR.ASRSReturnCode = PDA.ASRSPLCResult;
                    B_PR.RFIDConnectionStatus = PDA.RFIDReaderConSatus;
                    B_PR.R200ConnectionStatus = PDA.R200ConStatus;
                    B_PR.Distance = PDA.B_R200Distance;
                    B_PR.TagPositon = "B";
                    B_PR.RFID_ReturnCode = PDA.TagReadResult;
                    B_PR.CreateDate = DateTime.Now;
                    B_PR.ModifiedDate = DateTime.Now;
                    B_PR.ModifiedBy = "HOST";
                    bn.dev_productionRecord.Add(B_PR);
                    //bn.SaveChanges();
                    #endregion

                    #region 添加标签读取事件记录
                    dev_stationReadRecord TRER = new dev_stationReadRecord();
                    TRER.StationName = PDA.RFIDStationName; 
                    TRER.BoxID = PDA.F_BoxID == PDA.B_BoxID ? PDA.F_BoxID : PDA.F_BoxID + "&" + PDA.B_BoxID;
                    TRER.TagReadResult = PDA.TagReadResult;
                    TRER.F_TagReadStatus = (PDA.TagReadResult == "05" || PDA.TagReadResult == "07") ? "Success" : "Fail";
                    TRER.B_TagReadStatus = (PDA.TagReadResult == "06" || PDA.TagReadResult == "07") ? "Success" : "Fail";
                    switch (TRER.TagReadResult.ToUpper())
                    {
                        case "00": TRER.StatusPrompt = "Warning"; break;
                        case "FF": TRER.StatusPrompt = "Warning"; break;
                        case "04": TRER.StatusPrompt = "Error"; break;
                        case "05": TRER.StatusPrompt = "Warning"; break;
                        case "06": TRER.StatusPrompt = "Warning"; break;
                        case "07": TRER.StatusPrompt = "OK"; break;
                        case "0C": TRER.StatusPrompt = "Warning"; break;
                        default: TRER.StatusPrompt = "Error"; break;
                    }
                    TRER.CreateDate = DateTime.Now;
                    TRER.ModifiedDate = DateTime.Now;
                    bn.dev_stationReadRecord.Add(TRER);
                    bn.SaveChanges();
                    #endregion

                    #region 更新实时状态表
                    if (IOLinkData.RFIDList.Count > IOLinkIndex)
                    {
                        string StationName = "RFIDStation" + (IOLinkIndex + 1);
                        dev_realTimeStationData SSR = bn.dev_realTimeStationData.
                            FirstOrDefault(p => p.StationName == StationName);
                        if (SSR == null)
                        {
                            //添加
                            dev_realTimeStationData SR = new dev_realTimeStationData();
                            SR.StationName = PDA.RFIDStationName;
                            SR.ASRSReadSignal = PDA.ASRSPLCReadSignal;
                            SR.ASRSReturnCode = PDA.ASRSPLCResult;
                            SR.RFIDConnectionStatus = PDA.RFIDReaderConSatus;
                            SR.R200ConnectionStatus = PDA.R200ConStatus;
                            SR.F_TagID = PDA.F_TagID;
                            SR.F_BoxID = PDA.F_BoxID;
                            SR.F_BoxType = PDA.F_BoxType;
                            SR.F_R200Distance = PDA.F_R200Distance;
                            SR.B_TagID = PDA.B_TagID;
                            SR.B_BoxID = PDA.B_BoxID;
                            SR.B_BoxType = PDA.B_BoxType;
                            SR.B_R200Distance = PDA.B_R200Distance;
                            SR.RFID_ReturnCode = PDA.TagReadResult;
                            SR.RFID_ProductName = IOLinkData.RFIDList[IOLinkIndex].ProductName;
                            SR.RFID_SerialNumber = IOLinkData.RFIDList[IOLinkIndex].SerialNumber;
                            SR.RFID_HardwareVersion = IOLinkData.RFIDList[IOLinkIndex].HardwareVersion;
                            SR.RFID_FirmwareVersion = IOLinkData.RFIDList[IOLinkIndex].FirmwareVersion;
                            SR.R200_ProductName = IOLinkData.R200List[IOLinkIndex].ProductName;
                            SR.R200_SerialNumber = IOLinkData.R200List[IOLinkIndex].SerialNumber;
                            SR.R200_HardwareVersion = IOLinkData.R200List[IOLinkIndex].HardwareVersion;
                            SR.R200_FirmwareVersion = IOLinkData.R200List[IOLinkIndex].FirmwareVersion;
                            SR.ModifiedDate = DateTime.Now;
                            SR.ModifiedDateForDiagnosis = DateTime.Now;
                            bn.dev_realTimeStationData.Add(SR);
                        }
                        else
                        {
                            //更新
                            SSR.StationName = PDA.RFIDStationName;
                            SSR.ASRSReadSignal = PDA.ASRSPLCReadSignal;
                            SSR.ASRSReturnCode = PDA.ASRSPLCResult;
                            SSR.RFIDConnectionStatus = PDA.RFIDReaderConSatus;
                            SSR.R200ConnectionStatus = PDA.R200ConStatus;
                            SSR.F_TagID = PDA.F_TagID;
                            SSR.F_BoxID = PDA.F_BoxID;
                            SSR.F_BoxType = PDA.F_BoxType;
                            SSR.F_R200Distance = PDA.F_R200Distance;
                            SSR.B_TagID = PDA.B_TagID;
                            SSR.B_BoxID = PDA.B_BoxID;
                            SSR.B_BoxType = PDA.B_BoxType;
                            SSR.B_R200Distance = PDA.B_R200Distance;
                            SSR.RFID_ReturnCode = PDA.TagReadResult;
                            SSR.RFID_ProductName = IOLinkData.RFIDList[IOLinkIndex].ProductName;
                            SSR.RFID_SerialNumber = IOLinkData.RFIDList[IOLinkIndex].SerialNumber;
                            SSR.RFID_HardwareVersion = IOLinkData.RFIDList[IOLinkIndex].HardwareVersion;
                            SSR.RFID_FirmwareVersion = IOLinkData.RFIDList[IOLinkIndex].FirmwareVersion;
                            SSR.R200_ProductName = IOLinkData.R200List[IOLinkIndex].ProductName;
                            SSR.R200_SerialNumber = IOLinkData.R200List[IOLinkIndex].SerialNumber;
                            SSR.R200_HardwareVersion = IOLinkData.R200List[IOLinkIndex].HardwareVersion;
                            SSR.R200_FirmwareVersion = IOLinkData.R200List[IOLinkIndex].FirmwareVersion;
                            SSR.ModifiedDate = DateTime.Now;
                            SSR.ModifiedDateForDiagnosis = DateTime.Now;
                            bn.Entry(SSR).State = EntityState.Modified;
                        }

                    }
                    #endregion
                    bn.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                LogManager.WriteLog(LogFile.Trace, "Operate Production Error:" + ex.Message);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, "Operate Production Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 更新诊断数据表
        /// </summary>
        public static void UpdateDiagnoseTable()
        {
            try
            {
                using (BytonEntities bn = new BytonEntities())
                {
                    for (int i = 0; i < IOLinkData.RFIDList.Count; i++)
                    {
                        string StationName = IOLinkData.RFIDList[i].StationName;
                        dev_diagnosisRecord dr = bn.dev_diagnosisRecord.FirstOrDefault(p => p.StationName == StationName);
                        if (dr == null)
                        {
                            //insert
                            dev_diagnosisRecord ndr = new dev_diagnosisRecord();
                            ndr.StationName = IOLinkData.RFIDList[i].StationName;
                            ndr.RFID_VendorName = IOLinkData.RFIDList[i].VendorName;
                            ndr.RFID_ProductName = IOLinkData.RFIDList[i].ProductName;
                            ndr.RFID_SerialNumber = IOLinkData.RFIDList[i].SerialNumber;
                            ndr.RFID_HardwareVersion = IOLinkData.RFIDList[i].HardwareVersion;
                            ndr.RFID_FirmwareVersion = IOLinkData.RFIDList[i].FirmwareVersion;
                            ndr.RFID_DeviceStatus = IOLinkData.RFIDList[i].DeviceStatus;
                            ndr.RFID_Mode = IOLinkData.RFIDList[i].Mode;
                            ndr.RFID_MemoryArea = IOLinkData.RFIDList[i].MemoryArea;
                            ndr.RFID_BytesNumber = IOLinkData.RFIDList[i].NumberOfBytes;
                            ndr.RFID_StartAddress = IOLinkData.RFIDList[i].StartAddress;
                            ndr.RFID_AutoStart = IOLinkData.RFIDList[i].AutoStart;
                            ndr.RFID_TagType = IOLinkData.RFIDList[i].TagType;

                            ndr.R200_VendorName = IOLinkData.R200List[i].VendorName;
                            ndr.R200_ProductName = IOLinkData.R200List[i].ProductName;
                            ndr.R200_SerialNumber = IOLinkData.R200List[i].SerialNumber;
                            ndr.R200_HardwareVersion = IOLinkData.R200List[i].HardwareVersion;
                            ndr.R200_FirmwareVersion = IOLinkData.R200List[i].FirmwareVersion;
                            ndr.R200_DeviceStatus = IOLinkData.R200List[i].DeviceStatus;
                            ndr.R200_TemperatureIndicate = IOLinkData.R200List[i].TemperatureIndicate;
                            ndr.R200_OperationHours = IOLinkData.R200List[i].OperationHours;

                            ndr.ICE_OrderID = IOLinkData.ICEList[i].OrderID;
                            ndr.ICE_SerialNumber = IOLinkData.ICEList[i].SerialNumber;
                            ndr.ICE_HardwareVersion = IOLinkData.ICEList[i].HardwareVersion;
                            ndr.ICE_SoftwareVersion = IOLinkData.ICEList[i].SoftwareVersion;
                            ndr.ICE_IMVersion = IOLinkData.ICEList[i].IMVersion;

                            ndr.CreateDate = DateTime.Now;
                            ndr.ModifiedDate = DateTime.Now;
                            bn.dev_diagnosisRecord.Add(ndr);
                            //bn.SaveChanges();
                        }
                        else
                        {
                            //update Diagnose
                            dr.StationName = IOLinkData.RFIDList[i].StationName;
                            dr.RFID_VendorName = IOLinkData.RFIDList[i].VendorName;
                            dr.RFID_ProductName = IOLinkData.RFIDList[i].ProductName;
                            dr.RFID_SerialNumber = IOLinkData.RFIDList[i].SerialNumber;
                            dr.RFID_HardwareVersion = IOLinkData.RFIDList[i].HardwareVersion;
                            dr.RFID_FirmwareVersion = IOLinkData.RFIDList[i].FirmwareVersion;
                            dr.RFID_DeviceStatus = IOLinkData.RFIDList[i].DeviceStatus;
                            dr.RFID_Mode = IOLinkData.RFIDList[i].Mode;
                            dr.RFID_MemoryArea = IOLinkData.RFIDList[i].MemoryArea;
                            dr.RFID_BytesNumber = IOLinkData.RFIDList[i].NumberOfBytes;
                            dr.RFID_StartAddress = IOLinkData.RFIDList[i].StartAddress;
                            dr.RFID_AutoStart = IOLinkData.RFIDList[i].AutoStart;
                            dr.RFID_TagType = IOLinkData.RFIDList[i].TagType;

                            dr.R200_VendorName = IOLinkData.R200List[i].VendorName;
                            dr.R200_ProductName = IOLinkData.R200List[i].ProductName;
                            dr.R200_SerialNumber = IOLinkData.R200List[i].SerialNumber;
                            dr.R200_HardwareVersion = IOLinkData.R200List[i].HardwareVersion;
                            dr.R200_FirmwareVersion = IOLinkData.R200List[i].FirmwareVersion;
                            dr.R200_DeviceStatus = IOLinkData.R200List[i].DeviceStatus;
                            dr.R200_TemperatureIndicate = IOLinkData.R200List[i].TemperatureIndicate;
                            dr.R200_OperationHours = IOLinkData.R200List[i].OperationHours;

                            dr.ICE_OrderID = IOLinkData.ICEList[i].OrderID;
                            dr.ICE_SerialNumber = IOLinkData.ICEList[i].SerialNumber;
                            dr.ICE_HardwareVersion = IOLinkData.ICEList[i].HardwareVersion;
                            dr.ICE_SoftwareVersion = IOLinkData.ICEList[i].SoftwareVersion;
                            dr.ICE_IMVersion = IOLinkData.ICEList[i].IMVersion;

                            dr.ModifiedDate = DateTime.Now;
                            bn.Entry(dr).State = EntityState.Modified;
                            //bn.SaveChanges();
                            

                        }

                        dev_realTimeStationData real = bn.dev_realTimeStationData.FirstOrDefault(p => p.StationName == StationName);
                        if (real == null)
                        { }
                        else
                        {
                            //update realtimestation
                            real.RFID_ProductName = IOLinkData.RFIDList[i].ProductName;
                            real.RFID_SerialNumber = IOLinkData.RFIDList[i].SerialNumber;
                            real.RFID_HardwareVersion = IOLinkData.RFIDList[i].HardwareVersion;
                            real.RFID_FirmwareVersion = IOLinkData.RFIDList[i].FirmwareVersion;
                            real.R200_ProductName = IOLinkData.R200List[i].ProductName;
                            real.R200_SerialNumber = IOLinkData.R200List[i].SerialNumber;
                            real.R200_HardwareVersion = IOLinkData.R200List[i].HardwareVersion;
                            real.R200_FirmwareVersion = IOLinkData.R200List[i].FirmwareVersion;
                            real.ModifiedDateForDiagnosis = DateTime.Now;
                            bn.Entry(real).State = EntityState.Modified;
                        }

                        bn.SaveChanges();
                    }
                    
                }


            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, "Operate Diagnose Error:" + ex.Message);
            }
            
        }

        /// <summary>
        /// 更新系统监控表
        /// </summary>
        /// <param name="opcConnected"></param>
        /// <param name="signalQuality"></param>
        public static void UpdateSystemStatusTable(bool opcConnected,string signalQuality,bool InsertHistory)
        {
            try
            {
                using (BytonEntities bn = new BytonEntities())
                {
                    #region 操作系统状态表
                    if (bn.dev_sys_status.Count() > 0)
                    {
                        //update
                        dev_sys_status ss = bn.dev_sys_status.FirstOrDefault(p => p.id > 0);
                        if (opcConnected == true)
                        {
                            ss.sys_ASRS_PLC = SystemWatchData.AsrsPlcConEx;
                            ss.sys_daemon_status = "OK";
                            ss.sys_PLC_status = SystemWatchData.PfPlcEx;
                            ss.ICE_1_status = SystemWatchData.Ice1Ex;
                            ss.ICE_2_status = SystemWatchData.Ice2Ex;
                            ss.ICE_3_status = SystemWatchData.Ice3Ex;
                            ss.ICE_4_status = SystemWatchData.Ice4Ex;
                        }
                        else
                        {
                            ss.sys_daemon_status = "NG";
                        }
                        ss.ModifiedDate = DateTime.Now;
                        bn.Entry(ss).State = EntityState.Modified;
                    }
                    else
                    {
                        //insert
                        dev_sys_status ss = new dev_sys_status();
                        ss.sys_ASRS_PLC = SystemWatchData.AsrsPlcConEx;
                        ss.sys_daemon_status = (opcConnected == true ? "OK" : "NG");
                        ss.sys_ASRS_PLC = SystemWatchData.PfPlcEx;
                        ss.ICE_1_status = SystemWatchData.Ice1Ex;
                        ss.ICE_2_status = SystemWatchData.Ice2Ex;
                        ss.ICE_3_status = SystemWatchData.Ice3Ex;
                        ss.ICE_4_status = SystemWatchData.Ice4Ex;
                        ss.CreateDate = DateTime.Now;
                        ss.ModifiedDate = DateTime.Now;
                        bn.dev_sys_status.Add(ss);
                    }
                    #endregion

                    #region 插入系统状态历史表
                    //insert
                    if (InsertHistory)
                    {
                        dev_sys_statusRecord ssh = new dev_sys_statusRecord();
                        ssh.sys_ASRS_PLC = SystemWatchData.AsrsPlcConEx.ToString();
                        ssh.sys_daemon_status = (opcConnected == true ? "OK" : "NG");
                        ssh.sys_PLC_status = SystemWatchData.PfPlcEx.ToString();
                        ssh.ICE_1_status = SystemWatchData.Ice1Ex.ToString();
                        ssh.ICE_2_status = SystemWatchData.Ice2Ex.ToString();
                        ssh.ICE_3_status = SystemWatchData.Ice3Ex.ToString();
                        ssh.ICE_4_status = SystemWatchData.Ice4Ex.ToString();
                        ssh.CreateDate = DateTime.Now;
                        ssh.ModifiedDate = DateTime.Now;
                        bn.dev_sys_statusRecord.Add(ssh);
                        #endregion
                    }
                    bn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, "Operate System Error:" + ex.Message);
            }
            
        }

        /// <summary>
        /// 插入大事件记录表
        /// </summary>
        /// <param name="eventName"></param>
        public static void InsertBigEventTable(string eventName)
        {
            try
            {
                using (BytonEntities bn = new BytonEntities())
                {
                    dev_eventRecord BER = new dev_eventRecord();
                    BER.EventName = eventName;
                    BER.Excutor = "HOST";
                    BER.CreateDate = DateTime.Now;
                    bn.dev_eventRecord.Add(BER);
                    bn.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(LogFile.Trace, "Operate Event Error:" + ex.Message);
            }
        }
    }
}
