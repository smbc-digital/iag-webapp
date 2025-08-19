namespace StockportWebapp.Models.Mappers;

public static class CssClassMapper
{
    public static string GetCssClass(EColourScheme colourScheme) => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue => "-blue",
            EColourScheme.Dark_Overlay => "-dark",
            EColourScheme.Green => "-green",
            EColourScheme.Grey => "-grey",
            EColourScheme.Light_Overlay => "-light",
            EColourScheme.Multi => "-multi",
            EColourScheme.One_Stockport_Purple => "-one-stockport-purple",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink => "-pink",
            EColourScheme.Purple => "-purple",
            EColourScheme.Teal => "-teal",
            _ => "-teal"
        };

    public static string GetBoldCssClass(EColourScheme colourScheme) => colourScheme switch
        {
            EColourScheme.None => "-none",
            EColourScheme.Blue => "-blue",
            EColourScheme.Dark_Overlay => "-dark",
            EColourScheme.Green => "-green",
            EColourScheme.Grey => "-grey",
            EColourScheme.Light_Overlay => "-light",
            EColourScheme.Multi => "-multi",
            EColourScheme.One_Stockport_Purple => "-one-stockport-purple",
            EColourScheme.Orange => "-orange",
            EColourScheme.Pink => "-pink",
            EColourScheme.Purple => "-purple",
            EColourScheme.Teal => "-teal",
            _ => "-teal"
        };
}