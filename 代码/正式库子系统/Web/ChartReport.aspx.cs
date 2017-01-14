using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using FMOP.DB;
using FMOP.XHelp;
using FMOP.Tools;
using FMOP.Module;
using Hesion.Brick.Core;

public partial class ChartReport : System.Web.UI.Page
{

    #region 全局变量

    private string selectSQL = string.Empty;    //查询语句
    private string type = string.Empty;         //图表的类型
    private string subtype = string.Empty;      //子类型
    private string reportHeader = string.Empty; //报表title
    private string width = string.Empty;        //报表的宽度（默认为正常A4纸的宽度 800）
    private string height = string.Empty;       //报表的高度（默认为正常A4纸的高度 1131）
    private string chartWidth = string.Empty;   //图像宽度（默认为xxx）
    private string chartHeight = string.Empty;  //图像高度（默认为xxx）

    
    private string[] fieldName = new string[6];  //字段名称
    private string[] fieldNameFull = new string[6];//字段名称（全）
    private string[] title = new string[6];//字段描述
    private string[] fieldType = new string[6];//字段类型

    private string module = string.Empty;       //?参数
    private string connName = "FMOPConn";       //连接串名字

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["module"] != null)
            {
                module = Request.QueryString["module"].ToString();
                addrole();
                if (xmlParse() == 0)
                {
                    Response.Clear();
                    Response.Write("<script language=javascript>window.alert('没有此项报表！');history.back();</script>");
                    Response.End();
                    return;
                }
                else
                {
                    pageinit();
                    addTable();
                }
            }
            else
            {
                Response.Clear();
                Response.Write("<script language=javascript>window.alert('没有获取到模块参数！');history.back();</script>");
                Response.End();
                return;
            }
        }
        else
        {
            this.module = this.txtmodule.Text;
            xmlParse();
            int flag = int.Parse(Request["txtflag"]);
            switch (flag)
            {
                case 1://查询
                    addSQL();
                    break;
                case 2://即时刷新
                    selectSQL = txtSql.Text;
                    break;
            }
                       
        }
        if (!System.IO.File.Exists(Server.MapPath(".\\rdlc\\" + module + ".rdlc")))
        {
            addRDLC();
        } 
        dataSetBind();
    }

    /// <summary>
    /// 页面初始化
    /// </summary>
    private void pageinit()
    {
        this.txtflag.Text = "0";
        this.txtmodule.Text = module;//保存module值
        this.txtSql.Text = selectSQL;
        Telerik.WebControls.Tab ss = new Telerik.WebControls.Tab(TabTool.getTitle(module) + "图表");
        ss.NavigateUrl = "ChartReport.aspx?module=" + module;
        ss.ForeColor = System.Drawing.Color.Red;
        RadTabStrip1.Tabs.Add(ss);
    }

    /// <summary>
    /// xml文档解析，为全局变量赋值
    /// </summary>
    /// <param name="module">报表名称</param>
    /// <returns>0:没有此项报表;1:正确</returns>
    protected int xmlParse()
    {
        XmlDocument xmldoc = FMOP.XParse.xmlParse.GetXmlDoc(module);
        if (xmldoc != null)
        {
            XmlNode ReportModule = xmldoc.SelectSingleNode("//ChartModule");
            if (ReportModule == null)
            {
                return 0;
            }
            selectSQL = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "selectSQL", "");
            reportHeader = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "reportHeader", "");
            type = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "type", "Column");
            subtype = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "subtype", "Plain");
            width = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "width", "800");
            height = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "height", "1131");
            chartWidth = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "chartWidth", "800");
            chartHeight = FMOP.XHelp.XMLHelper.GetSingleString(ReportModule, "chartHeight", "600");


            XmlNode Seria_Field = xmldoc.SelectSingleNode("//ChartModule/Seria-Field");
            fieldNameFull[0] = FMOP.Tools.PageValidate.fieldDot(FMOP.XHelp.XMLHelper.GetSingleString(Seria_Field, "fieldName", ""));
            fieldName[0] = FMOP.XHelp.XMLHelper.GetSingleString(Seria_Field, "fieldName", "");
            title[0] = FMOP.XHelp.XMLHelper.GetSingleString(Seria_Field, "title", "");

            XmlNode Group_Field = xmldoc.SelectSingleNode("//ChartModule/Group-Field");
            fieldNameFull[1] = FMOP.XHelp.XMLHelper.GetSingleString(Group_Field, "fieldName", "");
            fieldName[1] = FMOP.Tools.PageValidate.fieldDot(FMOP.XHelp.XMLHelper.GetSingleString(Group_Field, "fieldName", ""));
            title[1] = FMOP.XHelp.XMLHelper.GetSingleString(Group_Field, "title", "");

            XmlNodeList xlist = xmldoc.SelectNodes("//ChartModule/Data-Fields/Data-Field");
            if (xlist.Count > 4)
            {
                FMOP.Common.MessageBox.Show(Page, "字段不能超过六个！");
            }
            int i = 2;
            foreach (XmlNode item in xlist)
            {
                fieldNameFull[i] = FMOP.XHelp.XMLHelper.GetSingleString(item, "fieldName", "");
                fieldName[i] = FMOP.Tools.PageValidate.fieldDot(FMOP.XHelp.XMLHelper.GetSingleString(item, "fieldName", ""));
                title[i] = FMOP.XHelp.XMLHelper.GetSingleString(item, "title", "");
                i++;
            }
            DataSet dss = new DataSet();
            try
            {
                dss = FMOP.DB.DbHelperSQL.Query(selectSQL);
            }
            catch (Exception es)
            {
                Hesion.Brick.Core.Log.Error(es.Message.ToString());
                FMOP.Common.MessageBox.Show(Page, "配置文件查询语句出错！"+es.Message.ToString());
                return 0;
            }
            try
            {
                fieldType = FMOP.Module.fieldTool.addfieldType(dss, fieldName);
            }
            catch (Exception e)
            {
                Hesion.Brick.Core.Log.Error(e.Message.ToString());
                FMOP.Common.MessageBox.Show(Page, "配置文件字段出错！"+e.Message.ToString());
				return 0;
            }
            return 1;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 生成rdlc文档
    /// </summary>
    /// <param name="module">报表名称</param>
    protected void addRDLC()
    {
        int i = 0;
        StringBuilder sb;
        StreamReader sr = File.OpenText(Server.MapPath(".\\rdlc\\template_chart.TXT"));
        string temp = sr.ReadToEnd();
        //替换数据库连接串和dataset名称
        temp = temp.Replace("{FMOPConn}", connName).Replace("{DataSetName}", module);
        #region 替换字段陈列
        sb = new StringBuilder();
        sb.Append("<Fields>");
        ///每一个字段名称+类型
        DataSet ds = FMOP.DB.DbHelperSQL.Query(selectSQL);
        for (i = 0; i < fieldName.Length; i++)
        {
            if (fieldName[i] == null || fieldName[i].Equals(string.Empty))
            {
                break;
            }
            else
            {
                sb.Append("<Field Name=\"" + fieldName[i] + "\"><rd:TypeName>" + ds.Tables[0].Columns[fieldName[i]].DataType.ToString() + "</rd:TypeName><DataField>" + fieldName[i] + "</DataField></Field>");
            }
        }
        sb.Append("</Fields>");

        temp = temp.Replace("{fields}", sb.ToString());
        #endregion
        //替换主体宽度和高度
        temp = temp.Replace("{Width}", Convert.ToString(int.Parse(width) / 38.1)).Replace("{Height}", Convert.ToString(int.Parse(height) / 38.1));
        //替换图表宽度和高度
        temp = temp.Replace("{chartWidth}", Convert.ToString(int.Parse(chartWidth) / 38.1)).Replace("{chartHeight}", Convert.ToString(int.Parse(chartHeight) / 38.1));
        //替换图表名称
        temp = temp.Replace("{ReportName}", reportHeader);
        //替换x轴y轴标题
        temp = temp.Replace("{y_title}", title[2]).Replace("{x_title}", title[1]);
        //替换图表类型
        temp = temp.Replace("{type}", type).Replace("{subtype}", subtype);
        //替换序列字段和分组字段
        temp = temp.Replace("{Serial-Field}", fieldName[0]).Replace("{Group-Field}", fieldName[1]);
        #region 替换datavalue
        sb = new StringBuilder();
        sb.Append("<DataValues>");

        for (i = 2; i < 6; i++)
        {
            if (fieldName[i] != null && fieldName[i] != string.Empty)
            {
                sb.Append("<DataValue><Value>=Sum(Fields!" + fieldName[i] + ".Value)</Value></DataValue>");
            }
            else
            {
                break;
            }
        }

        sb.Append("</DataValues>");
        temp = temp.Replace("{DataValues}", sb.ToString());
        #endregion


        #region 直写
        /*
        int ReportItemsName = 0;//报表项命名数字，防止name重复
        int i = 0;  //循环使用

        StringBuilder sb = new StringBuilder();
        //xml XmlDeclaration 
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        //report header
        sb.Append("<Report xmlns=\"http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition\" xmlns:rd=\"http://schemas.microsoft.com/SQLServer/reporting/reportdesigner\">");
        // DataSources
        sb.Append("<DataSources><DataSource Name=\"" + connName + "\"><rd:DataSourceID>958ce307-f849-413f-8987-2fab63fd42d8</rd:DataSourceID><DataSourceReference>" + connName + "</DataSourceReference></DataSource></DataSources>");
        // rd标签元素
        sb.Append("<rd:DrawGrid>true</rd:DrawGrid><rd:ReportID>d8e9e567-c659-4c32-b64f-9984f3c3e0ef</rd:ReportID><rd:GridSpacing>0.625in</rd:GridSpacing><rd:SnapToGrid>true</rd:SnapToGrid>");
        // 报表属性
        sb.Append("<TopMargin>0.25cm</TopMargin><LeftMargin>0.25cm</LeftMargin><BottomMargin>0.25cm</BottomMargin><RightMargin>0.25cm</RightMargin><InteractiveWidth>21cm</InteractiveWidth><InteractiveHeight>29.7cm</InteractiveHeight><PageWidth>21cm</PageWidth><PageHeight>29.7cm</PageHeight><Language>zh-CN</Language>");
        //width
        sb.Append("<Width>" + Convert.ToString(int.Parse(width) / 38.1) + "cm</Width>");
        // datasets 
        sb.Append("<DataSets>");
        ///第一个dataset
        sb.Append("<DataSet Name=\"" + module + "\">");
        ///字段陈列
        sb.Append("<Fields>");
        ///每一个字段名称+类型
        DataSet ds = FMOP.DB.DbHelperSQL.Query(selectSQL);
        for (i = 0; i < fieldName.Length; i++)
        {
            if (fieldName[i] == null || fieldName[i].Equals(string.Empty))
            {
                break;
            }
            else
            {
                sb.Append("<Field Name=\"" + fieldName[i] + "\"><rd:TypeName>" + ds.Tables[0].Columns[fieldName[i]].DataType.ToString() + "</rd:TypeName><DataField>" + fieldName[i] + "</DataField></Field>");
            }              
        }
        sb.Append("</Fields>");
        /// Query
        sb.Append("<Query><DataSourceName>" + connName + "</DataSourceName><CommandText></CommandText><Timeout>30</Timeout><rd:UseGenericDesigner>true</rd:UseGenericDesigner></Query>");
        // </DataSet>
        sb.Append("</DataSet>");
        sb.Append("</DataSets>");


        //body
        sb.Append("<Body><ColumnSpacing>0cm</ColumnSpacing>");
        //height
        sb.Append("<Height>" + Convert.ToString(int.Parse(height) / 38.1) + "cm</Height>");
        /// ReportItems
        sb.Append("<ReportItems>");

        #region chart
        sb.Append("<Chart Name=\"chart" + ReportItemsName.ToString() + "\">");
        sb.Append("<Type>" + type + "</Type><Subtype>" + subtype + "</Subtype><Top>0.5cm</Top><Left>0cm</Left>");
        //chartwidth  chartheight
        sb.Append("<Width>" + Convert.ToString(int.Parse(chartWidth) / 38.1) + "cm</Width><Height>" + Convert.ToString(int.Parse(chartHeight) / 38.1) + "cm</Height>");
        //datasetname = module
        sb.Append("<PointWidth>0</PointWidth><DataSetName>" + module + "</DataSetName>");

        //reportHeader 
        sb.Append("<Title><Caption>" + reportHeader + "</Caption><Style><FontFamily>宋体</FontFamily><FontWeight>800</FontWeight><FontSize>16pt</FontSize><Color>SteelBlue</Color></Style></Title>");

        //san D
        sb.Append("<ThreeDProperties><Rotation>30</Rotation><Inclination>30</Inclination><Shading>Simple</Shading><WallThickness>50</WallThickness></ThreeDProperties>");

        //图表区
        sb.Append("<Style><BorderStyle><Default>Solid</Default></BorderStyle><FontFamily>宋体</FontFamily><BackgroundColor>WhiteSmoke</BackgroundColor></Style>");

        //画图区
        sb.Append("<PlotArea><Style><BorderStyle><Default>Solid</Default></BorderStyle><BackgroundColor>LightGrey</BackgroundColor></Style></PlotArea>");

        //调色板
        sb.Append("<Palette>Excel</Palette>");

        sb.Append("<Legend><Visible>true</Visible><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style><Position>RightCenter</Position></Legend>");

        //yzhou
        sb.Append("<ValueAxis><Axis><Title><Caption>" + title[2] + "</Caption><Style><FontFamily>宋体</FontFamily><FontWeight>600</FontWeight></Style></Title>");
        sb.Append("<MajorGridLines><ShowGridLines>true</ShowGridLines><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></MajorGridLines>");
        sb.Append("<MinorGridLines><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></MinorGridLines>");
        sb.Append("<MajorTickMarks>Outside</MajorTickMarks><Min>0</Min><Margin>true</Margin><Visible>true</Visible>");
        sb.Append("<Scalar>true</Scalar></Axis></ValueAxis>");

        //xzhou
        sb.Append("<CategoryAxis><Axis><Title><Caption>" + title[1] + "</Caption><Style><FontFamily>宋体</FontFamily><FontWeight>600</FontWeight></Style></Title>");
        sb.Append("<MajorGridLines><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></MajorGridLines>");
        sb.Append("<MinorGridLines><Style><BorderStyle><Default>Solid</Default></BorderStyle></Style></MinorGridLines>");
        sb.Append("<MajorTickMarks>Outside</MajorTickMarks><Min>0</Min><Visible>true</Visible>");
        sb.Append("</Axis></CategoryAxis>");


        //xu lie zi duan 
        sb.Append("<SeriesGroupings><SeriesGrouping><DynamicSeries><Grouping Name=\"chart1_SeriesGroup1\"><GroupExpressions>");
        sb.Append("<GroupExpression>=Fields!" + fieldName[0] + ".Value</GroupExpression>");
        sb.Append("</GroupExpressions></Grouping><Label>=Fields!Number.Value</Label></DynamicSeries></SeriesGrouping></SeriesGroupings>");
        
        
        //lei bie zi duan 
        sb.Append("<CategoryGroupings><CategoryGrouping><DynamicCategories><Grouping Name=\"chart1_CategoryGroup1\"><GroupExpressions>");
        sb.Append("<GroupExpression>=Fields!" + fieldName[1] + ".Value</GroupExpression>");
        sb.Append("</GroupExpressions></Grouping><Label>=Fields!" + fieldName[1] + ".Value</Label></DynamicCategories></CategoryGrouping></CategoryGroupings>");

        //shu ju 
        sb.Append("<ChartData><ChartSeries><DataPoints><DataPoint><DataValues>");

        for(i = 2;i < 6;i++)
        {
            if (fieldName[i] != null && fieldName[i] != string.Empty)
            {
                sb.Append("<DataValue><Value>=Sum(Fields!" + fieldName[i] + ".Value)</Value></DataValue>");
            }
            else
            {
                break;
            }
        }
        
        sb.Append("</DataValues><DataLabel /><Marker /></DataPoint></DataPoints></ChartSeries></ChartData>");

        sb.Append("</Chart>");
        #endregion

        //</body>
        sb.Append("</ReportItems></Body>");
        //</Report>
        sb.Append("</Report>");
        
        string xmlstr = sb.ToString();
        */
        #endregion

        
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.RemoveAll();
        //xmldoc.LoadXml(xmlstr);
        xmldoc.LoadXml(temp);

        try
        {
            xmldoc.Save(Server.MapPath(".\\rdlc\\" + module + ".rdlc"));
        }
        catch (Exception e)
        {
            Hesion.Brick.Core.Log.Error(e.Message.ToString());
            Response.Write(e.Message);
        }
        
    }

    /// <summary>
    /// 生成报表
    /// </summary>
    /// <param name="module">报表名称</param>
    protected void dataSetBind()
    {

        DataSet ds = new DataSet();
        try
        {
            ds = FMOP.DB.DbHelperSQL.Query(selectSQL);
            //2009.06.05 王永辉 修改
            string dailiren = "无";
            if (Session["daililogin"] != null)
            {
                dailiren = Session["daililogin"].ToString();
            }
            Hesion.Brick.Core.FMEvengLog.SaveToLog(User.Identity.Name, module, Request.UserHostAddress, "chartreport.aspx", "", selectSQL, dailiren);
        }
        catch (Exception e)
        {
            Hesion.Brick.Core.Log.Error(e.Message.ToString());
            FMOP.Common.MessageBox.Show(Page, "组合查询语句出错！"+e.Message.ToString());

        }
        this.ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
        this.ReportViewer1.LocalReport.EnableHyperlinks = true;
        this.ReportViewer1.LocalReport.ReportPath = Server.MapPath(".\\rdlc\\" + module + ".rdlc");
        this.ReportViewer1.LocalReport.DataSources.Clear();
        Microsoft.Reporting.WebForms.ReportDataSource rds = new Microsoft.Reporting.WebForms.ReportDataSource();
        rds.Name = module;
        rds.Value = ds.Tables[0];
        this.ReportViewer1.LocalReport.DataSources.Add(rds);
        this.ReportViewer1.BackColor = System.Drawing.Color.White;
        //this.ReportViewer1.ForeColor = System.Drawing.Color.Green;
        this.ReportViewer1.ShowPrintButton = true;
        //this.ReportViewer1.Drillthrough +=new Microsoft.Reporting.WebForms.DrillthroughEventHandler(ReportViewer1_Drillthrough);
        //this.ReportViewer1.LocalReport.SubreportProcessing+=new Microsoft.Reporting.WebForms.SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        this.ReportViewer1.LocalReport.Refresh();
    }

    /// <summary>
    /// 填充查询div
    /// </summary>
    protected void addTable()
    {
        this.sele.InnerHtml = FMOP.Module.htmlWrite.addSelectTable(fieldType, fieldName, title);
    }

	/// <summary>
	/// 生成where语句，addSQL()
	/// </summary>
	protected ArrayList addwhere()
	{
		ArrayList al = new ArrayList();
		StringBuilder sb = new StringBuilder();

		int i = 0;
		#region 循环生成where语句
		for (i = 0; i < fieldName.Length; i++)
		{
			if (Request["txt" + fieldName[i]] != null && Request["txt" + fieldName[i]].ToString() != string.Empty)
			{
				sb = new StringBuilder();
				string str = Request["txt" + fieldName[i]].ToString();
				str = str.Replace("'", "\"");
				#region 判断用户输入是否合法
				if (!FMOP.Tools.PageValidate.selectValidate(fieldType[i], str))
				{
					return null;
				}
				#endregion

				sb.Append(fieldNameFull[i]);
				string mark = Request["sel" + fieldName[i]].ToString();
				sb.Append(FMOP.Tools.PageValidate.selectValue(mark, str, fieldType[i]));
				al.Add(sb.ToString());
			}
		}
		#endregion
		return al;
	}

	/// <summary>
	/// 生成sql语句，发送给dataSetBind
	/// </summary>
	protected void addSQL()
	{
		//string wherestr = addwhere();//where条件
		ArrayList al = addwhere();//where条件
		if (al == null)
		{
			FMOP.Common.MessageBox.Show(this, "您输入有误！");
			return;
		}


		#region where集成sql语句
		if (!selectSQL.Equals(string.Empty))
		{
			selectSQL = SQLBuilder.AddWhereToSQL(selectSQL, al);
			this.txtSql.Text = selectSQL;
		}
		else
		{
			return;
		}
		#endregion
	}

    /// <summary>
    /// 获取该页面的访问权限
    /// </summary>
    protected void addrole()
    {
        string id = User.Identity.Name;
        //判断配置信息中是否有此岗位
        Hesion.Brick.Core.WorkFlow.WorkFlowModule wf=new Hesion.Brick.Core.WorkFlow.WorkFlowModule (module);
        //FMOP.RR.RoportRole.IsRoportRole(module, id)
        if (wf.check==null&&!wf.authentication.IsRoportRole(id))
        {
            Response.Clear();
            Response.Write("<script language=javascript>window.alert('您没有此模块的权限！');history.back();</script>");
            Response.End();
        }
        else
        {
        }
    }
}
