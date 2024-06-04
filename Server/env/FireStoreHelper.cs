using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.IO;

namespace Server.env
{
    public class FireStoreHelper
    {
        static string fireconfig = @"
        {
              ""type"": ""service_account"",
              ""project_id"": ""musicapp-b6c08"",
              ""private_key_id"": ""b8d164c53b4314f43cd22c8ac64b3fdeba06b28b"",
              ""private_key"": ""-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQC3EAHChh/V6Ur4\nlFaWllimmlN/xxNQuTM99+tp4rgKLQmsuiEtTr5AUWLL0YwWmr16zPp3wo3b3Tb3\nJkH9sNeWQo2oEgidc6Z5K4U2GgiLEIw2irfaVgpffJ2vJPsQKjHgxx7xuOkW9ZDg\n078CA5iq6DcxKCWTVqEoUlJPqdTf0joX1MyVMWAlqXHEL28ZkNlaRb2u7evyU6+e\nKtWiLsQmFmjbT9KpTB0lMEPjQ4QV32KxbubGOylGF/Z7qjbhB7DcIMd+N/yPU3/e\n2mQ2we68tcmiiIAYKGWw+wHtjkpW36BoKuev3rcqbZYZQJIeOQRtFzyXhI4iyq8N\nFKxc33GbAgMBAAECggEAELr0Rjhm0XB0SxqXfUS5TmSdS/jAMajzX274g2FWi4uJ\nLlGGdbMUHSGzA0if4nL67S2tmGNd/qGNg/HppwtR8KgddSZMxkMLDFjnYEXp0PfO\nsK9l1Rz27rofWSHqlhQdIdtg3xuUpeYEex+u/bqhacE2RhZStM/+g4ZegmxViazv\nsIOIS+xlE+Y7xUOvUUb4/W2I3VulsFFHPCOd5AWDXzeRNXfeZOfkAVQmvh52ibz5\nrB984RiFysDMk7hhEaLlgqFy4vAMbjKJobi6FE3bNCTc/DkDFuOX3+RnN2/pMKsh\ndY01bhVJy2IMdU6bhqIa4ZWt+NbsVIYn4lGlGhkLpQKBgQDrOPz4BPPOlzLlbgBT\nx/iXilY8w7o/XWGS0zRJSoSa2FCjfTNX0EgU7svWM/edghH15R9B9p/uTMzTiTvQ\nbjA5gZk8FXNW3vJ3dwBLIDuRDjVZNRP6y+MGUHED3QG1Id5uO7hlHc+OpugseZ5s\naxXgHsXhmB8FcB2RZh3lEhdIjwKBgQDHO4oy1OV/7zdpar6bkOzaNhCzggO80ziy\noGYfn6U6sJJxDv5U5yaoDmEtU2+woQfFWq0Tl+AtWU7tJ/IIEyUfBvi+nk37LsnM\nqdRWHxjnm7xAFVKHI9njTJwRQyvEmpvZu9Z2NK5gHPLF/nlQn45bsNWS4Zw8XSwr\nM8u9wBnUNQKBgGlNYto5hVgYEh3px9W58Q0OThr32HojeNn9GSwyYvjbHAaEtyZ5\nxLsySCiFrTVjFF1LjFnAacqJsSyGBDQEECy+WvYt+CuMtWlL6eK39FK01Kcx/tbI\nzcJ24pFDME+BcQ1SSPNjjBalm8zFSWnp6qohvJ6ItmJ91Y7Q81MobSn7AoGAZK6L\nDVsXS4q5JvXBs14Ow8t0rzJx7xeS3HpAgZSs7DbVGntoPcG+gEkcBMrYc5s9ERfc\nxT0IFgK+5ww7vKboKIDebX4UjG49nsboPklizZCfFodv+Ek+0CYj7HlUgftb7TGG\nlJ/Uy091xGbwKbUoPN/lXl/TE5JGQuLfAFo2800CgYEAwVrzSfvtcrca1o6njhfO\n405ukp2Oo0sB0qiis52jzJ22b292nb0Y47MSiJTHC9mey6fcYD9XiQPzHBmQRhls\ndyB47nbW5gMaXyuYWUZ463tPipJrP7OMlaiN5JiuvxL/uXlRh0Bt6HMxEEiMeEYE\nBnF/3/5RWGczKUdVf0k6wgI=\n-----END PRIVATE KEY-----\n"",
              ""client_email"": ""firebase-adminsdk-zxcx6@musicapp-b6c08.iam.gserviceaccount.com"",
              ""client_id"": ""101496582040258066099"",
              ""auth_uri"": ""https://accounts.google.com/o/oauth2/auth"",
              ""token_uri"": ""https://oauth2.googleapis.com/token"",
              ""auth_provider_x509_cert_url"": ""https://www.googleapis.com/oauth2/v1/certs"",
              ""client_x509_cert_url"": ""https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk-zxcx6%40musicapp-b6c08.iam.gserviceaccount.com"",
              ""universe_domain"": ""googleapis.com""
        }";

        static string filepath = "";
        public static FirestoreDb db { get; private set; }
        public static void SetEnviromentVariable()
        {
            filepath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())) + ".json";
            File.WriteAllText(filepath, fireconfig);
            File.SetAttributes(filepath, FileAttributes.Hidden);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            db = FirestoreDb.Create("musicapp-b6c08");
            File.Delete(filepath);
        }
    }
}
