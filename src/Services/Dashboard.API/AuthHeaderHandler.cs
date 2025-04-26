using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
