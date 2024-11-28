using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Anahtar_oluşturma.Models
{
    public class KeyGenerationModel
    {
        public string PublicKeyPath { get; set; }
        public string PrivateKeyPath { get; set; }

        public KeyGenerationModel(string publicKeyPath, string privateKeyPath)
        {
            PublicKeyPath = publicKeyPath;
            PrivateKeyPath = privateKeyPath;
        }

        public void GenerateKeyPair()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                File.WriteAllText(PublicKeyPath, Convert.ToBase64String(rsa.ExportRSAPublicKey()));
                File.WriteAllText(PrivateKeyPath, Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
            }
        }

        public RSA ImportKeys(string publicKeyPath, string privateKeyPath)
        {
            var rsa = RSA.Create();

            string base64Public = File.ReadAllText(publicKeyPath);

            byte[] publicBytes = Convert.FromBase64String(base64Public);
            string base64Private = File.ReadAllText(privateKeyPath);

            byte[] privateBytes = Convert.FromBase64String(base64Private);

            rsa.ImportRSAPublicKey(publicBytes, out _);
            rsa.ImportRSAPrivateKey(privateBytes, out _);


            return rsa;
        }

        public string EncryptData(string data, RSA rsa)
        {
                

                byte[] DataBytes = Encoding.UTF8.GetBytes(data);

                byte[] encryptedData = rsa.Encrypt(DataBytes, RSAEncryptionPadding.Pkcs1);

                return Convert.ToBase64String(encryptedData);
        }

        public string DecryptData(string encryptedData, RSA rsa)
        {
                byte[] DataBytes = Convert.FromBase64String(encryptedData);

                byte[] Data = rsa.Decrypt(DataBytes, RSAEncryptionPadding.Pkcs1);

                return Encoding.UTF8.GetString(Data);
        }
    }
}
