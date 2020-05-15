using AEM_Enersol_Assessment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AEM_Enersol_Assessment.Data
{
    public class AEMContext : DbContext
    {

        public AEMContext(DbContextOptions<AEMContext> options)
        : base(options)
        { }

        public DbSet<Well> Well { get; set; }
        public DbSet<Platform> Platform { get; set; }
    }
}
