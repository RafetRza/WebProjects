using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext.Entities
{
    public class AppUser : IdentityUser
    {
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}
