using EK_MultipleTransporter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Data
{
    public class OtcsDbContext : DbContext
    {
        public OtcsDbContext()
           : base("name=OTCSCnnStr")
        { }
        public virtual DbSet<IndependentSection> IndependentSections { get; set; }
        public virtual DbSet<Litigation> Litigations { get; set; }
        public virtual DbSet<Plot> Plots { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
    }
}
