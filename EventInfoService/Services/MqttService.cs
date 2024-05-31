using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using EventInfoService.Models;

namespace EventInfoService.Services
{
    public class MqttService : BackgroundService
    {
        private readonly List<SensorData> _sensorDataList;
        private IMqttClient _mqttClient;

        public MqttService()
        {
            _sensorDataList = new List<SensorData>();
            ConnectToMqttBroker().Wait();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    await Task.CompletedTask;
}
private async Task ConnectToMqttBroker()
        {
            var mqttFactory = new MqttFactory();
            _mqttClient = mqttFactory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId("EventInfoService")
                .WithTcpServer("localhost", 1883)
                .Build();

            _mqttClient.ConnectedAsync += async e =>
            {
                Console.WriteLine("Connected to MQTT broker");

                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("analyzed_data").Build());

                Console.WriteLine("Subscribed to topic 'analyzed_data'");
            };

            _mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var sensorData = JsonConvert.DeserializeObject<SensorData>(payload);
                _sensorDataList.Add(sensorData);
                return Task.CompletedTask;
            };

            await _mqttClient.ConnectAsync(options);
        }

        public List<SensorData> GetSensorData()
        {
            lock (_sensorDataList)
            {
                return new List<SensorData>(_sensorDataList);
            }
        }
    }
}
