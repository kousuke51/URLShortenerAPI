using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using URLShortenerAPI.Data;
using URLShortenerAPI.Models;
using URLShortenerAPI.Utility;

namespace URLShortenerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlInfoController : ControllerBase
    {
        private readonly UrlInfoContext _context;
        private readonly IOptions<UrlConfig> _config;
        private ILogger _logger;

        public UrlInfoController(UrlInfoContext context, IOptions<UrlConfig> config, ILogger<UrlInfoController> logger)
        {
            _context = context;
            this._config = config;
            this._logger = logger;
        }

        // GET: api/UrlInfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UrlInfo>>> GetUrlInfo()
        {
            return await _context.UrlInfo.ToListAsync();
        }

        // GET: api/UrlInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UrlInfo>> GetUrlInfo(int id)
        {
            var urlInfo = await _context.UrlInfo.FindAsync(id);

            if (urlInfo == null)
            {
                return NotFound();
            }

            return urlInfo;
        }

        // PUT: api/UrlInfo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUrlInfo(int id, UrlInfo urlInfo)
        {
            if (id != urlInfo.UrlInfoId)
            {
                return BadRequest();
            }

            _context.Entry(urlInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UrlInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UrlInfo
        [HttpPost]
        public ActionResult<UrlInfo> PostUrlInfo(UrlInfo urlInfo)
        {
            try
            {
                UrlHelper.GenerateShortUrl(urlInfo, _config.Value.UrlPrefix, _context, _config.Value.DaysUntilExpire, _logger);                      
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, null);
            }
            return CreatedAtAction("GetUrlInfo", new { id = urlInfo.UrlInfoId }, urlInfo);
        }

        // DELETE: api/UrlInfo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UrlInfo>> DeleteUrlInfo(int id)
        {
            var urlInfo = await _context.UrlInfo.FindAsync(id);
            if (urlInfo == null)
            {
                return NotFound();
            }

            _context.UrlInfo.Remove(urlInfo);
            await _context.SaveChangesAsync();

            return urlInfo;
        }

        private bool UrlInfoExists(int id)
        {
            return _context.UrlInfo.Any(e => e.UrlInfoId == id);
        }
    }
}
