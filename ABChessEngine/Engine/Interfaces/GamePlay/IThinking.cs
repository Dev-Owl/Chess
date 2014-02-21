using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABChess.Engine
{
    public interface IThinking
    {
        void Show();
        void Close();
        void SetMessage(string Message);

    }
}
