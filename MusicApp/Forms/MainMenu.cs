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
