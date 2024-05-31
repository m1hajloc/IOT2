using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using EventInfoService.Services;
using EventInfoService.Models;

namespace EventInfoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly MqttService _mqttService;

        public DataController(MqttService mqttService)
        {
            _mqttService = mqttService;
        }

        [HttpGet]
        public IEnumerable<SensorData> Get()
        {
            return _mqttService.GetSensorData();
        }
    }
}
