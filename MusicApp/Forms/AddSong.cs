using FireSharp.Interfaces;
using FireSharp.Response;
using MusicApp.env;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Globalization;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Configuration;
using MusicApp.MaHoa;

namespace MusicApp.Forms
{
    public partial class AddSong : Form
    {
        private readonly Service _firebaseService;
        private readonly StorageClient _storageClient;
        private string Username;
        public AddSong(string username)
        {
            InitializeComponent();
            _firebaseService = new Service();
            _storageClient = _firebaseService.GetStorageClient();
            Username = username;
        }

        //chuyển thành kí tự viết thường không dấu và không có khoảng cách
        private string ConvertToUnsignAndRemoveSpaces(string input)
        {
            return Regex.Replace(RemoveDiacritics(input.ToLower()), @"\s+", "");
        }

        private string RemoveDiacritics(string text)
        {
            return new string(text
                .Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray())
                .Normalize(NormalizationForm.FormC);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Image";
            ofd.Filter = "Image Files(*.jpg) | *.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);
                imageBox.Image = img.GetThumbnailImage(400,200,null, new IntPtr());
            }
        }

        private async void btnInsert_Click(object sender, EventArgs e)
        {
            if (tbNameSong.Text == "" || tbNameSinger.Text == "" || tbSongTime.Text == "" || tbURLAudio.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Thông báo");
            }
            else
            {
                //INSERT - Picture into ms => byte array[] => toBase64String
                MemoryStream ms = new MemoryStream();
                imageBox.Image.Save(ms, ImageFormat.Jpeg);

                byte[] a = ms.GetBuffer();

                string output = Convert.ToBase64String(a);


                var data = new Song
                {
                    Username = Username,
                    NameSong = tbNameSong.Text.MaHoa(),
                    NameSinger = tbNameSinger.Text.MaHoa(),
                    SongTime = tbSongTime.Text,
                    Img = output
                };

                using (FileStream audioFileStream = File.OpenRead(tbURLAudio.Text))
                {
                    string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.FullName;

                    var builder = new ConfigurationBuilder()
                        .AddJsonFile(Path.Combine(projectDirectory, "appsettings.json"), optional: false, reloadOnChange: true);

                    // Tạo IConfiguration từ ConfigurationBuilder
                    IConfiguration config = builder.Build();
                    // Đọc các cấu hình từ "Firebase" section
                    string bucketName = config["Firebase:BucketName"];

                    var objectName = "audio/" + ConvertToUnsignAndRemoveSpaces(tbNameSong.Text) + ".mp3";
                    _storageClient.UploadObject("musicapp-b6c08.appspot.com", objectName, null, audioFileStream);
                    // Lưu liên kết đến file âm thanh vào trường Audio của đối tượng Song
                    string audioUrl = $"{bucketName}/{objectName}";
                    data.Audio = audioUrl;
                }

                IFirebaseClient client = _firebaseService.GetFirebaseClient();

                SetResponse response = await client.SetAsync("Songs/" + ConvertToUnsignAndRemoveSpaces(tbNameSong.Text), data);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Đăng thành công!");
                }
                else
                {
                    MessageBox.Show("Đăng thất bại!");
                }
            }
        }

        private void btnBrowseAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Audio Files|*.mp3;*.wav";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    tbURLAudio.Text = openFileDialog.FileName;
                }
            }
        }
    }
}
