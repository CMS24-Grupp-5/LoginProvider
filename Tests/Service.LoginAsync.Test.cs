using Xunit;
using Moq;
using System.Threading.Tasks;
using Grpc.Core;
using Presentation.Services;
using Presentation.Models;
using static Presentation.AccountGrpcService;
using Presentation;

namespace Tests;

public class ServiceTests
{
    private static AsyncUnaryCall<T> CreateAsyncUnaryCall<T>(T response)
    {
        return new AsyncUnaryCall<T>(
            Task.FromResult(response),
            Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess,
            () => new Metadata(),
            () => { }
        );
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        var mockClient = new Mock<AccountGrpcServiceClient>();

        var grpcResponse = new ValidateCredentialsReply
        {
            Succeeded = true,
            Message = "Login succeeded",
            UserId = "123"
        };

        mockClient
            .Setup(x => x.ValidateCredentialsAsync(
                It.IsAny<ValidateCredentialsRequest>(),
                null, null, default))
            .Returns(CreateAsyncUnaryCall(grpcResponse));

        var service = new Service(mockClient.Object);

        var loginForm = new LoginForm
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var result = await service.LoginAsync(loginForm);

        Assert.True(result.Success);
        Assert.Equal("Login succeeded", result.Message);
        Assert.Equal("123", result.UserId);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidCredentials_ReturnsFailure()
    {
        var mockClient = new Mock<AccountGrpcServiceClient>();

        var grpcResponse = new ValidateCredentialsReply
        {
            Succeeded = false,
            Message = "Invalid credentials"
        };

        mockClient
            .Setup(x => x.ValidateCredentialsAsync(
                It.IsAny<ValidateCredentialsRequest>(),
                null, null, default))
            .Returns(CreateAsyncUnaryCall(grpcResponse));

        var service = new Service(mockClient.Object);

        var loginForm = new LoginForm
        {
            Email = "wrong@example.com",
            Password = "wrongpassword"
        };

        var result = await service.LoginAsync(loginForm);

        Assert.False(result.Success);
        Assert.Equal("Invalid credentials", result.Message);
        Assert.Null(result.UserId);
    }

    [Fact]
    public async Task LoginAsync_WhenGrpcThrows_ReturnsErrorMessage()
    {
        var mockClient = new Mock<AccountGrpcServiceClient>();

        mockClient
            .Setup(x => x.ValidateCredentialsAsync(
                It.IsAny<ValidateCredentialsRequest>(),
                null, null, default))
            .Throws(new RpcException(new Status(StatusCode.Internal, "gRPC error")));

        var service = new Service(mockClient.Object);

        var loginForm = new LoginForm
        {
            Email = "test@example.com",
            Password = "password123"
        };

        var result = await service.LoginAsync(loginForm);

        Assert.False(result.Success);
        Assert.Equal("An error occurred while processing your request.", result.Message);
        Assert.Null(result.UserId);
    }
}
