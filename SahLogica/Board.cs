using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahLogica
{
    public class Board
    {
        private readonly Piece[,] pieces = new Piece[8, 8];

        private readonly Dictionary<Player, Position> pawnSkipPositions = new Dictionary<Player, Position>
        {
            { Player.Alb, null },
            {Player.Negru, null },
        };

        public Piece this[int row, int col]
        {
            get { return pieces[row, col]; }
            set { pieces[row, col] = value; }
        }

        public Piece this[Position pos]
        {
            get { return this[pos.Row, pos.Column]; }
            set { this[pos.Row, pos.Column] = value; }
        }

        public Position GetPawnSkipPosition(Player player)
        {
            return pawnSkipPositions[player];
        }

        public void SetPawnSkipPosition(Player player, Position pos)
        {
            pawnSkipPositions[player] = pos;
        }

        public static Board Initial()
        {
            Board board = new Board();
            board.AddStartPieces();
            return board;
        }

        private void AddStartPieces()
        {
            this[0, 0] = new Rook(Player.Negru);
            this[0, 1] = new Knight(Player.Negru);
            this[0, 2] = new Bishop(Player.Negru);
            this[0, 3] = new Queen(Player.Negru);
            this[0, 4] = new King(Player.Negru);
            this[0, 5] = new Bishop(Player.Negru);
            this[0, 6] = new Knight(Player.Negru);
            this[0, 7] = new Rook(Player.Negru);

            this[7, 0] = new Rook(Player.Alb);
            this[7, 1] = new Knight(Player.Alb);
            this[7, 2] = new Bishop(Player.Alb);
            this[7, 3] = new Queen(Player.Alb);
            this[7, 4] = new King(Player.Alb);
            this[7, 5] = new Bishop(Player.Alb);
            this[7, 6] = new Knight(Player.Alb);
            this[7, 7] = new Rook(Player.Alb);

            for (int i = 0; i < 8; i++)
            {
                this[1, i] = new Pawn(Player.Negru);
                this[6, i] = new Pawn(Player.Alb);
            }
        }

        public static bool IsInside(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Column >= 0 && pos.Column < 8;
        }

        public bool IsEmpty(Position pos)
        {
            return this[pos] == null;
        }

        public IEnumerable<Position> PiecePositions()
        {
            for(int r=0;r<8;r++)
            {
                for(int c=0;c<8;c++)
                {
                    Position pos = new Position(r, c);
                    if(!IsEmpty(pos))
                    {
                        yield return pos;
                    }
                }
            }
        }
        public IEnumerable<Position> PiecePositionsFor(Player player)
        {
            return PiecePositions().Where(pos => this[pos].Color == player);
        }

        public bool IsInChech(Player player)
        {
            return PiecePositionsFor(player.Opponent()).Any(pos =>
            {
                Piece piece = this[pos];
                return piece.CanCaptureOpponentKing(pos, this);
            });
        }

        public Board Copy()
        {
            Board copy = new Board();

            foreach(Position pos in PiecePositions())
            {
                copy[pos] = this[pos].Copy();
            }

            return copy;
        }

        public Counting CountPieces()
        {
            Counting counting = new Counting();

            foreach(Position pos in PiecePositions())
            {
                Piece piece = this[pos];
                counting.Increment(piece.Color, piece.Type);
            }

            return counting;
        }

        public bool InsufficientMaterial()
        {
            Counting counting = CountPieces();

            return IsKingBishopVKing(counting) || IsKingBishopVKingBishop(counting)
                || IsKingKnightVKing(counting) || IsKingVKing(counting);
        }

        private static bool IsKingVKing(Counting counting)
        {
            return counting.TotalCount == 2;
        }

        private static bool IsKingBishopVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Bishop) == 1 || counting.Black(PieceType.Bishop) == 1);
        }

        private static bool IsKingKnightVKing(Counting counting)
        {
            return counting.TotalCount == 3 && (counting.White(PieceType.Knight) == 1 || counting.Black(PieceType.Knight) == 1);
        }

        private bool IsKingBishopVKingBishop(Counting counting)
        {
            if(counting.TotalCount!=4)
            {
                return false;
            }

            if (counting.White(PieceType.Bishop) != 1 || counting.Black(PieceType.Bishop) != 1) 
            {
                return false;
            }

            Position wBishopPos = FindPiece(Player.Alb, PieceType.Bishop);
            Position bBishopPos = FindPiece(Player.Negru, PieceType.Bishop);

            return wBishopPos.SquareColor() == bBishopPos.SquareColor();
        }

        private Position FindPiece(Player color, PieceType type)
        {
            return PiecePositionsFor(color).First(pos => this[pos].Type == type);
        }
    }
}
