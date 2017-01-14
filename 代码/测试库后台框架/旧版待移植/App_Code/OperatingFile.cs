using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.IO;
using FMOP.SD;
/// <summary>
/// OperatingFile 的摘要说明
/// </summary>
namespace FMOP.FILE
{
    public class OperatingFile
    {
        public OperatingFile()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public static string upPath = string.Empty;

        public static bool SaveFile(Hashtable saveFile)
        {
            bool FileFlag = true;
            if (saveFile != null && saveFile.Count > 0)
            {
                foreach (DictionaryEntry file in saveFile)
                {
                    HttpPostedFile postedFile = (HttpPostedFile) file.Value;
                    postedFile.SaveAs(upPath + file.Key);
                    if (File.Exists(upPath + file.Key))
                    {
                        FileFlag = true;
                    }
                } 
            }

            return FileFlag;
        }

        /// <summary>
        /// 执行删除操作
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="path">路径名</param>
        /// <returns>删除是否成功</returns>
        public static void DeleteFile(SaveDepositary[] FileArray)
        {
            try
            {
                for (int i = 0; i < FileArray.Length; i++)
                {
                    string FilePath = upPath + FileArray[i].Name;

                    //判断文件是否存在
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据文件名删除
        /// </summary>
        /// <param name="FileArray"></param>
        public static void DeleteFile(string FileName)
        {
            try
            {
                string FilePath = upPath + FileName;

                //判断文件是否存在
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除上传文件
        /// </summary>
        /// <param name="FileArray"></param>
        public static void DeleteFile(ArrayList IDlist )
        {
            try
            {
                for (int i = 0; i < IDlist.Count; i++)
                {
                    string FilePath = upPath + IDlist[i].ToString();

                    //判断文件是否存在
                    if (File.Exists(FilePath))
                    {
                        File.Delete(FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

