using Microsoft.AspNetCore.Mvc;
using RedisData.Entities;
using StackExchange.Redis;

namespace RedisSample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IDatabase _redisDb;

        public PersonController(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        [HttpPost("addPerson")]
        public async Task<IActionResult> AddPerson([FromBody] PersonEntity person)
        {
            var success = await _redisDb.StringSetAsync(person.Mail, person.Name);
            return success ? Ok($"Person {person.Name} with email {person.Mail} is set") : StatusCode(500, "Internal Server Error.");
        }

        [HttpGet("getPerson")]
        public async Task<IActionResult> GetPerson(string mail)
        {
            var person = await _redisDb.StringGetAsync(mail);
            return person.IsNullOrEmpty ? NotFound() : Ok(person.ToString());
        }
    }
}
