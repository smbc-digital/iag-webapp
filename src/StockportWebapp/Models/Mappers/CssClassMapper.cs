namespace StockportWebapp.Models.Mappers;

public static class CssClassMapper
{
    public static string GetCssClass(EColourScheme colourScheme)
        => colourScheme switch
        {
            EColourScheme.None => "--teal",
            EColourScheme.Blue => "--blue",
            EColourScheme.BlueLight => "--blue-light",
            EColourScheme.Green => "--green",
            EColourScheme.GreenLight => "--green-light",
            EColourScheme.Orange => "--orange",
            EColourScheme.Pink => "--pink",
            EColourScheme.PinkLight => "--pink-light",
            EColourScheme.Purple => "--purple",
            EColourScheme.PurpleLight => "--purple-light",
            EColourScheme.Teal => "--teal",
            EColourScheme.TealLight => "--teal-light",
            _ => "--teal"
        };
}