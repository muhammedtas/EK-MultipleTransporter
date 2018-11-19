using System.Linq;
using EK_MultipleTransporter.Models.RootModel;

namespace EK_MultipleTransporter.Data
{
    public class ProjectRepo : RepositoryBase<Project, int> {}
    public class StaffRepo : RepositoryBase<Staff, int>  {}
    public class PlotRepo  : RepositoryBase<Plot, int> { }
    public class LitigationRepo : RepositoryBase<Litigation, int> { }
    public class IndependentSectionRepo : RepositoryBase<IndependentSection, int> { }
}
