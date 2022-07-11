using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using MimeKit.Text;

namespace SimpleEmailApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }

        [HttpGet("Download")]
        // you can make file name as a parameter of the api request aka. getFileAsync(string fname)
        public async Task<IActionResult> getFileAsync(bool IsImg, string? fileToDownload = null)
        {
            //Byte[] b;
            if (IsImg)
            {
                var folderName = Path.Combine("Resources", "Images");
                var pathToget = Path.Combine(Directory.GetCurrentDirectory(), folderName,fileToDownload ?? "204041712_72027270.jpg");
                var image = await System.IO.File.ReadAllBytesAsync(pathToget);
                return File(image, "image/jpg");
            }
            else
            {
                
                try
                {
                    var folderName = Path.Combine("Resources", "Files");
                    var FullPathWuthFile = Path.Combine(Directory.GetCurrentDirectory(), folderName, fileToDownload ?? "excel_list.xlsx");
                    var content = await System.IO.File.ReadAllBytesAsync(FullPathWuthFile);
                    var fileName = System.IO.Path.GetFileName(FullPathWuthFile);
                    new FileExtensionContentTypeProvider()
                        .TryGetContentType(fileName, out string contentType);
                    return File(content, contentType, fileName);
                }
                catch
                {
                    return BadRequest();
                }
            }
        }
    }
}
