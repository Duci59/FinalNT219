using Microsoft.Extensions.Configuration;
using FireSharp.Config;
using FireSharp.Interfaces;
using System.IO;
using System;

namespace MusicApp.env
{
    public class Service
    {
        private readonly IFirebaseConfig _firebaseConfig;
        private readonly IFirebaseClient _firebaseClient;

        public Service()
        {
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.FullName;

            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(projectDirectory, "appsettings.json"), optional: false, reloadOnChange: true);

            // Tạo IConfiguration từ ConfigurationBuilder
            IConfiguration config = builder.Build();
            // Đọc các cấu hình từ "Firebase" section
            string authSecret = config["Firebase:AuthSecret"];
            string basePath = config["Firebase:BasebPath"];

            _firebaseConfig = new FirebaseConfig
            {
                AuthSecret = authSecret,
                BasePath = basePath
            };

            _firebaseClient = new FireSharp.FirebaseClient(_firebaseConfig);
        }

        // Method để trả về Firebase Client nếu bạn muốn sử dụng nó ở ngoài lớp Service
        public IFirebaseClient GetFirebaseClient()
        {
            return _firebaseClient;
        }
    }
}
