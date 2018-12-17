using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using SD.Services.Data.Services.Contracts;

namespace SD.Web.Utilities.Quartz
{
    public class ScheduledJob : IJob
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<ScheduledJob> logger;
        private readonly ISensorDataService sensorDataService;


        public ScheduledJob(IConfiguration configuration, ILogger<ScheduledJob> logger, ISensorDataService sensorDataService)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.sensorDataService = sensorDataService;
        }
		
        public async Task Execute(IJobExecutionContext context)
        {
            await this.sensorDataService.GetSensorsDataAsync();

            await Task.CompletedTask;
        }
    }
}
