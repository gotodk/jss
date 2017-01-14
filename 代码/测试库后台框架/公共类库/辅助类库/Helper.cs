using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;


/// <summary>
/// Helper 的摘要说明
/// </summary>
public static class Helper
{
    //——————————————————————————————序列化——————————————————————————————————
    #region 以下代码为序列化/反序列化帮助方法
    /// <summary>
    /// 把DataTable序列化成Json（郭拓 2013-08-09）
    /// </summary>
    /// <param name="dt">要被序列化的数据表</param>
    /// <returns>序列化后的Json字符串</returns>
    public static string DataTableToJson(DataTable dt)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        ArrayList dic = new ArrayList();

        foreach (DataRow row in dt.Rows)
        {
            Dictionary<string, object> drow = new Dictionary<string, object>();
            foreach (DataColumn col in dt.Columns)
            {
                drow.Add(col.ColumnName, row[col.ColumnName]);
            }
            dic.Add(drow);
        }
        return jss.Serialize(dic);
    }

    /// <summary>
    /// 把Json序列化成DataTable（郭拓 2013-08-09）
    /// </summary>
    /// <param name="jsonStr">要被序列化的Json字符串</param>
    /// <returns>序列化后的DataTable</returns>
    public static DataTable JsonToDatatable(string jsonStr)
    {
        JavaScriptSerializer jss = new JavaScriptSerializer();
        ArrayList dic = jss.Deserialize<ArrayList>(jsonStr);
        DataTable dtb = new DataTable();
        if (dic.Count > 0)
        {
            foreach (Dictionary<string, object> drow in dic)
            {
                if (dtb.Columns.Count == 0)
                {
                    foreach (string key in drow.Keys)
                    {
                        dtb.Columns.Add(key, drow[key].GetType());
                    }
                }
                DataRow row = dtb.NewRow();
                foreach (string key in drow.Keys)
                {
                    row[key] = drow[key];
                }
                dtb.Rows.Add(row);
            }
        }
        return dtb;
    }

    /// <summary>
    /// 把DataTable序列化成XML（郭拓 2013-08-09）
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static string DataTableToXML(DataTable dt)
    {
        StringBuilder sb = new StringBuilder();
        XmlWriter wrter = XmlWriter.Create(sb);
        XmlSerializer seril = new XmlSerializer(typeof(DataTable));
        seril.Serialize(wrter, dt);
        wrter.Close();
        return sb.ToString();
    }

    /// <summary>
    /// 把XML序列化成DataTable（郭拓 2013-08-09）
    /// </summary>
    /// <param name="xmlStr"></param>
    /// <returns></returns>
    public static DataTable XMLToDataTable(string xmlStr)
    {
        StringReader strReader = new StringReader(xmlStr);
        XmlReader xmlReader = XmlReader.Create(strReader);
        XmlSerializer serializer = new XmlSerializer(typeof(DataTable));
        DataTable dt = serializer.Deserialize(xmlReader) as DataTable;
        return dt;
    }

    /// <summary>
    /// 将DataSet格式化成字节数组byte[]
    /// </summary>
    /// <param name="dsOriginal">DataSet对象</param>
    /// <returns>字节数组</returns>
    public static byte[] GetBinaryFormatData(DataSet dsOriginal)
    {
        byte[] binaryDataResult = null;
        MemoryStream memStream = new MemoryStream();
        IFormatter brFormatter = new BinaryFormatter();
        dsOriginal.RemotingFormat = SerializationFormat.Binary;
        brFormatter.Serialize(memStream, dsOriginal);
        binaryDataResult = memStream.ToArray();
        memStream.Close();
        memStream.Dispose();
        return binaryDataResult;
    }

    /// <summary>
    /// 将DataSet格式化成字节数组byte[]，并且已经经过压缩
    /// </summary>
    /// <param name="dsOriginal">DataSet对象</param>
    /// <returns>字节数组</returns>
    public static byte[] GetBinaryFormatDataCompress(DataSet dsOriginal)
    {
        byte[] binaryDataResult = null;
        MemoryStream memStream = new MemoryStream();
        IFormatter brFormatter = new BinaryFormatter();
        dsOriginal.RemotingFormat = SerializationFormat.Binary;
        brFormatter.Serialize(memStream, dsOriginal);
        binaryDataResult = memStream.ToArray();
        memStream.Close();
        memStream.Dispose();
        return Compress(binaryDataResult);
    }

    /// <summary>
    /// 解压数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Decompress(byte[] data)
    {
        byte[] bData;
        MemoryStream ms = new MemoryStream();
        ms.Write(data, 0, data.Length);
        ms.Position = 0;
        GZipStream stream = new GZipStream(ms, CompressionMode.Decompress, true);
        byte[] buffer = new byte[1024];
        MemoryStream temp = new MemoryStream();
        int read = stream.Read(buffer, 0, buffer.Length);
        while (read > 0)
        {
            temp.Write(buffer, 0, read);
            read = stream.Read(buffer, 0, buffer.Length);
        }
        //必须把stream流关闭才能返回ms流数据,不然数据会不完整
        stream.Close();
        stream.Dispose();
        ms.Close();
        ms.Dispose();
        bData = temp.ToArray();
        temp.Close();
        temp.Dispose();
        return bData;
    }

    /// <summary>
    /// 压缩数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static byte[] Compress(byte[] data)
    {
        byte[] bData;
        MemoryStream ms = new MemoryStream();
        GZipStream stream = new GZipStream(ms, CompressionMode.Compress, true);
        stream.Write(data, 0, data.Length);
        stream.Close();
        stream.Dispose();
        //必须把stream流关闭才能返回ms流数据,不然数据会不完整
        //并且解压缩方法stream.Read(buffer, 0, buffer.Length)时会返回0
        bData = ms.ToArray();
        ms.Close();
        ms.Dispose();
        return bData;
    }

    /// <summary>
    /// 将字节数组反序列化成DataSet对象
    /// </summary>
    /// <param name="binaryData">字节数组</param>
    /// <returns>DataSet对象</returns>
    public static DataSet RetrieveDataSet(byte[] binaryData)
    {
        DataSet dsOriginal = null;
        MemoryStream memStream = new MemoryStream(binaryData);
        IFormatter brFormatter = new BinaryFormatter();
        Object obj = brFormatter.Deserialize(memStream);
        dsOriginal = (DataSet)obj;
        return dsOriginal;
    }

    /// <summary>
    /// 将字节数组反解压后序列化成DataSet对象
    /// </summary>
    /// <param name="binaryData">字节数组</param>
    /// <returns>DataSet对象</returns>
    public static DataSet RetrieveDataSetDecompress(byte[] binaryData)
    {
        DataSet dsOriginal = null;
        MemoryStream memStream = new MemoryStream(Decompress(binaryData));
        IFormatter brFormatter = new BinaryFormatter();
        Object obj = brFormatter.Deserialize(memStream);
        dsOriginal = (DataSet)obj;
        return dsOriginal;
    }

    /// <summary>
    /// 将object格式化成字节数组byte[]
    /// </summary>
    /// <param name="dsOriginal">object对象</param>
    /// <returns>字节数组</returns>
    public static byte[] GetBinaryFormatData(object dsOriginal)
    {
        byte[] binaryDataResult = null;
        MemoryStream memStream = new MemoryStream();
        IFormatter brFormatter = new BinaryFormatter();
        brFormatter.Serialize(memStream, dsOriginal);
        binaryDataResult = memStream.ToArray();
        memStream.Close();
        memStream.Dispose();
        return binaryDataResult;
    }

    /// <summary>
    /// 将objec格式化成字节数组byte[]，并压缩
    /// </summary>
    /// <param name="dsOriginal">object对象</param>
    /// <returns>字节数组</returns>
    public static byte[] GetBinaryFormatDataCompress(object dsOriginal)
    {
        byte[] binaryDataResult = null;
        MemoryStream memStream = new MemoryStream();
        IFormatter brFormatter = new BinaryFormatter();
        brFormatter.Serialize(memStream, dsOriginal);
        binaryDataResult = memStream.ToArray();
        memStream.Close();
        memStream.Dispose();
        return Compress(binaryDataResult);
    }

    /// <summary>
    /// 将字节数组反序列化成object对象
    /// </summary>
    /// <param name="binaryData">字节数组</param>
    /// <returns>object对象</returns>
    public static object RetrieveObject(byte[] binaryData)
    {
        MemoryStream memStream = new MemoryStream(binaryData);
        IFormatter brFormatter = new BinaryFormatter();
        Object obj = brFormatter.Deserialize(memStream);
        return obj;
    }

    /// <summary>
    /// 将字节数组解压后反序列化成object对象
    /// </summary>
    /// <param name="binaryData">字节数组</param>
    /// <returns>object对象</returns>
    public static object RetrieveObjectDecompress(byte[] binaryData)
    {
        MemoryStream memStream = new MemoryStream(Decompress(binaryData));
        IFormatter brFormatter = new BinaryFormatter();
        Object obj = brFormatter.Deserialize(memStream);
        return obj;
    }

    /// <summary>
    /// 将DataSet序列化成byte[]（强制压缩） 2013-09-10 guotuo
    /// </summary>
    /// <param name="ds">要序列化的DataSet</param>
    /// <returns></returns>
    public static byte[] DataSet2Byte(DataSet ds)
    {
        /* 2013-09-10 郭拓&于海滨
         * 分隔符标准：
         * 表-表：[TnTn]
         * 表-列名-数据[TnCnD]
         * 列名-列名[CnCn]
         * 行-行[RR]
         * 列-列[CC]
         */
        byte[] result = null;
        StringBuilder resultSB = new StringBuilder();
        //开始循环表名
        for (int t = 0; t < ds.Tables.Count; t++)
        {
            resultSB.Append(ds.Tables[t].TableName);
            resultSB.Append("[TnCnD]");
            //开始循环列名
            for (int c = 0; c < ds.Tables[t].Columns.Count; c++)
            {
                if (c != 0)
                    resultSB.Append("[CnCn]");
                resultSB.Append(ds.Tables[t].Columns[c].ColumnName);
            }
            resultSB.Append("[TnCnD]");
            //开始循环数据
            for (int i = 0; i < ds.Tables[t].Rows.Count; i++)
            {
                if (i != 0)
                    resultSB.Append("[RR]");
                for (int j = 0; j < ds.Tables[t].Columns.Count; j++)
                {
                    if (j != 0)
                        resultSB.Append("[CC]");
                    resultSB.Append(ds.Tables[t].Rows[i][j].ToString());//每个单元格的数据
                }
            }
            resultSB.Append("[TnTn]");
        }
        result = Encoding.Unicode.GetBytes(resultSB.ToString());
        result = Compress(result);
        return result;
    }
    /// <summary>
    /// 将byte[]（压缩后）序列化为DataSet 2013-09-10 guotuo
    /// </summary>
    /// <param name="b">要序列化的压缩后的byte[]</param>
    /// <returns></returns>
    public static DataSet Byte2DataSet(byte[] b)
    {
        /* 2013-09-10 郭拓&于海滨
         * 分隔符标准：
         * 表-表：[TnTn]
         * 表-列名-数据[TnCnD]
         * 列名-列名[CnCn]
         * 行-行[RR]
         * 列-列[CC]
         */
        DataSet ds = new DataSet();
        //解压Byte数组
        b = Decompress(b);
        //获取源串
        string strResource = Encoding.Unicode.GetString(b);

        string[] Tables = strResource.Split(new string[] { "[TnTn]" }, StringSplitOptions.RemoveEmptyEntries);
        for (int TablesCount = 0; TablesCount < Tables.Length; TablesCount++)
        {
            DataTable dt = new DataTable();//循环出来的第一个数据
            string[] TableInfo = Tables[TablesCount].Split(new string[] { "[TnCnD]" }, StringSplitOptions.None);//单个数据表的源串
            dt.TableName = TableInfo[0];//表名

            string[] ColumsInfo = TableInfo[1].Split(new string[] { "[CnCn]" }, StringSplitOptions.None);//列名
            for (int ColumnsCount = 0; ColumnsCount < ColumsInfo.Length; ColumnsCount++)
            {
                dt.Columns.Add(ColumsInfo[ColumnsCount]);
            }//到这里已经完成单个DataTable架构的初始化

            //开始插入表数据
            if (TableInfo.Length >= 3)
            {
                string[] DataInfo = TableInfo[2].Split(new string[] { "[RR]" }, StringSplitOptions.None);
                for (int RowsCount = 0; RowsCount < DataInfo.Length; RowsCount++)
                {
                    dt.Rows.Add(DataInfo[RowsCount].Split(new string[] { "[CC]" }, StringSplitOptions.None));
                }
            }
            ds.Tables.Add(dt);
        }



        return ds;
    }



    /// <summary>
    /// 将byte[]（压缩后）序列化为DataSet 2013-09-11 于海滨复制修改
    /// </summary>
    /// <param name="b">要序列化的压缩后的byte[]</param>
    /// <param name="HTspColumns">要特殊设置的列类型key是列名，值是类型标记（例如System.Int32）</param>
    /// <returns></returns>
    public static DataSet Byte2DataSet(byte[] b, Hashtable HTspColumns)
    {
        /* 2013-09-10 郭拓&于海滨
         * 分隔符标准：
         * 表-表：[TnTn]
         * 表-列名-数据[TnCnD]
         * 列名-列名[CnCn]
         * 行-行[RR]
         * 列-列[CC]
         */
        DataSet ds = new DataSet();
        //解压Byte数组
        b = Decompress(b);
        //获取源串
        string strResource = Encoding.Unicode.GetString(b);

        string[] Tables = strResource.Split(new string[] { "[TnTn]" }, StringSplitOptions.RemoveEmptyEntries);
        for (int TablesCount = 0; TablesCount < Tables.Length; TablesCount++)
        {
            DataTable dt = new DataTable();//循环出来的第一个数据
            string[] TableInfo = Tables[TablesCount].Split(new string[] { "[TnCnD]" }, StringSplitOptions.None);//单个数据表的源串
            dt.TableName = TableInfo[0];//表名

            string[] ColumsInfo = TableInfo[1].Split(new string[] { "[CnCn]" }, StringSplitOptions.None);//列名
            for (int ColumnsCount = 0; ColumnsCount < ColumsInfo.Length; ColumnsCount++)
            {
                dt.Columns.Add(ColumsInfo[ColumnsCount]);
                if (HTspColumns.ContainsKey(ColumsInfo[ColumnsCount]))
                {
                    dt.Columns[ColumnsCount].DataType = Type.GetType(HTspColumns[ColumsInfo[ColumnsCount]].ToString());
                }
            }//到这里已经完成单个DataTable架构的初始化

            //开始插入表数据
            if (TableInfo.Length >= 3)
            {
                string[] DataInfo = TableInfo[2].Split(new string[] { "[RR]" }, StringSplitOptions.None);
                for (int RowsCount = 0; RowsCount < DataInfo.Length; RowsCount++)
                {
                    dt.Rows.Add(DataInfo[RowsCount].Split(new string[] { "[CC]" }, StringSplitOptions.None));
                }
            }
            ds.Tables.Add(dt);
        }



        return ds;
    }
    #endregion

    //—————————————————————————————————拼音———————————————————————————————————
    #region 本标记中的代码为拼音帮助类
    #region 属性数据定义
    /// <summary>
    /// 汉字的机内码数组
    /// </summary>
    private static int[] pyValue = new int[]
        {
            -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
            -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
            -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
            -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
            -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
            -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
            -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
            -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
            -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
            -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
            -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
            -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
            -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
            -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
            -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
            -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
            -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
            -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
            -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
            -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
            -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
            -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
            -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
            -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
            -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
            -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
            -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
            -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
            -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
            -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
            -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
            -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
            -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };
    /// <summary>
    /// 机内码对应的拼音数组
    /// </summary>
    private static string[] pyName = new string[]
        {
            "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
            "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
            "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
            "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
            "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
            "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
            "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
            "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
            "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
            "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
            "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
            "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
            "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
            "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
            "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
            "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
            "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
            "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
            "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
            "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
            "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
            "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
            "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
            "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
            "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
            "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
            "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
            "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
            "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
            "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
            "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
            "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
            "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };
    #endregion

    #region 把汉字转换成拼音(全拼)无间隔符号
    /// <summary>
    /// 把汉字转换成拼音(全拼)
    /// </summary>
    /// <param name="hzString">汉字字符串</param>
    /// <returns>转换后的拼音(全拼)字符串</returns>
    public static string Convert(string hzString)
    {
        // 匹配中文字符
        Regex regex = new Regex("^[\u4e00-\u9fa5]$");
        byte[] array = new byte[2];
        string pyString = "";
        int chrAsc = 0;
        int i1 = 0;
        int i2 = 0;
        char[] noWChar = hzString.ToCharArray();
        for (int j = 0; j < noWChar.Length; j++)
        {
            // 中文字符
            if (regex.IsMatch(noWChar[j].ToString()))
            {

                array = Encoding.GetEncoding("gb2312").GetBytes(noWChar[j].ToString());
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrAsc = i1 * 256 + i2 - 65536;
                if (chrAsc > 0 && chrAsc < 160)
                {
                    pyString += noWChar[j];
                }
                else
                {
                    // 修正部分文字
                    if (chrAsc == -9254)  // 修正“圳”字
                        pyString += "Zhen";
                    else
                    {
                        for (int i = (pyValue.Length - 1); i >= 0; i--)
                        {
                            if (pyValue[i] <= chrAsc)
                            {
                                pyString += pyName[i];
                                break;
                            }
                        }
                    }
                }
            }
            // 非中文字符
            else
            {
                pyString += noWChar[j].ToString();
            }
        }
        return pyString;
    }
    #endregion
    #region 把汉字转换成拼音(全拼) 用空格间隔
    /// <summary>
    /// 把汉字转换成拼音(全拼)
    /// </summary>
    /// <param name="hzString">汉字字符串</param>
    /// <returns>转换后的拼音(全拼)字符串</returns>
    public static string ConvertWithBlank(string hzString)
    {
        // 匹配中文字符
        Regex regex = new Regex("^[\u4e00-\u9fa5]$");
        byte[] array = new byte[2];
        string pyString = "";
        int chrAsc = 0;
        int i1 = 0;
        int i2 = 0;
        char[] noWChar = hzString.ToCharArray();
        for (int j = 0; j < noWChar.Length; j++)
        {
            // 中文字符
            if (regex.IsMatch(noWChar[j].ToString()))
            {
                array = Encoding.GetEncoding("gb2312").GetBytes(noWChar[j].ToString());
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrAsc = i1 * 256 + i2 - 65536;
                if (chrAsc > 0 && chrAsc < 160)
                {
                    pyString = pyString + " " + noWChar[j];
                }
                else
                {
                    // 修正部分文字
                    if (chrAsc == -9254)  // 修正“圳”字
                        pyString = pyString + " " + "Zhen";
                    else
                    {
                        for (int i = (pyValue.Length - 1); i >= 0; i--)
                        {
                            if (pyValue[i] <= chrAsc)
                            {
                                pyString = pyString + " " + pyName[i];
                                break;
                            }
                        }
                    }
                }
            }
            // 非中文字符
            else
            {
                pyString = pyString + " " + noWChar[j].ToString();
            }
        }
        return pyString.Trim();
    }
    #endregion
    #region 把汉字转换成拼音(全拼) 用特定的字符间隔
    /// <summary>
    /// 把汉字转换成拼音(全拼)
    /// </summary>
    /// <param name="hzString">汉字字符串</param>
    /// <returns>转换后的拼音(全拼)字符串</returns>
    public static string ConvertWithSplitChar(string hzString, string splitChar)
    {
        // 匹配中文字符
        Regex regex = new Regex("^[\u4e00-\u9fa5]$");
        byte[] array = new byte[2];
        string pyString = "";
        int chrAsc = 0;
        int i1 = 0;
        int i2 = 0;
        char[] noWChar = hzString.ToCharArray();
        for (int j = 0; j < noWChar.Length; j++)
        {
            // 中文字符
            if (regex.IsMatch(noWChar[j].ToString()))
            {
                array = Encoding.GetEncoding("gb2312").GetBytes(noWChar[j].ToString());
                i1 = (short)(array[0]);
                i2 = (short)(array[1]);
                chrAsc = i1 * 256 + i2 - 65536;
                if (chrAsc > 0 && chrAsc < 160)
                {
                    pyString = pyString + splitChar + noWChar[j];
                }
                else
                {
                    // 修正部分文字
                    if (chrAsc == -9254)  // 修正“圳”字
                        pyString = pyString + splitChar + "Zhen";
                    else
                    {
                        for (int i = (pyValue.Length - 1); i >= 0; i--)
                        {
                            if (pyValue[i] <= chrAsc)
                            {
                                pyString = pyString + splitChar + pyName[i];
                                break;
                            }
                        }
                    }
                }
            }
            // 非中文字符
            else
            {
                pyString = pyString + splitChar + noWChar[j].ToString();
            }
        }
        char[] trimAChar = splitChar.ToCharArray();
        return pyString.TrimStart(trimAChar);
    }
    #endregion
    #region 汉字转拼音缩写 (字符串) (小写)
    /// <summary>
    /// 汉字转拼音缩写
    /// </summary>
    /// <param name="str">要转换的汉字字符串</param>
    /// <returns>拼音缩写</returns>
    public static string GetSpellStringLower(string str)
    {
        string tempStr = "";
        foreach (char c in str)
        {
            if ((int)c >= 33 && (int)c <= 126)
            {
                //字母和符号原样保留
                tempStr += c.ToString();
            }
            else
            {
                //累加拼音声母
                tempStr += GetSpellCharLower(c.ToString());
            }
        }
        return tempStr;
    }
    #endregion
    #region 汉字转拼音缩写 (字符串) (小写) (空格间隔)
    /// <summary>
    /// 汉字转拼音缩写 (字符串) (小写) (空格间隔)
    /// </summary>
    /// <param name="str">要转换的汉字字符串</param>
    /// <returns>拼音缩写</returns>
    public static string GetSpellStringLowerSplitWithBlank(string str)
    {
        string tempStr = "";
        foreach (char c in str)
        {
            if ((int)c >= 33 && (int)c <= 126)
            {
                //字母和符号原样保留
                tempStr = tempStr + " " + c.ToString();
            }
            else
            {
                //累加拼音声母
                tempStr = tempStr + " " + GetSpellCharLower(c.ToString());
            }
        }
        return tempStr.Trim();
    }
    #endregion
    #region 汉字转拼音缩写 (字符串)(大写)
    /// <summary>
    /// 汉字转拼音缩写 (大写)
    /// </summary>
    /// <param name="str">要转换的汉字字符串</param>
    /// <returns>拼音缩写</returns>
    public static string GetSpellStringSupper(string str)
    {
        string tempStr = "";
        foreach (char c in str)
        {
            if ((int)c >= 33 && (int)c <= 126)
            {
                //字母和符号原样保留
                tempStr += c.ToString();
            }
            else
            {
                //累加拼音声母
                tempStr += GetSpellCharSupper(c.ToString());
            }
        }
        return tempStr;
    }
    #endregion
    #region 汉字转拼音缩写 (字符串)(大写)(空格间隔)
    /// <summary>
    /// 汉字转拼音缩写  (字符串)(大写)(空格间隔)
    /// </summary>
    /// <param name="str">要转换的汉字字符串</param>
    /// <returns>拼音缩写</returns>
    public static string GetSpellStringSupperSplitWithBlank(string str)
    {
        string tempStr = "";
        foreach (char c in str)
        {
            if ((int)c >= 33 && (int)c <= 126)
            {
                //字母和符号原样保留
                tempStr = tempStr + " " + c.ToString();
            }
            else
            {
                //累加拼音声母
                tempStr = tempStr + " " + GetSpellCharSupper(c.ToString());
            }
        }
        return tempStr.Trim();
    }
    #endregion
    #region 取单个字符的拼音声母(字符)(大写)
    /// <summary>
    /// 取单个字符的拼音声母
    /// </summary>
    /// <param name="c">要转换的单个汉字</param>
    /// <returns>拼音声母</returns>
    public static string GetSpellCharSupper(string c)
    {
        return Convert(c).Substring(0, 1).ToUpper();
    }
    #endregion
    #region 取单个字符的拼音声母(字符)(小写)
    /// <summary>
    /// 取单个字符的拼音声母
    /// </summary>
    /// <param name="c">要转换的单个汉字</param>
    /// <returns>拼音声母</returns>
    public static string GetSpellCharLower(string c)
    {
        return Convert(c).Substring(0, 1).ToLower();
    }
    #endregion

    /// <summary>
    /// 获取汉字字符串的首字母，例如：白菜->BC
    /// </summary>
    /// <param name="str">汉字字符串</param>
    /// <returns></returns>
    public static string GetHeadLetters(string str)
    {
        string Result = "";
        char[] cArray = str.ToCharArray();//转换成char数组
        foreach (char c in cArray)
        {
            Result += Convert(c.ToString()).Substring(0, 1);
        }
        return Result;
    }
    #endregion


    //————————————————————————————————————————————————————————————————————————
}