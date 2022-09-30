using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment hosting;

        public FileController(IWebHostEnvironment hosting)
        {
            this.hosting = hosting;
        }

        [HttpPost("singleFile")]
        public async Task<IActionResult> SingleFile(IFormFile file)
        {
            var dir = hosting.ContentRootPath;

            await CreateFile(file, Path.Combine(dir, file.FileName));

            return Ok();
        }

        private async Task CreateFile(IFormFile file, string path) {

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        [HttpPost("MultipleFiles")]
        public IActionResult MultipleFiles(IEnumerable<IFormFile> file)
        {
            var dir = hosting.ContentRootPath;

            Task.WaitAll(
                file.Select(x =>
                {
                    return CreateFile(x, Path.Combine(dir, x.FileName));
                }).ToArray());

            return Ok();
        }
    }
}
