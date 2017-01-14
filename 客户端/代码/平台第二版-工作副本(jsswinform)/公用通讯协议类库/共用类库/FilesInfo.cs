using System;
using System.Text;
using System.IO;
namespace 公用通讯协议类库.共用类库
{
    /// <summary>
    /// 磁盘信息类
    /// </summary>
    [Serializable]
    public class FilesInfo
    {

        //名称(用于文件，文件夹，磁盘)
        protected string name;
        /// <summary>
        /// 名称(用于文件，文件夹，磁盘)
        /// </summary>
        public string p_Name
        {
            get { return name; }
            set { name = value; }
        }

        //类型(用于文件，文件夹，磁盘)
        protected string type;
        /// <summary>
        /// 类型(用于文件，文件夹，磁盘)
        /// </summary>
        public string p_Type
        {
            get { return type; }
            set { type = value; }
        }


        /// <summary>
        /// 完整路径(用于文件，文件夹，磁盘)
        /// </summary>
        protected string allpath;
        /// <summary>
        /// 完整路径(用于文件，文件夹，磁盘)
        /// </summary>
        public string p_AllPath
        {
            get { return allpath; }
            set { allpath = value; }
        }


        //卷标(用于磁盘)
        protected string volumelabel;
        /// <summary>
        /// 卷标(用于磁盘)
        /// </summary>
        public string d_VolumeLabel
        {
            get { return volumelabel; }
            set { volumelabel = value; }
        }

        //剩余容量(用于磁盘)
        protected string totalfreespace;
        /// <summary>
        /// 剩余容量(用于磁盘)
        /// </summary>
        public string d_TotalFreeSpace
        {
            get { return totalfreespace; }
            set { totalfreespace = value; }
        }

        //总容量(用于磁盘)
        protected string totalsize;
        /// <summary>
        /// 总容量(用于磁盘)
        /// </summary>
        public string d_TotalSize
        {
            get { return totalsize; }
            set { totalsize = value; }
        }

        //文件系统(用于磁盘)
        protected string driveformat;
        /// <summary>
        /// 文件系统(用于磁盘)
        /// </summary>
        public string d_DriveFormat
        {
            get { return driveformat; }
            set { driveformat = value; }
        }

        //驱动器类型(用于磁盘)
        protected string drivetype;
        /// <summary>
        /// 驱动器类型(用于磁盘)
        /// </summary>
        public string d_DriveType
        {
            get { return drivetype; }
            set { drivetype = value; }
        }





        //修改日期(用于文件夹，文件)
        protected string lastwritetime;
        /// <summary>
        /// 修改日期(用于文件夹，文件)
        /// </summary>
        public string f_LastWriteTime
        {
            get { return lastwritetime; }
            set { lastwritetime = value; }
        }

        //最后访问时间(用于文件夹，文件)
        protected string lastaccesstime;
        /// <summary>
        /// 最后访问时间(用于文件夹，文件)
        /// </summary>
        public string f_LastAccessTime
        {
            get { return lastaccesstime; }
            set { lastaccesstime = value; }
        }

        //占用空间(用于文件夹，文件)
        protected string length;
        /// <summary>
        /// 占用空间(用于文件夹，文件)
        /// </summary>
        public string f_Length
        {
            get { return length; }
            set { length = value; }
        }

        //建立时间(用于文件夹，文件)
        protected string creationtime;
        /// <summary>
        /// 建立时间(用于文件夹，文件)
        /// </summary>
        public string f_CreationTime
        {
            get { return creationtime; }
            set { creationtime = value; }
        }

        //文件夹属性(用于文件夹，文件)
        protected string attributes;
        /// <summary>
        /// 文件夹属性(用于文件夹，文件)
        /// </summary>
        public string f_Attributes
        {
            get { return attributes; }
            set { attributes = value; }
        }

        //上级路径(用于文件，文件夹)
        protected string uppath;
        /// <summary>
        /// 上级路径(用于文件夹，文件)
        /// </summary>
        public string f_UpPath
        {
            get { return uppath; }
            set { uppath = value; }
        }


        
    }
}
