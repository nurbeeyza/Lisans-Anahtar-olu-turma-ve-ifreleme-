using Microsoft.AspNetCore.Mvc;
using Anahtar_oluşturma.Models;
using System.IO;

namespace Anahtar_oluşturma.Controllers
{
    public class KeyGenerationController : Controller
    {
        private readonly string publicKeyPath = "C:\\Anahtar\\publicKey.txt";
        private readonly string privateKeyPath = "C:\\Anahtar\\privateKey.txt";
        private readonly string encryptedDataPath = "C:\\Anahtar\\encryptedData.txt"; // Şifreli veriyi saklamak için yol

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateKeys()
        {
            var model = new KeyGenerationModel(publicKeyPath, privateKeyPath);
            model.GenerateKeyPair();

            ViewBag.Message = "Anahtar çiftleri başarıyla oluşturuldu.";
            return View("Index");
        }

        [HttpPost]
        public IActionResult EncryptAndDecryptData(string data)
        {
            var model = new KeyGenerationModel(publicKeyPath, privateKeyPath);
            var rsa = model.ImportKeys(publicKeyPath, privateKeyPath);

            // Veriyi şifrele
            string encData = model.EncryptData(data, rsa);
            // Şifreli veriyi dosyaya yaz
            System.IO.File.WriteAllText(encryptedDataPath, encData);

            // Şifreli veriyi deşifre et
            ViewBag.Enc = encData;
            ViewBag.Dec = model.DecryptData(encData, rsa);
            return View("Index");
        }

        [HttpPost]
        public IActionResult DecryptData(string encdata)
        {
            var model = new KeyGenerationModel(publicKeyPath, privateKeyPath);
            var rsa = model.ImportKeys(publicKeyPath, privateKeyPath);

            ViewBag.EncData = encdata;
            ViewBag.DecData = model.DecryptData(encdata, rsa);
            return View("Index");
        }

        [HttpPost]
        public IActionResult ReadEncryptedData()
        {
            // Dosyadan şifreli veriyi oku
            if (System.IO.File.Exists(encryptedDataPath))
            {
                string encDataFromFile = System.IO.File.ReadAllText(encryptedDataPath);
                var model = new KeyGenerationModel(publicKeyPath, privateKeyPath);
                var rsa = model.ImportKeys(publicKeyPath, privateKeyPath);

                ViewBag.EncDataFromFile = encDataFromFile;
                ViewBag.DecDataFromFile = model.DecryptData(encDataFromFile, rsa);
            }
            else
            {
                ViewBag.Message = "Şifreli veri dosyası bulunamadı.";
            }
            return View("Index");
        }
    }
}
