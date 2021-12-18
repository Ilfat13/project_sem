using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project_sem.Database;
using project_sem.Models;

namespace project_sem.Pages
{
    public class ProfileModel : PageModel
    {
        public Profile profile{ get; set; }
        public User user { get; set; }
        public async Task<ActionResult> OnGetAsync()
        {
            if (HttpContext.Session.Keys.Contains("Id"))
            {
                Guid id = Guid.Parse(HttpContext.Session.GetString("Id"));
                var users = await MyDatabase.GetAllUsers();
                var user = users.FirstOrDefault(u => u.id == id);
                this.user = user;
                var profiles = await MyDatabase.GetAllProfiles();
                var profile = profiles.FirstOrDefault(p => p.Id == id);
                this.profile = profile;

                return Page();
            }
            else
            {
                return RedirectToPage("/authorization");
            }
        }
        public async Task<ActionResult> OnPostAsync(string City, string Description)
        {
            Guid id = Guid.Parse(HttpContext.Session.GetString("Id"));
            var users = await MyDatabase.GetAllUsers();
            var user = users.FirstOrDefault(u => u.id == id);
            this.user = user;
            var profiles = await MyDatabase.GetAllProfiles();
            var profile = profiles.FirstOrDefault(p => p.Id == id);
            profile.City = City;
            profile.Description = Description;
            MyDatabase.Update(profile);
            return RedirectToPage("/Profile");
        }
    }
}
