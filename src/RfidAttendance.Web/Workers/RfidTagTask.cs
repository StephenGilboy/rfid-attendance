using System;
using System.Threading.Channels;

namespace RfidAttendance.Web.Workers
{
	public class RfidTagHostedService : BackgroundService
	{
		private readonly ILogger<RfidTagHostedService> _logger;

		public IRfidTagTaskQueue TaskQueue { get; }

		public RfidTagHostedService(IRfidTagTaskQueue taskQueue, ILogger<RfidTagHostedService> logger)
		{
			TaskQueue = taskQueue;
			_logger = logger;
		}

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Rfid Tag hosted service is running");
			await BackgroundProcessing(cancellationToken);
        }

		private async Task BackgroundProcessing(CancellationToken cancellationToken)
		{
			while(!cancellationToken.IsCancellationRequested)
			{
				var workItem = await TaskQueue.DequeueAsync(cancellationToken);

				try
				{
					await workItem(cancellationToken);
				}
				catch(Exception ex)
				{
					_logger.LogError(ex, "Error occured executing {WorkItem}", workItem);
				}
			}
		}

        public override Task StopAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Stopping Rfid Tag hosted service");
			return base.StopAsync(cancellationToken);
        }
    }

	public interface IRfidTagTaskQueue
	{
		ValueTask QueueRfidTagTaskAsync(Func<CancellationToken, ValueTask> workItem);
		ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
	}

	public class RfidTagTaskQueue : IRfidTagTaskQueue
	{
		private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

		public RfidTagTaskQueue(int capacity)
		{
			var options = new BoundedChannelOptions(capacity)
			{
				FullMode = BoundedChannelFullMode.Wait
			};
			_queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(options);
		}

        public async ValueTask QueueRfidTagTaskAsync(Func<CancellationToken, ValueTask> workItem)
        {
			if (workItem is null)
			{
				throw new ArgumentNullException(nameof(workItem));
			}
			await _queue.Writer.WriteAsync(workItem);
        }

        public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken)
        {
			var workItem = await _queue.Reader.ReadAsync(cancellationToken);
			return workItem;
        }
    }
}

