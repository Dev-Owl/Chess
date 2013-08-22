using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chess.Log
{
    class EventLog
    {

        private Control logControl;
        
        public EventLog(Control LogControl)
        {
            this.logControl = LogControl;
        }

        public void Log(string Message)
        {
            this.logControl.Text += String.Format("{0}{1}: {2}", Environment.NewLine, DateTime.Now.ToShortTimeString(), Message);
        }
    }
}
