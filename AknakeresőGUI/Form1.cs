using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace AknakeresőGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        class Game
        {
            private Field[,] fields;
            
            public Game(Form1 form, int rows,int collums)
            {
                form.Size = new Size(collums * 50 + 16, rows * 50 + 39);
                fields = new Field[rows, collums];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        fields[i, j] = new Field(form, i, j);
                    }
                }
            }
        }
        class Field
        {
            private PictureBox pb;
            private (int, int) position;
            private bool isbomb;
            private bool isopened;
            private bool isflagged;
            private int numberofnearbybombs;
            private Game game;

            public Field(Form1 form, int i,int j)
            {
                position = (i, j);
                pb = new PictureBox();
                pb.Size = new Size(50, 50);
                pb.BackColor = Color.Red;
                pb.Location = new Point(j * 50, i * 50);
                pb.BorderStyle = BorderStyle.FixedSingle;
                pb.Click += new EventHandler(open);
                form.Controls.Add(pb);
            }
            void open(object sender, EventArgs e)
            {
                pb.BackColor = Color.Green;
                MessageBox.Show($"sor: {position.Item1}\toszlop{position.Item2}");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Dispose();
            new Game(this, 7, 10);
        }
    }
}
