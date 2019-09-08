using OpcUaClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiSelfHost {
    public class SingleOpcClient {
        private static SingleOpcClient instance;
        private UaClient opcClient;
        private static readonly object obj = new object();
    
        public static SingleOpcClient CreateInstance() {
            if (instance == null) {
                lock (obj) {
                    if (instance == null) {
                        instance = new SingleOpcClient();
                    }
                }
            }
            return instance;
        }
        private SingleOpcClient() {
            opcClient = new UaClient();
        }
 
        public int WriteRFIDTag(OpcUaClient.OpcParam mOpcParam) {
            lock (obj) {
                return opcClient.WriteDataToOpcServer(mOpcParam);
            }
        }
        public string ReadRFIDTag(string station) {
            lock (obj) {
                return opcClient.ReadDataFromOpcServer(station);
            }
        }


        public string ReadAnyTag(string station) {
            lock (obj) {
                return opcClient.ReadDataFromOpcServer(station);
            }
        }
    }
}