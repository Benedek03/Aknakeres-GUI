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
            public int pixellength;
            public Label flagsdispaly;
            public Button newgamebutton;

            public Game(Form1 form, int rows, int collums, int numberofbombs, int pixellength)
            {
                this.rows = rows;
                this.collums = collums;
                this.fields = new Field[this.rows, this.collums];
                this.numberofbombs = numberofbombs;
                this.numberofflags = this.numberofbombs;
                this.pixellength = pixellength;
                this.newgamebutton = new Button();
                this.flagsdispaly = new Label();

                form.Size = new Size(this.collums * this.pixellength + 16, this.rows * this.pixellength + 39 + this.pixellength);
                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collums; j++)
                        this.fields[i, j] = new Field(this, form, new Position(i, j));

                Random random = new Random();
                List<Position> bombpositions = new List<Position>();
                for (int i = 0; i < numberofbombs; i++)
                {
                    int r1 = random.Next(rows);
                    int r2 = random.Next(collums);
                    if (!bombpositions.Any(x => x.Equals(new Position(r1, r2))))
                    {
                        this.fields[r1, r2].isbomb = true;
                        bombpositions.Add(new Position(r1, r2));
                    }
                    else { i -= 1; }
                }
                
                List<Position> a = new List<Position>{
                            new Position(-1,-1 ),
                            new Position(-1, 0 ),
                            new Position(-1,+1 ),
                            new Position( 0,+1 ),
                            new Position(+1,+1 ),
                            new Position(+1, 0 ),
                            new Position(+1,-1 ),
                            new Position( 0,-1 )};
                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collums; j++)
                        for (int x = 0; x < 8; x++)
                        { 
                            Position newpos = new Position(i + a[x].Row, j + a[x].Collum);
                            if (newpos.Row > -1 && newpos.Row < this.rows &&
                                newpos.Collum > -1 && newpos.Collum < this.collums &&
                                this.fields[newpos.Row, newpos.Collum].isbomb)
                            {
                                this.fields[i, j].numberofnearbybombs++;
                            }
                        }

                this.newgamebutton.Location = new Point(0, 0);
                this.newgamebutton.Size = new Size(this.pixellength * 3, this.pixellength);
                this.newgamebutton.Text = "New Game";
                this.newgamebutton.Click += Newgamebuttonclick;
                form.Controls.Add(this.newgamebutton);

                this.flagsdispaly.Location = new Point(3 * this.pixellength, 0);
                this.flagsdispaly.Size = new Size(this.pixellength * 3, this.pixellength);
                this.flagsdispaly.Text = this.numberofflags.ToString();
                form.Controls.Add(this.flagsdispaly);
            }
            public void Newgamebuttonclick(object sender, EventArgs e) => Application.Restart();
            public void Over(bool won)
            {
                if (won) MessageBox.Show("nyertél");
                else
                {
                    for (int i = 0; i < this.rows; i++)
                        for (int j = 0; j < this.collums; j++)
                            if (this.fields[i, j].isbomb) this.fields[i, j].pb.Image = new Bitmap("bomb.png");
                    MessageBox.Show("vesztettél");
                }

                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collums; j++)
                        this.fields[i, j].Removeclick();
            }
            public void Check()
            {
                int sum = 0;
                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collums; j++)
                        if (!this.fields[i, j].isopened) sum++;
                if (sum == this.numberofbombs) Over(true);
            }
        }
        class Field
        {
            public PictureBox pb;
            public Position position;
            public bool isbomb;
            public bool isopened;
            public bool isflagged;
            public int numberofnearbybombs;
            public Game game;

            public Field(Game game, Form1 form, Position position)
            {
                this.game = game;
                this.position = position;

                this.isbomb = false;
                this.isopened = false;
                this.isflagged = false;
                this.numberofnearbybombs = 0;

                this.pb = new PictureBox();
                this.pb.Location = new Point(this.position.Row * this.game.pixellength, this.position.Collum * this.game.pixellength + this.game.pixellength);
                this.pb.Size = new Size(this.game.pixellength, this.game.pixellength);
                this.pb.SizeMode = PictureBoxSizeMode.Zoom;
                this.pb.Image = new Bitmap("notopened.png");
                this.pb.BorderStyle = BorderStyle.FixedSingle;
                this.pb.MouseClick += Click;
                form.Controls.Add(this.pb);
            }
            public void Click(object sender, MouseEventArgs e)
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
            public void Removeclick() => pb.MouseClick -= Click;
            public void Open()
            {
                if (this.isflagged) return;
                
                this.isopened = true;
                this.Removeclick();
                if (this.isbomb)
                {
                    this.pb.Image = new Bitmap("bomb.png");
                    this.game.Over(false);
                    return;
                }
                else this.pb.Image = new Bitmap($"{this.numberofnearbybombs.ToString()}.png");
                if (this.numberofnearbybombs == 0)
                {
                    List<Position> a = new List<Position>{
                            new Position(-1,-1 ),
                            new Position(-1, 0 ),
                            new Position(-1,+1 ),
                            new Position( 0,+1 ),
                            new Position(+1,+1 ),
                            new Position(+1, 0 ),
                            new Position(+1,-1 ),
                            new Position( 0,-1 )};
                    for (int x = 0; x < 8; x++)
                    {
                        Position newpos = this.position + a[x];
                        if (newpos.Row > -1 && newpos.Row < this.game.rows &&
                            newpos.Collum > -1 && newpos.Collum < this.game.collums &&
                            !this.game.fields[newpos.Row, newpos.Collum].isopened)
                        {
                            this.game.fields[newpos.Row, newpos.Collum].Open();
                        }
                    }
                }
                this.game.Check();
            }
            public void Flag()
            {
                if (!this.isopened)
                {
                    if (this.isflagged)
                    {
                        this.isflagged = false;
                        this.pb.Image = new Bitmap("notopened.png");
                        this.game.numberofflags++;
                    }
                    else if (this.game.numberofflags > 0)
                    {
                        this.isflagged = true;
                        this.pb.Image = new Bitmap("flagged.png");
                        this.game.numberofflags--;
                    }
                    this.game.flagsdispaly.Text = this.game.numberofflags.ToString();
                }
            }
        }
        class Position
        {
            public int Row;
            public int Collum;
            public Position(int r, int c)
            {
                this.Row = r;
                this.Collum = c;
            }
            public static Position operator +(Position a, Position b) => new Position(a.Row + b.Row, a.Collum + b.Collum);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int input1;
            int input2;
            int input3;
            if (int.TryParse(in1.Text, out input1) && !(input1 < 4))
                if (int.TryParse(in2.Text, out input2) && !(input2 < 4))
                    if (int.TryParse(in3.Text, out input3) && !(input3 < 4))
                    {
                        in1.Dispose();
                        in2.Dispose();
                        in3.Dispose();
                        button1.Dispose();
                        new Game(this, input1, input2, input3, 50);
                    }
                    else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
                else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
            else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
        }
    }
}
