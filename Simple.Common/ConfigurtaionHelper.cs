using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace Simple.Common
{
    public class ConfigurtaionHelper
    {
        //private static readonly object locker = new object ();
        //单例
        //private static ConfigurtaionHelper _instance;

        public static ConfigurtaionHelper Instance
        {
            get
            {
                // lock (locker) {
                //     if (_instance == null)
                //         _instance = new ConfigurtaionHelper ();
                // }
                // return _instance;
                return SingletonHolder.Instance;
            }
        }

        private static class SingletonHolder
        {
            internal static readonly ConfigurtaionHelper Instance = new ConfigurtaionHelper();
        }

        /// <summary>
        /// 获取配置文件对象
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <returns></returns>
        public IConfiguration GetConfiguration(string configPath = "appsettings.json")
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();
            return config;
        }

        /// <summary>
        /// 获取自定义配置文件配置
        /// </summary>
        /// <typeparam name="T">配置模型</typeparam>
        /// <param name="key">根节点</param>
        /// <param name="configPath">配置文件名称</param>
        /// <returns></returns>
        public T GetAppSettings<T>(string key, string configPath = "siteconfig.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();

            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return appconfig;
        }

        /// <summary>
        /// 获取自定义配置文件配置（异步方式）
        /// </summary>
        /// <typeparam name="T">配置模型</typeparam>
        /// <param name="key">根节点</param>
        /// <param name="configPath">配置文件名称</param>
        /// <returns></returns>
        public async Task<T> GetAppSettingsAsync<T>(string key, string configPath = "siteconfig.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();

            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<T>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;

            return await Task.Run(() => appconfig);
        }

        /// <summary>
        /// 获取自定义配置文件配置
        /// </summary>
        /// <typeparam name="T">配置模型</typeparam>
        /// <param name="key">根节点</param>
        /// <param name="configPath">配置文件名称</param>
        /// <returns></returns>
        public List<T> GetListAppSettings<T>(string key, string configPath = "siteconfig.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();

            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<List<T>>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<List<T>>>()
                .Value;

            return appconfig;
        }
        /// <summary>
        /// 获取自定义配置文件配置（异步方式）
        /// </summary>
        /// <typeparam name="T">配置模型</typeparam>
        /// <param name="key">根节点</param>
        /// <param name="configPath">配置文件名称</param>
        /// <returns></returns>
        public async Task<List<T>> GetListAppSettingsAsync<T>(string key, string configPath = "siteconfig.json") where T : class, new()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();

            var appconfig = new ServiceCollection()
                .AddOptions()
                .Configure<List<T>>(config.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<List<T>>>()
                .Value;

            return await Task.Run(() => appconfig);
        }
    }
}