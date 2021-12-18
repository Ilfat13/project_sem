using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using project_sem.Database;
using project_sem.Models;

namespace project_sem.Pages
{
    public class Index1Model : PageModel
    {
        public IActionResult OnGet()
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

        public async Task<ActionResult> OnPost(string email, string username, string password)
        {
            
                if (ModelState.IsValid)
                {
                    var users = await MyDatabase.GetAllUsers();
                    var user = users.FirstOrDefault(u => u.email == email || u.username == username);
                    if (user == null)
                    {
                        var currentUser = new User
                        {
                            id = Guid.NewGuid(),
                            email = email,
                            password = Encryption.EncryptString(password),
                            username = username,
                        };
                        var profile = new Models.Profile()
                        {
                            Id = currentUser.id,
                        };
                        currentUser.Profile = profile;

                        await MyDatabase.Add(currentUser);
                        await MyDatabase.Add(profile);

                    HttpContext.Session.Clear();
                    HttpContext.Session.SetString("Id", currentUser.id.ToString());
                    }
                    else
                        ModelState.AddModelError("", $"Пользователь с такой почтой или именем пользователя  уже зарегистрирован.");
                }
            
            return RedirectToPage("/Register");
        }







        //public void OnPost(string username, string password, string email)
        //{
        //    user.id = Guid.NewGuid().ToString();
        //    user.username = username;
        //    user.password = password;
        //    user.email = email;
        //    string connString = "User ID=postgres; Server=localhost; port=5433; Database=project; Password=1234;";
        //    NpgsqlConnection npgsql = new NpgsqlConnection(connString);
        //    try
        //    {
        //        npgsql.Open();
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    NpgsqlCommand npgc = new NpgsqlCommand($"Insert into \"users\" values ('{user.id}','{user.username}','{user.password}' , '{user.email}')", npgsql);
        //    int exec = npgc.ExecuteNonQuery();
        //    npgsql.Close();


        //}
    }
}
