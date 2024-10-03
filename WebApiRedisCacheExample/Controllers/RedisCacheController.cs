using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace WebApiRedisCacheExample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RedisCacheController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public RedisCacheController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> SetCacheData(string cacheKey, string data)
        {
            //var cacheKey = "SampleData";
            var cacheData = Encoding.UTF8.GetBytes(data);

            // Cache'e veri yazıyoruz
            await _cache.SetAsync(cacheKey, cacheData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) // 5 dakika geçerli olacak
            });

            return Ok("Data cached successfully");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCacheData(string cacheKey)
        {
            //var cacheKey = "SampleData";

            // Cache'den veriyi alıyoruz
            var cachedData = await _cache.GetAsync(cacheKey);

            if (cachedData == null)
            {
                return NotFound("Data not found in cache");
            }

            var cachedValue = Encoding.UTF8.GetString(cachedData);
            return Ok($"Cached Data: {cachedValue}");
        }

        [HttpGet("remove")]
        public async Task<IActionResult> RemoveCacheData(string cacheKey)
        {
            //var cacheKey = "SampleData";

            // Cache'deki veriyi siliyoruz
            await _cache.RemoveAsync(cacheKey);

            return Ok("Data removed from cache");
        }
    }
}
