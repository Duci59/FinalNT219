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
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MusicApp.MaHoa;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace MusicApp.Forms
{
    public partial class SignUp : Form
    {
        private readonly Service _firebaseService;
        public SignUp()
        {
            InitializeComponent();
            _firebaseService = new Service();
        }
        public int checkDK = 0;

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinsize_Click(object sender, EventArgs e)
        {
            this.WindowState |= FormWindowState.Minimized;
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void HienLoi(string errormess, Control control)
        {
            errorlb.Text = errormess;
            errorlb.Visible = true;
            control.Focus();
        }

        private async void btnSignUp_Click(object sender, EventArgs e)
        {
            if (tbTDN.Text.Trim() == "" || tbTHT.Text.Trim() == "" || tbMK.Text.Trim() == "")
            {
                if (tbTDN.Text.Trim() == "")
                    HienLoi("Nhập đủ thông tin!", tbTDN);
                else if (tbTHT.Text.Trim() == "")
                    HienLoi("Nhập đủ thông tin!", tbTHT);
                else if (tbMK.Text.Trim() == "")
                    HienLoi("Nhập đủ thông tin!", tbMK);
                else
                    HienLoi("Nhập đủ thông tin!", null);
            }
            else if (tbTDN.Text.Contains("~") || tbTDN.Text.Contains("^") || tbTDN.Text.Contains(" "))
            {
                HienLoi("Tên đăng nhập không chứa các kí tự ^, ~, .", tbTDN);
                tbTDN.Text = "";
            }
            else if (tbMK.Text.Length < 6)
            {
                HienLoi("Mật khẩu phải nhiều hơn 6 kí tự.", tbMK);
            }
            else if (!tbMK.Text.Any(char.IsUpper) || !tbMK.Text.Any(c => !char.IsLetterOrDigit(c)))
            {
                HienLoi("Mật khẩu phải chứa ít nhất một ký tự in hoa và ký tự đặc biệt.", tbMK);
            }
            else if (tbMK.Text != tbNLMK.Text)
            {
                HienLoi("Mật khẩu không giống nhau", tbNLMK);
                tbNLMK.Text = "";
            }
            else if (!IsValidEmail(tbDK.Text.Trim()))
            {
                HienLoi("Địa chỉ Email không hợp lệ", tbDK);
            }
            // Trong phương thức btSignUp_Click
            else
            {
                IFirebaseClient client = _firebaseService.GetFirebaseClient();

                // Kiểm tra username đã tồn tại hay chưa
                FirebaseResponse usernameResponse = await client.GetAsync("Users");
                if (usernameResponse.Body.Contains(tbTDN.Text.MaHoa()))
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    tbTDN.Focus();
                    return;
                }

                // Kiểm tra email đã tồn tại hay chưa
                FirebaseResponse emailResponse = await client.GetAsync("Users");
                if (emailResponse.Body.Contains(tbDK.Text.MaHoa()))
                {
                    MessageBox.Show("Email đã được sử dụng!");
                    tbDK.Focus();
                    return;
                }

                var data = new User
                {
                    displayName = tbTHT.Text.MaHoa(),
                    password = tbMK.Text.MaHoaMotChieu(),
                    email = tbDK.Text.MaHoa(),
                    userType = "casual"
                };

                SetResponse response = await client.SetAsync("Users/" + tbTDN.Text.MaHoa(), data);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Đăng ký thành công!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại!");
                }
            }
        }
    }
}
