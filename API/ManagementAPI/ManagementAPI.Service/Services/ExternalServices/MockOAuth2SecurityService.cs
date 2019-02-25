using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Services.DataTransferObjects;

namespace ManagementAPI.Service.Services
{
    public class MockOAuth2SecurityService : IOAuth2SecurityService
    {
        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            return new RegisterUserResponse
            {
                UserId = Guid.NewGuid()
            };
        }
    }
}