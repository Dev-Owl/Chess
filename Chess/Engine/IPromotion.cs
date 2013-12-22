using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess.Engine
{
    public interface IPromotion
    {
        EFigures GetDecision();
    }
}
