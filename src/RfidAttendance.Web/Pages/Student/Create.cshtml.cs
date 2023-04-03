using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RfidAttendance.Web.Data;

namespace RfidAttendance.Web.Pages.Student
{
    [BindProperties]
    [AutoValidateAntiforgeryToken]
	public class CreateModel : PageModel
    {
        private readonly AttendanceContext _db;

        [Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "RFID Tag")]
        public string TagId { get; set; }

        public List<Data.RfidTag> AvailableTags { get; set; }

        public CreateModel(AttendanceContext context)
        {
            _db = context;
            AvailableTags = new List<Data.RfidTag>();
            FirstName = string.Empty;
            LastName = string.Empty;
            TagId = string.Empty;
        }

        public async Task OnGetAsync()
        {
            AvailableTags = await _db.RfidTags.Include(r => r.Student).Where(r => r.Student == null).ToListAsync();
        }

        public async Task<ActionResult> OnPostAsync()
        {
            Data.RfidTag? tag = null;
            if (!string.IsNullOrEmpty(TagId))
            {
                tag = await _db.RfidTags.Include(r => r.Student).FirstOrDefaultAsync(r => r.Id == TagId);
                if (tag is null)
                {
                    ModelState.AddModelError(nameof(TagId), "RFID tag doest not exist");
                } else if (tag.Student != null)
                {
                    ModelState.AddModelError(nameof(TagId), "RFID tag is already assigned");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var student = new Data.Student()
            {
                Id = Guid.NewGuid(),
                FirstName = FirstName,
                LastName = LastName
            };

            if (tag != null)
            {
                student.RfidTag = tag;
            }

            _db.Add(student);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Student/Index");
        }
    }
}
