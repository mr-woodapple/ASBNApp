using MudBlazor;

public static class LightTheme
{
    public static MudTheme GetTheme()
    {
        return new MudTheme
        {
            Palette = new PaletteLight
            {     
                AppbarText = Colors.Shades.White
            }
        };
    }
}