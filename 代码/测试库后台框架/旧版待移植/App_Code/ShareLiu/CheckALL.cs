using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Hesion.Brick.Core;
//加密解密
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections;

/// <summary>
///CheckALL 的摘要说明
//刘杰：2009-12-10
/// </summary>
namespace ShareLiu
{
    public class CheckALL
    {
        public CheckALL()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 检索控件为空,支持TextBox,FileUpload
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="Msg">控件名称</param>
        /// <param name="page">控件所属页面</param>
        public static Boolean CheckKong(object sender, string Msg, System.Web.UI.Page page)
        {

            if ((sender is TextBox) && ((sender as TextBox).Text == ""))
            {
                MessageBox.Show(page, Msg + "不能为空");
                (sender as TextBox).Focus();
                return false;
            }
            if ((sender is FileUpload) && ((sender as FileUpload).FileName == ""))
            {
                MessageBox.Show(page, Msg + "不能为空");
                return false;
            }
            if ((sender is DropDownList) && ((sender as DropDownList).Text == ""))
            {
                MessageBox.Show(page, Msg + "不能为空");
                (sender as DropDownList).Focus();
                return false;
            }
            return true;

        }
        /// <summary>
        /// 给控件内容循环添加年份
        /// </summary>
        /// <param name="sender">web控件，如下拉列表</param>
        /// <param name="startNum">开始数字，如2010</param>
        /// <param name="xhNum">循环数</param>
        public static void AddYears(WebControl sender,int startNum,int xhNum)
        {
             
            for(int i=0;i<xhNum;i++)
            {
                if((sender is DropDownList)==true)
                {
                    (sender as DropDownList).Items.Add((startNum+i).ToString());
                }
            }

        }
        /// <summary>
        /// 给控件循环添加月份
        /// </summary>
        /// <param name="sender">web控件，如下拉列表</param>
        ///
        public static void AddMonth(WebControl sender)
        {

            for (int i = 1; i <=12; i++)
            {
                if ((sender is DropDownList) == true)
                {
                    if (i < 10)
                    {
                        (sender as DropDownList).Items.Add("0"+(i).ToString());
                    }
                    else
                    {
                        (sender as DropDownList).Items.Add((i).ToString());
                    }
                }
            }

        }
        /// <summary>
        /// 控制当前年的前一个月的年份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="startNum"></param>
        /// <param name="m"></param>
        public static void AddMYears(WebControl sender, int startNum, int m)
        {


            if ((sender is DropDownList) == true)
            {

                if (m == 1)
                {
                    (sender as DropDownList).Items.Add((startNum - 1).ToString());
                }
                else
                {
                    (sender as DropDownList).Items.Add((startNum).ToString());
                }
            }


        }
        /// <summary>
        /// 控制m月之前的月份
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="m"></param>
        public static void AddMonth(WebControl sender,int m,string types)
        {
            int n = 0;
            if (m == 1)
            {
                n = 12;
            }
            else
            {
                n = m - 1;
            }
            for (int i = 1; i <=n; i++)
            {
                if ((sender is DropDownList) == true)
                {
                    if (types == "双")
                    {
                        if (i < 10)
                        {
                            (sender as DropDownList).Items.Add("0" + (i).ToString());
                        }
                        else
                        {
                            (sender as DropDownList).Items.Add((i).ToString());
                        }
                    }
                    else
                    {
                        (sender as DropDownList).Items.Add((i).ToString());
                    }



                }
            }

        }
        /// <summary>
        /// 检查是否office文件(word,excel)
        /// </summary>
        /// <param name="fu">上传控件</param>
        /// <param name="page">页面对象</param>
        /// <returns>true/false</returns>
        public static Boolean CheckOfficeType(FileUpload fu, System.Web.UI.Page page)
        {
            string exname;
            exname = fu.FileName.Substring(fu.FileName.LastIndexOf(".") + 1).ToUpper();
            switch (exname)
            {
                case "DOC":
                    return true;
                    break;
                case "doc":
                    return true;
                    break;
                case "DOCX":
                    return true;
                    break;
                case "docx":
                    return true;
                    break;
                case "XLS":
                    return true;
                    break;
                case "xls":
                    return true;
                    break;

                case "XLSX":
                    return true;
                    break;
                case "xlsx":
                    return true;
                    break;
                default:
                    MessageBox.Show(page, "文件类型不是office(word,excel)文件");
                    return false;
                    break;
            }

        }
        /// <summary>
        /// 得到文件扩展名1
        /// </summary>
        /// <param name="fu">通过fileupload控件</param>
        /// <returns></returns>
        public static string GetfileEname(FileUpload fu)
        {
            string exname;
            exname = fu.FileName.Substring(fu.FileName.LastIndexOf(".") + 1).ToUpper();
            return exname;

        }
        /// <summary>
        /// 得到文件扩展名2
        /// </summary>
        /// <param name="filename">通过文件名</param>
        /// <returns></returns>
        public static string GetfileEname(string filename)
        {
            string exname;
            exname = filename.Substring(filename.LastIndexOf(".") + 1).ToUpper();
            return exname;
        }
        /// <summary>
        /// 得到特殊字符间隔
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="keys">间隔字符</param>
        /// <returns>分割后的字符数组</returns>
        public static string[] GetSplit(string s, char keys)
        {
            string[] a;
            return s.Split(keys);
        }
        /// <summary>
        /// 转化为字符串为字符arraylist
        /// 
        /// </summary>
        /// <param name="strs">字符串</param>
        /// <param name="strCount">长度控制</param>
        /// <returns>ArrayList</returns>
        public static ArrayList CLString(string strs, int strCount)
        {
            ArrayList ac = new ArrayList();
            int j = 0;
            string ss = "";
            foreach (char i in strs)
            {
                j = j + 1;
                ss = ss + i.ToString();
                if (j == strCount)
                {
                    ac.Add(ss);
                    j = 0;
                    ss = "";
                }
            }
            ac.Add(ss);
            return ac;

        }
        /// <summary>
        /// 得到去除<a></a>框架
        /// 刘杰 2010-09-17 处理映射的
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static string getQJStr(string strs)
        {
            if (strs.Contains("</a>"))
            {
                strs = strs.Replace("</a>", "").Replace("<a", "");
                int i = strs.IndexOf('>');
                strs = strs.Substring(i + 1);
            }
            return strs;
        }
        /// <summary>
        /// 转换DataSet为DataTable
        /// </summary>
        /// <param name="ds">DataSet记录集</param>
        /// <returns>DataTable</returns>
        public static DataTable DStoDT(DataSet ds)
        {
            DataTable dt = new DataTable();
            int dsColCount = ds.Tables[0].Columns.Count; //列数
            int dsRowCount = ds.Tables[0].Rows.Count;   //行数
            string[] a = new string[dsColCount];
            for (int i = 0; i < dsColCount; i++) //遍历列
            {
                a[i] = ds.Tables[0].Columns[i].ColumnName.ToString();
                dt.Columns.Add(a[i], Type.GetType("System.String"));

            }
            for (int j = 0; j < dsRowCount; j++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dsColCount; i++)
                {
                    dr[a[i]] = ds.Tables[0].Rows[j][i].ToString();

                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        /// DataSet加入序号 2010-10-21
        /// </summary>
        /// <param name="ds">DataSet记录集</param>
        /// <param name="idName">序号外显</param>
        /// <returns></returns>
        public static DataSet DSAddID(DataSet ds, string idName)
        {
            DataTable dt = new DataTable();
            int dsColCount = ds.Tables[0].Columns.Count; //列数
            int dsRowCount = ds.Tables[0].Rows.Count;   //行数
            string[] a = new string[dsColCount + 1];   //加序号所以加1
            a[0] = idName;
            dt.Columns.Add(a[0], Type.GetType("System.String"));
            for (int i = 0; i < dsColCount; i++) //遍历列
            {

                a[i + 1] = ds.Tables[0].Columns[i].ColumnName.ToString(); //i后移一位
                dt.Columns.Add(a[i + 1], Type.GetType("System.String"));

            }
            for (int j = 0; j < dsRowCount; j++)
            {
                DataRow dr = dt.NewRow();
                dr[a[0]] = Convert.ToString(j + 1);
                for (int i = 0; i < dsColCount; i++) //遍历列
                {

                    dr[a[i + 1]] = ds.Tables[0].Rows[j][i].ToString(); //i后移一位

                }
                dt.Rows.Add(dr);
            }
            DataSet rds = new DataSet();
            rds.Tables.Add(dt);
            return rds;
        }
        /// <summary>
        /// 两个DataSet的连接。两个DataSet第一列就是遍历列，过滤条件只有该列 2010-10-21
        /// </summary>
        /// <param name="sql">循环列，只显示一次</param>
        /// <param name="ds1">主DataSet</param>
        /// <param name="ds2">附加DataSet</param>
        /// <returns></returns>
        public static DataSet DSListADD(string[] sql, DataSet ds1, DataSet ds2)
        {
            int rowCount1 = ds1.Tables[0].Rows.Count;
            int colCount1 = ds1.Tables[0].Columns.Count;
            int rowCount2 = ds2.Tables[0].Rows.Count;
            int colCount2 = ds2.Tables[0].Columns.Count;
            int newCol = colCount1 + colCount2 - 1; //新列数
            DataTable dtAll = new DataTable();
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            DataTable dt2 = new DataTable();
            dt2 = ds2.Tables[0];
            //画出表头
            string[] a = new string[newCol]; //记录列名称
            for (int i = 0; i < newCol; i++) //遍历列
            {
                if (i < colCount1)
                {
                    a[i] = ds1.Tables[0].Columns[i].ColumnName.ToString();//先加第一个记录集的列

                }
                else
                {
                    a[i] = ds2.Tables[0].Columns[i - colCount1 + 1].ColumnName.ToString();//后加第二个记录集的，去掉了第二个记录集的第一列
                }
                dtAll.Columns.Add(a[i], Type.GetType("System.String"));
            }

            for (int j = 0; j < sql.Length; j++) //循环查询列，填充数据
            {

                DataRow dr = dtAll.NewRow(); //新加入一行
                dr[a[0]] = sql[j]; //填充第一列
                //sql是关键列数组，a[]是表头数组，dr[]是记录填写问题。
                // 填写入数据
                DataRow[] dr1 = null;
                dr1 = dt1.Select(a[0].ToString() + "='" + sql[j] + "'");
                if (dr1.Length > 0)
                {
                    for (int k = 1; k < colCount1; k++)
                    {

                        dr[a[k]] = dr1[0][a[k]].ToString();
                    }
                }
                else
                {
                    for (int k = 1; k < colCount1; k++)
                    {
                        dr[a[k]] = "0";
                    }
                }
                DataRow[] dr2 = null;
                dr2 = dt2.Select(a[0].ToString() + "='" + sql[j] + "'");
                if (dr2.Length > 0)
                {
                    for (int k = colCount1; k < newCol; k++)
                    {
                        dr[a[k]] = dr2[0][a[k]].ToString();
                    }
                }
                else
                {
                    for (int k = colCount1; k < newCol; k++)
                    {
                        dr[a[k]] = "0";
                    }
                }
                dtAll.Rows.Add(dr);
            }
            //处理DataTable
            DataSet newds = new DataSet();
            newds.Tables.Add(dtAll);
            return newds;

        }
        /// <summary>
        /// 判断是否偶数 2010-10-22
        /// </summary>
        /// <param name="a">整数</param>
        /// <returns>[是]返回true,[否]返回false</returns>
        public static Boolean isEven(int a)
        {
           if((a & 1) == 0)
           {
               return true;
           }
           else
           {
               return false;
           }

        }

        /// <summary>
        /// 偶数行加入间隔列，列名定义为"左侧列名加定义名keyname
        /// </summary>
        /// <param name="ds">DataSet源</param>
        /// <param name="keyname">标示名称，如比例,会将最侧列名加入进来</param>
        /// <param name="delCol">生成间隔后结果的再删除列，0表示不删除任何列,绝对位置列</param>
        /// <returns></returns>
        public static DataSet AddColEven(DataSet ds,string keyname,int delCol)
        {
            DataTable dt = new DataTable();
            int dsColCount = ds.Tables[0].Columns.Count; //列数
            int dsRowCount = ds.Tables[0].Rows.Count;   //行数
            dsColCount = 2 * dsColCount;
            string[] a = new string[dsColCount];
            int l = 0;
            for (int i = 0; i < dsColCount; i++) //遍历列
            {

                if (isEven(i) == true)
                {

                    a[i] = ds.Tables[0].Columns[l].ColumnName.ToString();
                    l = l + 1;
                }
                else
                {
                    a[i] = a[i - 1].ToString() +keyname;
                }
                dt.Columns.Add(a[i], Type.GetType("System.String"));

            }

            for (int j = 0; j < dsRowCount; j++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dsColCount; i++)
                {
                    if (isEven(i) == true)
                    {
                        dr[a[i]] = ds.Tables[0].Rows[j][i / 2].ToString();

                    }
                }
                dt.Rows.Add(dr);
            }
            if(delCol!=0)
            {
                dt.Columns.RemoveAt(delCol-1);
            }
            
            DataSet dss = new DataSet();
            dss.Tables.Add(dt);
            return dss;
        }
 
        /// <summary>
        /// 在第二列开始循环加入两空列定义。
        /// </summary>
        /// <param name="ds">处理的DataSet</param>
        /// <param name="oneCol">左侧第一列</param>
        /// <param name="twoCol">左侧第二列</param>
        /// <param name="delTwoCol">删除第N列，为"零"则不删除任何列,默认删除两列</param>
        /// <param name="delCount">删除多少列数，从左到右，上个参数需不等于零</param>
        /// <returns>DataSet记录集</returns>
        public static DataSet AddColTwo(DataSet ds, string oneCol, String twoCol,int delTwoCol,int delCount)
        {
            DataTable dt = new DataTable();
            int dsColCount = ds.Tables[0].Columns.Count; //列数
            int dsRowCount = ds.Tables[0].Rows.Count;   //行数
            dsColCount = 3 * dsColCount;
            string[] a = new string[dsColCount];
            int l = 0;
            for (int i = 0; i < dsColCount; i++) //遍历列
            {
                if (i % 3 == 0)
                {
                    a[i] = ds.Tables[0].Columns[l].ColumnName.ToString();
                    l = l + 1;
                }
                else
                {
                    if (i % 3 == 1) //余1
                    {
                        a[i] = a[i - 1].ToString() + oneCol.ToString().Trim();
                    }
                    else //余2
                    {
                        a[i] = a[i - 2].ToString() + twoCol.ToString().Trim();
                    }

                }

                dt.Columns.Add(a[i], Type.GetType("System.String"));

            }

            for (int j = 0; j < dsRowCount; j++)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dsColCount; i++)
                {
                    if (i % 3 == 0)
                    {
                        dr[a[i]] = ds.Tables[0].Rows[j][i / 3].ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            if(delTwoCol!=0)
            {
                for (int k = 0; k < delCount; k++)
                {
                    dt.Columns.RemoveAt(delTwoCol - 1);
                }
                
            }
            
            DataSet dss = new DataSet();
            dss.Tables.Add(dt);
            return dss;
        }
        /// <summary>
        /// 数字转换成文本，主要是前置加零
        /// </summary>
        /// <param name="oldNum">老数字</param>
        /// <param name="len">位数</param>
        public static string NumToStr(int oldNum, int len)
        {
            string newStr = "";
            Byte[] b=Encoding.Default.GetBytes(oldNum.ToString().Trim());
            
            if(b.Length<len)
            {
                for(int i=0;i<len-b.Length;i++)
                {
                    newStr = newStr + "0";

                }
                return newStr.Trim() + oldNum;
            }
            else
            {
                return oldNum.ToString();
            }
            
        }
        /// <summary>
        /// 根据当前日期，转换为上月20日-下月19日区间,返回开始结束日期，有特定性
        /// </summary>
        /// <param name="thisStartdate">日期参数，yyyy-mm-dd</param>
        /// <returns>yyyymmdd</returns>
        public static ArrayList GetStartEndDate(string thisStartdate)
        {
            ArrayList al = new ArrayList();
            string years = thisStartdate.Substring(0, 4);
            string months = thisStartdate.Substring(5, 2);
            string days = thisStartdate.Substring(8, 2);

            if (Convert.ToInt16(days) >= 20)
            {
                if (Convert.ToInt16(months) == 12)
                {
                    int newyear = Convert.ToInt16(years) + 1; //新ID

                    al.Add(years + "1220");
                    al.Add(newyear.ToString() + "0119");
                }
                else
                {
                    int newmonth = Convert.ToInt16(months) + 1;
                    al.Add(years + months + "20");
                    al.Add(years + NumToStr(newmonth, 2) + "19");
                }

            }
            else
            {
                if (Convert.ToInt16(months) == 1)
                {
                    int newyear = Convert.ToInt16(years) - 1; //新ID
                    al.Add(newyear + "1220");
                    al.Add(years.ToString() + "0119");
                }
                else
                {
                    
                    int newmonth = Convert.ToInt16(months) - 1;

                    al.Add(years + NumToStr(newmonth, 2) + "20");
                    al.Add(years + months + "19");
                }

            }
            return al;

        }
        /// <summary>
        /// 返回本月到当前，从01日到当天,返回yyyymmdd
        /// </summary>
        /// <param name="thisStartdate">日期yyyy-mm-dd</param>
        /// <returns>yyyymmdd</returns>
        public static ArrayList GetMonthToThisDay(string thisStartdate)
        {
            ArrayList al = new ArrayList();
            string years = thisStartdate.Substring(0, 4);
            string months = thisStartdate.Substring(5, 2);
            string days = thisStartdate.Substring(8, 2);

            al.Add(years + months + "01");
            al.Add(years + months + days);
            return al;
        }
        /// <summary>
        /// 返回本月到当前，从01日到当天,返回yyyy-mm-dd
        /// </summary>
        /// <param name="thisStartdate">日期yyyy-mm-dd</param>
        /// <returns>yyyy-mm-dd</returns>
        public static ArrayList GetMonthToThisDay(string thisStartdate,string types)
        {
            ArrayList al = new ArrayList();
            string years = thisStartdate.Substring(0, 4);
            string months = thisStartdate.Substring(5, 2);
            string days = thisStartdate.Substring(8, 2);

            al.Add(years +types.Trim()+ months +types.Trim()+"01");
            al.Add(years +types.Trim()+ months +types.Trim()+days);
            return al;
        }
        /// <summary>
        /// 主记录集合并间隔式记录集，并运算空列与主记录的比例
        /// </summary>
        /// <param name="sql">条件列</param>
        /// <param name="ds1">主DataSet</param>
        /// <param name="ds2">从DataSet,偶数空列与主DS第N列比较</param>
        /// <param name="chuCol">主DataSet的被除列，绝对列</param>
        /// <returns></returns>
        public static DataSet DSListADD(string[] sql, DataSet ds1, DataSet ds2,int chuCol)
        {
            int rowCount1 = ds1.Tables[0].Rows.Count;
            int colCount1 = ds1.Tables[0].Columns.Count;
            int rowCount2 = ds2.Tables[0].Rows.Count;
            int colCount2 = ds2.Tables[0].Columns.Count;
            int newCol = colCount1 + colCount2 - 1; //新列数
            DataTable dtAll = new DataTable();
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            DataTable dt2 = new DataTable();
            dt2 = ds2.Tables[0];
            //画出表头
            string[] a = new string[newCol]; //记录列名称
            for (int i = 0; i < newCol; i++) //遍历列
            {
                if (i < colCount1)
                {
                    a[i] = ds1.Tables[0].Columns[i].ColumnName.ToString();//先加第一个记录集的列

                }
                else
                {
                    a[i] = ds2.Tables[0].Columns[i - colCount1 + 1].ColumnName.ToString();//后加第二个记录集的，去掉了第二个记录集的第一列
                }
                dtAll.Columns.Add(a[i], Type.GetType("System.String"));
            }

            for (int j = 0; j < sql.Length; j++) //循环查询列，填充数据
            {

                DataRow dr = dtAll.NewRow(); //新加入一行
                dr[a[0]] = sql[j]; //填充第一列
                //sql是关键列数组，a[]是表头数组，dr[]是记录填写问题。
                // 填写入数据
                DataRow[] dr1 = null;
                dr1 = dt1.Select(a[0].ToString() + "='" + sql[j] + "'");
                if (dr1.Length > 0)
                {
                    for (int k = 1; k < colCount1; k++)
                    {

                        dr[a[k]] = dr1[0][a[k]].ToString();
                    }
                }
                else
                {
                    for (int k = 1; k < colCount1; k++)
                    {
                        dr[a[k]] = "0";
                    }
                }
                DataRow[] dr2 = null;
                dr2 = dt2.Select(a[0].ToString() + "='" + sql[j] + "'");
                if (dr2.Length > 0)
                {
                    for (int k = colCount1; k < newCol; k++)
                    {

                        if ((k - colCount1) % 2 == 0)
                        {
                            dr[a[k]] = dr2[0][a[k]].ToString();
                        }
                        else
                        {
                            if (dr[a[chuCol-1]].ToString() == "0")
                            {
                                dr[a[k]] = "无";
                            }
                            else
                            {
                                dr[a[k]] = (Convert.ToDouble(dr[a[k - 1]]) / Convert.ToDouble(dr[a[chuCol-1]])).ToString("P");
                            }


                        }
                    }
                }
                else
                {
                    for (int k = colCount1; k < newCol; k++)
                    {
                        dr[a[k]] = "0";
                    }
                }
                dtAll.Rows.Add(dr);
            }
            //处理DataTable
            DataSet newds = new DataSet();
            newds.Tables.Add(dtAll);
            return newds;

        }
        /// <summary>
        /// 返回日期间隔月份，相同月份为返回1，含算边缘月份。2010-12-13
        /// </summary>
        /// <param name="Startdate">开始日期yyyy-mm-dd</param>
        /// <param name="Enddate">结束日期yyyy-mm-dd</param>
        /// <returns>整数</returns>
        public static int  MJM(string Startdate, string Enddate)
        {
            if (Startdate == "" && Enddate == "")
            {
                return 0;
            }

            int Syear = Convert.ToInt32(Startdate.Substring(0, 4));
            int Eyear = Convert.ToInt32(Enddate.Substring(0, 4));
            int Smonth = Convert.ToInt32(Startdate.Substring(5, 2));
            int Emonth = Convert.ToInt32(Enddate.Substring(5, 2));
            int r = (Eyear - Syear) * 12 + (Emonth - Smonth);
            if (r == 0)
            {
                return 1;
            }
            else if (r > 0)
            {
                r = r + 1;
                return r;
            }
            else
            {
                return 0;
            }


        }
        /// <summary>
        /// 得到某控件对某字段的like查询，支持文本框，下拉框控件，可扩展。
        /// </summary>
        /// <param name="wc">控件名</param>
        /// <param name="zdName">字段名称</param>
        /// <returns>sql条件语句</returns>
        public static string GetLikeWhere(WebControl wc,string zdName)
        {
            string sql = "";
            if((wc is TextBox)&&((wc as TextBox).Text!=""))
            {
                sql=" and "+zdName.Trim()+" like '%"+(wc as TextBox).Text.Trim()+"%'";
            }
            if ((wc is DropDownList) && ((wc as DropDownList).Text != ""))
            {
                sql = " and " + zdName.Trim() + " like '%" + (wc as DropDownList).Text.Trim() + "%'";
            }

            return sql;
        }
        /// <summary>
        /// GridView导出Excel
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="GV">GridView控件名</param>
        /// <param name="thispage">当前页对象this.page</param>
        public static void OutPutExcel(string fileName, GridView GV,System.Web.UI.Page thispage)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            Page page = new Page();
            HtmlForm form = new HtmlForm();

             GV.EnableViewState = false;


            page.EnableEventValidation = false;

            page.DesignerInitialize();

            page.Controls.Add(form);
            form.Controls.Add(GV);

            page.RenderControl(htw);

            thispage.Response.Clear();
            thispage.Response.Buffer = true;
            thispage.Response.ContentType = "application/vnd.ms-excel";
            thispage.Response.AddHeader("Content-Disposition", "attachment;filename=" +fileName.Trim()+ ".xls");
            thispage.Response.Charset = "UTF-8";
            thispage.Response.ContentEncoding = Encoding.Default;
            thispage.Response.Write(sb.ToString());
            thispage.Response.End();

        }

    }
}
