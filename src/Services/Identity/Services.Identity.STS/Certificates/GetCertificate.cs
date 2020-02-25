using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Services.Identity.STS.Certificates
{
    static class CertificatesHelper
    {
        public static X509Certificate2 Get(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            var certificateConfig = configuration.GetSection("Certificate");

            // https://benjii.me/2017/06/creating-self-signed-certificate-identity-server-azure/
            X509Certificate2 certificate;

            if (webHostEnvironment.IsProduction())
            {
                throw new System.NotImplementedException();
            }
            else
            {
                var certLocation = certificateConfig.GetValue<string>("Location");
                var certPassword = certificateConfig.GetValue<string>("Password");

                using (FileStream fs = File.OpenRead(certLocation))
                {
                    certificate = new X509Certificate2(ReadStream(fs), certPassword);
                }
            }
            
            return certificate;
        }

        static byte[] ReadStream(System.IO.Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}