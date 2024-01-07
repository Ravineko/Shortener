using System;
using Microsoft.AspNetCore.Mvc;
using Shortener.Services;
using Shortener.Models;

namespace Shortener.Controllers
{
    public class LinkController : Controller
    {
        private readonly ShortenLinkGenerator _shortenLinkGenerator;
        private readonly ApplicationDbContext _dbContext;

        public LinkController(ShortenLinkGenerator shortenLinkGenerator, ApplicationDbContext dbContext)
        {
            _shortenLinkGenerator = shortenLinkGenerator;
            _dbContext = dbContext;
        }
        [HttpGet("List")]
        public IEnumerable<LinkModel> GetLinks()
        {
            return _dbContext.Links.ToList();
        }

        [HttpPost("Add")]
        public IActionResult AddLink([FromBody] string originalLink)
        {
            if (string.IsNullOrEmpty(originalLink))
            {
                return BadRequest("Original link cannot be empty");
            }

            // Генеруємо скорочене посилання
            string shortenedLink = ShortenLinkGenerator.ShortenLink(originalLink);

            // Створюємо новий об'єкт LinkModel
            var newLink = new LinkModel
            {
                OriginalLink = originalLink,
                ShortenedLink = shortenedLink
            };

            // Додаємо новий об'єкт до бази даних
            _dbContext.Links.Add(newLink);
            _dbContext.SaveChanges();

            return Ok(newLink); // Повертаємо створений об'єкт, якщо потрібно
        }

        public IActionResult Redirect(string id)
        {
            var linkModel = _dbContext.Links.FirstOrDefault(l => l.ShortenedLink == id);

            if (linkModel != null)
            {
                return Redirect(linkModel.OriginalLink);
            }
            else
            {

                return NotFound();
            }
        }
    }
}
