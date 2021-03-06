using ApiAsamblea.Models;
using ApiAsamblea.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAsamblea.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }

        public DbSet<Asambleista> Asambleista { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
