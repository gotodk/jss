using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;

namespace 客户端主程序.Support
{
    public class NetLink
    {
        /// <summary>
        /// 打开浏览器并访问网址
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="openInNewWindow"></param>
        public static void OpenLink(string URL, bool openInNewWindow)
        {
            try
            {
                /// Reads path of default browser from registry   
                string key = @"http\shell\open\command";
                RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
                // get default browser path. e.g. "C:\Program Files\Internet Explorer\iexplore.exe" -nohome   
                string defaultBrowserPath = ((string)registryKey.GetValue(null, null)).Split('"')[1];
                // launch default browser   
                if (openInNewWindow)
                {
                    Process p = new Process();
                    p.StartInfo.FileName = defaultBrowserPath;
                    p.StartInfo.Arguments = URL;
                    p.Start();
                }
                else
                {
                    Process.Start(defaultBrowserPath, URL);
                }
            }
            catch (Win32Exception)
            {
                try
                {
                    System.Diagnostics.Process.Start(URL);
                }
                catch (Exception exc2)
                {
                    // still nothing we can do so just log the error for a miracle.   
                }
            }
        }

    }
}
