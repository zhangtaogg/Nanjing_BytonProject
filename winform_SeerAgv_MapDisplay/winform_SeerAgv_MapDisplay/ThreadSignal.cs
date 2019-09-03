using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace winform_SeerAgv_MapDisplay
{
    class ThreadSignal
    {
        public Thread thread;
        private bool stopThread = false;
        public bool StopThread
        {
            get { return stopThread; }
            set { stopThread = value; }
        }
       // private delegate void myDelegate();
       // private myDelegate mydelegate;
        public ThreadSignal()
        {
            StopThread = false;
            //mydelegate = new myDelegate(func_delegate);
            //thread = new Thread(myfunc,mydelegate);
        }
        public void SetBack()
        {
            thread.IsBackground = true;
        }

        public void Start()
        {
            thread.Start();
        }
        public void Abort()
        {
            StopThread = true;
           
        }
    }
}
