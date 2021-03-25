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
            public int numberofflags;
            
            public Game(Form1 form, int rows, int collums, int numberofbombs)
            {
                form.Size = new Size(collums * 50 + 16, rows * 50 + 39);
                fields = new Field[rows, collums];
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        fields[i, j] = new Field(this, form, i, j);
                    }
                }

                Random r = new Random();
                List<(int, int)> bombpositions = new List<(int, int)>();
                for (int i = 0; i < numberofbombs; i++)
                {
                    int r1 = r.Next(rows);
                    int r2 = r.Next(collums);
                    if (!bombpositions.Any(x => x.Equals((r1, r2))))
                    {
                        bombpositions.Add((r1,r2));
                        fields[r1, r2].Isbomb = true;
                    }
                    else
                    {
                        i -= 1;
                    }
                }

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        List<(int, int)> a = new List<(int, int)>{
                            (-1,-1 ),
                            (-1, 0 ),
                            (-1,+1 ),
                            ( 0,+1 ),
                            (+1,+1 ),
                            (+1, 0 ),
                            (+1,-1 ),
                            ( 0,-1 )
                            };
                        for (int x = 0; x < 8; x++)
                        {
                            (int, int) newpos = (i + a[x].Item1, j + a[x].Item2);
                            if (newpos.Item1 > -1 && newpos.Item1 < rows &&
                                newpos.Item2 > -1 && newpos.Item2 < collums &&
                                fields[newpos.Item1, newpos.Item2].Isbomb)
                            {
                                fields[i, j].Numberofnearbybombs++;
                            }
                        }
                    }
                }
                numberofflags = numberofbombs;



                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        if (fields[i,j].Isbomb == true)
                        {
                            fields[i, j].pb.Image = new Bitmap("bomb.png");
                        }
                        else
                        {
                            fields[i, j].pb.Image = new Bitmap($"{fields[i, j].Numberofnearbybombs.ToString()}.png");
                        }
                    }
                }
            }
        }
        class Field
        {
            public PictureBox pb;
            private (int, int) position;
            private bool isbomb;
            private bool isopened;
            private bool isflagged;
            private int numberofnearbybombs;
            private Game game;

            public bool Isbomb { get { return isbomb; } set { isbomb = value; } }
            public int Numberofnearbybombs { get { return numberofnearbybombs; } set { numberofnearbybombs = value; } }

            public Field(Game g, Form1 form, int i,int j)
            {
                isflagged = false;
                isopened = false;
                game = g;
                position = (i, j);
                pb = new PictureBox();
                pb.Size = new Size(50, 50);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Image = new Bitmap("notopened.png");
                pb.Location = new Point(j * 50, i * 50);
                pb.BorderStyle = BorderStyle.FixedSingle;
                pb.MouseClick += new MouseEventHandler(Click);
                form.Controls.Add(pb);
            }
            void Click(object sender, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Open();
                        break;
                    case MouseButtons.Right:
                        Flag();
                        break;
                }
            }
            void Open()
            {
                MessageBox.Show($"sor: {position.Item1}\toszlop{position.Item2}");
            }
            void Flag()
            {
               
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Dispose();
            new Game(this, 7, 10 ,10);
        }
    }
}
