namespace MusicApp.Forms
{
    partial class SignUp
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
            this.tbDK = new System.Windows.Forms.TextBox();
            this.tbNLMK = new System.Windows.Forms.TextBox();
            this.tbMK = new System.Windows.Forms.TextBox();
            this.tbTHT = new System.Windows.Forms.TextBox();
            this.tbTDN = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btSignUp = new System.Windows.Forms.Button();
            this.errorlb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbDK
            // 
            this.tbDK.Location = new System.Drawing.Point(270, 316);
            this.tbDK.Name = "tbDK";
            this.tbDK.Size = new System.Drawing.Size(169, 22);
            this.tbDK.TabIndex = 19;
            // 
            // tbNLMK
            // 
            this.tbNLMK.Location = new System.Drawing.Point(270, 250);
            this.tbNLMK.Name = "tbNLMK";
            this.tbNLMK.PasswordChar = '●';
            this.tbNLMK.Size = new System.Drawing.Size(169, 22);
            this.tbNLMK.TabIndex = 18;
            // 
            // tbMK
            // 
            this.tbMK.Location = new System.Drawing.Point(270, 180);
            this.tbMK.Name = "tbMK";
            this.tbMK.PasswordChar = '●';
            this.tbMK.Size = new System.Drawing.Size(169, 22);
            this.tbMK.TabIndex = 17;
            // 
            // tbTHT
            // 
            this.tbTHT.Location = new System.Drawing.Point(270, 112);
            this.tbTHT.Name = "tbTHT";
            this.tbTHT.Size = new System.Drawing.Size(169, 22);
            this.tbTHT.TabIndex = 16;
            // 
            // tbTDN
            // 
            this.tbTDN.Location = new System.Drawing.Point(270, 43);
            this.tbTDN.Name = "tbTDN";
            this.tbTDN.Size = new System.Drawing.Size(169, 22);
            this.tbTDN.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(62, 312);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 25);
            this.label5.TabIndex = 13;
            this.label5.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(62, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(168, 25);
            this.label4.TabIndex = 12;
            this.label4.Text = "Nhập lại mật khẩu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(62, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 25);
            this.label3.TabIndex = 11;
            this.label3.Text = "Mật khẩu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(62, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tên hiển thị";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(62, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 35);
            this.label1.TabIndex = 14;
            this.label1.Text = "Tên đăng nhập";
            // 
            // btSignUp
            // 
            this.btSignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btSignUp.Location = new System.Drawing.Point(170, 380);
            this.btSignUp.Name = "btSignUp";
            this.btSignUp.Size = new System.Drawing.Size(142, 56);
            this.btSignUp.TabIndex = 20;
            this.btSignUp.Text = "Đăng ký";
            this.btSignUp.UseVisualStyleBackColor = true;
            this.btSignUp.Click += new System.EventHandler(this.btSignUp_Click);
            // 
            // errorlb
            // 
            this.errorlb.AutoSize = true;
            this.errorlb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.errorlb.ForeColor = System.Drawing.Color.Red;
            this.errorlb.Location = new System.Drawing.Point(67, 354);
            this.errorlb.Name = "errorlb";
            this.errorlb.Size = new System.Drawing.Size(77, 18);
            this.errorlb.TabIndex = 21;
            this.errorlb.Text = "errormess";
            this.errorlb.Visible = false;
            // 
            // SignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 481);
            this.Controls.Add(this.errorlb);
            this.Controls.Add(this.btSignUp);
            this.Controls.Add(this.tbDK);
            this.Controls.Add(this.tbNLMK);
            this.Controls.Add(this.tbMK);
            this.Controls.Add(this.tbTHT);
            this.Controls.Add(this.tbTDN);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SignUp";
            this.Text = "SignUp";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDK;
        private System.Windows.Forms.TextBox tbNLMK;
        private System.Windows.Forms.TextBox tbMK;
        private System.Windows.Forms.TextBox tbTHT;
        private System.Windows.Forms.TextBox tbTDN;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btSignUp;
        private System.Windows.Forms.Label errorlb;
    }
}