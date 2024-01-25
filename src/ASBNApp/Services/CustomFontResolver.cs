// required implementation of the IFontResolver, required to use custom fonts within PDFSharpCore

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


    public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        // TODO: Not really sure this could be used in a smarter way
        return new FontResolverInfo("CourierNew.ttf");
    }
}