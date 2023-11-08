namespace YoutubeDownloadBot
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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            button2 = new Button();
            label3 = new Label();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(166, 31);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(493, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(166, 69);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(493, 23);
            textBox2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(55, 34);
            label1.Name = "label1";
            label1.Size = new Size(105, 15);
            label1.TabIndex = 2;
            label1.Text = "API (Telegram Bot)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 72);
            label2.Name = "label2";
            label2.Size = new Size(137, 15);
            label2.TabIndex = 3;
            label2.Text = "Path to DIR (С:\\Videos...)";
            // 
            // button1
            // 
            button1.Location = new Point(665, 28);
            button1.Name = "button1";
            button1.Size = new Size(95, 23);
            button1.TabIndex = 4;
            button1.Text = "Применить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(665, 68);
            button2.Name = "button2";
            button2.Size = new Size(95, 23);
            button2.TabIndex = 5;
            button2.Text = "Применить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(276, 101);
            label3.Name = "label3";
            label3.Size = new Size(267, 12);
            label3.TabIndex = 7;
            label3.Text = "Для вступления изменений в силу перезагрузите приложение - ";
            // 
            // button3
            // 
            button3.Location = new Point(642, 107);
            button3.Name = "button3";
            button3.Size = new Size(118, 23);
            button3.TabIndex = 8;
            button3.Text = "Свернуть в трей";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button4.AutoSize = true;
            button4.Font = new Font("Segoe UI", 6.75F, FontStyle.Regular, GraphicsUnit.Point);
            button4.Location = new Point(540, 96);
            button4.Name = "button4";
            button4.Size = new Size(44, 22);
            button4.TabIndex = 9;
            button4.Text = "ReLoad";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(772, 142);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(label3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "YoutubeDownloadBot";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label label1;
        private Label label2;
        private Button button1;
        private Button button2;
        private Label label3;
        private Button button3;
        private Button button4;
    }
}