using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Server.env;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using Server.DAO;

namespace Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("May chu bat dau hoat dong ...");
            String serverIP = "127.0.0.1";
            int port = 8080;
            //Khởi tạo firestorehelper
            FireStoreHelper.SetEnviromentVariable();
            //Khởi tạo
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(serverIP), port);
            sk.Bind(ep);
            sk.Listen(100);
            while (true)
            {
                Socket skXL = sk.Accept();

                Byte[] duLieu = new byte[102400000];
                int demNhan = skXL.Receive(duLieu);
                String noidung = Encoding.UTF8.GetString(duLieu, 0, demNhan);
                if (noidung.StartsWith("DangNhap"))
                {
                    int check;
                    string username = noidung.Split('~')[1];
                    string password = MD5Helper.Instance.MaHoaMotChieu(MD5Helper.Instance.GiaiMa(noidung.Split('~')[2]));
                    check = await UserInter.Instance.login(username, password);
                    byte[] traLoi;
                    switch (check)
                    {
                        case 0:
                            traLoi = Encoding.UTF8.GetBytes("User doesn't exist");
                            break;
                        case 1:
                            Dictionary<string, object> userInfo = await UserInter.Instance.LoadInfo(username);
                            traLoi = Encoding.UTF8.GetBytes("success~" + userInfo["username"].ToString() + "~" + MD5Helper.Instance.MaHoa(userInfo["displayName"].ToString()) + "~" + MD5Helper.Instance.MaHoa(userInfo["email"].ToString()) + "~" + MD5Helper.Instance.MaHoa(userInfo["usertype"].ToString()));
                            break;
                        case -1:
                            traLoi = Encoding.UTF8.GetBytes("Password didn't match");
                            break;
                        default:
                            traLoi = Encoding.UTF8.GetBytes("An error have occurred");
                            break;
                    }
                    skXL.Send(traLoi);
                }
                else if (noidung.StartsWith("CheckTK"))
                {
                    bool checkuser, checkemail;
                    //Đăng ký: [DangKy] ~ username ~ Email 
                    string username = noidung.Split('~')[1];
                    string email = noidung.Split('~')[2];
                    checkuser = await UserInter.Instance.field_exist("users", "username", username);
                    checkemail = await UserInter.Instance.field_exist("users", "email", email);
                    if (checkuser)
                    {
                        byte[] traLoi = Encoding.UTF8.GetBytes("User exit");
                        skXL.Send(traLoi);
                    }
                    else if (checkemail)
                    {
                        byte[] traLoi = Encoding.UTF8.GetBytes("Email exit");
                        skXL.Send(traLoi);
                    }
                }
                else if (noidung.StartsWith("DangKi"))
                {
                    bool checkuser, checkemail;
                    //Đăng ký: [DangKy] ~ username ~ displayname ~ Pass ~ Email 
                    string username = MD5Helper.Instance.GiaiMa(noidung.Split('~')[1]);
                    string displayname = MD5Helper.Instance.GiaiMa(noidung.Split('~')[2]);
                    string password = MD5Helper.Instance.MaHoaMotChieu(MD5Helper.Instance.GiaiMa(noidung.Split('~')[3]));
                    string email = MD5Helper.Instance.GiaiMa(noidung.Split('~')[4]);
                    string usertype = MD5Helper.Instance.GiaiMa(noidung.Split('~')[5]);
                    checkuser = await UserInter.Instance.field_exist("users", "username", username);
                    checkemail = await UserInter.Instance.field_exist("users", "email", email);
                    if (checkuser || checkemail)
                    {
                        byte[] traLoi = Encoding.UTF8.GetBytes("User or email already exit");
                        skXL.Send(traLoi);
                    }
                    else
                    {
                        await UserInter.Instance.RegisterUserAsync(username, displayname, email, password, usertype);
                        byte[] traLoi = Encoding.UTF8.GetBytes("OK");
                        skXL.Send(traLoi);
                    }
                }

                skXL.Close();
                skXL.Dispose();
            }
        }
    }
}
