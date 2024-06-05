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

namespace MusicApp.Forms
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }
        public int checkDK = 0;
        private void HienLoi(string errormess, Control control)
        {
            errorlb.Text = errormess;
            errorlb.Visible = true;
            control.Focus();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

        private async void btSignUp_Click(object sender, EventArgs e)
        {
            string yeuCau = "CheckTK~" + tbTDN.Text + "~" + tbDK.Text;
            string ketQua = await Task.Run(() => Result.Instance.Request(yeuCau));
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
            else if (ketQua == "User or email already exit")
            {
                HienLoi("Địa chỉ email hoặc tên đăng nhập đã tồn tại", tbTDN);
            }
            else
            {
                errorlb.Visible = false;

                yeuCau = "DangKi~" + tbTDN.Text.Trim().MaHoa() + "~" + tbTHT.Text.Trim().MaHoa() + "~" + tbMK.Text.Trim().MaHoa() + "~" + tbDK.Text.Trim().MaHoa() + "~" + "Normal".MaHoa();
                ketQua = await Task.Run(()=>Result.Instance.Request(yeuCau));

                if (String.IsNullOrEmpty(ketQua))
                {
                    MessageBox.Show("Máy chủ không phản hồi");
                }
                else if (ketQua == "OK")
                {
                    DialogResult dialogResult = MessageBox.Show("Đăng ký thành công. Đăng nhập ngay?", "Thông báo", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        checkDK = 1;
                        this.Close();
                        Login login = new Login();
                        login.Show();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                else if (ketQua == "User or email already exit")
                {
                    MessageBox.Show("Người dùng hoặc email đã tồn tại");
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
        }
    }
}
