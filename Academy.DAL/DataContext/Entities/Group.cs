using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.DAL.DataContext.Entities
{
    public class Group : Entity
    {
        public string Name { get; set; } = null!;

        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
