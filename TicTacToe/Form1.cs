using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe
{
    public enum Player
    {
        X, O
    }

    public class Move
    {
        public int index { get; set; }
        public int score { get; set; }
    }

    public partial class Form1 : Form
    {
        string[] tempboard = { "O", "X", "X", "X", "O", "O", "", "", ""};
        string[] tempEmptyBoard = { "", "", "", "", "", "", "", "", "" };
        private List<Button> board;
        private List<int> availIndices;
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
            availIndices = findAvailIndices(flattenBoard());
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

        private List<int> findAvailIndices(List<string> newBoard)
        {
            List<int> availIndices = new List<int>();
            for(int i = 0; i < 9; i++)
            {
                if(newBoard[i] == "")
                {
                    availIndices.Add(i);
                }
            }
            return availIndices;
        }

        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            currentPlayer = Player.O;
            button.Text = currentPlayer.ToString();
            button.Enabled = false;
            availIndices = findAvailIndices(flattenBoard());
            updateIndices(Oindices, "O");
            if (isWinningCombo(Oindices))
            {
                win(currentPlayer);
            }
            else
            {
                currentPlayer = Player.X;
                int bestMove = miniMax(flattenBoard(), currentPlayer).index;
                Xindices.Add(bestMove);
                board[bestMove].Text = currentPlayer.ToString();
                board[bestMove].Enabled = false;
                if (isWinningCombo(Xindices))
                {
                    win(currentPlayer);
                }
            }
            if(findAvailIndices(flattenBoard()).Count == 0 && !isWinningCombo(Oindices) && !isWinningCombo(Xindices))
            {
                MessageBox.Show("Tie!");
                clearButtons();
                button10.Text = "Restart Game";
            }

        }

        private void win(Player p)
        {
            MessageBox.Show("Win ");
            if (p == Player.O)
            {
                playerWin++;
                label1.Text = "Player Wins - " + playerWin.ToString();
            }
            else
            {
                compWin++;
                label2.Text = "Computer Wins - " + compWin.ToString();
            }
            clearButtons();
            button10.Text = "Restart Game";
        }


        private List<string> flattenBoard()
        {
            List<string> flatBoard = new List<string>();
            foreach(Button b in board)
            {
                flatBoard.Add(b.Text);
            }
            return flatBoard;
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
        private Move miniMax(List<string> newBoard, Player player)
        {
            var newAvailIndices = findAvailIndices(newBoard);
            Move m = new Move();
            if (isWinningCombo(Oindices))
            {
                m.score = -10;
                return m;
            }
            else if (isWinningCombo(Xindices))
            {
                m.score = 10;
                return m;
            }
            else if (newAvailIndices.Count == 0)
            {
                m.score = 0;
                return m;
            }

            var AllMoves = new List<Move>();
            for (int i = 0; i < newAvailIndices.Count; i++)
            {
                Move move = new Move();
                move.index = newAvailIndices[i]; 
                newBoard[newAvailIndices[i]] = player.ToString();
                if (player == Player.X)
                {
                    Xindices.Add(newAvailIndices[i]);
                    move.score = miniMax(newBoard, Player.O).score;
                    Xindices.Remove(newAvailIndices[i]);
                }
                else
                {
                    Oindices.Add(newAvailIndices[i]);
                    move.score = miniMax(newBoard, Player.X).score;
                    Oindices.Remove(newAvailIndices[i]);
                }
                newBoard[newAvailIndices[i]] = "";
                AllMoves.Add(move);

            }
            Move bestMove = new Move();
            if (player == Player.X)
            {
                bestMove.score = Int32.MinValue;
                for(int i = 0; i < AllMoves.Count; i++)
                {
                    if (AllMoves[i].score > bestMove.score)
                    {
                        bestMove.score = AllMoves[i].score;
                        bestMove.index = AllMoves[i].index;
                    }
                }
            }
            else
            {
                bestMove.score = Int32.MaxValue;
                for(int i = 0; i < AllMoves.Count; i++)
                {
                    if(AllMoves[i].score < bestMove.score)
                    {
                        bestMove.score = AllMoves[i].score;
                        bestMove.index = AllMoves[i].index;
                    }
                }
            }
            return bestMove;
        }
    }
}
