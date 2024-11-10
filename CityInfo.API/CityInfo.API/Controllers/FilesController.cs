using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(
            FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(
                    nameof(fileExtensionContentTypeProvider));
        }
        
        
        
        [HttpGet("{fileId}")]

        public ActionResult GetFile(string fileId)
        {
            // Look up the actual file, depending on the fileId
            var pathToFile = "2023. J.Resume Sang Thai.pdf";

            // Check whether the file exists
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(
                pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";

            }


            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }

        // class represent a file sent with an HTTP request
        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            // add some restriction: - to avoid consumers uploading latge file
            // Validate the input. Put a limit on filesize to avoid large upload attacks.
            // Only accept .pdf files (check content-type)
            if (file.Length == 0 || file.Length > 20971520
            || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or an invalid one has been inputted");
            }
            
            // Create the file path. Avoid using file.FileName, as an attacker can provide a 
            // malicious one, including full paths or relative paths.
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf");

            // Copy the bytes to a stream and save that streams as a file
            // initialize a new file stream on this and call CopyToAsync
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // return a 200 ok
            return Ok("Your File has been uploaded successfully.");
            

        }

    }
}
