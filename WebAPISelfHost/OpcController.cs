using WebApiSelfHost;
using System.Collections.Generic;
using System.Web.Http;

namespace WebApiSelfHost {
    public class OpcController : ApiController
    {

        
        [HttpPost]
        public int WriteOpc([FromBody]OpcUaClient.OpcParam mOpcParam) {
            OpcUaClient.OpcParam c = mOpcParam;
            SingleOpcClient opc = SingleOpcClient.CreateInstance();
            return opc.WriteRFIDTag(c);
        }
        [HttpGet]
        public int List()
        {
            return 1;
        }


        /*
                [HttpPost]
                public string ReadRFIDTag() {
                    SingleOpcClient opc = SingleOpcClient.CreateInstance();
                    return "";
                }


                [HttpPost]
                public string ReadAnyTag() {
                    SingleOpcClient opc = SingleOpcClient.CreateInstance();
                    return "";
                }
                */

    }
}
