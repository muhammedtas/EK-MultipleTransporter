using EK_MultipleTransporter.Model;
using System.Linq;

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

    public class StaffRepo : RepositoryBase<Staff, int>
    {
        public Staff GetByName(string name)
        {
            var db = new OtcsDbContext();
            return db.Staffs.FirstOrDefault(x => x.Name == name);
        }
    }
}
