using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ABChess.Engine
{
    public interface IAI : IPromotion
    {
        void Init();
        
        void YourTurn();

        void ForceMove();

        void MatchEnding();
    }
}
