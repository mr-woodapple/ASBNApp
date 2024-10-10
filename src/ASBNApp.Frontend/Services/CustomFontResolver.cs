/// <summary>
/// Required implementation of the IFontResolver, required to use custom fonts within PDFSharp
/// </summary>

using PdfSharp.Fonts;

public class CustomFontResolver : IFontResolver
{
    private readonly Fonts _fontLoaded;

    public CustomFontResolver(Fonts fontLoaded)
    {
        _fontLoaded = fontLoaded;
    }

    // Set the default font
    public string DefaultFontName => "CourierNew";

    public byte[] GetFont(string fontFaceName)
    {
        return fontFaceName switch
        {
            "CourierNew.ttf" => _fontLoaded.CourierNew,
            _ => _fontLoaded.CourierNew // used if there's nothing here?
        };
    }


    // Needs to be updated if we have more than one option in the family
    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        return new FontResolverInfo("CourierNew.ttf");
    }
}