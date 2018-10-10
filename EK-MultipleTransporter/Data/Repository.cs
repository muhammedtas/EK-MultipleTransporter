using EK_MultipleTransporter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EK_MultipleTransporter.Data
{
    public class ProjectRepo : RepositoryBase<Project, int>
    {
        public Project GetByName(string name )
        {
            OTCSDbContext db = new OTCSDbContext();
            return db.Projects.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
