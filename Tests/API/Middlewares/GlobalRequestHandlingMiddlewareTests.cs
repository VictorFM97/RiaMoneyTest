using API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Tests.API.Middleware;

public class GlobalRequestHandlingMiddlewareTests
{
    private readonly Mock<ILogger<GlobalRequestHandlingMiddleware>> _logger;

    public GlobalRequestHandlingMiddlewareTests()
    {
        _logger = new Mock<ILogger<GlobalRequestHandlingMiddleware>>();
    }

    [Fact]
    public async void InvokeAsync_ShouldReturn500IfGenericExceptionOccurs()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Path = "/Customers";

        var middleware = new GlobalRequestHandlingMiddleware(_logger.Object);
        await middleware.InvokeAsync(httpContext, RequestDelegateFactory.Create(() => { throw new Exception("error"); }).RequestDelegate);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        httpContext.Response.ContentType.Should().Be("application/json");
    }

    [Fact]
    public async void InvokeAsync_ShouldBe200IfNoExceptionsHappened()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "GET";
        httpContext.Request.Path = "/Customers";

        var middleware = new GlobalRequestHandlingMiddleware(_logger.Object);
        await middleware.InvokeAsync(httpContext, RequestDelegateFactory.Create(() => { }).RequestDelegate);

        httpContext.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
