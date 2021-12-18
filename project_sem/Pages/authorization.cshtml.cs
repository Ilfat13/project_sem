using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project_sem.Database;

namespace project_sem.Pages
{
    public class authorizationModel : PageModel
    {
        public ActionResult OnGet()
        {
            if (HttpContext.Session.Keys.Contains("Id"))
            {
                return RedirectToPage("/Profile");
            }
            else
            {
                return Page();
            }
        }
        public async Task<ActionResult> OnPost(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var users = await MyDatabase.GetAllUsers();
                var user = users.FirstOrDefault(u => (u.email == username || u.username == username) && Encryption.DecryptString(u.password) == password);
                if (user != null)
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("Id", user.id.ToString());
                }
                else
                    ModelState.AddModelError("", $"Данные введены некорректно.");
            }
            return RedirectToPage("/authorization");
        }
    }
}
