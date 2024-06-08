using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace MusicApp
{
    public class CustomPanel : Panel
    {
        private PictureBox pictureBox;
        private PictureBox pictureBoxButton;
        private Label label1;
        private Label label2;
        private Label label3;

        public CustomPanel(string txt, string txt1, string t)
        {
            InitializeComponents(txt, txt1, t);
        }

        private void InitializeComponents(string t, string txt, string time)
        {
            // Initialize PictureBox
            pictureBox = new PictureBox
            {
                Image = Image.FromFile("C:\\Users\\vongu\\OneDrive\\Pictures\\Screenshots\\Screenshot 2024-06-07 020512.png"),
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(22, 6),
                Size = new Size(67, 67)
            };

            // Initialize PictureBoxButton as a Button
            pictureBoxButton = new PictureBox
            {
                Image = Properties.Resources.icons8_circled_play_50,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(104, 22),

                Size = new Size(40, 40),
                Cursor = Cursors.Hand // Change cursor to hand to indicate clickable
            };
            pictureBoxButton.Click += PictureBoxButton_Click;

            // Initialize Labels
            label1 = new Label
            {
                Text = t,
                Location = new Point(206, 28),
                Font = new Font("Constantia", 13, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Silver,
            };

            label2 = new Label
            {
                Text = txt,
                Location = new Point(428, 33),
                Font = new Font("Century Gothic", 10, FontStyle.Regular),
                AutoSize = true,
                ForeColor = Color.Silver
            };

            label3 = new Label
            {
                Text = time,
                Location = new Point(575, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White
            };

            // Add controls to the panel
            Controls.Add(pictureBox);
            Controls.Add(pictureBoxButton);
            Controls.Add(label1);
            Controls.Add(label2);
            Controls.Add(label3);

            // Set panel properties
            Size = new Size(629, 80);
        }

        private void PictureBoxButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("phat nhac");
        }
    }
}
