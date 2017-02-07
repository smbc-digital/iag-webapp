namespace StockportWebapp.Config
{
    public class AppSetting
    {
        private readonly string _value;

        private AppSetting(string value = "")
        {
            _value = value;
        } 

        public bool IsValid()
        {
            return _value != "";
        }

        public static AppSetting GetAppSetting(string setting)
        {
            return setting == null ? new AppSetting() : new AppSetting(setting);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}