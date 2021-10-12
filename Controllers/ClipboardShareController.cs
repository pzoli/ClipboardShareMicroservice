using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace ClipboardShareMicroservice.Controllers
{
    public class ClipboardContent
    {
        public string content { get; set; }
    }
    public static class Clipboard
    {
        public static void SetText(string text)
        {
            var powershell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-command \"Set-Clipboard -Value \\\"{text}\\\"\""
                }
            };
            powershell.Start();
            powershell.WaitForExit();
        }

        public static string GetText()
        {
            var powershell = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    RedirectStandardOutput = true,
                    FileName = "powershell",
                    Arguments = "-command \"Get-Clipboard\""
                }
            };

            powershell.Start();
            string text = powershell.StandardOutput.ReadToEnd();
            powershell.StandardOutput.Close();
            powershell.WaitForExit();
            return text.TrimEnd();
        }
    }

    [ApiController]
    [Route("clipboard")]
    public class ClipboardShareController : ControllerBase
    {
        [HttpGet]
        [Route("qrcode/{type}")]
        public IActionResult GetImage(string type)
        {
            HostString ipList = Request.Host;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://"+ipList+"/clipboard/"+type, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            IActionResult result;
            using (var memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, ImageFormat.Jpeg);
                result = File(memoryStream.ToArray(), "image/jpeg");
            }
            return result;
        }

        [HttpGet]
        [Route("get")]
        public String Get()
        {
            return Clipboard.GetText();
        }

        [HttpPost]
        [Route("jsonpost")]
        public string DotnetPost([FromBody] ClipboardContent clipboardContent)
        {
            Clipboard.SetText(clipboardContent.content);
            return "ready";
        }

        [HttpPost]
        [Route("post")]
        public void Post([FromForm] string content)
        {
            Clipboard.SetText(content);
        }

    }
}
