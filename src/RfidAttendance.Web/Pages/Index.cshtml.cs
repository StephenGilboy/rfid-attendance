using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RfidAttendance.Web.Data;

namespace RfidAttendance.Web.Pages;

public class IndexModel : PageModel
{
    private readonly AttendanceContext _db;
    private readonly ILogger<IndexModel> _logger;

    public List<Data.Student> Students { get; set; }

    public IndexModel(AttendanceContext dbContext,  ILogger<IndexModel> logger)
    {
        _logger = logger;
        _db = dbContext;
        Students = new List<Data.Student>();
    }

    public async Task OnGetAsync()
    {
        Students = await _db.Students.Include(s => s.RfidTag).ToListAsync();
    }
}
