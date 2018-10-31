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
        public Project GetByName(string name)
        {
            var db = new OtcsDbContext();
            return db.Projects.FirstOrDefault(x => x.Name == name);
        }
    }
}
