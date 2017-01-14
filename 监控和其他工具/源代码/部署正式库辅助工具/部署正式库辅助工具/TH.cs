using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.IO;

namespace 部署正式库辅助工具
{
    class TH
    {
        //向线程传递的回调参数
        private delegateForThread DForThread;
        //向线程传递的数据参数
        private Hashtable InPutHT;


                /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="PHT">需要传入线程的参数</param>
        /// <param name="DFT">线程委托</param>
        public TH(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 对比文件
        /// </summary>
        public void Run_files()
        {
            Thread.Sleep(500);

            string[] files1 = (string[])(InPutHT["files1"]);
            string[] files2 = (string[])(InPutHT["files2"]);
            ArrayList al = new ArrayList();
            for (int f1 = 0; f1 < files1.Count(); f1++)
            {
                string f1MD5 = GetMd5Hash(files1[f1]);
                string f1NAME = Path.GetFileName(files1[f1]);
                for (int f2 = 0; f2 < files2.Count(); f2++)
                {
                    string f2NAME = Path.GetFileName(files2[f2]);
                    if (f1NAME.Trim() == f2NAME.Trim())
                    {
                        string f2MD5 = GetMd5Hash(files2[f2]);
                        if (f1MD5 == f2MD5 && f1MD5.Trim() != "" && f2MD5.Trim() != "")
                        {
                            al.Add("相符" + "[md5_1:" + f1MD5 + "]" + "[md5_2:" + f2MD5 + "]" + "【" + files1[f1] + "】" + "【" + files2[f2] + "】");
                        }
                        else
                        {
                            al.Add("不相符" + "[md5_1:" + f1MD5 + "]" + "[md5_2:" + f2MD5 + "]" + "【" + files1[f1] + "】" + "【" + files2[f2] + "】");
                        }
                        break;
                    }
                }
            }

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行标记"] = "文件对比";
            OutPutHT["对比结果"] = al;//测试参数
            DForThread(OutPutHT);

        }

        //获得md5
        private string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =
                new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {
                oFileStream = new System.IO.FileStream(pathName, System.IO.FileMode.Open,
                                                       System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); //计算指定Stream 对象的哈希值

                oFileStream.Close();

                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”

                strHashData = System.BitConverter.ToString(arrbytHashValue);

                //替换-
                strHashData = strHashData.Replace("-", "");

                strResult = strHashData;
            }

            catch (System.Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return strResult;
        }


        /// <summary>
        /// 测试
        /// </summary>
        public void Run()
        {
            Thread.Sleep(500);

            string cmdreturn = Execute(InPutHT["cmd"].ToString(), 0);
            string xmlsavepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\svnlog.xml";
            DataSet ds = new DataSet();
            ds.ReadXml(xmlsavepath);
            DataTable dtnew = GetXmlNodeInformation(xmlsavepath, ds.Tables[0]);
     


            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["执行标记"] = "绑定";
            OutPutHT["测试列表数据表"] = dtnew;//测试参数
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 对比表
        /// </summary>
        public void Run_DBduibi()
        {
            Thread.Sleep(500);
            Hashtable OutPutHT = new Hashtable();

            string kaitou = InPutHT["对比开头"].ToString();
            //打开第一个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF1;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL1;
            I_DBF1 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL1 = I_DBF1.DbLinkSqlMain(InPutHT["连接串1"].ToString());

            //打开第二个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF2;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL2;
            I_DBF2 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL2 = I_DBF2.DbLinkSqlMain(InPutHT["连接串2"].ToString());

            //获得第一个数据库的所有表 
            DataSet ds_table_View1 = new DataSet();
            Hashtable ht_table_View1 = new Hashtable();
            ht_table_View1 = I_DBL1.RunProc("select '数据库1' as DB_name, name as table_name,xtype as table_type,table_owner='dbo' from sysobjects where xtype='U' and name<>'dtproperties'  and name like '" + kaitou + "%'  order by name", ds_table_View1);
            if (ht_table_View1["return_ds"] != null)
            {
                ds_table_View1 = (DataSet)(ht_table_View1["return_ds"]);
            }
            //获得第二个数据库的所有表 
            DataSet ds_table_View2 = new DataSet();
            Hashtable ht_table_View2 = new Hashtable();
            ht_table_View2 = I_DBL2.RunProc("select  '数据库2' as DB_name,name as table_name,xtype as table_type,table_owner='dbo' from sysobjects where xtype='U' and name<>'dtproperties'  and name like '" + kaitou + "%'  order by name", ds_table_View2);
            if (ht_table_View2["return_ds"] != null)
            {
                ds_table_View2 = (DataSet)(ht_table_View2["return_ds"]);
            }
            //对比表 
            DataTable duo = ds_table_View2.Tables[0].Clone(); //差异表，第一个数据库比第二个数据库多出来的表

            int allsl = Convert.ToInt32(ds_table_View1.Tables[0].Rows.Count) * Convert.ToInt32(ds_table_View2.Tables[0].Rows.Count);
            int nowsl = 1;
            for (int i1 = 0; i1 < ds_table_View1.Tables[0].Rows.Count; i1++)
            {
                bool chongfu = false;
                for (int i2 = 0; i2 < ds_table_View2.Tables[0].Rows.Count; i2++)
                {
                    if (nowsl % 100 == 0)
                    {
                        OutPutHT = new Hashtable();
                        OutPutHT["进度显示"] = "约" + nowsl.ToString() + "，共" + allsl.ToString();
                        OutPutHT["执行标记"] = "进度显示";
                        DForThread(OutPutHT);
                    }
                    nowsl++;

                    if (ds_table_View1.Tables[0].Rows[i1]["table_owner"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_owner"].ToString() && ds_table_View1.Tables[0].Rows[i1]["table_name"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_name"].ToString() && ds_table_View1.Tables[0].Rows[i1]["table_type"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_type"].ToString())
                    {
                        //若数据相同，忽略掉;
                        chongfu = true;
                        break;
                    }
                }
                //若数据库一中该条数据在数据库二中不存在，并且有用，放入新增表
                if (!chongfu)
                {
                    duo.Rows.Add(ds_table_View1.Tables[0].Rows[i1].ItemArray);
                }
            
            }


            OutPutHT = new Hashtable();
            OutPutHT["对比结果"] = duo;
            OutPutHT["执行标记"] = "数据表对比完成";
            DForThread(OutPutHT);


        }


        /// <summary>
        /// 对比字段
        /// </summary>
        public void Run_DBduibi_ziduan()
        {
            Hashtable OutPutHT = new Hashtable();

            Thread.Sleep(500);

            string kaitou = InPutHT["对比开头"].ToString();
            //打开第一个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF1;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL1;
            I_DBF1 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL1 = I_DBF1.DbLinkSqlMain(InPutHT["连接串1"].ToString());

            //打开第二个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF2;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL2;
            I_DBF2 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL2 = I_DBF2.DbLinkSqlMain(InPutHT["连接串2"].ToString());

            //获得第一个数据库的所有表 
            DataSet ds_table_View1 = new DataSet();
            Hashtable ht_table_View1 = new Hashtable();
            ht_table_View1 = I_DBL1.RunProc("select   '数据库1' as DB_name, name as table_name,xtype as table_type,table_owner='dbo' from sysobjects where xtype='U' and name<>'dtproperties'  and name like '" + kaitou + "%'  order by name", ds_table_View1);
            if (ht_table_View1["return_ds"] != null)
            {
                ds_table_View1 = (DataSet)(ht_table_View1["return_ds"]);
            }
            //获得第二个数据库的所有表 
            DataSet ds_table_View2 = new DataSet();
            Hashtable ht_table_View2 = new Hashtable();
            ht_table_View2 = I_DBL2.RunProc("select  '数据库2' as DB_name, name as table_name,xtype as table_type,table_owner='dbo' from sysobjects where xtype='U' and name<>'dtproperties'  and name like '" + kaitou + "%'  order by name", ds_table_View2);
            if (ht_table_View2["return_ds"] != null)
            {
                ds_table_View2 = (DataSet)(ht_table_View2["return_ds"]);
            }
            //对比表 
            DataTable duo = ds_table_View2.Tables[0].Clone(); //相同表，第一个数据库与第二个数据库相同的表
            string inbiao = " '十全大补丸' ";

            int allsl = Convert.ToInt32(ds_table_View1.Tables[0].Rows.Count) * Convert.ToInt32(ds_table_View2.Tables[0].Rows.Count);
            int nowsl = 1;

            for (int i1 = 0; i1 < ds_table_View1.Tables[0].Rows.Count; i1++)
            {
   
                for (int i2 = 0; i2 < ds_table_View2.Tables[0].Rows.Count; i2++)
                {
                    if (nowsl % 100 == 0)
                    {
                        OutPutHT = new Hashtable();
                        OutPutHT["进度显示"] = "第一步：约" + nowsl.ToString() + "，共" + allsl.ToString();
                        OutPutHT["执行标记"] = "进度显示";
                        DForThread(OutPutHT);
                    }
                    nowsl++;

                    if (ds_table_View1.Tables[0].Rows[i1]["table_owner"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_owner"].ToString() && ds_table_View1.Tables[0].Rows[i1]["table_name"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_name"].ToString() && ds_table_View1.Tables[0].Rows[i1]["table_type"].ToString() == ds_table_View2.Tables[0].Rows[i2]["table_type"].ToString())
                    {
                            //将存在的表放入临时表，这是带对比字段的表
                            duo.Rows.Add(ds_table_View1.Tables[0].Rows[i1].ItemArray);
                            inbiao = inbiao + " ,'"+ds_table_View1.Tables[0].Rows[i1]["table_name"].ToString()+"' ";
                    }
                }
 

            }



            //获得第一个数据库的所有字段
            DataSet ds_table_View11 = new DataSet();
            Hashtable ht_table_View11 = new Hashtable();
            ht_table_View1 = I_DBL1.RunProc(getSQL("数据库1",inbiao), ds_table_View11);
            if (ht_table_View11["return_ds"] != null)
            {
                ds_table_View11 = (DataSet)(ht_table_View11["return_ds"]);
            }
            //获得第二个数据库的所有字段
            DataSet ds_table_View22 = new DataSet();
            Hashtable ht_table_View22 = new Hashtable();
            ht_table_View22 = I_DBL2.RunProc(getSQL("数据库2", inbiao), ds_table_View22);
            if (ht_table_View22["return_ds"] != null)
            {
                ds_table_View22 = (DataSet)(ht_table_View22["return_ds"]);
            }


            //对比字段
            DataTable duo0 = ds_table_View22.Tables[0].Clone(); //差异表，第一个数据库比第二个数据库多出来的表
            allsl = Convert.ToInt32(ds_table_View11.Tables[0].Rows.Count) * Convert.ToInt32(ds_table_View22.Tables[0].Rows.Count);
            nowsl = 1;


            for (int i1 = 0; i1 < ds_table_View11.Tables[0].Rows.Count; i1++)
            {
                bool chongfu = false;
                for (int i2 = 0; i2 < ds_table_View22.Tables[0].Rows.Count; i2++)
                {
                    if (nowsl % 100 == 0)
                    {
                        OutPutHT = new Hashtable();
                        OutPutHT["进度显示"] = "第二步：约" + nowsl.ToString() + "，共" + allsl.ToString();
                        OutPutHT["执行标记"] = "进度显示";
                        DForThread(OutPutHT);
                    }
                    nowsl++;
                    if (ds_table_View11.Tables[0].Rows[i1]["TableName"].ToString().Replace("【数据库1】", "") == ds_table_View22.Tables[0].Rows[i2]["TableName"].ToString().Replace("【数据库2】", "") && ds_table_View11.Tables[0].Rows[i1]["ColumnName"].ToString() == ds_table_View22.Tables[0].Rows[i2]["ColumnName"].ToString() && ds_table_View11.Tables[0].Rows[i1]["PrimaryKey"].ToString() == ds_table_View22.Tables[0].Rows[i2]["PrimaryKey"].ToString() && ds_table_View11.Tables[0].Rows[i1]["IDENTITY"].ToString() == ds_table_View22.Tables[0].Rows[i2]["IDENTITY"].ToString() && ds_table_View11.Tables[0].Rows[i1]["Type"].ToString() == ds_table_View22.Tables[0].Rows[i2]["Type"].ToString() && ds_table_View11.Tables[0].Rows[i1]["Length"].ToString() == ds_table_View22.Tables[0].Rows[i2]["Length"].ToString() && ds_table_View11.Tables[0].Rows[i1]["Precision"].ToString() == ds_table_View22.Tables[0].Rows[i2]["Precision"].ToString() && ds_table_View11.Tables[0].Rows[i1]["Scale"].ToString() == ds_table_View22.Tables[0].Rows[i2]["Scale"].ToString() && ds_table_View11.Tables[0].Rows[i1]["NullAble"].ToString() == ds_table_View22.Tables[0].Rows[i2]["NullAble"].ToString() && ds_table_View11.Tables[0].Rows[i1]["Default"].ToString() == ds_table_View22.Tables[0].Rows[i2]["Default"].ToString())
                    {
                        //若数据相同，忽略掉;
                        chongfu = true;
                        break;
                    }
         
                }

                //若有用，放入新增表
                if (!chongfu)
                {
                    duo0.Rows.Add(ds_table_View11.Tables[0].Rows[i1].ItemArray);
                    DataRow[] dr2 = ds_table_View22.Tables[0].Select("TableName = '【数据库2】" + ds_table_View11.Tables[0].Rows[i1]["TableName"].ToString().Replace("【数据库1】", "") + "' and ColumnName = '" + ds_table_View11.Tables[0].Rows[i1]["ColumnName"].ToString() + "'");
                    if (dr2 != null && dr2.Count() > 0)
                    {
                        duo0.Rows.Add(dr2[0].ItemArray);
                    }
                }
            
            }



            OutPutHT = new Hashtable();
            OutPutHT["对比结果"] = duo0;
            OutPutHT["执行标记"] = "数据表对比完成";
            DForThread(OutPutHT);


        }


        
 

        /// <summary>
        /// 对比存储过程、视图、函数
        /// </summary>
        public void Run_DBduibi_SPSP()
        {
            Thread.Sleep(500);
            Hashtable OutPutHT = new Hashtable();

            //打开第一个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF1;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL1;
            I_DBF1 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL1 = I_DBF1.DbLinkSqlMain(InPutHT["连接串1"].ToString());

            //打开第二个数据库连接
            Galaxy.ClassLib.DataBaseFactory.I_DBFactory I_DBF2;
            Galaxy.ClassLib.DataBaseFactory.I_Dblink I_DBL2;
            I_DBF2 = new Galaxy.ClassLib.DataBaseFactory.DBFactory();
            I_DBL2 = I_DBF2.DbLinkSqlMain(InPutHT["连接串2"].ToString());

            //获得第一个数据库的所有存储过程、视图、函数
            DataSet ds_table_View1 = new DataSet();
            Hashtable ht_table_View1 = new Hashtable();
            ht_table_View1 = I_DBL1.RunProc("SELECT  '数据库1' as DB_name, [name],[type],[type_desc], '名称' as 不同处 FROM sys.all_objects  WHERE [type] in ('FN','IF','P','PC','TF','X','V')  AND [is_ms_shipped] = 0  ORDER BY [type],[name]", ds_table_View1);
            if (ht_table_View1["return_ds"] != null)
            {
                ds_table_View1 = (DataSet)(ht_table_View1["return_ds"]);
            }
            //获得第二个数据库的所有存储过程、视图、函数
            DataSet ds_table_View2 = new DataSet();
            Hashtable ht_table_View2 = new Hashtable();
            ht_table_View2 = I_DBL2.RunProc("SELECT  '数据库2' as DB_name, [name],[type],[type_desc], '名称' as 不同处 FROM sys.all_objects  WHERE [type] in ('FN','IF','P','PC','TF','X','V')  AND [is_ms_shipped] = 0  ORDER BY [type],[name]", ds_table_View2);
            if (ht_table_View2["return_ds"] != null)
            {
                ds_table_View2 = (DataSet)(ht_table_View2["return_ds"]);
            }
            //对比存储过程、视图、函数
            DataTable duo = ds_table_View2.Tables[0].Clone(); //差异表，第一个数据库比第二个数据库多出来的存储过程、视图、函数


            int allsl = Convert.ToInt32(ds_table_View1.Tables[0].Rows.Count) * Convert.ToInt32(ds_table_View2.Tables[0].Rows.Count);
            int nowsl = 1;

            for (int i1 = 0; i1 < ds_table_View1.Tables[0].Rows.Count; i1++)
            {
                bool chongfu = false;
                for (int i2 = 0; i2 < ds_table_View2.Tables[0].Rows.Count; i2++)
                {
                    if (nowsl % 500 == 0)
                    {
                        OutPutHT = new Hashtable();
                        OutPutHT["进度显示"] = "约" + nowsl.ToString() + "，共" + allsl.ToString();
                        OutPutHT["执行标记"] = "进度显示";
                        DForThread(OutPutHT);
                    }
                    nowsl++;
                    if (ds_table_View1.Tables[0].Rows[i1]["name"].ToString() == ds_table_View2.Tables[0].Rows[i2]["name"].ToString() && ds_table_View1.Tables[0].Rows[i1]["type"].ToString() == ds_table_View2.Tables[0].Rows[i2]["type"].ToString() && ds_table_View1.Tables[0].Rows[i1]["type_desc"].ToString() == ds_table_View2.Tables[0].Rows[i2]["type_desc"].ToString())
                    {
                        //若数据相同，对比生成sql， 若语句一致，忽略掉;，若不一致，插入放入新增表。
                        chongfu = true;


                        //获得存储过程、视图、函数的生成语句对比串（去掉注释行、空行、空格后，合并后的字符串，非正常SQL）
                        //获得第一个数据库指定某个存储过程、视图、函数的SQL语句对比串
                        DataSet ds_table_View11 = new DataSet();
                        Hashtable ht_table_View11 = new Hashtable();
                        ht_table_View11 = I_DBL1.RunProc("exec sp_helptext '" + ds_table_View1.Tables[0].Rows[i1]["name"].ToString() + "'", ds_table_View11);
                        if (ht_table_View11["return_ds"] != null)
                        {
                            ds_table_View11 = (DataSet)(ht_table_View11["return_ds"]);
                        }
                        string SP_sql1 = "";
                        if (ds_table_View11 != null && ds_table_View11.Tables.Count > 0 && ds_table_View11.Tables[0] != null && ds_table_View11.Tables[0].Rows.Count > 0)
                        {
                            for (int c = 0; c < ds_table_View11.Tables[0].Rows.Count; c++)
                            {
                                string thisc = ds_table_View11.Tables[0].Rows[c][0].ToString();
                                string thsc_quk = thisc.Replace(" ", "").Replace("	", "").Replace("\r", "").Replace("\n", "");
                                if (thsc_quk.IndexOf("--") != 0 && thsc_quk != "")
                                {
                                    SP_sql1 = SP_sql1 + thsc_quk;
                                }
                            }
                        }

                        //获得第二个数据库指定某个存储过程、视图、函数的SQL语句对比串
                        DataSet ds_table_View22 = new DataSet();
                        Hashtable ht_table_View22 = new Hashtable();
                        ht_table_View22 = I_DBL2.RunProc("exec sp_helptext '" + ds_table_View2.Tables[0].Rows[i2]["name"].ToString() + "'", ds_table_View22);
                        if (ds_table_View2.Tables[0].Rows[i2]["name"].ToString() == "ClearZero")
                        {
                            string aa = "";
                        }
                        if (ht_table_View22["return_ds"] != null)
                        {
                            ds_table_View22 = (DataSet)(ht_table_View22["return_ds"]);
                        }
                        string SP_sql2 = "";
                        if (ds_table_View22 != null && ds_table_View22.Tables.Count > 0 && ds_table_View22.Tables[0] != null && ds_table_View22.Tables[0].Rows.Count > 0)
                        {
                            for (int c = 0; c < ds_table_View22.Tables[0].Rows.Count; c++)
                            {
                                string thisc = ds_table_View22.Tables[0].Rows[c][0].ToString();
                                string thsc_quk = thisc.Replace(" ", "").Replace("	", "").Replace("\r", "").Replace("\n", "");
                                if (thsc_quk.IndexOf("--") != 0 && thsc_quk != "")
                                {
                                    SP_sql2 = SP_sql2 + thsc_quk;
                                }
                            }
                        }

                        if (SP_sql1 == SP_sql2)
                        {
                            chongfu = true;
                            
                        }
                        else
                        {
                            int numbt = Compare(SP_sql1, SP_sql2);
                            ds_table_View1.Tables[0].Rows[i1]["不同处"] = "SQL语句(" + numbt.ToString() + ")";
                            ds_table_View2.Tables[0].Rows[i2]["不同处"] = "SQL语句(" + numbt.ToString() + ")";
                            chongfu = false;
                        }
                        break;



                    }
                }
                //若数据库一中该条数据在数据库二中不存在，并且有用，放入新增表
                if (!chongfu)
                {
                    duo.Rows.Add(ds_table_View1.Tables[0].Rows[i1].ItemArray);
                }

            }


            OutPutHT = new Hashtable();
            OutPutHT["对比结果"] = duo;
            OutPutHT["执行标记"] = "数据表对比完成";
            DForThread(OutPutHT);
        }


        private int Compare(string str, string str1)
        {
            //string str = "abcd";
            //string str1 = "abce";
            for (int i = 0; i < str.Length; i++)
            {
                if (i >= str1.Length) continue;
                if (str[i] != str1[i]) return i;
            }
            return -1;//一样
        }

        public void Run_del()
        {
            Thread.Sleep(500);
            int xzh = Convert.ToInt32(InPutHT["xzh"]);
            int ygy = Convert.ToInt32(InPutHT["ygy"]);

            Hashtable OutPutHT = new Hashtable();

            for (int p = ygy; p > xzh; p--)
            {
                //填充传入参数哈希表
                OutPutHT["执行标记"] = "删除";
                OutPutHT["被删除行索引"] = p.ToString();//测试参数
                DForThread(OutPutHT);
                Thread.Sleep(10);

            }

            //填充传入参数哈希表
            OutPutHT = new Hashtable();
            OutPutHT["执行标记"] = "删除完成";
            DForThread(OutPutHT);

        }

        public void fileBeginduibi()
        {
            Thread.Sleep(500);
            Hashtable OutPutHT = new Hashtable();


            string path_ceshi = InPutHT["测试库目录"].ToString();
            string path_zhengshi = InPutHT["正式库目录"].ToString();
            string[] path_paichu = (string[])(InPutHT["要排除的目录"]);
            string runcopy = InPutHT["是否同时执行拷贝"].ToString();
            //得到测试库目录下所有文件，包含子目录
            string[] allfile = Directory.GetFiles(path_ceshi, "*.*", SearchOption.AllDirectories);
            //文件总数量
            int wenjian_num = allfile.Count();
            for (int i = 0; i < wenjian_num; i++)
            {

                if ((i+1) % 500 == 0)
                {
                    OutPutHT = new Hashtable();
                    OutPutHT["进度显示"] = "约" + (i + 1).ToString() + "，共" + wenjian_num.ToString();
                    OutPutHT["执行标记"] = "进度显示";
                    DForThread(OutPutHT);
                }


                string thisonePath_ceshi = allfile[i];
                string thisonePath_zhengshi = path_zhengshi + allfile[i].Remove(0, path_ceshi.Count());
                //若是隐藏文件，跳过不处理
                if (File.GetAttributes(thisonePath_ceshi).ToString().IndexOf("Hidden") >= 0)
                {
                    continue;
                }
                //指定的被跳过的目录或文件，跳过不处理
                bool sftg = false;
                for (int p = 0; p < path_paichu.Count(); p++)
                {
                    if (path_paichu[p].Trim() == "")
                    {
                        continue;
                    }
                    //目录
                    if (thisonePath_ceshi.IndexOf(@"\"+path_paichu[p]+@"\") >= 0)
                    {
                        sftg = true;
                        break;
                    }
                    //文件
                    if (thisonePath_ceshi.IndexOf(@"\" + path_paichu[p]) == thisonePath_ceshi.Count() - (@"\" + path_paichu[p]).Count())
                    {
                        sftg = true;
                        break;
                    }
                }
                if (sftg)
                {
                    continue;
                }
                //正式库是否存在文件
                string zt = "";
                bool sfcz = File.Exists(thisonePath_zhengshi);
                if (sfcz)
                {
                    zt = "修改";
                }
                else
                {
                    zt = "新增";
                }

                //得到测试库最后一次修改时间
                DateTime dtLastWriteTime_ceshi = File.GetLastWriteTime(thisonePath_ceshi);

                //如果文件存在
                string shijian_zhengshi = "       ";
                if (sfcz)
                {
                    DateTime dtLastWriteTime_zhengshi;
                    //得到正式库最后一次修改时间
                    dtLastWriteTime_zhengshi = File.GetLastWriteTime(thisonePath_zhengshi);
                    //测试库时间大于等于正式库时间，忽略
                    if (dtLastWriteTime_ceshi <= dtLastWriteTime_zhengshi)
                    { continue; }
                    shijian_zhengshi = dtLastWriteTime_zhengshi.ToString();

                }

                //

                //拷贝
                string kaobeizhuangtai = "";
                if (runcopy == "是")
                {
                    File.Copy(thisonePath_ceshi, thisonePath_zhengshi, true);
                    kaobeizhuangtai = "已拷贝";
                }
                else
                {
                    kaobeizhuangtai = "等待";
                }

                string[] re = new string[] { zt, kaobeizhuangtai, dtLastWriteTime_ceshi.ToString(), shijian_zhengshi, thisonePath_ceshi, thisonePath_zhengshi };

                Thread.Sleep(1);
                //填充传入参数哈希表
                OutPutHT = new Hashtable();
                OutPutHT["执行标记"] = "完成一个";
                OutPutHT["执行结果"] = re;
                DForThread(OutPutHT);
            }

            //填充传入参数哈希表
            OutPutHT = new Hashtable();
            OutPutHT["执行标记"] = "文件对比完成";
            OutPutHT["执行结果"] = "";
            DForThread(OutPutHT);

        }



        /// <summary> 
        /// 执行DOS命令，返回DOS命令的输出 
        /// </summary> 
        /// <param name="dosCommand">dos命令</param> 
        /// 如果设定为0，则无限等待</param> 
        /// <returns>返回DOS命令的输出</returns> 
        private string Execute(string command, int seconds)
        {
            string output = ""; //输出字符串 
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令 
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出 
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;//不重定向输入 
                startInfo.RedirectStandardOutput = true; //重定向输出 
                startInfo.CreateNoWindow = true;//不创建窗口 
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程 
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束 
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒 
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出 
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }




        private DataTable GetXmlNodeInformation(string xmlPath, DataTable dtmain)
        {
            DataTable dt = new DataTable("整合后");
            dt.Columns.Add("author");
            dt.Columns.Add("date");
            dt.Columns.Add("revision");


            dt.Columns.Add("action");
            dt.Columns.Add("kind");
            dt.Columns.Add("path");



            try
            {
                //定义并从xml文件中加载节点（根节点）
                XElement rootNode = XElement.Load(xmlPath);
                ////查询语句: 获得根节点下name子节点（此时的子节点可以跨层次：孙节点、重孙节点......）
                //IEnumerable<XElement> targetNodes = from target in rootNode.Descendants("name")
                //                                    select target;
                //foreach (XElement node in targetNodes)
                //{
                //    Console.WriteLine("name = {0}", node.Value);
                //}

                //查询语句: 获取ID属性值等于"111111"并且函数子节点的所有User节点（并列条件用"&&"符号连接）

                for (int p = 0; p < dtmain.Rows.Count; p++)
                {
                    IEnumerable<XElement> myTargetNodes = from myTarget in rootNode.Descendants("path")
                                                          where myTarget.Parent.Parent.Attribute("revision").Value.Equals(dtmain.Rows[p]["revision"].ToString())

                                                          select myTarget;


                    int pp = myTargetNodes.Count();
                    string author = dtmain.Rows[p]["author"].ToString();
                    string date = dtmain.Rows[p]["date"].ToString();
                    string revision = dtmain.Rows[p]["revision"].ToString();


                    if (revision == "1559")
                    {
                        string aaa = "";
                    }
                    foreach (XElement node in myTargetNodes)
                    {





                        string path = node.Value;
                        string action = node.Attribute("action").Value;
                        string kind = node.Attribute("kind").Value;
                        dt.Rows.Add(new string[] { author, date, revision, kind, action, path });

                    }
                }

            }
            catch (Exception ex)
            {
                return dt;
            }
            return dt;
        }


        //获取表结构查询的SQL
        private string getSQL(string tablenameEX,string inbiao)
        {
            return @"--sql server 2005
-- 1. 表结构信息查询 
-- ========================================================================
-- 表结构信息查询
-- 邹建 2005.08(引用请保留此信息)
-- ========================================================================
SELECT 
    TableName='【"
                +tablenameEX+@"】'+O.name,
    TableDesc=PTB.[value],
    Column_id=C.column_id,
    ColumnName=C.name,
    PrimaryKey=ISNULL(IDX.PrimaryKey,N''),
    [IDENTITY]=CASE WHEN C.is_identity=1 THEN N'√'ELSE N'' END,
    Computed=CASE WHEN C.is_computed=1 THEN N'√'ELSE N'' END,
    Type=T.name,
    Length=C.max_length,
    Precision=C.precision,
    Scale=C.scale,
    NullAble=CASE WHEN C.is_nullable=1 THEN N'√'ELSE N'' END,
    [Default]=ISNULL(D.definition,N''),
    ColumnDesc=ISNULL(PFD.[value],N''),
    IndexName=ISNULL(IDX.IndexName,N''),
    IndexSort=ISNULL(IDX.Sort,N''),
    Create_Date=O.Create_Date,
    Modify_Date=O.Modify_date
FROM sys.columns C
    INNER JOIN sys.objects O
        ON C.[object_id]=O.[object_id]
            AND O.type='U'
            AND O.is_ms_shipped=0
    INNER JOIN sys.types T
        ON C.user_type_id=T.user_type_id
    LEFT JOIN sys.default_constraints D
        ON C.[object_id]=D.parent_object_id
            AND C.column_id=D.parent_column_id
            AND C.default_object_id=D.[object_id]
    LEFT JOIN sys.extended_properties PFD
        ON PFD.class=1 
            AND C.[object_id]=PFD.major_id 
            AND C.column_id=PFD.minor_id
--             AND PFD.name='Caption'  -- 字段说明对应的描述名称(一个字段可以添加多个不同name的描述)
    LEFT JOIN sys.extended_properties PTB
        ON PTB.class=1 
            AND PTB.minor_id=0 
            AND C.[object_id]=PTB.major_id
--             AND PFD.name='Caption'  -- 表说明对应的描述名称(一个表可以添加多个不同name的描述) 
    LEFT JOIN                       -- 索引及主键信息
    (
        SELECT 
            IDXC.[object_id],
            IDXC.column_id,
            Sort=CASE INDEXKEY_PROPERTY(IDXC.[object_id],IDXC.index_id,IDXC.index_column_id,'IsDescending')
                WHEN 1 THEN 'DESC' WHEN 0 THEN 'ASC' ELSE '' END,
            PrimaryKey=CASE WHEN IDX.is_primary_key=1 THEN N'√'ELSE N'' END,
            IndexName=IDX.Name
        FROM sys.indexes IDX
        INNER JOIN sys.index_columns IDXC
            ON IDX.[object_id]=IDXC.[object_id]
                AND IDX.index_id=IDXC.index_id
        LEFT JOIN sys.key_constraints KC
            ON IDX.[object_id]=KC.[parent_object_id]
                AND IDX.index_id=KC.unique_index_id
        INNER JOIN  -- 对于一个列包含多个索引的情况,只显示第1个索引信息
        (
            SELECT [object_id], Column_id, index_id=MIN(index_id)
            FROM sys.index_columns
            GROUP BY [object_id], Column_id
        ) IDXCUQ
            ON IDXC.[object_id]=IDXCUQ.[object_id]
                AND IDXC.Column_id=IDXCUQ.Column_id
                AND IDXC.index_id=IDXCUQ.index_id
    ) IDX
        ON C.[object_id]=IDX.[object_id]
            AND C.column_id=IDX.column_id 
 WHERE O.name in ("
                + inbiao+@")       -- 如果只查询指定表,加上此条件 
ORDER BY O.name,C.column_id";
        }


    }
}
