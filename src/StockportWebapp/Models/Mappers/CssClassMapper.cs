namespace StockportWebapp.Models.Mappers;

public static class CssClassMapper
{
    public static string GetCssClass(EColourScheme colourScheme) => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue => "-blue",
            EColourScheme.Blue_Light => "-blue-light",
            EColourScheme.Dark_Overlay => "-dark",
            EColourScheme.Green => "-green",
            EColourScheme.Green_Light => "-green-light",
            EColourScheme.Grey => "-grey",
            EColourScheme.Grey_Light => "-grey-light",
            EColourScheme.Light_Overlay => "-light",
            EColourScheme.Multi => "-multi",
            EColourScheme.One_Stockport_Purple => "-one-stockport-purple",
            EColourScheme.One_Stockport_Purple_Light => "-one-stockport-purple-light",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink => "-pink",
            EColourScheme.Pink_Light => "-pink-light",
            EColourScheme.Purple => "-purple",
            EColourScheme.Purple_Light => "-purple-light",
            EColourScheme.Teal => "-teal",
            EColourScheme.Teal_Light => "-teal-light",
            _ => "-teal"
        };

    public static string GetBoldCssClass(EColourScheme colourScheme) => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue or EColourScheme.Blue_Light => "-blue",
            EColourScheme.Dark_Overlay => "-dark",
            EColourScheme.Green or EColourScheme.Green_Light => "-green",
            EColourScheme.Grey or EColourScheme.Grey_Light => "-grey",
            EColourScheme.Light_Overlay => "-light",
            EColourScheme.Multi => "-multi",
            EColourScheme.One_Stockport_Purple or EColourScheme.One_Stockport_Purple_Light => "-one-stockport-purple",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink or EColourScheme.Pink_Light => "-pink",
            EColourScheme.Purple or EColourScheme.Purple_Light => "-purple",
            EColourScheme.Teal or EColourScheme.Teal_Light => "-teal",
            _ => "-teal"
        };
}