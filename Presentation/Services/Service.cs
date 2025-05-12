using Presentation.Models;

namespace Presentation.Services;

public interface IService
{
    Task<LoginResult> LoginAsync(LoginForm formData);
}

public class Service(AccountGrpcService.AccountGrpcServiceClient accountClient) : IService
{
    private readonly AccountGrpcService.AccountGrpcServiceClient _accountClient = accountClient;
    public async Task<LoginResult> LoginAsync(LoginForm formData)
    {
        var request = new ValidateCredentialsRequest
        {
            Email = formData.Email,
            Password = formData.Password
        };
        try
        {
            var result = await _accountClient.ValidateCredentialsAsync(request);
            return result.Succeeded ? new LoginResult
            {
                Success = result.Succeeded,
                Message = result.Message,
                UserId = result.UserId
            } : new LoginResult
            {
                Success = result.Succeeded,
                Message = result.Message,
            };
        }
        catch
        {
            return new LoginResult
            {
                Success = false,
                Message = "An error occurred while processing your request."
            };
        }
    }
}
