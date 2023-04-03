using System;
namespace RfidAttendance.Web.Models
{
	public class ExternalTagRequest
	{
		public ExternalTag[] tags { get; set; }
	}

	public class ExternalTag
	{
		public int ant { get; set; }
		public int rssi { get; set; }
		public int count { get; set; }
		public int epcLen { get; set; }
		public string epc { get; set; }
	}
}

