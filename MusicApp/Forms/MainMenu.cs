using FireSharp.Interfaces;
using FireSharp.Response;
using Google.Cloud.Storage.V1;
using MusicApp.env;
using MusicApp.MaHoa;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicApp.Forms
{
    public partial class MainMenu : Form
    {
        string Username, Usertype;
        private readonly Service _firebaseService;
        private readonly StorageClient _storageClient;
        private readonly IFirebaseClient client;
        private IWavePlayer waveOut;
        private WaveStream mp3Reader;
        private WaveOutEvent outputDevice;
        private MediaFoundationReader mediaReader;
        private float previousVolume = 1.0f;  // Lưu âm thanh trước khi mute

        public MainMenu(string username, string usertype)
        {
            InitializeComponent();
            _firebaseService = new Service();
            _storageClient = _firebaseService.GetStorageClient();
            client = _firebaseService.GetFirebaseClient();
            Username = username;
            Usertype = usertype;
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


        private string ConvertGsToHttps(string gsUrl)
        {
            const string baseUrl = "https://firebasestorage.googleapis.com/v0/b/";

            // Extract bucket and path from the gsUrl
            var match = Regex.Match(gsUrl, @"gs://([^/]+)/(.+)");
            if (!match.Success)
            {
                throw new ArgumentException("Invalid gsUrl format", nameof(gsUrl));
            }

            string bucket = match.Groups[1].Value;
            string path = match.Groups[2].Value.Replace("/", "%2F");

            return $"{baseUrl}{bucket}/o/{path}?alt=media";
        }

        private void PlayAudioFromUrl(string url)
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Stop();
            }

            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }

            if (mp3Reader != null)
            {
                mp3Reader.Dispose();
                mp3Reader = null;
            }

            waveOut = new WaveOutEvent();

            using (var webClient = new WebClient())
            {
                byte[] audioData = webClient.DownloadData(url);

                string tempEncryptedFile = Path.Combine(Path.GetTempPath(), "encryptedAudio.wav");
                string tempDecryptedFile = Path.Combine(Path.GetTempPath(), "decryptedAudio.wav");
                string tempMp3File = Path.Combine(Path.GetTempPath(), "audio.mp3");

                File.WriteAllBytes(tempEncryptedFile, audioData);

                // Giải mã file đã tải xuống
                MD5Helper.DecryptWavFile(tempEncryptedFile, tempDecryptedFile);

                // Chuyển đổi WAV sang MP3
                using (var reader = new AudioFileReader(tempDecryptedFile))
                {
                    using (var writer = new LameMP3FileWriter(tempMp3File, reader.WaveFormat, LAMEPreset.STANDARD))
                    {
                        reader.CopyTo(writer);
                    }
                }

                // Đọc và phát file đã giải mã
                mp3Reader = new Mp3FileReader(tempMp3File);
                waveOut.Init(mp3Reader);
                waveOut.Play();

                // Xóa các file tạm
                File.Delete(tempEncryptedFile);
                File.Delete(tempDecryptedFile);
                File.Delete(tempMp3File);
            }
        }
        //test
        private async Task PlayEncryptedAudioFromUrl(string url)
        {
            try
            {
                // Download the encrypted audio file
                string encryptedFilePath = Path.GetTempFileName();
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    byte[] fileBytes = await response.Content.ReadAsByteArrayAsync();
                    File.WriteAllBytes(encryptedFilePath, fileBytes); // Sử dụng phương thức đồng bộ
                }

                // Decrypt the audio file
                string decryptedFilePath = Path.GetTempFileName();
                MD5Helper.DecryptWavFile(encryptedFilePath, decryptedFilePath);

                // Play the decrypted audio file
                PlayAudioFromFile(decryptedFilePath);

                // Clean up temporary files
                File.Delete(encryptedFilePath);
                File.Delete(decryptedFilePath);
            }
            catch (HttpRequestException httpEx)
            {
                string errorMessage = $"Error playing audio: An exception occurred during an HttpClient request. Message: {httpEx.Message}";
                MessageBox.Show(errorMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing audio: " + ex.Message);
            }
        }
        private void PlayAudioFromFile(string filePath)
        {
            try
            {
                if (outputDevice != null)
                {
                    outputDevice.Stop();
                    outputDevice.Dispose();
                    outputDevice = null;
                }

                if (mediaReader != null)
                {
                    mediaReader.Dispose();
                    mediaReader = null;
                }

                mediaReader = new MediaFoundationReader(filePath);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(mediaReader);
                outputDevice.Play();
                btnPauseMusic.Visible = true;
                btnPlayMusic.Visible = false;
                //MessageBox.Show("OK");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing audio: " + ex.Message);
            }
        }


        //

        private void PictureBoxButton_Click(object sender, EventArgs e)
        {
            // Lấy CustomPanel chứa PictureBoxButton đã được click
            PictureBox clickedButton = sender as PictureBox;
            CustomPanel parentPanel = clickedButton.Parent as CustomPanel;

            //Xử lý khi bật nhạc
            lbSinger2.Text = parentPanel.NAMESINGER;
            lbSinger1.Text = parentPanel.NAMESINGER;
            lbSong.Text = parentPanel.NAMESONG;
            lbTimeEnd.Text = parentPanel.TIME;
            byte[] b = Convert.FromBase64String(parentPanel.IMG);

            MemoryStream ms = new MemoryStream();
            ms.Write(b, 0, Convert.ToInt32(b.Length));

            Bitmap bm = new Bitmap(ms, false);
            ms.Dispose();
            Bitmap resizedImage = new Bitmap(312, 347);
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(bm, 0, 0, 312, 347);
            }

            pictureBox1.Image = resizedImage;

            Bitmap resizedImage2 = new Bitmap(977, 295);

            using (Graphics g = Graphics.FromImage(resizedImage2))
            {
                g.DrawImage(bm, 0, 0, 977, 295);
            }

            panel2.BackgroundImage = resizedImage;

            if (parentPanel != null)
            {
                // Lấy AudioLink từ CustomPanel và phát âm thanh
                string audioLink = parentPanel.AudioLink;
                string audioUrl = ConvertGsToHttps(audioLink);
                PlayEncryptedAudioFromUrl(audioUrl);
            }
        }

        private async Task LoadSongs()
        {
            panel6.Controls.Clear();
            FirebaseResponse response = await client.GetAsync("Songs/");
            if (response != null)
            {
                Dictionary<string, Song> songs = response.ResultAs<Dictionary<string, Song>>();
                if (songs == null)
                {
                    return;
                }
                int y = 0;
                foreach (var song in songs)
                {
                    try
                    {
                        string nameSong = song.Value.NameSong.ToString().GiaiMa(); // Giải mã tên bài hát
                        string nameSinger = song.Value.NameSinger.ToString().GiaiMa(); // Giải mã tên ca sĩ

                        CustomPanel pn = new CustomPanel(nameSong, nameSinger, song.Value.SongTime, song.Value.Img, song.Value.Audio)
                        {
                            Location = new Point(0, y), // Đặt vị trí của CustomPanel theo vị trí y hiện tại
                        };
                        y += pn.Height + 10;  // Tăng vị trí y để các CustomPanel được đặt cách nhau 10 pixel dọc
                        pn.pictureBoxButton.Click += PictureBoxButton_Click;
                        panel6.Controls.Add(pn);
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi hoặc xử lý lỗi giải mã
                        MessageBox.Show("Lỗi khi giải mã dữ liệu: " + ex.Message);
                    }
                }
            }
            else
            {
                return;
            }
        }
        private void btnMinsize_Click(object sender, EventArgs e)
        {
            this.WindowState |= FormWindowState.Minimized;
        }

        private async void MainMenu_Load(object sender, EventArgs e)
        {
            await LoadSongs();
        }

        

        private async void btnReload_Click(object sender, EventArgs e)
        {
            await LoadSongs();
        }

        private async Task SeekForward(int seconds)
        {
            if (mediaReader != null)
            {
                TimeSpan newPosition = mediaReader.CurrentTime.Add(TimeSpan.FromSeconds(seconds));
                if (newPosition > mediaReader.TotalTime)
                {
                    newPosition = mediaReader.TotalTime;
                }
                mediaReader.CurrentTime = newPosition;
            }
        }

        private async Task SeekBackward(int seconds)
        {
            if (mediaReader != null)
            {
                TimeSpan newPosition = mediaReader.CurrentTime.Subtract(TimeSpan.FromSeconds(seconds));
                if (newPosition < TimeSpan.Zero)
                {
                    newPosition = TimeSpan.Zero;
                }
                mediaReader.CurrentTime = newPosition;
            }
        }

        private async void btnNext_Click(object sender, EventArgs e)
        {
            await SeekForward(5);
        }

        private async void btnBack_Click(object sender, EventArgs e)
        {
            await SeekBackward(5);
        }
        private async void btnPlayMusic_Click(object sender, EventArgs e)
        {
            // Chức năng play nhạc từ nút bấm (nếu cần thiết)
            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    // If music is playing, pause it
                    outputDevice.Pause();
                }
                else if (outputDevice.PlaybackState == PlaybackState.Paused)
                {
                    // If music is paused, resume it
                    outputDevice.Play();
                    btnPauseMusic.Visible = true;
                    btnPlayMusic.Visible = false;
                }
            }
            else
            {
                MessageBox.Show("No music is currently loaded.");
            }
        }
        private void btnPauseMusic_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    // If music is playing, pause it
                    outputDevice.Pause();
                    btnPauseMusic.Visible = false;
                    btnPlayMusic.Visible = true ;
                }
                else if (outputDevice.PlaybackState == PlaybackState.Paused)
                {
                    // If music is paused, resume it
                    outputDevice.Play();
                }
            }
            else
            {
                MessageBox.Show("No music is currently loaded.");
            }
        }

        private void btnUnmute_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                // Restore the volume to the previous level before muting
                outputDevice.Volume = previousVolume;
                btnUnmute.Visible = false;
                btnMute.Visible = true;
            }
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            if (outputDevice != null)
            {
                // Store the current volume level
                previousVolume = outputDevice.Volume;
                // Set the volume to 0 to mute the audio
                outputDevice.Volume = 0;
                btnUnmute.Visible = true;
                btnMute.Visible = false;
            }
        }

        private void bunifuHSlider2_ValueChanged(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ValueChangedEventArgs e)
        {
            // Adjust the audio volume based on the slider value
            if (outputDevice != null)
            {
                outputDevice.Volume = (float)bunifuHSlider2.Value / 100.0f;
            }
        }

        private void bunifuHSlider1_ValueChanged(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ValueChangedEventArgs e)
        {
            // Calculate the new position based on the slider value and total duration
            if (mediaReader != null)
            {
                double newPositionSeconds = mediaReader.TotalTime.TotalSeconds * (double)bunifuHSlider1.Value / 100.0;
                TimeSpan newPosition = TimeSpan.FromSeconds(newPositionSeconds);

                // Set the new playback position
                mediaReader.CurrentTime = newPosition;
            }
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