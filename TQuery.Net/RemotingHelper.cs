using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;

namespace TQuery.Net
{
    /// <summary>
    /// Remoting操作类,注意:本类在实际开发中无用,将通过配置文件完成所有功能！
    /// </summary>    
    public class RemotingHelper
    {
        #region 创建一个Tcp通道
        /// <summary>
        /// 创建一个Tcp通道,用于Remoting服务器
        /// </summary>
        /// <param name="port">监听的端口</param>        
        public static TcpChannel CreateChannel_Tcp(int port)
        {
            return new TcpChannel(port);
        }

        /// <summary>
        /// 创建一个Tcp通道,用于Remoting客户端
        /// </summary>        
        public static TcpChannel CreateChannel_Tcp()
        {
            return new TcpChannel();
        }
        #endregion

        #region 创建一个Http通道
        /// <summary>
        /// 创建一个Http通道,用于Remoting服务器
        /// </summary>
        /// <param name="port">监听的端口</param>        
        public static HttpChannel CreateChannel_Http(int port)
        {
            return new HttpChannel(port);
        }

        /// <summary>
        /// 创建一个Http通道,用于Remoting客户端
        /// </summary>        
        public static HttpChannel CreateChannel_Http()
        {
            return new HttpChannel();
        }
        #endregion

        #region 注册一个通道
        /// <summary>
        /// 注册一个Tcp通道
        /// </summary>
        /// <param name="channel">Tcp通道对象</param>
        public static void RegisterChannel(TcpChannel channel)
        {
            ChannelServices.RegisterChannel(channel, false);
        }

        /// <summary>
        /// 注册一个Http通道
        /// </summary>
        /// <param name="channel">Http通道对象</param>
        public static void RegisterChannel(HttpChannel channel)
        {
            ChannelServices.RegisterChannel(channel, false);
        }
        #endregion

        #region 创建并注册一个Tcp通道
        /// <summary>
        /// 创建并注册一个Tcp通道,用于Remoting服务器端
        /// </summary>
        /// <param name="port">监听的端口</param>
        public static void CreateAndRegisterChannel_Tcp(int port)
        {
            //创建一个Tcp通道
            TcpChannel channel = new TcpChannel(port);

            //注册通道
            ChannelServices.RegisterChannel(channel, true);
        }

        /// <summary>
        /// 创建并注册一个Tcp通道,用于Remoting客户端
        /// </summary>
        public static void CreateAndRegisterChannel_Tcp()
        {
            //创建一个Tcp通道
            TcpChannel channel = new TcpChannel();

            //注册通道
            ChannelServices.RegisterChannel(channel, true);
        }
        #endregion

        #region 创建并注册一个Http通道
        /// <summary>
        /// 创建并注册一个Http通道,用于Remoting服务器端
        /// </summary>
        /// <param name="port">监听的端口</param>
        public static void CreateAndRegisterChannel_Http(int port)
        {
            //创建一个Http通道
            HttpChannel channel = new HttpChannel(port);

            //注册通道
            ChannelServices.RegisterChannel(channel, false);
        }

        /// <summary>
        /// 创建并注册一个Http通道,用于Remoting客户端
        /// </summary>
        public static void CreateAndRegisterChannel_Http()
        {
            //创建一个Http通道
            HttpChannel channel = new HttpChannel();

            //注册通道
            ChannelServices.RegisterChannel(channel, false);
        }
        #endregion

        #region 注册服务端激活的对象
        /// <summary>
        /// 注册服务端激活的单一实体对象，特点：所有客户端共享该对象。
        /// </summary>
        /// <typeparam name="T">注册对象的类名</typeparam>
        /// <param name="objUri">注册的远程对象的URI标识符</param>
        public static void RegisterServerObject_Singleton<T>(string objUri)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(T), objUri, WellKnownObjectMode.Singleton);

        }

        /// <summary>
        /// 注册服务端激活的单一调用对象，特点：为每个客户端创建一个实例。
        /// </summary>
        /// <typeparam name="T">注册对象的类名</typeparam>
        /// <param name="objUri">注册的远程对象的URI标识符</param>
        public static void RegisterServerObject_SingleCall<T>(string objUri)
        {
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(T), objUri, WellKnownObjectMode.SingleCall);

        }
        #endregion

        #region 注册客户端激活的服务器对象
        /// <summary>
        /// 注册客户端激活的服务器对象
        /// </summary>
        /// <typeparam name="T">要注册的服务器对象的类名</typeparam>
        /// <param name="appName">远程处理应用程序名,即Uri</param>
        public static void RegisterServerObject_ClientActivated<T>(string appName)
        {
            //设置远程处理应用程序名
            RemotingConfiguration.ApplicationName = appName;

            //注册客户端激活的服务器对象
            RemotingConfiguration.RegisterActivatedServiceType(typeof(T));
        }
        #endregion

        #region 创建远程对象的代理对象
        /// <summary>
        /// 创建远程对象的代理对象
        /// </summary>
        /// <typeparam name="T">远程对象类</typeparam>
        /// <param name="objUrl">远程对象的URL地址</param>        
        public static T CreateProxy<T>(string objUrl)
        {
            return ReflectionHelper.CreateProxy<T>(objUrl);
        }
        #endregion
    }
}
