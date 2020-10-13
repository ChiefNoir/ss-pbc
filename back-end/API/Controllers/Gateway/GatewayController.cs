using Abstractions.API;
using Abstractions.IRepository;
using Abstractions.ISecurity;
using Abstractions.Supervision;
using Microsoft.Extensions.Configuration;

namespace API.Controllers.Gateway
{
    public partial class GatewayController : GatewayControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;
        private readonly ISupervisor _supervisor;
        private readonly ITokenManager _tokenManager;

        public GatewayController(IConfiguration configuration, IAccountRepository accountRepository, ISupervisor supervisor, ITokenManager tokenManager)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _supervisor = supervisor;
            _tokenManager = tokenManager;
        }

    }
}
