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
            public int numberofbombs;
            public int rows;
            public int collums;
            public int plength;
            public Label flagsdispaly;
            public Button newgamebutton;

            public Game(Form1 form, int rows, int collums, int numberofbombs, int length)
            {
                this.numberofbombs = numberofbombs;

                newgamebutton = new Button();
                newgamebutton.Text = "New Game";
                newgamebutton.Location = new Point(0, 0);
                newgamebutton.Size = new Size(length * 3, length);
                newgamebutton.Click += Newgamebuttonclick;
                form.Controls.Add(newgamebutton);

                flagsdispaly = new Label();
                flagsdispaly.Size = new Size(length * 3, length);
                flagsdispaly.Location = new Point(3 * length, 0);
                form.Controls.Add(flagsdispaly);

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
                        bombpositions.Add((r1, r2));
                        fields[r1, r2].isbomb = true;
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
                                fields[newpos.Item1, newpos.Item2].isbomb)
                            {
                                fields[i, j].numberofnearbybombs++;
                            }
                        }
                    }
                }
                numberofflags = numberofbombs;
                flagsdispaly.Text = numberofflags.ToString();

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
                    MessageBox.Show("nyertél");
                }
                else
                {
                    MessageBox.Show("vesztettél");
                }
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        fields[i, j].Removeclick();
                    }
                }
            }
            public void Check()
            {
                int sum = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < collums; j++)
                    {
                        if (!fields[i, j].isopened) sum++;
                    }
                }
                if (sum == numberofbombs) Over(true);
            }
        }
        class Field
        {
            public PictureBox pb;
            public (int, int) position;
            public bool isbomb;
            public bool isopened;
            public bool isflagged;
            public int numberofnearbybombs;
            public Game game;

            public Field(Game g, Form1 form, int i, int j)
            {
                isflagged = false;
                isopened = false;
                game = g;
                position = (i, j);
                pb = new PictureBox();
                pb.Size = new Size(game.plength, game.plength);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Image = new Bitmap("notopened.png");
                pb.Location = new Point(j * game.plength, i * game.plength + +game.plength);
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
                if (!isflagged)
                {
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
                    game.Check();
                }
            }
            void Flag()
            {
                if (!this.isopened)
                {
                    if (isflagged)
                    {
                        this.isflagged = false;
                        pb.Image = new Bitmap("notopened.png");
                        game.numberofflags++;
                    }
                    else if (game.numberofflags > 0)
                    {
                        this.isflagged = true;
                        pb.Image = new Bitmap("flagged.png");
                        game.numberofflags--;
                    }
                }
                game.flagsdispaly.Text = game.numberofflags.ToString();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            int input1;
            int input2;
            int input3;
            if (int.TryParse(in1.Text, out input1) && !(input1 < 4))
            {
                if (int.TryParse(in2.Text, out input2) && !(input2 < 4))
                {
                    if (int.TryParse(in3.Text, out input3) && !(input3 < 4))
                    {
                        in1.Dispose();
                        in2.Dispose();
                        in3.Dispose();
                        button1.Dispose();
                        new Game(this, input1, input2, input3, 50);
                    }
                    else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
                }
                else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
            }
            else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
        }
    }
}
