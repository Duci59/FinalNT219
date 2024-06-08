using FireSharp.Interfaces;
using FireSharp.Response;
using MusicApp.env;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp.Forms
{
    public partial class MainMenu : Form
    {
        string Username, Usertype;
        private readonly Service firebaseService;
        public MainMenu(string username, string usertype)
        {
            InitializeComponent();
            Username = username;
            Usertype = usertype;
            if (Usertype == "counterpart")
                btnUploadFiles.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
       
        }

        private void btnMaxsize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnMinsize_Click(object sender, EventArgs e)
        {
            this.WindowState |= FormWindowState.Minimized;
        }
        string[] activities = { "Lối Nhỏ", "Một triệu like", "Đi Về Nhà" };
        string[] auth = { "Đen Vâu", "Đen", "Vâu " };
        string[] time = { "3:32", "3:42", "4:00" };
        private void MainMenu_Load(object sender, EventArgs e)
        {
            int y = 0; // Đặt vị trí y ban đầu là 0
            for (int i = 0; i < 12; i++)
            {
                CustomPanel pn = new CustomPanel("Lối Nhỏ", "Đen Vâu", "3:32")
                {
                    Location = new Point(0, y), // Đặt vị trí của CustomPanel theo vị trí y hiện tại
                };
                y += pn.Height + 10; // Tăng vị trí y để các CustomPanel được đặt cách nhau 10 pixel dọc
                panel6.Controls.Add(pn);
            }
        }

        private async void btnPlayMusic_Click(object sender, EventArgs e)
        {
            IFirebaseClient client = firebaseService.GetFirebaseClient();

            // Kiểm tra username đã tồn tại hay chưa
            FirebaseResponse usernameResponse = await client.GetAsync("Songsd");

        }

        private void btnUploadFiles_Click(object sender, EventArgs e)
        {
            if (Usertype == "counterpart")
            {
                Forms.AddSong form = new Forms.AddSong(Username);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Tính năng chỉ dành cho đối tác", "Thông báo");
            }
        }
    }
}
