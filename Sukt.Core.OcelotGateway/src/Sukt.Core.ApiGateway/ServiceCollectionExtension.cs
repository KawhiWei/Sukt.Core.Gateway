using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Sukt.Core.ApiGateway
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 得到操作设置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>

        public static AppOptionSettings GetAppSettings(this IServiceCollection services)
        {
            //services.NotNull(nameof(services));
            return services.GetService<IOptions<AppOptionSettings>>()?.Value;
        }
        /// <summary>
        /// 得到注入服务
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TType GetService<TType>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            return provider.GetService<TType>();
        }
        /// <summary>
        /// 是否为空或者为null
        /// </summary>
        /// <param name="value">要判断的值</param>
        /// <returns>返回true/false</returns>
        public static bool IsNullOrEmpty(this string value)
        {

            return string.IsNullOrEmpty(value);
        }
    }
}
