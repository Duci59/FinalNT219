using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.MaHoa
{
    static class MD5Helper
    {
        static string matkhau = "1h87h8712j";
        public static string MaHoa(this string duLieuCanMaHoa)
        {
            byte[] input = Encoding.UTF8.GetBytes(duLieuCanMaHoa);
            byte[] output = bMaHoa(input);
            return Convert.ToBase64String(output, 0, output.Length);
        }

        public static string GiaiMa(this string duLieuCanGiaiMa)
        {
            byte[] input = Convert.FromBase64String(duLieuCanGiaiMa);
            byte[] output = bGiaiMa(input);
            return Encoding.UTF8.GetString(output);
        }
        public static string MaHoaMotChieu(this string duLieuCanMaHoa)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(duLieuCanMaHoa);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2")); // Chuyển byte thành chuỗi hex
                }

                return sb.ToString();
            }
        }

        static byte[] bMaHoa(byte[] duLieuCanMaHoa)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
                {
                    des.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(matkhau));
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform tran = des.CreateEncryptor())
                    {
                        byte[] output = tran.TransformFinalBlock(duLieuCanMaHoa, 0, duLieuCanMaHoa.Length);
                        return output;
                    }
                }
            }
        }

        static byte[] bGiaiMa(byte[] duLieuCanGiaiMa)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
                {
                    des.Key = md5.ComputeHash(Encoding.UTF8.GetBytes(matkhau));
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform tran = des.CreateDecryptor())
                    {

                        byte[] output = tran.TransformFinalBlock(duLieuCanGiaiMa, 0, duLieuCanGiaiMa.Length);
                        return output;
                    }
                }
            }
        }
    }
}
