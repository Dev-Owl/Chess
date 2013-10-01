using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chess.Game;
using Chess.GUI;

namespace Chess.GUI
{
    //Read figure from resouce folder 

   public class Figure
    {
        public static int WHITE = 0;
        public static int BLACK = 1;

       


        bool ingame;
        public bool Ingame
        {
            get { return ingame; }
            set { ingame = value; }
        }

        bool moved;
        public bool Moved
        {
            get { return moved; }
            set { moved = value; }
        }

        int color;
        public int Color
        {
            get { return color; }
            set { color = value; }
        }

        int value;
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        FigurePosition position;
        public FigurePosition Position
        {
            get { return position; }
            set { position = value; }
        }
        
        EFigures figuretype;
        public EFigures Figuretype
        {
            get { return figuretype; }
            set { figuretype = value; }
        }
        
        GameBoard gameBoard;
        public GameBoard GameBoard
        {
            get { return gameBoard; }
            set { gameBoard = value; }
        }

        public Figure(EFigures FigureType,int Color,GameBoard Board)
        {
            set(FigureType, Color, Board, null);
        }

        public Figure(EFigures FigureType, int Color, GameBoard Board, FigurePosition Position)
        {
            set(FigureType, Color, Board, Position);
        }

        private void set(EFigures FigureType, int Color, GameBoard Board, FigurePosition Position)
        {
            this.color = Color;
            this.figuretype = FigureType;
            this.gameBoard = Board;
            this.position = Position;
            
            configure();
        }

