namespace Sukt.Core.ApiGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class AppOptionSettings
    {
        public CorsOptions Cors { get; set; }
    }
    /// <summary>
    /// Cors操作
    /// </summary>
    public class CorsOptions
    {

        /// <summary>
        /// 策略名
        /// </summary>
        public string PolicyName { get; set; }

        /// <summary>
        /// Cors地址
        /// </summary>
        public string Url { get; set; }
    }
}
