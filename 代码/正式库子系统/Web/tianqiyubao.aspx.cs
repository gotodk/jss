using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using System.Text;


public partial class Web_tianqiyubao : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //济南101120101     长清101120102     章丘101120104    济阳101120106    天桥101120107  商河101120103  平阴101120105
        //
        DataSet dstq = GetNowTQ("101120101");
        GridView1.DataSource = dstq.Tables[0].DefaultView;
        GridView1.DataBind();
    }

    /// <summary>
    /// 根据城市代码获取天气，来源为中国天气网
    /// </summary>
    /// <param name="cityid"></param>
    /// <returns></returns>
    public DataSet GetNowTQ(string cityid)
    {
        try
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.GetEncoding("utf-8");
            string reply = client.DownloadString("http://m.weather.com.cn/data/" + cityid + ".html");
            DataSet ds = GetDataSetformJSON(reply);

            Hashtable htDZB = new Hashtable();
            htDZB["city"] = "城市中文名";
            htDZB["city_en"] = "城市英文名";
            htDZB["date_y"] = "完整日期";
            htDZB["date"] = "date作用未知";
            htDZB["week"] = "星期";
            htDZB["fchh"] = "fchh作用未知";
            htDZB["cityid"] = "城市编号";

            htDZB["temp1"] = "第一天摄氏度";
            htDZB["temp2"] = "第二天摄氏度";
            htDZB["temp3"] = "第三天摄氏度";
            htDZB["temp4"] = "第四天摄氏度";
            htDZB["temp5"] = "第五天摄氏度";
            htDZB["temp6"] = "第六天摄氏度";

            htDZB["tempF1"] = "第一天华氏度";
            htDZB["tempF2"] = "第二天华氏度";
            htDZB["tempF3"] = "第三天华氏度";
            htDZB["tempF4"] = "第四天华氏度";
            htDZB["tempF5"] = "第五天华氏度";
            htDZB["tempF6"] = "第六天华氏度";

            htDZB["weather1"] = "第一天全天天气";
            htDZB["weather2"] = "第二天全天天气";
            htDZB["weather3"] = "第三天全天天气";
            htDZB["weather4"] = "第四天全天天气";
            htDZB["weather5"] = "第五天全天天气";
            htDZB["weather6"] = "第六天全天天气";

            htDZB["img1"] = "第一天白天天气图标";
            htDZB["img2"] = "第一天夜晚天气图标";
            htDZB["img3"] = "第二天白天天气图标";
            htDZB["img4"] = "第二天夜晚天气图标";
            htDZB["img5"] = "第三天白天天气图标";
            htDZB["img6"] = "第三天夜晚天气图标";
            htDZB["img7"] = "第四天白天天气图标";
            htDZB["img8"] = "第四天夜晚天气图标";
            htDZB["img9"] = "第五天白天天气图标";
            htDZB["img10"] = "第五天夜晚天气图标";
            htDZB["img11"] = "第六天白天天气图标";
            htDZB["img12"] = "第六天夜晚天气图标";
            htDZB["img_single"] = "第一天单独图标";

            htDZB["img_title1"] = "第一天白天天气标题";
            htDZB["img_title2"] = "第一天夜晚天气标题";
            htDZB["img_title3"] = "第二天白天天气标题";
            htDZB["img_title4"] = "第二天夜晚天气标题";
            htDZB["img_title5"] = "第三天白天天气标题";
            htDZB["img_title6"] = "第三天夜晚天气标题";
            htDZB["img_title7"] = "第四天白天天气标题";
            htDZB["img_title8"] = "第四天夜晚天气标题";
            htDZB["img_title9"] = "第五天白天天气标题";
            htDZB["img_title10"] = "第五天夜晚天气标题";
            htDZB["img_title11"] = "第六天白天天气标题";
            htDZB["img_title12"] = "第六天夜晚天气标题";
            htDZB["img_title_single"] = "第一天单独标题";

            htDZB["wind1"] = "第一天的风力风向";
            htDZB["wind2"] = "第二天的风力风向";
            htDZB["wind3"] = "第三天的风力风向";
            htDZB["wind4"] = "第四天的风力风向";
            htDZB["wind5"] = "第五天的风力风向";
            htDZB["wind6"] = "第六天的风力风向";

            htDZB["fx1"] = "第一天风向";
            htDZB["fx2"] = "第二天风向";

            htDZB["fl1"] = "第一天风力";
            htDZB["fl2"] = "第二天风力";
            htDZB["fl3"] = "第三天风力";
            htDZB["fl4"] = "第四天风力";
            htDZB["fl5"] = "第五天风力";
            htDZB["fl6"] = "第六天风力";

            htDZB["index"] = "第一天的穿衣指数";
            htDZB["index_d"] = "第一天穿衣指数描述";
            htDZB["index48"] = "48小时穿衣指数";
            htDZB["index48_d"] = "48小时穿衣指数描述";
            htDZB["index_uv"] = "第一天紫外线指数";
            htDZB["index48_uv"] = "48小时紫外线指数";
            htDZB["index_xc"] = "第一天洗车指数";
            htDZB["index_tr"] = "index_tr未知";
            htDZB["index_co"] = "第一天舒适度指数";

            htDZB["st1"] = "st1作用未知";
            htDZB["st2"] = "st2作用未知";
            htDZB["st3"] = "st3作用未知";
            htDZB["st4"] = "st4作用未知";
            htDZB["st5"] = "st5作用未知";
            htDZB["st6"] = "st6作用未知";

            htDZB["index_cl"] = "第一天晨练指数";
            htDZB["index_ls"] = "第一天晾晒指数";
            htDZB["index_ag"] = "第一天过敏气象指数";

            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                string thisname = ds.Tables[0].Columns[i].ColumnName;
                if (htDZB.ContainsKey(thisname))
                {
                    ds.Tables[0].Columns[i].ColumnName = htDZB[thisname].ToString();
                }
            }
            return ds;
        }
        catch (Exception ex)
        {
            string err = ex.ToString();
            return null;
        }

    }

    /// <summary>
    /// 通过json格式数据，返回数据集文档
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns></returns>
    public DataSet GetDataSetformJSON(string jsonstr)
    {
        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonstr);
        DataSet ds = new DataSet();
        XmlNodeReader reader = new XmlNodeReader(doc);
        ds.ReadXml(reader);
        return ds;
    }


    /// <summary>
    /// 通过json格式数据，返回xml文档
    /// </summary>
    /// <param name="jsonstr"></param>
    /// <returns></returns>
    public string GetJSONformXML(XmlDocument xmldoc)
    {
        string json = JsonConvert.SerializeXmlNode(xmldoc.DocumentElement);
        return json;
    }
}