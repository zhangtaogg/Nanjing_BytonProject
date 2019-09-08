using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytonOpcService.DataModel
{
    public class ProductionData
    {
        /// <summary>
        /// 站点号
        /// </summary>
        public string RFIDStationName { get; set; }

        /// <summary>
        /// ASRS触发RFID站点读取指令
        /// </summary>
        public string ASRSPLCReadSignal { get; set; }

        /// <summary>
        /// RFID站点读取结果
        /// </summary>
        public string TagReadResult { get; set; }

        /// <summary>
        /// ASRS PLC数据接收结果,1=接收完成，0=接收未完成
        /// </summary>
        public string ASRSPLCResult { get; set; }

        /// <summary>
        /// RFID站点请求人工干预,1=接收完成,0=接收未完成
        /// </summary>
        public string ManualWriteBoxRequest { get; set; }

        /// <summary>
        /// RFID读头连接状态,1=正常，0=存在故障
        /// </summary>
        public string RFIDReaderConSatus { get; set; }

        /// <summary>
        /// R200连接状态,1=正常，0=存在故障
        /// </summary>
        public string R200ConStatus { get; set; }

        /// <summary>
        /// 前标签读取的料框号
        /// </summary>
        public string F_BoxID { get; set; }

        /// <summary>
        /// 前标签读取的料框类型
        /// </summary>
        public string F_BoxType { get; set; }

        /// <summary>
        /// 前标签ID
        /// </summary>
        public string F_TagID { get; set; }

        /// <summary>
        /// 前标签写入日期
        /// </summary>
        public string F_TagWriteDate { get; set; }

        /// <summary>
        /// 前标签R200距离
        /// </summary>
        public string F_R200Distance { get; set; }
        

        /// <summary>
        /// 后标签读取的料框号
        /// </summary>
        public string B_BoxID { get; set; }

        /// <summary>
        /// 后标签读取的料框类型
        /// </summary>
        public string B_BoxType { get; set; }

        /// <summary>
        /// 后标签ID
        /// </summary>
        public string B_TagID { get; set; }

        /// <summary>
        /// 后标签写入日期
        /// </summary>
        public string B_TagWriteDate { get; set; }

        /// <summary>
        /// 后标签R200距离
        /// </summary>
        public string B_R200Distance { get; set; }

    }
}