        private void configure()
        { 
            //Set default pos if pos is null
            //Set value for figure
            switch (this.figuretype)
            {
                case EFigures.King:
                    {
                        this.value = 10;
                        if (position == null)
                        {
                            if (this.color == Figure.WHITE)
                            {
                                this.position = new FigurePosition('e', 1);
                            }
                            else
                            {
                                this.position = new FigurePosition('e', 8);
                            }
                        }
                    }
                    break;
                case EFigures.Queen:
                    {
                        this.value = 9;
                        if (position == null)
                        {
                            if (this.color == Figure.WHITE)
                            {
                                this.position = new FigurePosition('d', 1);
                            }
                            else
                            {
                                this.position = new FigurePosition('d', 8);
                            }
                        }
                    }
                    break;
                case EFigures.Rook:
                    {
                        this.value = 5;
                        if (position == null)
                        {
                            if (this.color == Figure.WHITE)
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Rook && k.Color == Figure.WHITE) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('h', 1);
                                }
                                else
                                { 
                                    //Add first Rook for color
                                    this.position = new FigurePosition('a', 1);
                                }
                            }
                            else
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Rook && k.Color == Figure.BLACK) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('h', 8);
                                }
                                else
                                {
                                    //Add first Rook for color
                                    this.position = new FigurePosition('a', 8);
                                }
                            }
                        }

                    }
                    break;
                case EFigures.Bishop:
                    {
                        this.value = 3;
                        if (position == null)
                        {
                            if (this.color == Figure.WHITE)
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Bishop && k.Color == Figure.WHITE) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('c', 1);
                                }
                                else
                                {
                                    //Add first Rook for color
                                    this.position = new FigurePosition('f', 1);
                                }
                            }
                            else
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Bishop && k.Color == Figure.BLACK) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('c', 8);
                                }
                                else
                                {
                                    //Add first Rook for color
                                    this.position = new FigurePosition('f', 8);
                                }
                            }
                        }
                    }
                    break;
                case EFigures.Knight:
                    {
                        this.value = 3;
                        if (position == null)
                        {
                            if (this.color == Figure.WHITE)
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Knight && k.Color == Figure.WHITE) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('b', 1);
                                }
                                else
                                {
                                    //Add first Rook for color
                                    this.position = new FigurePosition('g', 1);
                                }
                            }
                            else
                            {
                                if (this.gameBoard.Figures.Find(k => k.Figuretype == EFigures.Knight && k.Color == Figure.BLACK) != null)
                                {
                                    //Add second Rook for color
                                    this.position = new FigurePosition('b', 8);
                                }
                                else
                                {
                                    //Add first Rook for color
                                    this.position = new FigurePosition('g', 8);
                                }
                            }
                        }
                    }
                    break;
                case EFigures.Pawn:
                    {
                        this.value = 1;
                        if (position == null)
                        {
                            
                            if (this.color == Figure.WHITE)
                            {
                                this.position = new FigurePosition((char)(65 + this.gameBoard.Figures.Count<Figure>(f => f.Figuretype == EFigures.Pawn && f.Color == Figure.WHITE)), 2);
                            }
                            else
                            {
                                this.position = new FigurePosition((char)(65 + this.gameBoard.Figures.Count<Figure>(f => f.Figuretype == EFigures.Pawn && f.Color == Figure.BLACK)), 7);
                            }
                        }
                    }
                    break;
                default:
                    {
                        throw new Exception("Unkown figure type");
                    }
            }
        }

        public List<FigurePosition> GetLegalMoves()
        {
            
            
            //TODO Bishop ( and queen bishop part) are not correct yet!!!!
            //TODO Check for the king is not implemented
            

            List<FigurePosition> legalMoves = new List<FigurePosition>();
            if (this.ingame)
            {
                switch (this.figuretype)
                {
                    case EFigures.Pawn:
                        {
                            #region Pawn
                            //Check all free steps
                            if (!this.moved)
                            {
                                legalMoves.Add(this.Color == Figure.WHITE ? this.position.Forward():this.position.Backward());
                                legalMoves.Add(this.Color == Figure.WHITE ? this.position.Forward().Forward() : this.position.Backward().Backward());
                            }
                            else
                            {
                                legalMoves.Add(this.Color == Figure.WHITE ? this.position.Forward() : this.position.Backward());
                            }

                            Figure lastFig = null;
                            List<Figure> blocking = this.gameBoard.GetFigureAt(legalMoves.ToArray());
                            foreach (Figure single in blocking)
                            {
                                if (lastFig == null)
                                {
                                    lastFig = single;
                                }
                                legalMoves.RemoveAll(p => this.Color == Figure.WHITE ? p.Above(single.Position) || p.Same(single.Position) : p.Below(single.Position) || p.Same(single.Position));
                            }
                            //Check possible attack position
                            List<FigurePosition> attackPositions = new List<FigurePosition>();
                            if (this.position.PositionX == 'a' || this.position.PositionX == 'h')
                            {
                                attackPositions.Add(new FigurePosition(this.position.PositionX == 'a' ? 'b' : 'g', this.color == Figure.BLACK ? this.position.PositionY - 1 : this.position.PositionY + 1));
                            }
                            else
                            {
                                attackPositions.Add(new FigurePosition((char)(this.position.PositionX + 1), this.color == Figure.BLACK ? this.position.PositionY - 1 : this.position.PositionY + 1));
                                attackPositions.Add(new FigurePosition((char)(this.position.PositionX - 1), this.color == Figure.BLACK ? this.position.PositionY - 1 : this.position.PositionY + 1));
                            }
                            List<Figure> attackFigures = this.gameBoard.GetFigureAt(attackPositions.ToArray());
                            if (attackFigures.Count > 0)
                            {
                                attackFigures.FindAll(f => f.Color != this.Color).ForEach(p=> legalMoves.Add(p.Position));
                            }
                       

                            #endregion
                        }
                        break;
                    case EFigures.Rook:
                        {
                            #region Rook
                            if (this.Position.Left() != null)
                            {
                                FigurePosition Left = this.position.Left();
                                Figure lastFig = null;
                                while (Left != null)
                                {
                                    
                                    lastFig = this.gameBoard.GetFigureAt(Left);
                                    if (lastFig != null)
                                    {
                                        if (lastFig.Color != this.Color)
                                        {
                                            legalMoves.Add(Left);
                                        }
                                        break;
                                    }
                                    legalMoves.Add(Left);
                                    Left=Left.Left();
                                }
                            }

                            if (this.Position.Right() != null)
                            {
                                FigurePosition Right = this.position.Right();
                                Figure lastFig = null;
                                while (Right != null)
                                {

                                    lastFig = this.gameBoard.GetFigureAt(Right);
                                    if (lastFig != null)
                                    {
                                        if (lastFig.Color != this.Color)
                                        {
                                            legalMoves.Add(Right);
                                        }
                                        break;
                                    }
                                    legalMoves.Add(Right);
                                    Right = Right.Right();
                                }

                            }

                            if (this.Position.Forward() != null)
                            {
                                FigurePosition Forward = this.position.Forward();
                                Figure lastFig = null;
                                while (Forward != null)
                                {

                                    lastFig = this.gameBoard.GetFigureAt(Forward);
                                    if (lastFig != null)
                                    {
                                        if (lastFig.Color != this.Color)
                                        {
                                            legalMoves.Add(Forward);
                                        }
                                        break;
                                    }
                                    legalMoves.Add(Forward);
                                    Forward = Forward.Forward();
                                }

                            }

                            if (this.Position.Backward() != null)
                            {
                                FigurePosition Backward = this.position.Backward();
                                Figure lastFig = null;
                                while (Backward != null)
                                {

                                    lastFig = this.gameBoard.GetFigureAt(Backward);
                                    if (lastFig != null)
                                    {
                                        if (lastFig.Color != this.Color)
                                        {
                                            legalMoves.Add(Backward);
                                        }
                                        break;
                                    }
                                    legalMoves.Add(Backward);
                                    Backward = Backward.Backward();
                                }

                            } 
                            #endregion
                        }
                        break;
                    case EFigures.Bishop:
                        {
                            #region Bishop
                            FigurePosition workingPos = this.position.TopLeft();
                            Figure lastFig = null;
                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.TopLeft();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.position.TopRight();

                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.TopRight();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }

                            workingPos = this.position.LowerLeft();

                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.LowerLeft();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }

                            workingPos = this.position.LowerRight();
                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.LowerRight();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            #endregion
                        }
                        break;
                    case EFigures.Knight:
                        {
                            #region Knight
                            FigurePosition workingPos = this.Position.Forward();
                            FigurePosition sourcePos = workingPos;
                            
                            if (workingPos != null)
                            {
                                workingPos = workingPos.Forward();
                                if (workingPos != null)
                                {
                                    sourcePos = workingPos;
                                    workingPos = workingPos.Left();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                    workingPos = sourcePos.Right();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                }
                                 
                            }

                            workingPos = this.Position.Left();
                            sourcePos = workingPos;
                            if (workingPos != null)
                            {
                                workingPos = workingPos.Left();
                                if (workingPos != null)
                                {
                                    sourcePos = workingPos;
                                    workingPos = workingPos.Forward();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                    workingPos = sourcePos.Backward();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                }
                            }

                            workingPos = this.Position.Right();
                            sourcePos = workingPos;
                            if (workingPos != null)
                            {
                                workingPos = workingPos.Right();
                                if (workingPos != null)
                                {
                                    sourcePos = workingPos;
                                    workingPos = workingPos.Forward();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                    workingPos = sourcePos.Backward();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                }
                            }
                            
                            workingPos = this.Position.Backward();
                            sourcePos = workingPos;
                            if (workingPos != null)
                            {
                                workingPos = workingPos.Backward();
                                if (workingPos != null)
                                {
                                    sourcePos = workingPos;
                                    workingPos = workingPos.Right();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                    workingPos = sourcePos.Left();
                                    if (workingPos != null)
                                    {
                                        Figure destinationFigure = this.gameBoard.GetFigureAt(workingPos);
                                        if (destinationFigure == null || destinationFigure.Color != this.Color)
                                        {
                                            legalMoves.Add(workingPos);
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                        break;
                    case EFigures.Queen:
                        {
                            #region Queen
                            FigurePosition workingPos = this.position.TopLeft();
                            Figure lastFig = null;
                            #region Bishop
                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.TopLeft();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.position.TopRight();

                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.TopRight();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }

                            workingPos = this.position.LowerLeft();

                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.LowerLeft();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }

                            workingPos = this.position.LowerRight();
                            if (workingPos != null)
                            {
                                lastFig = this.gameBoard.GetFigureAt(workingPos);
                                if (lastFig == null)
                                {
                                    legalMoves.Add(workingPos);
                                    while (workingPos != null)
                                    {
                                        workingPos = workingPos.LowerRight();
                                        if (workingPos != null)
                                        {
                                            lastFig = this.gameBoard.GetFigureAt(workingPos);
                                            if (lastFig == null || lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(workingPos);
                                                if (lastFig != null)
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else if (lastFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            #endregion
                            #region Rook part

                            if (this.Position.Left() != null)
                                {
                                    FigurePosition Left = this.position.Left();
                                    lastFig = null;
                                    while (Left != null)
                                    {

                                        lastFig = this.gameBoard.GetFigureAt(Left);
                                        if (lastFig != null)
                                        {
                                            if (lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(Left);
                                            }
                                            break;
                                        }
                                        legalMoves.Add(Left);
                                        Left = Left.Left();
                                    }
                                }

                                if (this.Position.Right() != null)
                                {
                                    FigurePosition Right = this.position.Right();
                                    lastFig = null;
                                    while (Right != null)
                                    {

                                        lastFig = this.gameBoard.GetFigureAt(Right);
                                        if (lastFig != null)
                                        {
                                            if (lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(Right);
                                            }
                                            break;
                                        }
                                        legalMoves.Add(Right);
                                        Right = Right.Right();
                                    }

                                }

                                if (this.Position.Forward() != null)
                                {
                                    FigurePosition Forward = this.position.Forward();
                                    lastFig = null;
                                    while (Forward != null)
                                    {

                                        lastFig = this.gameBoard.GetFigureAt(Forward);
                                        if (lastFig != null)
                                        {
                                            if (lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(Forward);
                                            }
                                            break;
                                        }
                                        legalMoves.Add(Forward);
                                        Forward = Forward.Forward();
                                    }

                                }

                                if (this.Position.Backward() != null)
                                {
                                    FigurePosition Backward = this.position.Backward();
                                    lastFig = null;
                                    while (Backward != null)
                                    {

                                        lastFig = this.gameBoard.GetFigureAt(Backward);
                                        if (lastFig != null)
                                        {
                                            if (lastFig.Color != this.Color)
                                            {
                                                legalMoves.Add(Backward);
                                            }
                                            break;
                                        }
                                        legalMoves.Add(Backward);
                                        Backward = Backward.Backward();
                                    }
                                }
                            
                               
                                    #endregion
                            #endregion
                        }
                        break;
                    case EFigures.King:
                        {
                            #region King
                            FigurePosition workingPos = this.Position.Forward();
                            Figure workingFig = null;
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.Backward();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.Left();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.Right();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.TopLeft();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.TopRight();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.LowerLeft();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            workingPos = this.Position.LowerRight();
                            if (workingPos != null)
                            {
                                workingFig = this.gameBoard.GetFigureAt(workingPos);
                                if (workingFig == null || workingFig.Color != this.Color)
                                {
                                    legalMoves.Add(workingPos);
                                }
                            }
                            //All figs that are ingame and not the same color
                            List<Figure> opponentFigs = this.gameBoard.Figures.FindAll(F => F.Color != this.Color && F.Ingame);
                            //Remove all Paws and the King that are more than two fields away
                            opponentFigs.RemoveAll(F => F.Figuretype == EFigures.Pawn || F.Figuretype == EFigures.King && F.Position.DifferenceY(this.position.PositionY) > 2);
                            opponentFigs.RemoveAll(F => F.Figuretype == EFigures.Knight &&( F.Position.DifferenceY(this.position.PositionY) > 3 || F.Position.DifferenceX(this.position.PositionX) > 3));
                            opponentFigs.RemoveAll(F => F.Figuretype == EFigures.Rook && ( F.Position.PositionX != this.Position.PositionX && F.Position.PositionY != this.Position.PositionY));
                            opponentFigs.RemoveAll(F => F.Figuretype == EFigures.Bishop && F.Position.ColorEqual(this.Position));
                            List<FigurePosition> attackPos = new List<FigurePosition>();
                            foreach (Figure figure in opponentFigs)
                            {
                                attackPos.AddRange(figure.GetLegalMoves());
                            }
                            
                            
                            #endregion
                        }
                        break;
                }
            }
            return legalMoves;
        }

      
        
    }
}
