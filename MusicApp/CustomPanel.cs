using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Windows.Forms;

namespace MusicApp
{
    public class CustomPanel : Panel
    {
        public PictureBox pictureBox;
        public PictureBox pictureBoxButton;
        public Label label1;
        public Label label2;
        public Label label3;
        private string audiolink;
        public CustomPanel()
        {
            InitializeComponents("1","2","3",null,"3");
        }

        public CustomPanel(string txt, string txt1, string t, string img = null, string a = null)
        {
            InitializeComponents(txt, txt1, t, img, a);
        }

        public string GetAudio()
        {
            return audiolink;
        }

        private void InitializeComponents(string t, string txt, string time, string img, string audio)
        {
            this.audiolink = audio;
            if (img != null)
            {
                // Convert img: base64 -> byte[] -> ms -> bitmap
                byte[] b = Convert.FromBase64String(img);

                MemoryStream ms = new MemoryStream();
                ms.Write(b, 0, Convert.ToInt32(b.Length));

                Bitmap bm = new Bitmap(ms, false);
                ms.Dispose();

                // Initialize PictureBox
                pictureBox = new PictureBox
                {
                    Image = bm,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(22, 6),
                    Size = new Size(67, 67)
                };
            }


            // Initialize PictureBoxButton as a Button
            pictureBoxButton = new PictureBox
            {
                Image = Properties.Resources.icons8_circled_play_50,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(104, 22),

                Size = new Size(40, 40),
                Cursor = Cursors.Hand // Change cursor to hand to indicate clickable
            };

            // Initialize Labels
            label1 = new Label
            {
                Text = t,
                Location = new Point(150, 28),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.Silver,
            };

            label2 = new Label
            {
                Text = txt,
                Location = new Point(428, 33),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                ForeColor = Color.White
            };

            label3 = new Label
            {
                Text = time,
                Location = new Point(575, 33),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
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
        public string AudioLink => audiolink;
    }
}
