namespace Puzzle8
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            panel1 = new Panel();
            button2 = new Button();
            button3 = new Button();
            panel2 = new Panel();
            richTextBox1 = new RichTextBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            button5 = new Button();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            button4 = new Button();
            label2 = new Label();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(6, 22);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Importar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Location = new Point(821, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(529, 500);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // button2
            // 
            button2.Location = new Point(6, 22);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 2;
            button2.Text = "Resolver";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(87, 22);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 3;
            button3.Text = "Embaralha";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(richTextBox1);
            panel2.Location = new Point(22, 340);
            panel2.Name = "panel2";
            panel2.Size = new Size(368, 392);
            panel2.TabIndex = 4;
            panel2.Paint += panel2_Paint;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(362, 386);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 322);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 5;
            label1.Text = "Passo a Passo";
            label1.Click += label1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button5);
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(279, 67);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Imagem";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // button5
            // 
            button5.Location = new Point(168, 22);
            button5.Name = "button5";
            button5.Size = new Size(94, 23);
            button5.TabIndex = 4;
            button5.Text = "Editar Posição";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button2);
            groupBox2.Location = new Point(12, 100);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(173, 76);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Busca Gulosa";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button4);
            groupBox3.Location = new Point(12, 199);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(173, 83);
            groupBox3.TabIndex = 9;
            groupBox3.TabStop = false;
            groupBox3.Text = "Horizontal";
            groupBox3.Enter += groupBox3_Enter;
            // 
            // button4
            // 
            button4.Location = new Point(6, 22);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 0;
            button4.Text = "Resolver";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(420, 346);
            label2.Name = "label2";
            label2.Size = new Size(12, 15);
            label2.TabIndex = 10;
            label2.Text = "*";
            label2.Click += label2_Click_2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1362, 749);
            Controls.Add(label2);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            panel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Panel panel1;
        private Button button2;
        private Button button3;
        private Panel panel2;
        private Label label1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private Button button4;
        private Button button5;
        private RichTextBox richTextBox1;
        private Label label2;
    }
}