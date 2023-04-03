using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidAttendance.Web.Data
{
	[Table("rfid_tags")]
	public class RfidTag
	{
		[Key, Column("id")]
		public string Id { get; set; }

		[Column("is_currently_in_attendance"), Required]
		public bool IsCurrenltyInAttendance { get; set; }

		[Column("last_seen"), Required]
		public DateTime LastSeen { get; set; }

		public Student Student { get; set; }
	}
}

