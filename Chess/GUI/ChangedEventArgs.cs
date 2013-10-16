using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.GUI
{
   public class ChangedEventArgs : EventArgs
    {
        object _eventData;

        public object EventData
        {
            get { return _eventData; }
            set { _eventData = value; }
        }

        public ChangedEventArgs(Object EventData)
            : base()
        {
            this._eventData = EventData;
        }
    }
}
