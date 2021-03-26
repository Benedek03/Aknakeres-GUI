namespace AknakeresőGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.in1 = new System.Windows.Forms.TextBox();
            this.in2 = new System.Windows.Forms.TextBox();
            this.in3 = new System.Windows.Forms.TextBox();
            this.in4 = new System.Windows.Forms.TextBox();
            this.pre1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 120);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // in1
            // 
            this.in1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.in1.Location = new System.Drawing.Point(0, 0);
            this.in1.Name = "in1";
            this.in1.Size = new System.Drawing.Size(100, 30);
            this.in1.TabIndex = 2;
            this.in1.Text = "sorok";
            // 
            // in2
            // 
            this.in2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.in2.Location = new System.Drawing.Point(0, 30);
            this.in2.Name = "in2";
            this.in2.Size = new System.Drawing.Size(100, 30);
            this.in2.TabIndex = 3;
            this.in2.Text = "oszlopok";
            // 
            // in3
            // 
            this.in3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.in3.Location = new System.Drawing.Point(0, 60);
            this.in3.Name = "in3";
            this.in3.Size = new System.Drawing.Size(100, 30);
            this.in3.TabIndex = 4;
            this.in3.Text = "bombák";
            // 
            // in4
            // 
            this.in4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.in4.Location = new System.Drawing.Point(0, 90);
            this.in4.Name = "in4";
            this.in4.Size = new System.Drawing.Size(100, 30);
            this.in4.TabIndex = 5;
            this.in4.Text = "pixelhoszz";
            // 
            // pre1
            // 
            this.pre1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pre1.Location = new System.Drawing.Point(0, 120);
            this.pre1.Name = "pre1";
            this.pre1.Size = new System.Drawing.Size(200, 30);
            this.pre1.TabIndex = 6;
            this.pre1.Text = "10sor 10oszlop 10bomba";
            this.pre1.UseVisualStyleBackColor = true;
            this.pre1.Click += new System.EventHandler(this.pre1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 150);
            this.Controls.Add(this.pre1);
            this.Controls.Add(this.in4);
            this.Controls.Add(this.in3);
            this.Controls.Add(this.in2);
            this.Controls.Add(this.in1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox in1;
        private System.Windows.Forms.TextBox in2;
        private System.Windows.Forms.TextBox in3;
        private System.Windows.Forms.TextBox in4;
        private System.Windows.Forms.Button pre1;
    }
}

