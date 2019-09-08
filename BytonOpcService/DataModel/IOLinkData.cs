using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytonOpcService.DataModel
{
   public class IOLinkData
    {
        public static List<RFID> RFIDList = new List<RFID>();
        public static List<R200> R200List = new List<R200>();
        public static List<ICE> ICEList = new List<ICE>();

        public static void ClearAll()
        {
            RFIDList.Clear();
            R200List.Clear();
            ICEList.Clear();
        }
    }
    
    public class RFID
    {
        /// <summary>
        /// 站点号
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 厂商
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 产品名
        /// </summary>
        public string ProductName { get; set; }//64

        /// <summary>
        /// 序列号
        /// </summary>
        public string SerialNumber { get; set; }//16

        /// <summary>
        /// 硬件版本
        /// </summary>
        public string HardwareVersion { get; set; }//64

        /// <summary>
        /// 固件版本
        /// </summary>
        public string FirmwareVersion { get; set; }//64

        public string DeviceStatus { get; set; }//1

        /// <summary>
        /// 操作模式
        /// </summary>
        public string Mode { get; set; }//1

        /// <summary>
        /// 操作内存分区  0x80=UID，0x0=User Memory
        /// </summary>
        public string MemoryArea { get; set; }//1

        /// <summary>
        /// 字节数
        /// </summary>
        public string NumberOfBytes { get; set; }//1

        /// <summary>
        /// 起始地址
        /// </summary>
        public string StartAddress { get; set; }//2

        /// <summary>
        /// 自动开始 0x80=on，0x0=off
        /// </summary>
        public string AutoStart { get; set; }//1

        /// <summary>
        /// 标签类型 20=automatic
        /// </summary>
        public string TagType { get; set; }//1
    }

    public class R200
    {
        public string VendorName { get; set; }//64
        public string ProductName { get; set; }//64
        public string SerialNumber { get; set; }//16
        public string HardwareVersion { get; set; }//64
        public string FirmwareVersion { get; set; }//64
        /// <summary>
        /// "0=Device OK
        ///1=Maintenance required
        ///2=Out of specification
        ///3=Functional check
        ///4=Failure"
        /// </summary>
        public string DeviceStatus { get; set; }//1
        /// <summary>
        /// "0=Safe operation temperature
        ///1=Critical high ambient temperature
        ///2=Temperature above specified limit
        ///3=Critical low ambient temperature
        ///4=Temperature below specified limit"
        /// </summary>
        public string TemperatureIndicate { get; set; }//1
        public string OperationHours { get; set; }//4
    }

    public class ICE
    {
        public string OrderID { get; set; }//20
        public string SerialNumber { get; set; }//16
        public string HardwareVersion { get; set; }//2
        public string SoftwareVersion { get; set; }//4
        public string IMVersion { get; set; }//2
    }
}
