using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace BytonOpcService
{
   public class XMLSettings
    {
        public static string FileName = "Settings.xml";
        public static string FilePath;

        public const string Key_Con = "Connection";
        public const string Key_NodeIdList = "NodeIdList";
        public const string Key_IOLinkList = "IOLinkList";
        public const string Key_DataType = "DataType";
        public const string Key_Access = "Access";
        public const string Key_NodeId = "NodeId";
        public const string Key_WriteNodeId = "WriteNodeId";

        //public const string Value_IP = "IP";
        public const string Value_Port = "Port";
        public const string Value_Password = "Password";
        public const string Value_NodeIdList = "NodeIdList";
        public const string Value_NodeIdIOLink = "IOLink";
        public const string Value_NodeIdSystem = "System";
        public const string Value_NodeIdIPCReceiveResult = "IPCReceiveResult";
        public const string Value_NodeCount = "NodeCount";
        public const string Value_UserName = "UserName";

        public string URL
        { get; set; }
        public string Port
        { get; set; }
        public string UserName
        { get; set; }
        public string Password
        { get; set; }
        public string[] NodeIdList
        { get; set; }
        public string[] IOLinkList
        { get; set; }
        public string SystemNodeId
        { get; set; }
        public string IPCReceiveResult
        { get; set; }
        public int NodeCount
        { get; set; }
        public string DataType
        { get; set; }
        public string Access
        { get; set; }

        public XMLSettings() { FilePath = AppDomain.CurrentDomain.BaseDirectory + "Settings//"+FileName; }

        #region 读取配置文件，将相应参数填入实例对象的属性中
        /// <summary>
        /// 读取配置文件，将相应参数填入实例对象的属性中
        /// </summary>
        public void ReadXMLSettings()
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(FilePath);
                XmlNodeList nodeListsOfRoot = xml.SelectSingleNode("Root").ChildNodes;
                for (int i = 0; i < nodeListsOfRoot.Count; i++)
                {
                    switch (nodeListsOfRoot[i].Name)
                    {
                        case Key_Con:
                            this.Port = nodeListsOfRoot[i].SelectSingleNode(Value_Port).InnerText;
                            this.URL = "opc.tcp://" + GetHostIP() + ":" + this.Port;
                            this.UserName = nodeListsOfRoot[i].SelectSingleNode(Value_UserName).InnerText;
                            this.Password = nodeListsOfRoot[i].SelectSingleNode(Value_Password).InnerText;
                            break;
                        case Key_NodeId:
                            this.SystemNodeId = nodeListsOfRoot[i].SelectSingleNode(Value_NodeIdSystem).InnerText;
                            break;
                        case Key_WriteNodeId:
                            this.IPCReceiveResult = nodeListsOfRoot[i].SelectSingleNode(Value_NodeIdIPCReceiveResult).InnerText;
                            break;
                        case Key_NodeIdList:
                            this.NodeIdList = new string[nodeListsOfRoot[i].ChildNodes.Count];
                            for (int j = 0; j < NodeIdList.Length; j++)
                            {
                                NodeIdList[j] = nodeListsOfRoot[i].ChildNodes[j].InnerText;
                            }
                            this.NodeCount = NodeIdList.Length;
                            break;
                        case Key_IOLinkList:
                            this.IOLinkList = new string[nodeListsOfRoot[i].ChildNodes.Count];
                            for (int j = 0; j < IOLinkList.Length; j++)
                            {
                                IOLinkList[j] = nodeListsOfRoot[i].ChildNodes[j].InnerText;
                            }
                            break;
                        case Key_DataType:
                            this.DataType = nodeListsOfRoot[i].InnerText;
                            break;
                        case Key_Access:
                            this.Access = nodeListsOfRoot[i].InnerText;
                            break;
                        default: break;
                    }
                }
            }
            catch (Exception ex)
            { }
          
        }
        #endregion

        #region 获取本机IPV4地址
        /// <summary>
        /// 获取本机IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string GetHostIP()
        {
            string hostname = Dns.GetHostName();
            IPHostEntry localentry = Dns.GetHostEntry(hostname);
            IPAddress localaddress = localentry.AddressList.FirstOrDefault(d => d.AddressFamily.ToString().Equals("InterNetwork"));
            //IPAddress localaddress = localentry.AddressList[0]; 
            return localaddress.ToString();
        }
        #endregion

    }
}
