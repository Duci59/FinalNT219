using FireSharp.Interfaces;
using FireSharp.Response;
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

namespace MusicApp.Forms
{
    public partial class Login : Form
    {
        private readonly Service firebaseService;
        string username, usertype;
        public Login()
        {
            InitializeComponent();
            firebaseService = new Service();
        }
        private void HienLoi(string errormess, Control control)
        {
            errorlb.Text = errormess;
            errorlb.Visible = true;
            control.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaxsize_Click(object sender, EventArgs e)
        {
        }

        private void btnMinsize_Click(object sender, EventArgs e)
        {
            this.WindowState |= FormWindowState.Minimized;
        }

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

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            username = tbUsername.Text.Trim();
            string password = tbPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                HienLoi("Nhập đầy đủ thông tin", tbUsername);
                return;
            }

            IFirebaseClient client = firebaseService.GetFirebaseClient();

            // Kiểm tra xem người dùng có tồn tại trong cơ sở dữ liệu không
            FirebaseResponse response = await client.GetAsync("Users/" + username.MaHoa());
            if (response.Body != "null")
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                User user = response.ResultAs<User>();

                // Kiểm tra mật khẩu
                if (user.password == password.MaHoaMotChieu())
                {
                    // Đăng nhập thành công
                    usertype = user.userType;

                    // Chuyển đến giao diện chính hoặc thực hiện các hành động khác tùy theo loại người dùng
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo");
                    this.Hide();
                    MainMenu mainMenu = new MainMenu(username.MaHoa(), usertype.MaHoa());
                    mainMenu.Show();
                }
                else
                {
                    HienLoi("Mật khẩu không đúng", tbPass);
                }
            }
            else
            {
                HienLoi("Tài khoản không tồn tại", tbUsername);
            }
        }
    }
}
