using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Supervision;
using Microsoft.Extensions.Configuration;

namespace API.Controllers.Private
{
    public partial class PrivateController : PrivateControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IFileRepository _fileRepository;
        private readonly IIntroductionRepository _introductionRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISupervisor _supervisor;
        private readonly ITokenManager _tokenManager;

        public PrivateController(IAccountRepository accountRepository, ICategoryRepository categoryRepository, IConfiguration configuration, IFileRepository fileRepository, IIntroductionRepository introductionRepository, IProjectRepository projectRepository, ISupervisor supervisor, ITokenManager tokenManager)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _fileRepository = fileRepository;
            _introductionRepository = introductionRepository;
            _projectRepository = projectRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
        }
    }
}
