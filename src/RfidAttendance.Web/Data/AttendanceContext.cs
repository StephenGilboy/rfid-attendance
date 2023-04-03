using System;
using Microsoft.EntityFrameworkCore;

namespace RfidAttendance.Web.Data
{
	public class AttendanceContext : DbContext
	{
		public DbSet<RfidTag> RfidTags { get; set; }
		public DbSet<Student> Students { get; set; }

		public string DbPath { get; }

		public AttendanceContext()
		{
			var folder = Environment.SpecialFolder.LocalApplicationData;
			var path = Environment.GetFolderPath(folder);
			DbPath = System.IO.Path.Join(path, "attendance.db");
			Console.WriteLine($"Saved db to {DbPath}");
			Database.EnsureCreated();
		}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			optionsBuilder.UseSqlite($"Data Source={DbPath}");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
			modelBuilder.Entity<Student>()
				.HasOne(s => s.RfidTag)
				.WithOne(r => r.Student)
				.HasForeignKey<Student>(s => s.RfidTagId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

