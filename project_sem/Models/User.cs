using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_sem.Models
{
    public class User
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }

        public Profile Profile { get; set; }
    }
}
