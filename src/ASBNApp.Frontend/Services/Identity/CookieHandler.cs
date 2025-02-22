using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace ASBNApp.Frontend.Services.Identity;

/// <summary>
/// Handler to ensure cookie credentials are automatically sent over with each request.
/// </summary>
public class CookieHandler : DelegatingHandler
{
	/// <summary>
	/// Main method to override for the handler.
	/// </summary>
	/// <param name="request">The original request.</param>
	/// <param name="cancellationToken">The token to handle cancellations.</param>
	/// <returns>The <see cref="HttpResponseMessage"/>.</returns>
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		// Include cookies for each request!
		request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
		request.Headers.Add("X-Requested-With", ["XMLHttpRequest"]);
		
		// OData property, sets the preference what data should be returned on a PATCH request
		request.Headers.Add("Prefer", "return=representation");

		return base.SendAsync(request, cancellationToken);
	}
}
