using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe
{
    public enum Player
    {
        X, O
    }

    public partial class Form1 : Form
    {
        string[] tempboard = { "O", "", "X", "X", "", "X", "", "O", "O" };
        string[] tempEmptyBoard = { "", "", "", "", "", "", "", "", "" };
        private List<Button> board;
        private List<Button> availButtons;
        private List<int> Xindices;
        private List<int> Oindices;
        private List<List<int>> winningCombos = new List<List<int>> {
            new List<int> { 0, 4, 8 },
            new List<int> { 6, 4, 2 },
            new List<int> { 0, 1, 2 },
            new List<int> { 3, 4, 5 },
            new List<int> { 6, 7, 8 },
            new List<int> { 0, 3, 6 },
            new List<int> { 1, 4, 7 },
            new List<int> { 2, 5, 8 },
        };
        private Player currentPlayer;
        private int playerWin = 0;
        private int compWin = 0;

        public Form1()
        {
            InitializeComponent();
            board = new List<Button> {button1, button2, button3, button4,
                button5, button6, button7, button8, button9};
            Xindices = new List<int>();
            Oindices = new List<int>();
            showText();
            findAvailButtons();
        }

        //Will be removed when game is working correctly
        private void showText()
        {
            for (int i = 0; i < 9; i++)
            {
                board[i].Text = tempboard[i];
                if (tempboard[i] != "")
                {
                    board[i].Enabled = false;
                }
                if (tempboard[i] == "X")
                {
                    Xindices.Add(i);
                }
                if (tempboard[i] == "O")
                {
                    Oindices.Add(i);
                }
            }
        }

        private bool isWinningCombo(List<int> indices)
        {
            foreach (List<int> combo in winningCombos)
            {
                if (!combo.Except(indices).Any())
                {
                    return true;
                }
            }
            return false;
        }


        private void updateIndices(List<int> indices, string symbol)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[i].Text == symbol && !indices.Contains(i))
                {
                    indices.Add(i);
                }
            }
        }

        private void findAvailButtons()
        {
            var avail = from button in board
                        where button.Text == ""
                        select button;
            availButtons = avail.ToList();
        }

        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            currentPlayer = Player.O;
            button.Text = currentPlayer.ToString();
            button.Enabled = false;
            availButtons.Remove(button);
            updateIndices(Oindices, "O");
            if (isWinningCombo(Oindices))
            {
                MessageBox.Show("Win");
                playerWin++;
                label1.Text = "Player Wins - " + playerWin.ToString();
                clearButtons();
                button10.Text = "Restart Game";
            }
            else
            {
                currentPlayer = Player.X;
                int bestMove = miniMax(board, currentPlayer, 0);
            }

        }

        private void clearButtons()
        {
            Xindices.Clear();
            Oindices.Clear();
            foreach (Button b in board)
            {
                b.Text = "";
                b.Enabled = true;
            }
            tempboard = tempEmptyBoard;
        }

        private void restart_Click(object sender, EventArgs e)
        {
            button10.Text = "Restart Game";
            clearButtons();
            showText();
        }


        //Returns the index of the best move computed
        private int miniMax(List<Button> newBoard, Player player, int score)
        {
            if (isWinningCombo(Oindices)) score = 10;
            else if (isWinningCombo(Xindices)) score = 10;
            else if (availButtons.Count == 0) score = 0;
            var moves = new List<int>();
            for (int avail = 0; avail < availButtons.Count; avail++)
            {

            }

            return 0;
        }
    }
}
