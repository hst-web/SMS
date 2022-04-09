/*----------------------------------------------------------------

// 文件名：UnityContainerFactory.cs
// 功能描述：Unity容器工厂类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Collections.Concurrent;

namespace Canve.ESH.Core
{
    /// <summary>
    /// Unity容器工厂类
    /// </summary>
    public class UnityContainerFactory
    {
        static object _lockobj = new object();
        /// <summary>
        /// 容器集合<容器名，容器>
        /// </summary>
        static ConcurrentDictionary<string, IUnityContainer> _containers = null;

        /// <summary>
        /// 取得容器
        /// </summary>
        /// <param name="containerName">容器名</param>
        /// <returns></returns>
        public static IUnityContainer GetContainer(string containerName = "")
        {
            if (_containers == null)
            {
                lock (_lockobj)
                {
                    if (_containers == null)
                    {
                        _containers = new ConcurrentDictionary<string, IUnityContainer>();

                        //加载所有Unity容器配置
                        LoadUnityContainers();
                    }
                }
            }

            if (_containers.ContainsKey(containerName))
            {
                return _containers[containerName];
            }

            return null;
        }

        /// <summary>
        /// 加载所有容器
        /// </summary>
        static void LoadUnityContainers()
        {
            UnityConfigurationSection section
                = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            var container = new UnityContainer();
            foreach (var item in section.Containers)
            {
                container.LoadConfiguration(section, item.Name);
                _containers.TryAdd(item.Name, container);
            }

        }
    }
}
