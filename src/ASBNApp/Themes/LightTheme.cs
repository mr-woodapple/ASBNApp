using MudBlazor;

public static class LightTheme
{
    public static MudTheme GetTheme()
    {
        return new MudTheme
        {
            Palette = new PaletteLight
            {
                Primary = Colors.Orange.Accent4,
                AppbarBackground = Colors.Orange.Accent4,
                AppbarText = Colors.Shades.White
            }
        };
    }
}