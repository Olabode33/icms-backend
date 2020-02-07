namespace ICMSDemo
{
    public class ICMSDemoConsts
    {
        public const string LocalizationSourceName = "ICMSDemo";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;

        public const bool AllowTenantsToChangeEmailSettings = false;

        public const string Currency = "NGN";

        public const string CurrencySign = "₦";

        public const string AbpApiClientUserAgent = "AbpApiClient";
    }


    public enum Severity
    {
        Low, Medium, High
    }

    public enum Status
    {
        Open, Closed
    }

    public enum ControlType
    {
        Manual, Automated, ITDependent
    }

    public enum DataTypes
    {
        String, Number, Bool, Date, List
    }




    public enum Frequency
    {
        Daily, Weekly, Monthly, Quarterly, Yearly
    }
}