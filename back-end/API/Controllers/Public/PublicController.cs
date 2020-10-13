using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.Supervision;

namespace API.Controllers.Public
{
    public partial class PublicController : PublicControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIntroductionRepository _introductionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISupervisor _supervisor;

        public PublicController(ISupervisor supervisor, ICategoryRepository categoryRepository, IIntroductionRepository introductionRepository, IProjectRepository projectRepository)
        {
            _supervisor = supervisor;
            _categoryRepository = categoryRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
        }
    }
}
