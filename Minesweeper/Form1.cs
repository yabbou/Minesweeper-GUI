using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{

    public class MinesweeperModel
    {
        public List<Cell> Move(MoveType dig)
        {
            throw new NotImplementedException();
        }
    }

    public class Cell
    {
        private Point location;
        // Status 
    }

    public partial class Form1 : Form
    {
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        public MinesweeperModel MinesweeperModel { get; set; }

        public Form1(int rowCount = 10, int colCount = 8)
        {
            RowCount = rowCount;
            ColCount = colCount;

            InitializeComponent2();
        }

        private System.Windows.Forms.Button[,] buttons;

        private void InitializeComponent2()
        {

            
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();

            this.buttons = new System.Windows.Forms.Button[RowCount, ColCount];
            for (int i = 0; i < RowCount; i++)
            for (int j = 0; j < ColCount; j++)
                buttons[i, j] = new System.Windows.Forms.Button();

            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = ColCount;
            for (int j = 0; j < ColCount; j++)
                this.tableLayoutPanel1.ColumnStyles.Add(
                    new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / ColCount));
            for (int i = 0; i < RowCount; i++)
            for (int j = 0; j < ColCount; j++)
            {
                var b = buttons[i, j];
                this.tableLayoutPanel1.Controls.Add(b, j, i);
                b.Dock = System.Windows.Forms.DockStyle.Fill;
                b.TabIndex = i * ColCount + j;
                b.UseVisualStyleBackColor = true;
                b.Tag = new Point(j, i);

                b.Click += buttonClick;


            }

            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = RowCount;
            for (int i = 0; i < RowCount; i++)
            {
                this.tableLayoutPanel1.RowStyles.Add(
                    new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F / RowCount));
            }

            this.tableLayoutPanel1.Size = new System.Drawing.Size(1021, 843);
            this.tableLayoutPanel1.TabIndex = 0;

            //// 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 843);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        private void buttonClick(object sender, EventArgs e)
        {
            var b = sender as Button;
            if (b == null) // not a Button
            {
                throw new ArgumentException($"sender must be Button was [{sender.GetType()}]");
            }
            Point p = (Point) b.Tag;
            Console.WriteLine( p);
            Object o = "bob";

            MinesweeperModel.Move(MoveType.Dig);


        }

    }

    internal enum MoveType
    {
        Dig
    }
}
