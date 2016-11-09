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
            if (setting == null) return new AppSetting();
            return new AppSetting(setting);
        }

        public override string ToString()
        {
            return _value;
        }
    }
}