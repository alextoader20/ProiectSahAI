namespace SahLogica
{
    public enum Player
    {
        None, //aaaaaaa
        Alb,
        Negru // aadadasdasdaada
    }

    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            switch (player)
            {
                case Player.Alb:
                    return Player.Negru;
                case Player.Negru:
                    return Player.Alb;
                default:
                    return Player.None;
            }
        }
    }
}