using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABChess.Engine
{
    public interface IPromotion
    {
        EFigures GetDecision();
    }
}
