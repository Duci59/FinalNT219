using MusicApp.env;
using MusicApp.MaHoa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp
{
    public partial class Login : Form
    {
        string username, password, usertype, displayName, email;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Forms.SignUp signUp = new Forms.SignUp();
            signUp.ShowDialog();
            if (signUp.checkDK == 0)
            {
                this.Show();
            }
        }

        public Login()
        {
            InitializeComponent();
        }

        private async void btSignIn_Click(object sender, EventArgs e)
        {
            if (tbUsername.Text.Trim() == "" || tbPass.Text.Trim() == "")
            {
                MessageBox.Show("Nhập đầy đủ thông tin tài khoản.", "Thông báo");
            }
            else
            {
                username = tbUsername.Text.Trim();
                password = tbPass.Text.Trim();
                string yeuCau = "DangNhap~" + username + "~" + password.MaHoa();
                string ketQua = await Task.Run(() => Result.Instance.Request(yeuCau));

                if (String.IsNullOrEmpty(ketQua))
                {
                    MessageBox.Show("Máy chủ không phản hồi");
                }
                else if (ketQua.Contains("success"))
                {
                    MessageBox.Show("OK");
                }
                else if (ketQua == "Password didn't match")
                {
                    MessageBox.Show("Mật khẩu không khớp");
                    tbPass.Focus();
                }
                else if (ketQua == "User doesn't exist")
                {
                    MessageBox.Show("Người dùng không tồn tại");
                    tbUsername.Focus();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
        }
    }
}
