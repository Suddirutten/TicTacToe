using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TicTacToe
{
    public enum Player
    {
        X,O
    }

    public partial class Form1 : Form
    {
        string[] board = { "O", "", "X", "X", "", "X", "", "O", "O" };
        private List<Button> buttons;
        private int[][] winningCombos = new int[8][] {
            new int[] { 1, 5, 9 },
            new int[] { 7, 5, 3 },
            new int[] { 1, 2, 3 },
            new int[] { 4, 5, 6 },
            new int[] { 7, 8, 9 },
            new int[] { 1, 4, 7 },
            new int[] { 2, 5, 8 },
            new int[] { 3, 6, 9 },
        };
        private Player currentPlayer;
      
        public Form1()
        {
            InitializeComponent();
            buttons = new List<Button> {button1, button2, button3, button4,
                button5, button6, button7, button8, button9};
            showText();
        }

        private void showText()
        {
            for (int i = 0; i < 9; i++) buttons[i].Text = board[i];
        }

        private bool isWinningCombo(char ch)
        {
            int[] charIndices = new int[5];
            int j = 0;
            for(int i = 0; i < 9; i++)
            {
                if (board[i] == ch.ToString()) charIndices[j] = i; j++;
            }
            if (winningCombos.Contains(charIndices)) return true;
            return false;
        }

        private void findAvailBoxes()
        {
            var availIndices = from item in board
                           where item == ""
                           select item;
        }

        private void playerClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            currentPlayer = Player.X;
            button.Text = currentPlayer.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}
