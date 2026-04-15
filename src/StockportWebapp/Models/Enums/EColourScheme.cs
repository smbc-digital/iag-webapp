using static System.Net.Mime.MediaTypeNames;

namespace StockportWebapp.Models.Enums;

public enum EColourScheme
{  
    None,
    Blue,
    Dark_Overlay,
    Green,
    Grey,
    Light_Overlay,
    Multi,
    One_Stockport_Purple,
    Orange,
    Pink,
    Purple,
    Teal,
    OS_Lilac,
    OS_Pink,
    OS_Purple,
    OS_Multi,
    OS_Teal,
    OS_Yellow
}

public static class EColourSchemeExtensions
{
    public static bool IsOSStyle(this EColourScheme eColourScheme)
    {
        if (eColourScheme == EColourScheme.OS_Lilac
            || eColourScheme == EColourScheme.OS_Pink
            || eColourScheme == EColourScheme.OS_Purple
            || eColourScheme == EColourScheme.OS_Multi
            || eColourScheme == EColourScheme.OS_Teal
            || eColourScheme == EColourScheme.OS_Yellow)
            return true;

        return false;
    }
}