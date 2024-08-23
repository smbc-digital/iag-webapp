namespace StockportWebapp.Models.Mappers;

public static class CssClassMapper
{
    public static string GetCssClass(EColourScheme colourScheme)
        => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue => "-blue",
            EColourScheme.Blue_Light => "-blue-light",
            EColourScheme.Green => "-green",
            EColourScheme.Green_Light => "-green-light",
            EColourScheme.Grey => "-grey",
            EColourScheme.Grey_Light => "-grey-light",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink => "-pink",
            EColourScheme.Pink_Light => "-pink-light",
            EColourScheme.Purple => "-purple",
            EColourScheme.Purple_Light => "-purple-light",
            EColourScheme.Teal => "-teal",
            EColourScheme.Teal_Light => "-teal-light",
            _ => "-teal"
        };

    public static string GetBoldCssClass(EColourScheme colourScheme)
        => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue => "-blue",
            EColourScheme.Blue_Light => "-blue",
            EColourScheme.Green => "-green",
            EColourScheme.Green_Light => "-green",
            EColourScheme.Grey => "-grey",
            EColourScheme.Grey_Light => "-grey",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink => "-pink",
            EColourScheme.Pink_Light => "-pink",
            EColourScheme.Purple => "-purple",
            EColourScheme.Purple_Light => "-purple",
            EColourScheme.Teal => "-teal",
            EColourScheme.Teal_Light => "-teal",
            _ => "-teal"
        };
}