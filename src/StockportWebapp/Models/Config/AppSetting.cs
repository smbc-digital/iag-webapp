namespace StockportWebapp.Models.Config;

public class AppSetting
{
    private readonly string _value;

    private AppSetting(string value = "") =>
        _value = value;

    public bool IsValid() =>
        !_value.Equals(string.Empty);

    public static AppSetting GetAppSetting(string setting) =>
        setting is null
            ? new AppSetting()
            : new AppSetting(setting);

    public override string ToString() =>
        _value;
}