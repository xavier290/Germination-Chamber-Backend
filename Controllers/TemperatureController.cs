using Microsoft.AspNetCore.Mvc;
using ModelsMQTT_Server;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MQTTnet.Samples.Server.Controllers
{
    [Route("api/temperature")]
    [ApiController]
    public class TemperatureController : ControllerBase
    {
        private readonly MqttDbContext _context;

        public TemperatureController(MqttDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Temperature>>> GetTemperatures()
        {
            string filter = "";
            
            try
            {
                return await _context.Temperature.Where(e => e.Topic.Contains(filter)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the message: {ex.Message}");
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Temperature>> GetTemperature(int id)
        {
            var temperature = await _context.Temperature.FindAsync(id);

            if (temperature == null)
            {
                return NotFound();
            }

            return temperature;
        }
    }
}
