using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TraceLoop.Tracer
{
    public class TraceService : IHostedService
    {
        private readonly ILogger<TraceService> _logger;
        private Timer _timer;
        private int _executionCount;
        private const string Hostname = "8.8.8.8";


        public TraceService(ILogger<TraceService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(ExecuteTrace, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void ExecuteTrace(object state)
        {
            var count = Interlocked.Increment(ref _executionCount);
            _logger.LogInformation($"{DateTime.Now} - Starting Traceroute to {Hostname}");

            var tracertEntries = TraceExecutor.Tracert(Hostname, 999, 100);
            
            foreach (var tracertEntry in tracertEntries)
            {
                _logger.LogInformation(tracertEntry.ToString());
            }

            _logger.LogInformation($"{DateTime.Now} - Traceroute to {Hostname} executed {{Count}} times", count);
        }
    }
}