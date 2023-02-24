using Microsoft.EntityFrameworkCore;
using SmartStartWebAPI.Domain;
using System.Collections.Generic;

namespace SmartStartWebAPI.Infrastructure
{
    public class StudentsDBContext : DbContext
    {
        public StudentsDBContext(DbContextOptions<StudentsDBContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }

    }
}