using System;
using System.Windows.Controls;
using System.Windows;
using SahLogica;

namespace SahUI
{
    /// <summary>
    /// Interaction logic for GameOverMenu.xaml
    /// </summary>
    public partial class GameOverMenu : UserControl
    {
        public event Action<Option> OptionSelected;

        public GameOverMenu(GameState gameState)
        {
            InitializeComponent();

            Result result = gameState.Result;
            WinnerText.Text = GetWinnerText(result.Winner);
            ReasonText.Text = GetReasonText(result.Reason, gameState.CurrentPlayer);
        }

        private static string GetWinnerText(Player winner)
        {
            switch (winner)
            {
                case Player.Alb:
                    return "WHITE WINS!";
                case Player.Negru:
                    return "BLACK WINS!";
                default:
                    return "IT'S A DRAW!";
            }
        }

        private static string PlayerString(Player player)
        {
            switch (player)
            {
                case Player.Alb:
                    return "WHITE";
                case Player.Negru:
                    return "BLACK";
                default:
                    return "";
            }
        }

        private static string GetReasonText(EndReason reason, Player currentPlayer)
        {
            switch (reason)
            {
                case EndReason.Stalemate:
                    return $"STALEMATE - {PlayerString(currentPlayer)} CAN'T MOVE";
                case EndReason.Checkmate:
                    return $"CHECKMATE - {PlayerString(currentPlayer)} CAN'T MOVE";
                case EndReason.FiftyMoveRule:
                    return "FIFTY MOVE RULE";
                case EndReason.InsufficientMaterial:
                    return "INSUFFICIENT MATERIAL";
                case EndReason.ThreefoldRepetition:
                    return "THREEFOLD REPETITION";
                default:
                    return "";
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Restart);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            OptionSelected?.Invoke(Option.Exit);
        }
    }
}