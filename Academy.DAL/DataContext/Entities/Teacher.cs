using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext.Entities
{
    public class Teacher : Entity
    {
        public string Name { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;

        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
