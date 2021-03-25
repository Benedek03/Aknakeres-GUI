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
        class Field
        {
            public PictureBox pb;
            public (int, int) position;

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
                Thread.Sleep(10);
            }
            void open(object sender, EventArgs e)
            {
                pb.BackColor = Color.Green;
                MessageBox.Show($"{position.Item1} {position.Item2}");
            }
        }
        Field[,] ketd;
        void f(int s, int o)
        {
            this.Size = new Size(o * 50 + 16, s * 50 + 39);
            ketd = new Field[s, o];
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < o; j++)
                {
                    Field item = ketd[i, j];
                    item = new Field(this,i,j);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Dispose();
            f(7, 10);
        }
    }
}
