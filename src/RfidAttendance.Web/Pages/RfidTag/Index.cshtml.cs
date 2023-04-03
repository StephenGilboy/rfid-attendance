using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RfidAttendance.Web.Data;

namespace RfidAttendance.Web.Pages.RfidTag
{
	public class IndexModel : PageModel
    {
        private readonly AttendanceContext _db;
        public List<Data.RfidTag> Tags { get; set; }


        public IndexModel(AttendanceContext dbContext)
        {
            _db = dbContext;
            Tags = new List<Data.RfidTag>();
        }

        public async Task OnGetAsync(CancellationToken token)
        {
            Tags = await _db.RfidTags.Include(r => r.Student).ToListAsync();
        }
    }
}
