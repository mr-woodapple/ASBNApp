using MudBlazor;

public static class DefaultTheme
{
    public static MudTheme GetTheme()
    {
        return new MudTheme
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.Orange.Accent4,
                AppbarBackground = Colors.Orange.Accent4,
                AppbarText = Colors.Shades.White,
            },
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Orange.Accent4,
            },
            Typography = new Typography()
            {
                Default = new DefaultTypography()
                {
                    FontFamily = ["Inter"]
                }
            }
        };
    }
}