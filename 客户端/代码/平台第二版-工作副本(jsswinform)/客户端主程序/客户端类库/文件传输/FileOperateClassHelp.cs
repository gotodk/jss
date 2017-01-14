using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using 公用通讯协议类库.共用类库;

namespace 客户端主程序通讯类库.客户端类库.文件传输
{
    /// <summary>
    /// 用于支持文件操作类中的常用操作
    /// </summary>
    class FileOperateClassHelp
    {
        /// <summary>
        /// 获取磁盘分区信息
        /// </summary>
        /// <returns></returns>
        public FilesInfoCollection GetPathAllInfo_Disk()
        {
            try
            {
                FilesInfoCollection FIC = new FilesInfoCollection();

                DriveInfo[] drive = DriveInfo.GetDrives();
                for (int i = 0; i < drive.Length; i++)
                {
                    if (drive[i].IsReady)
                    {

                        FilesInfo FI = new FilesInfo();
                        FI.p_AllPath = drive[i].Name;
                        FI.p_Name = drive[i].Name;
                        FI.p_Type = "磁盘分区";

                        FI.d_DriveFormat = drive[i].DriveFormat;
                        FI.d_DriveType = drive[i].DriveType.ToString();
                        FI.d_TotalFreeSpace = drive[i].TotalFreeSpace.ToString();
                        FI.d_TotalSize = drive[i].TotalSize.ToString();
                        FI.d_VolumeLabel = drive[i].VolumeLabel;
                        FIC.Add(FI);
                        /*
                         Unknown 驱动器类型未知。 
     NoRootDirectory 此驱动器没有根目录。 
     Removable 此驱动器是一个可移动存储设备，如软盘驱动器或 USB 闪存驱动器。 
     Fixed 此驱动器是一个固定磁盘。 
     Network 此驱动器是一个网络驱动器。 
     CDRom 此驱动器是一个光盘设备，如 CD 或 DVD-ROM。 
     Ram 此驱动器是一个 RAM 磁盘。 
 
                        */

                    }

                }
                return FIC;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件夹下文件夹和文件信息
        /// </summary>
        /// <param name="FilesPath">路径</param>
        /// <returns></returns>
        public FilesInfoCollection GetPathAllInfo_Path(string FilesPath)
        {
            try
            {
                if (!Directory.Exists(FilesPath))
                {
                    return null;
                }


                FilesInfoCollection FIC = new FilesInfoCollection();

                DirectoryInfo CurDir = new DirectoryInfo(FilesPath);
                DirectoryInfo[] dirs = CurDir.GetDirectories(); //返回当前目录的子目录
                FileInfo[] files = CurDir.GetFiles(); //返回当前目录的文件列表

                FilesInfo FI_up = new FilesInfo();
                if (FilesPath.EndsWith(@":\") || FilesPath.EndsWith(@":/") || FilesPath.EndsWith(@":"))
                {
                    FI_up.p_AllPath = "磁盘列表";
                }
                else
                {
                    FI_up.p_AllPath = Directory.GetParent(FilesPath).FullName;
                }

                FI_up.p_Name = "上一级目录...";
                FI_up.p_Type = "上一级";
                FI_up.f_UpPath = FilesPath;
                FIC.Add(FI_up);



                foreach (DirectoryInfo dir in dirs)
                {

                    FilesInfo FI = new FilesInfo();
                    FI.p_AllPath = dir.FullName;
                    FI.p_Name = dir.Name;
                    FI.p_Type = "文件夹";

                    FI.f_Attributes = dir.Attributes.ToString();
                    FI.f_CreationTime = dir.CreationTime.ToString();
                    FI.f_LastAccessTime = dir.LastAccessTime.ToString();
                    FI.f_LastWriteTime = dir.LastWriteTime.ToString();
                    //FI.f_Length = FileSize(dir.FullName).ToString();
                    FI.f_Length = "0";
                    FI.f_UpPath = FilesPath;

                    FIC.Add(FI);

                }
                foreach (FileInfo file in files)
                {
                    FilesInfo FI = new FilesInfo();
                    FI.p_AllPath = file.FullName;
                    FI.p_Name = file.Name;
                    FI.p_Type = "文件";

                    FI.f_Attributes = file.Attributes.ToString();
                    FI.f_CreationTime = file.CreationTime.ToString();
                    FI.f_LastAccessTime = file.LastAccessTime.ToString();
                    FI.f_LastWriteTime = file.LastWriteTime.ToString();
                    FI.f_Length = file.Length.ToString();
                    FI.f_UpPath = FilesPath;

                    FIC.Add(FI);
                }
                return FIC;
            }
            catch
            {
                return null;
            }
        }



        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public long FileSize(string filePath)
        {
            try
            {
                long temp = 0;
                //判断当前路径所指向的是否为文件
                if (File.Exists(filePath) == false)
                {
                    string[] str1 = Directory.GetFileSystemEntries(filePath);
                    foreach (string s1 in str1)
                    {
                        temp += FileSize(s1);
                    }
                }
                else
                {

                    //定义一个FileInfo对象,使之与filePath所指向的文件向关联,

                    //以获取其大小
                    FileInfo fileInfo = new FileInfo(filePath);
                    return fileInfo.Length;
                }
                return temp;
            }
            catch
            {
                return 0;
            }


        }


        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        public static bool islong(string str)
        {
            if(str == null || str.Trim() == "")
            {
                return false;
            }
            Regex r1 = new Regex("^[0-9]+$");
            Match m1 = r1.Match(str);
            if (m1.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
