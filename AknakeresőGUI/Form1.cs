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
            public Field[,] fields;
            public int numberofflags;
            public int rows;
            public int collums;
            public int plength;
            public PictureBox newgamebutton;

            public Game(Form1 form, int rows, int collums, int numberofbombs, int length)
            {
                newgamebutton = new PictureBox();
                newgamebutton.Location = new Point(0, 0);
                newgamebutton.Size = new Size(length * 3, length);
                newgamebutton.SizeMode = PictureBoxSizeMode.Zoom;
                newgamebutton.Image = new Bitmap("bomb.png");
                newgamebutton.Click += Newgamebuttonclick;
                form.Controls.Add(newgamebutton);
                
                plength = length;
                this.rows = rows;
                this.collums = collums;

                form.Size = new Size(collums * length + 16, rows * length + 39 + length);
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
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {      
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



                /*debug
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
                }*/
            }
            public void Newgamebuttonclick(object sender, EventArgs e)
            {
                Application.Restart();
            }
            public void Over(bool won)
            {
                if (won)
                {
                    MessageBox.Show("nyert");
                }
                else
                {
                    MessageBox.Show("vesztett");
                }
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        fields[i, j].Removeclick();
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
                pb.Size = new Size(game.plength, game.plength);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Image = new Bitmap("notopened.png");
                pb.Location = new Point(j * game.plength, i * game.plength+ +game.plength);
                pb.BorderStyle = BorderStyle.FixedSingle;
                pb.MouseClick += Click;
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
            public void Removeclick()
            {
                pb.MouseClick -= Click;
            }
            void Open()
            {
                //MessageBox.Show($"sor: {position.Item1}\toszlop{position.Item2}");
                this.isopened = true;
                Removeclick();
                if (this.isbomb)
                { 
                    pb.Image = new Bitmap("bomb.png");
                    game.Over(false);
                    return;
                }
                else
                {
                    pb.Image = new Bitmap($"{numberofnearbybombs.ToString()}.png");
                }
                if (this.numberofnearbybombs == 0)
                {
                    List<(int, int)> a = new List<(int, int)>{
                            (-1, 0 ),
                            ( 0,+1 ),
                            (+1, 0 ),
                            ( 0,-1 )
                            };
                    for (int x = 0; x < 4; x++)
                    {
                        (int, int) newpos = (position.Item1 + a[x].Item1, position.Item2 + a[x].Item2);
                        if (newpos.Item1 > -1 && newpos.Item1 < game.rows &&
                            newpos.Item2 > -1 && newpos.Item2 < game.collums &&
                            !game.fields[newpos.Item1, newpos.Item2].isopened)
                        {
                            game.fields[newpos.Item1, newpos.Item2].Open();
                        }
                    }
                }
            }
            void Flag()
            {
               
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Dispose();
            new Game(this, 6, 13, 10, 70);
        }
    }
}
