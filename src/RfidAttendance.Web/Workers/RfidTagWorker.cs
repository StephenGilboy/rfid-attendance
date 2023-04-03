using System;
using Microsoft.EntityFrameworkCore;
using RfidAttendance.Web.Data;

namespace RfidAttendance.Web.Workers
{
	public class RfidTagWorker
	{
		private readonly IRfidTagTaskQueue _taskQueue;
		private readonly ILogger<RfidTagWorker> _logger;

		public RfidTagWorker(IRfidTagTaskQueue taskQueue, ILogger<RfidTagWorker> logger)
		{
			_taskQueue = taskQueue;
			_logger = logger;
		}

		public async ValueTask RfidTagSeen(IServiceScopeFactory serviceScopeFactory, RfidTag tag)
		{
			await _taskQueue.QueueRfidTagTaskAsync(async token =>
			{
				using (var scope = serviceScopeFactory.CreateScope())
				{
					var dbContext = scope.ServiceProvider.GetRequiredService<AttendanceContext>();

					// See if it exists
					var dbTag = await dbContext.RfidTags.Include(r => r.Student).FirstOrDefaultAsync(t => t.Id == tag.Id);

					// If Not add it
					if (dbTag is null)
					{
						await dbContext.AddAsync(tag);
						await dbContext.SaveChangesAsync();
						return;
					}

					// If student update attendance
					if (dbTag.Student != null)
					{
						dbTag.IsCurrenltyInAttendance = !dbTag.IsCurrenltyInAttendance;
					}

					dbTag.LastSeen = DateTime.UtcNow;

					// Save Changes
					await dbContext.SaveChangesAsync();
				}
			});
		}
	}
}
