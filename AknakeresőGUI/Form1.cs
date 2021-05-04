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
            this.Icon = Icon.FromHandle(new Bitmap("img/bomb.png").GetHicon());
            this.Text = "Minesweeper";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            New();
        }
        public void New(){new Game(this);}
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
                this.pb.Location = new Point(this.position.row * this.game.pixellength, this.position.collumn * this.game.pixellength + this.game.pixellength);
                this.pb.Size = new Size(this.game.pixellength, this.game.pixellength);
                this.pb.SizeMode = PictureBoxSizeMode.Zoom;
                this.pb.Image = new Bitmap("img/notopened.png");
                this.pb.BorderStyle = BorderStyle.FixedSingle;
                this.pb.MouseClick += Click;
                form.Controls.Add(this.pb);
            }
            public void Click(object sender, MouseEventArgs e)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        if (!this.game.firstmove) this.Open();
                        else { this.game.firstmove = false; this.game.Putdownbombs(this.position); this.Open(); }
                        break;
                    case MouseButtons.Right:
                        this.Flag();
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
                    this.pb.Image = new Bitmap("img/bomb.png");
                    this.game.Over(false);
                    return;
                }
                else this.pb.Image = new Bitmap($"img/{this.numberofnearbybombs.ToString()}.png");
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
                        if (newpos.row > -1 && newpos.row < this.game.rows &&
                            newpos.collumn > -1 && newpos.collumn < this.game.collumns &&
                            !this.game.fields[newpos.row, newpos.collumn].isopened)
                        {
                            this.game.fields[newpos.row, newpos.collumn].Open();
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
                        this.pb.Image = new Bitmap("img/notopened.png");
                        this.game.numberofflags++;
                    }
                    else if (this.game.numberofflags > 0)
                    {
                        this.isflagged = true;
                        this.pb.Image = new Bitmap("img/flagged.png");
                        this.game.numberofflags--;
                    }
                    this.game.flagsdispaly.Text = this.game.numberofflags.ToString();
                }
            }
        }
        class Position
        {
            public int row;
            public int collumn;
            public Position(int r, int c)
            {
                this.row = r;
                this.collumn = c;
            }
            public static Position operator +(Position a, Position b) => new Position(a.row + b.row, a.collumn + b.collumn);
        }
        class Game
        {
            public Form1 form;
            public Field[,] fields;
            public int numberofflags;
            public int numberofbombs;
            public int rows;
            public int collumns;
            public int pixellength;
            public Label flagsdispaly;
            public Button newgamebutton;
            public bool firstmove;

            Label rowsLabel;
            Label collumnsLabel;
            Label bombsLabel;
            Label pixelsLabel;

            TextBox rowsTextBox;
            TextBox collumnsTextBox;
            TextBox bombsTextBox;
            TextBox pixelsTextBox;
            Button startbutton;

            Button beginner;
            void Beginner(object sender, EventArgs e)
            {
                rowsTextBox.Text = "10";
                collumnsTextBox.Text = "10";
                bombsTextBox.Text = "10";
                pixelsTextBox.Text = "50";
            }
            Button intermediate;
            void Intermediate(object sender, EventArgs e)
            {
                rowsTextBox.Text = "16";
                collumnsTextBox.Text = "16";
                bombsTextBox.Text = "26";
                pixelsTextBox.Text = "50";
            }

            public Game(Form1 f)
            {
                this.form = f;
                form.Size = new Size(320 + 16, 180 + 39);
                #region
                rowsLabel = new Label();
                collumnsLabel = new Label();
                bombsLabel = new Label();
                pixelsLabel = new Label();

                rowsLabel.Location = new Point(0, 0);
                collumnsLabel.Location = new Point(0, 30);
                bombsLabel.Location = new Point(0, 60);
                pixelsLabel.Location = new Point(0, 90);

                rowsLabel.Text = "rows";
                collumnsLabel.Text = "collumns";
                bombsLabel.Text = "bombs";
                pixelsLabel.Text = "pixels";

                rowsLabel.Font = new Font("Microsoft Sans Serif", 15F);
                collumnsLabel.Font = new Font("Microsoft Sans Serif", 15F);
                bombsLabel.Font = new Font("Microsoft Sans Serif", 15F);
                pixelsLabel.Font = new Font("Microsoft Sans Serif", 15F);

                rowsTextBox = new TextBox();
                collumnsTextBox = new TextBox();
                bombsTextBox = new TextBox();
                pixelsTextBox = new TextBox();
                startbutton = new Button();

                form.Controls.Add(rowsTextBox);
                form.Controls.Add(collumnsTextBox);
                form.Controls.Add(bombsTextBox);
                form.Controls.Add(pixelsTextBox);
                form.Controls.Add(startbutton);

                form.Controls.Add(rowsLabel);
                form.Controls.Add(collumnsLabel);
                form.Controls.Add(bombsLabel);
                form.Controls.Add(pixelsLabel);

                rowsTextBox.Location = new Point(100, 0);
                collumnsTextBox.Location = new Point(100, 30);
                bombsTextBox.Location = new Point(100, 60);
                pixelsTextBox.Location = new Point(100, 90);

                rowsTextBox.Size = new Size(100, 30);
                collumnsTextBox.Size = new Size(100, 30);
                bombsTextBox.Size = new Size(100, 30);
                pixelsTextBox.Size = new Size(100, 30);

                rowsTextBox.Font = new Font("Microsoft Sans Serif", 15F);
                collumnsTextBox.Font = new Font("Microsoft Sans Serif", 15F);
                bombsTextBox.Font = new Font("Microsoft Sans Serif", 15F);
                pixelsTextBox.Font = new Font("Microsoft Sans Serif", 15F);

                startbutton.Location = new Point(200, 0);
                startbutton.Size = new Size(120, 120);
                startbutton.Text = "Start";
                startbutton.Click += Start;
                #endregion
                beginner = new Button();
                intermediate = new Button();

                beginner.Click += Beginner;
                intermediate.Click += Intermediate;

                beginner.Text = "Beginner";
                intermediate.Text = "Intermediate";

                beginner.Location = new Point(0,120);
                intermediate.Location = new Point(0,150);

                beginner.Size = new Size(320, 30);
                intermediate.Size = new Size(320, 30);

                form.Controls.Add(beginner);
                form.Controls.Add(intermediate);
            }

            void Start(object sender, EventArgs e)
            {
                int r, c, b, p;
                if (int.TryParse(rowsTextBox.Text, out r) && !(r < 4))
                    if (int.TryParse(collumnsTextBox.Text, out c) && !(c < 4))
                        if (int.TryParse(bombsTextBox.Text, out b) && !(b < 4))
                            if (int.TryParse(pixelsTextBox.Text, out p) && !(p < 4))
                            {
                                rowsTextBox = null;
                                collumnsTextBox = null;
                                bombsTextBox = null;
                                pixelsTextBox = null;
                                startbutton = null;
                                beginner = null;
                                intermediate = null;
                                form.Controls.Clear();
                                Start(r, c, b, p);
                            }
                            else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
                        else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
                    else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
                else MessageBox.Show("Rossz input! Minden inputnak számnak kell lennie és legalább négynek");
            }

            public void Start(int rows, int collumns, int numberofbombs, int pixellength)
            {
                this.rows = rows;
                this.collumns = collumns;
                this.fields = new Field[this.rows, this.collumns];
                this.numberofbombs = numberofbombs;
                this.numberofflags = this.numberofbombs;
                this.pixellength = pixellength;
                this.newgamebutton = new Button();
                this.flagsdispaly = new Label();
                this.firstmove = true;

                form.Size = new Size(this.collumns * this.pixellength + 16, this.rows * this.pixellength + 39 + this.pixellength);
                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collumns; j++)
                        this.fields[i, j] = new Field(this, form, new Position(i, j));

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
            public void Newgamebuttonclick(object sender, EventArgs e) 
            {
                fields = null;
                flagsdispaly = null;
                newgamebutton = null;
                form.Controls.Clear();
                form.New();
            }
            public void Over(bool won)
            {
                if (won) MessageBox.Show("nyertél");
                else
                {
                    for (int i = 0; i < this.rows; i++)
                        for (int j = 0; j < this.collumns; j++)
                            if (this.fields[i, j].isbomb) this.fields[i, j].pb.Image = new Bitmap("img/bomb.png");
                    MessageBox.Show("vesztettél");
                }

                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collumns; j++)
                        this.fields[i, j].Removeclick();
            }
            public void Check()
            {
                int sum = 0;
                for (int i = 0; i < this.rows; i++)
                    for (int j = 0; j < this.collumns; j++)
                        if (!this.fields[i, j].isopened) sum++;
                if (sum == this.numberofbombs) Over(true);
            }
            public void Putdownbombs(Position nothere)
            {
                Random random = new Random();
                int c = 0;
                while (c != numberofbombs)
                {
                    Position newpos = new Position(random.Next(rows), random.Next(collumns));
                    if (!this.fields[newpos.row, newpos.collumn].isbomb)
                    {
                        if (nothere.row != newpos.row && nothere.collumn!= newpos.collumn)
                        {
                            this.fields[newpos.row, newpos.collumn].isbomb = true;
                            c++;
                        }
                    }
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
                    for (int j = 0; j < this.collumns; j++)
                        for (int x = 0; x < 8; x++)
                        {
                            Position newpos = new Position(i + a[x].row, j + a[x].collumn);
                            if (newpos.row > -1 && newpos.row < this.rows &&
                                newpos.collumn > -1 && newpos.collumn < this.collumns &&
                                this.fields[newpos.row, newpos.collumn].isbomb)
                            {
                                this.fields[i, j].numberofnearbybombs++;
                            }
                        }
            }
        }
    }
}
