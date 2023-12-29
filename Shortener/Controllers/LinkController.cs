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
        [HttpGet]
        public IEnumerable<LinkModel> GetLinks()
        {
            return _dbContext.Links.ToList();
        }

        [HttpPost("add")]
        public IActionResult AddLink([FromBody] LinkModel newLink)
        {

            _dbContext.Links.Add(newLink);
            _dbContext.SaveChanges();

            return Ok();
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
