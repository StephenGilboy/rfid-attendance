using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RfidAttendance.Web.Data;

namespace RfidAttendance.Web.Pages.Student
{
	public class IndexModel : PageModel
    {
        public List<Data.Student> Students { get; set; }
        private readonly AttendanceContext _db;

        public IndexModel(AttendanceContext dbContext)
        {
            _db = dbContext;
            Students = new List<Data.Student>();
        }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            Students = await _db.Students.Include(s => s.RfidTag).ToListAsync(cancellationToken);
        }
    }
}
