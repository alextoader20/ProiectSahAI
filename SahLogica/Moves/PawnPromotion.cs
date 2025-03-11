namespace SahLogica
{
    public class PawnPromotion : Move
    {
        public override MoveType Type => MoveType.PawnPromotion;
        public override Position FromPos { get; }
        public override Position ToPos { get; }

        private readonly PieceType newType;
        public PawnPromotion(Position from, Position to, PieceType newType)
        {
            FromPos = from;
            ToPos = to;
            this.newType = newType;
        }

        private Piece CreatePromotionPiece(Player color)
        {
            Piece promotionPiece;
            switch (newType)
            {
                case PieceType.Knight:
                    promotionPiece = new Knight(color);
                    break;
                case PieceType.Bishop:
                    promotionPiece = new Bishop(color);
                    break;
                case PieceType.Rook:
                    promotionPiece = new Rook(color);
                    break;
                default:
                    promotionPiece = new Queen(color);
                    break;
            }
            return promotionPiece;
        }

        public override bool Execute(Board board)
        {
            Piece pawn = board[FromPos];
            board[FromPos] = null;

            Piece promotionPiece = CreatePromotionPiece(pawn.Color);
            promotionPiece.HasMoved = true;
            board[ToPos] = promotionPiece;

            return true;
        }
    }
}
