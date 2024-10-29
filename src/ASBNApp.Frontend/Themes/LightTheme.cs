using MudBlazor;

public static class LightTheme
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

            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontFamily = ["Inter"]
                }
            }
        };
    }
}