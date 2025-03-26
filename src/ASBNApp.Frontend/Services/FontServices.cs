using ASBNApp.Frontend.Models;

/// <summary>
/// Service to load custom fonts, required because we don't have access to fonts from the 
/// browser -> we need to load in any font we need, as in this case
/// </summary>
public class FontServices
{
	// Registering the httpClient
	private readonly HttpClient _httpClient;

	public FontServices(IHttpClientFactory httpClientFactory)
			=> _httpClient = httpClientFactory.CreateClient("FrontendClient");

	/// <summary>
	/// Wrapper to handle loading fonts
	/// </summary>
	/// <returns>Fonts object including all available fonts</returns>
	public async Task<Fonts> LoadFonts()
    {
        Fonts fonts = new Fonts()
        {
            CourierNew = await GetFontData("CourierNew.ttf")
        };

        return fonts;
    }


    /// <summary>
    /// Load the actual font file via an http request from our wwwroot folder,
    /// return it as a bytestream
    /// </summary>
    /// <param name="name">Name of the font</param>
    /// <returns>Font data as a Bytestream</returns>
    private async Task<byte[]> GetFontData(string name)
    {
        var sourceStream = await _httpClient.GetStreamAsync($"fonts/{name}");
        using MemoryStream memoryStream = new();

        sourceStream.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }

}