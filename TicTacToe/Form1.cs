﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe
{
    //Enum for which player is playing
    public enum Player
    {
        X, O
    }

    //Class for a move, keeps track of index and score
    public class Move
    {
        public int index { get; set; }
        public int score { get; set; }
    }

    public partial class Form1 : Form
    {
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

        //Initiates the indices and the board
        public Form1()
        {
            InitializeComponent();
            board = new List<Button> {button1, button2, button3, button4,
                button5, button6, button7, button8, button9};
            Xindices = new List<int>();
            Oindices = new List<int>();
            availIndices = Enumerable.Range(0, 9).ToList(); 
        }

        //Method to check if there is any winning combo present in the indices
        //Returns true/false
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

        //Method to calculate the available indices for the minimax method
        private List<int> findAvailIndices(List<string> newBoard)
        {
            List<int> aIndices = new List<int>();
            for(int i = 0; i < 9; i++)
            {
                if(newBoard[i] == "")
                {
                    aIndices.Add(i);
                }
            }
            return aIndices;
        }

        //Method for when player has clicked button
        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            currentPlayer = Player.O;
            button.Text = currentPlayer.ToString();
            button.Enabled = false;
            Oindices.Add(board.IndexOf(button));
            if (isWinningCombo(Oindices))
            {
                win(currentPlayer);
            }
            else
            {
                currentPlayer = Player.X;
                int bestMove = miniMax(flattenBoard(), currentPlayer).index;
                if(bestMove > -1)
                {
                    Xindices.Add(bestMove);
                    board[bestMove].Text = currentPlayer.ToString();
                    board[bestMove].Enabled = false;
                    if (isWinningCombo(Xindices))
                    {
                        win(currentPlayer);
                    }
                }
                
            }
            availIndices = findAvailIndices(flattenBoard());
            if (availIndices.Count == 0 && !isWinningCombo(Oindices) && !isWinningCombo(Xindices))
            {
                MessageBox.Show("Tie!");
                clearButtons();
                button10.Text = "Restart Game";
            }

        }

        //Method for showing win-window and restarting the buttons
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

        //Method for flattening the board from button to string
        private List<string> flattenBoard()
        {
            List<string> flatBoard = new List<string>();
            foreach(Button b in board)
            {
                flatBoard.Add(b.Text);
            }
            return flatBoard;
        }

        //clears the buttons and the index lists
        private void clearButtons()
        {
            Xindices.Clear();
            Oindices.Clear();
            foreach (Button b in board)
            {
                b.Text = "";
                b.Enabled = true;
            }
        }

        //Clears the buttons if player wishes to restart game
        private void restart_Click(object sender, EventArgs e)
        {
            clearButtons();
        }


        //Returns the index of the best move computed using the minimax method
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
                m.index = -1;
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
