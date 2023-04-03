using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RfidAttendance.Web.Data
{
	[Table("students")]
	public class Student
	{
		[Key]
		[Column("id")]
		public Guid Id { get; set; }

		[Column("first_name"), Required, MinLength(2)]
		public string FirstName { get; set; }

		[Column("last_name"), Required, MinLength(2)]
		public string LastName { get; set; }

		[Column("rfid_tag_id")]
		public string RfidTagId { get; set; }

		public RfidTag RfidTag { get; set; }
	}
}
