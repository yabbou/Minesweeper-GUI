using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Model;

namespace Minesweeper
{
    internal class ComboItem
    {
        public int ID { get; set; }
        public string Text { get; set; }
    }

    public partial class Minesweeper : Form
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public double Percent { get; set; }

        public MinesweeperModel Model { get; set; }

        private TableLayoutPanel board;
        private MenuStrip menuBar;
        private ComboBox dropdownMenu;
        private Button[,] buttons;

        public Minesweeper(int rowCount = 10, int colCount = 8, double percent = 0.10)
        {
            RowCount = rowCount;
            ColCount = colCount;
            Percent = percent;

            Model = new MinesweeperModel(rowCount, colCount, percent);
            OverriddenInitializeComponent();
        }

        private void OverriddenInitializeComponent()
        {
            board = new TableLayoutPanel();
            buttons = new Button[RowCount, ColCount];

            menuBar = new MenuStrip();
            dropdownMenu = new ComboBox();

            InitMenuBar();
            InitButtons();

            this.board.SuspendLayout();
            this.SuspendLayout();

            this.board.ColumnCount = ColCount;
            for (int c = 0; c < ColCount; c++)
            {
                this.board.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / ColCount));
            }

            SetUpButtons();
            SetUpMenuBar();

            this.board.Dock = DockStyle.Fill;
            this.board.RowCount = RowCount;
            for (int r = 0; r < RowCount; r++)
            {
                this.board.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / RowCount));
            }

            SetUpBoard();

            this.board.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void InitMenuBar()
        {

        }

        private void InitButtons()
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    buttons[row, col] = new Button();
                }
            }
        }

        private void SetUpButtons()
        {
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    var b = buttons[row, col];
                    this.board.Controls.Add(b, col, row);

                    b.Dock = DockStyle.Fill;
                    b.TabIndex = row * ColCount + col;
                    b.Tag = new Point(col, row);
                    b.BackColor = SetShadeOfGreen((Point)b.Tag);

                    b.MouseClick += Mouse_Left_Click;
                    b.MouseHover += Mouse_Hover;
                }
            }
        }

        private void SetUpMenuBar()
        {
            dropdownMenu.DataSource = new ComboItem[] {
                new ComboItem{ ID = 1, Text = "Easy" },
                new ComboItem{ ID = 2, Text = "Medium" },
                new ComboItem{ ID = 3, Text = "Hard" }
            };
        }
        private void SetUpBoard()
        {
            int x = 1021;
            int y = 843;

            this.board.Size = new System.Drawing.Size(x, y);
            this.board.TabIndex = 0;
            this.board.BackColor = Color.Bisque;

            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(x, y);

            this.Controls.Add(this.menuBar);
            this.Controls.Add(this.board);
        }

        #region button clicks 
        private void Mouse_Left_Click(object sender, EventArgs e)
        {
            var b = sender as System.Windows.Forms.Button;
            CheckSenderIsButton(sender, b);

            CheckButtonForBombs(b);
            CheckGameStatus();
        }

        private void CheckButtonForBombs(Button b)
        {
            Point location = (Point)b.Tag;
            int row = location.Y;
            int col = location.X;

            string totalSurroundingBombs = Model.MakeMove(MoveType.Dig, location);
            b.Text = totalSurroundingBombs.Equals("0") ? "" : totalSurroundingBombs;

            if (totalSurroundingBombs.Equals("0"))
            {
                for (int r = Math.Max(0, row - 1); r <= Math.Min(row + 1, RowCount - 1); r++)
                {
                    for (int c = Math.Max(0, col - 1); c <= Math.Min(col + 1, ColCount - 1); c++)
                    {
                        var cell = Model.GetBoardCell(r, c);

                        if (!cell.HasBomb && !cell.HasBeenDug)
                        {
                            var button = buttons[r, c];
                            button.Text = Model.MakeMove(MoveType.Dig, (Point)button.Tag);
                            EditButtonState(b);

                            CheckButtonForBombs(button);
                        }
                    }
                }
            }
            EditButtonState(b);
        }

        private void EditButtonState(Button b)
        {
            b.Enabled = b.Text.Equals("FLAG");
            SetButtonColor(b);
        }

        private static void SetButtonColor(Button b)
        {
            if (b.Text.Equals("BOMB!")) b.BackColor = Color.Red;
            else if (b.Text.Equals("FLAG")) b.BackColor = Color.Orange;
            else b.BackColor = Color.Beige;
        }

        private static Color SetShadeOfGreen(Point location)
        {
            return (location.X + location.Y) % 2 == 0 ? Color.GreenYellow : Color.LawnGreen;
        }

        private void Mouse_Hover(object sender, EventArgs e)
        {
            var b = sender as Button;
            CheckSenderIsButton(sender, b);

            Point location = (Point)b.Tag;
            b.Text = Model.MakeMove(MoveType.Flag, location);
            b.BackColor = b.Text.Equals("FLAG") ? Color.Orange : SetShadeOfGreen(location);

            CheckGameStatus();
        }

        private static void CheckSenderIsButton(object sender, Button b)
        {
            if (b == null)
            {
                throw new ArgumentException($"sender must be Button was [{sender.GetType()}]");
            }
        }



        #endregion

        #region new game 

        private void CheckGameStatus()
        {
            if (Model.IsGameOver())
            {
                CongratulateUser();
            }
        }

        public void CongratulateUser()
        {
            string gameStatus = Model.GameStatus;
            string offerNewGame = "Would you like to play again?";

            string title = "Congrats!";
            string message =
                gameStatus + Environment.NewLine + Environment.NewLine +
                offerNewGame;

            OfferToPlayAgain(title, message);
        }

        private void OfferToPlayAgain(string title, string message)
        {
            if (System.Windows.MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Model.ResetInternalVariables();
                ResetButtons();
            }
            else
            {
                this.Close();
            }
        }

        private void ResetButtons()
        {
            foreach (Control b in board.Controls)
            {
                b.Enabled = true;
                b.Text = "";
                b.BackColor = SetShadeOfGreen((Point)b.Tag);
            }
        }

        #endregion

    }
}
