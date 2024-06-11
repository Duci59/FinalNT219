using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MusicApp.MaHoa
{
    static class MH
    {
        static string matkhau = "1h87h8712j";
        static byte[] encryptionKey;
        static byte[] encryptionIV;

        public static string MaHoa(this string duLieuCanMaHoa)
        {
            byte[] input = Encoding.UTF8.GetBytes(duLieuCanMaHoa);
            byte[] output = bMaHoa(input);
            return Convert.ToBase64String(output, 0, output.Length);
        }

        public static string GiaiMa(this String duLieuCanGiaiMa)
        {
            byte[] input = Convert.FromBase64String(duLieuCanGiaiMa);
            byte[] output = bGiaiMa(input);
            return Encoding.UTF8.GetString(output);
        }

        static byte[] bMaHoa(byte[] duLieuCanMaHoa)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GenerateKeyFromPassword(matkhau);
                aesAlg.IV = GenerateIVFromPassword(matkhau);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    return Transform(duLieuCanMaHoa, encryptor);
                }
            }
        }

        static byte[] bGiaiMa(byte[] duLieuCanGiaiMa)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = GenerateKeyFromPassword(matkhau);
                aesAlg.IV = GenerateIVFromPassword(matkhau);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    try
                    {
                        return Transform(duLieuCanGiaiMa, decryptor);
                    }
                    catch
                    {
                        return Encoding.UTF8.GetBytes("[Key bị thay đổi]-Để xem lại cần set về key cũ");
                    }
                }
            }
        }
        static byte[] Transform(byte[] data, ICryptoTransform transform)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                    cryptoStream.FlushFinalBlock();
                    return memoryStream.ToArray();
                }
            }
        }
        static byte[] GenerateKeyFromPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
        static byte[] GenerateIVFromPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                byte[] iv = new byte[16];
                Array.Copy(hash, iv, iv.Length);
                return iv;
            }
        }
        // ma hoa ne nhe m 
        public static void EncryptWavFile(string inputFile, string outputFile)
        {

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.FullName;

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(projectDirectory, "appsettings.json"), optional: false, reloadOnChange: true);

            // Tạo IConfiguration từ ConfigurationBuilder
            IConfiguration config = builder.Build();
            // Đọc các cấu hình từ "Firebase" section
            string key = config["Firebase:encryptionKey"];
            string IV = config["Firebase:encryptionIV"];
            encryptionKey = Convert.FromBase64String(key);
            encryptionIV = Convert.FromBase64String(IV);
            // Đọc dữ liệu từ file âm thanh đầu vào
            byte[] inputBytes = File.ReadAllBytes(inputFile);

            // Mã hóa dữ liệu âm thanh
            byte[] encryptedBytes = EncryptBytes(inputBytes, encryptionKey, encryptionIV);

            // Ghi dữ liệu đã mã hóa vào file đầu ra
            File.WriteAllBytes(outputFile, encryptedBytes);
        }
        private static byte[] EncryptBytes(byte[] inputBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        return memoryStream.ToArray();
                    }
                }
            }
        } 
        // con cai nay la giai ma 
        public static void DecryptWavFile(string inputFile, string outputFile)
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.FullName;

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(projectDirectory, "appsettings.json"), optional: false, reloadOnChange: true);

            // Tạo IConfiguration từ ConfigurationBuilder
            IConfiguration config = builder.Build();
            // Đọc các cấu hình từ "Firebase" section
            string key = config["Firebase:encryptionKey"];
            string IV = config["Firebase:encryptionIV"];
            encryptionKey = Convert.FromBase64String(key);
            encryptionIV = Convert.FromBase64String(IV);
            // Đọc dữ liệu từ file âm thanh đầu vào
            byte[] inputBytes = File.ReadAllBytes(inputFile);

            // Giải mã dữ liệu âm thanh
            byte[] decryptedBytes = DecryptBytes(inputBytes, encryptionKey, encryptionIV);

            // Ghi dữ liệu đã giải mã vào file đầu ra
            File.WriteAllBytes(outputFile, decryptedBytes);
        }

        private static byte[] DecryptBytes(byte[] inputBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream memoryStream = new MemoryStream(inputBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream decryptedStream = new MemoryStream())
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead;

                            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                decryptedStream.Write(buffer, 0, bytesRead);
                            }

                            return decryptedStream.ToArray();
                        }
                    }
                }
            }
        }

        public static string MaHoaMotChieu(this string duLieuCanMaHoa)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(duLieuCanMaHoa);
                byte[] hash = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
