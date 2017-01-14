using FMDBHelperClass;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services.Description;
using System.Web.Services.Discovery;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

public partial class IPCadmin_addJK : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Button)wuccaidan1.FindControl("Button4")).ForeColor = System.Drawing.Color.Red;
            beginshowJK();
            beginshowjkpath();
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //如果当前行尾数据行
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //添加鼠标在当前行时的background-color属性
            e.Row.Attributes.Add("onmouseover", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#aadef6';}");
            //鼠标离开当前行后
            e.Row.Attributes.Add("onmouseout", "if(this.style.backgroundColor!='#fbed90'){this.style.backgroundColor='#ffffff';}");
            e.Row.Attributes.Add("ondblclick", "if(this.style.backgroundColor=='#aadef6' || this.style.backgroundColor=='#ffffff'){this.style.backgroundColor='#fbed90';}else{this.style.backgroundColor='#ffffff';}");
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView1.DataSource = null;
        GridView1.DataBind();
        lblJKguid.Text = "";
        lblerrmsg.Text = "";
        lblerrmsg.Visible = false;
        TextBox3.Text = "";
        TextBox4.Text = "";
        TextBox5.Text = "";
        if (TextBox1.Text.Trim() == "" || TextBox2.Text.Trim() == "" || TextBox1.Text.Trim() == "请选择" || TextBox2.Text.Trim() == "请选择")
        {
            //Response.Write("接口地址及接口域名不能为空！");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('接口地址及接口域名不能为空！');</script>");
            return;
        }
        try
        {           
            //检查接口情况
            string mubiao_url = "http://" + TextBox1.Text + "/" + TextBox2.Text + "?wsdl";            
            XmlTextReader reader = new XmlTextReader(mubiao_url);
            ServiceDescription service = ServiceDescription.Read(reader);
            string JKzhushi = service.Documentation; //接口注释
            string JKleiming = service.Services[0].Name;  //接口类名。咱要求和文件名一致，所以这里用不上


            //准备获得方法的具体参数返回值等等详细资料 
            ServiceDescriptionImporter importer = new ServiceDescriptionImporter();//创建客户端代理代理类
            importer.ProtocolName = "Soap";
            importer.Style = ServiceDescriptionImportStyle.Client;	//生成客户端代理						
            importer.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
            importer.AddServiceDescription(service, null, null);//添加WSDL文档
            //使用CodeDom编译客户端代理类					
            CodeNamespace nmspace = new CodeNamespace("临时名空间");	//为代理类添加命名空间				
            CodeCompileUnit unit = new CodeCompileUnit();
            unit.Namespaces.Add(nmspace);


            DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
            dcp.DiscoverAny(mubiao_url);
            dcp.ResolveAll();
            foreach (object osd in dcp.Documents.Values)
            {
                if (osd is ServiceDescription) importer.AddServiceDescription((ServiceDescription)osd, null, null); ;
                if (osd is XmlSchema) importer.Schemas.Add((XmlSchema)osd);
            }


            ServiceDescriptionImportWarnings warning = importer.Import(nmspace, unit);
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CompilerParameters parameter = new CompilerParameters();
            parameter.ReferencedAssemblies.Add("System.dll");
            parameter.ReferencedAssemblies.Add("System.XML.dll");
            parameter.ReferencedAssemblies.Add("System.Web.Services.dll");
            parameter.ReferencedAssemblies.Add("System.Data.dll");
            parameter.GenerateExecutable = false;
            parameter.GenerateInMemory = false;
            parameter.IncludeDebugInformation = false;
            CompilerResults result = provider.CompileAssemblyFromDom(parameter, unit);
            provider.Dispose();

            Assembly serviceAsm = result.CompiledAssembly;
            Type[] types = serviceAsm.GetTypes();
            string objTypeName = "";
            foreach (Type t in types)
            {
                if (t.BaseType == typeof(SoapHttpClientProtocol))
                {
                    objTypeName = t.Name;
                    break;
                }
            }


            //判断接口是否在数据库中存在
            Hashtable inputHT = new Hashtable();
            DataSet dsre = new DataSet();//用于接口查询返回数据集      
            DataSet dsreM = new DataSet();//用于接口中的方法查询返回数据集       
            inputHT["@jkym"] = TextBox1.Text.Trim();           
            inputHT["@jkdz"] = TextBox2.Text.Trim();
            string sql = "select * from AAA_ipcJK where JK_host=@jkym and JK_path=@jkdz";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口信息", inputHT);
            if ((bool)(return_ht["return_float"])) //执行完成
            {              
                dsre = (DataSet)return_ht["return_ds"];
                if (dsre != null && dsre.Tables[0].Rows.Count > 0)
                {//存在对应数据               
                    lblJKguid.Text = dsre.Tables[0].Rows[0]["JK_guid"].ToString();
                    TextBox3.Text = dsre.Tables[0].Rows[0]["JK_shuoming"].ToString();
                    TextBox4.Text = dsre.Tables[0].Rows[0]["JK_banben"].ToString();
                    ListBox1.SelectedValue = dsre.Tables[0].Rows[0]["JK_open"].ToString();
                    TextBox5.Text = dsre.Tables[0].Rows[0]["JK_port"].ToString();

                    //接口和方法同时添加的按钮不可用
                    Button2.Enabled = false;
                    Button2.ToolTip = "该接口已经添加，仅可添加新方法";
                    btnAddM.Enabled = true;
                    btnAddM.ToolTip = "";
                }
                else
                {//接口还没有添加过
                    Button2.Enabled = true;
                    Button2.ToolTip = "";
                    btnAddM.Enabled = false;
                    btnAddM.ToolTip = "";                  

                    //版本号,符合注释规则才行。
                    if (JKzhushi.IndexOf("-&gt;") >= 0)
                    {
                        TextBox3.Text = JKzhushi.Replace("-&gt;", "*").Split('*')[1];
                        TextBox4.Text = JKzhushi.Replace("-&gt;", "*").Split('*')[0];
                    }
                }
            }
            else
            {
                btnAddM.Enabled = false;
                btnAddM.ToolTip = "";
                Button2.Enabled = false;
                Button2.ToolTip = "";
                //读取数据库出错。。。
                Response.Write("接口查询出错。" + return_ht["return_errmsg"].ToString());
                return;
            }

            //如果接口已经添加过，获取此接口中已经添加过的方法
            if (lblJKguid.Text != "")
            {
                Hashtable inputM = new Hashtable();
                inputM["@jkguid"] = dsre.Tables[0].Rows[0]["JK_guid"].ToString();
                string sqlM = "select * from AAA_ipcFF where FF_JK_guid=@jkguid";
                Hashtable return_htM = I_DBL.RunParam_SQL(sqlM, "方法列表", inputM);

                if ((bool)(return_htM["return_float"]))
                {
                    dsreM = (DataSet)return_htM["return_ds"];
                }
                else
                {
                    //读取数据库出错。。。
                    Response.Write("接口中的方法查询出错。" + return_ht["return_errmsg"].ToString());
                    return;
                }
            }
            else
            {
                dsreM = null;
            }

            DataTable DTfangfa = new DataTable();
            DTfangfa.Columns.Add("业务名称");
            DTfangfa.Columns.Add("方法名");
            DTfangfa.Columns.Add("返回值类型");
            DTfangfa.Columns.Add("参数类型");
            DTfangfa.Columns.Add("方法注释");
            DTfangfa.Columns.Add("方法是否有效");
            DTfangfa.Columns.Add("操作特点");

            //获取接口中的所有方法
            foreach (Operation ort in service.PortTypes[0].Operations)
            {
                string FF_name = ort.Name;  //方法名
                string FF_shuoming = ort.Documentation; //方法注释
                string FF_yewumingcheng = ort.Messages.Input.Name;
                
                MethodInfo mi = serviceAsm.GetType("临时名空间." + objTypeName).GetMethod(FF_name);
                string FF_rename = mi.ReturnParameter.ParameterType.Name;
                ParameterInfo[] pi = mi.GetParameters();
                string FF_cs = "";
                for (int i = 0; i < pi.Length; i++)
                {
                    FF_cs = FF_cs + mi.GetParameters()[i].ParameterType.Name + "{" + mi.GetParameters()[i].Name + "}" + "★";
                }


                if (dsreM != null)
                {
                    DataRow[] dr = dsreM.Tables[0].Select("FF_name='" + FF_name + "'");
                    if (dr.Length <= 0)
                    {
                        DTfangfa.Rows.Add(new string[] { FF_yewumingcheng, FF_name, FF_rename, FF_cs, FF_shuoming, "", "" });
                    }
                }
                else
                {
                    DTfangfa.Rows.Add(new string[] { FF_yewumingcheng, FF_name, FF_rename, FF_cs, FF_shuoming, "", "" });
                }

            }
            if (DTfangfa.Rows.Count <= 0)
            {
                lblFF.Text = "此接口中未找到没有添加过的方法";
                lblFF.Visible = true;
            }
            else
            {
                GridView1.DataSource = DTfangfa.DefaultView;
                GridView1.DataBind();
                lblFF.Text = "";
                lblFF.Visible = false;
            }
        }
        catch (Exception ex)
        {
            //this.ClientScript.RegisterStartupScript(this.GetType(), "message", "<script language='javascript' defer>alert('"+ex.ToString ()+"');</script>");
            lblerrmsg.Text = ex.ToString();
            lblerrmsg.Visible = true;
            //Response.Write(ex.ToString());
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Hashtable input = new Hashtable();
        ArrayList alsql = new ArrayList();

        string JK_guid = Guid.NewGuid().ToString();
        string JK_host = TextBox1.Text;       
        string JK_path = TextBox2.Text;
        string JK_shuoming = TextBox3.Text;
        string JK_banben = TextBox4.Text;
        string JK_open = ListBox1.SelectedValue;
        string JK_port = TextBox5.Text;       

        if (JK_host.Trim() == "" || JK_path.Trim() == "" || JK_shuoming.Trim() == "" || JK_banben.Trim() == "" || JK_open.Trim() == "" || JK_port.Trim() == "")
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，接口资料不全！');</script>");
            return;
        }

        input["@JK_guid"] = JK_guid;
        input["@JK_host"] = JK_host;
        input["@JK_path"] = JK_path;
        input["@JK_shuoming"] = JK_shuoming;
        input["@JK_banben"] = JK_banben;
        input["@JK_open"] = JK_open;
        input["@JK_port"] = JK_port;

        alsql.Add(" INSERT INTO  AAA_ipcJK(JK_guid,JK_host,JK_path,JK_shuoming,JK_banben,JK_open,JK_port) VALUES (@JK_guid,@JK_host,@JK_path,@JK_shuoming,@JK_banben,@JK_open,@JK_port) ");

        string FF_addtime = DateTime.Now.ToString();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            CheckBox ck = (CheckBox)GridView1.Rows[i].FindControl("FF_chkItem");
            if (ck.Checked)
            {
                string FF_guid = Guid.NewGuid().ToString();
                string FF_JK_guid = JK_guid;
                string FF_yewuname = ((TextBox)GridView1.Rows[i].FindControl("FF_yewuname")).Text;
                string FF_name = ((TextBox)GridView1.Rows[i].FindControl("FF_name")).Text;
                string FF_retype = ((TextBox)GridView1.Rows[i].FindControl("FF_retype")).Text;
                string FF_canshu = ((TextBox)GridView1.Rows[i].FindControl("FF_canshu")).Text;
                string FF_shuoming = ((TextBox)GridView1.Rows[i].FindControl("FF_shuoming")).Text;
                string FF_open = ((ListBox)GridView1.Rows[i].FindControl("FF_open")).SelectedValue;
                string FF_CorE = ((ListBox)GridView1.Rows[i].FindControl("FF_CorE")).SelectedValue;
                string FF_Log = "";
                CheckBoxList cbl = (CheckBoxList)GridView1.Rows[i].FindControl("FF_Log");
                foreach (ListItem item in cbl.Items)
                {
                    if (item.Selected == true)
                        FF_Log += item.Text + "|";
                }
                if (FF_Log == "")
                {
                    FF_Log = "关闭";
                }

                if (FF_yewuname.Trim() == "" || FF_name.Trim() == "" || FF_retype.Trim() == "" || FF_canshu.Trim() == "" || FF_shuoming.Trim() == "" || FF_open.Trim() == "" || FF_CorE.Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，方法资料不全！');</script>");
                    return;
                }

                input["@FF_guid" + i] = FF_guid;
                input["@FF_JK_guid"] = FF_JK_guid;
                input["@FF_yewuname" + i] = FF_yewuname;
                input["@FF_name" + i] = FF_name;
                input["@FF_retype" + i] = FF_retype;
                input["@FF_canshu" + i] = FF_canshu;
                input["@FF_shuoming" + i] = FF_shuoming;
                input["@FF_open" + i] = FF_open;
                input["@FF_CorE" + i] = FF_CorE;
                input["@FF_log" + i] = FF_Log;
                input["@FF_addtime" + i] = FF_addtime;
                input["@FF_edittime" + i] = FF_addtime;

                alsql.Add(" INSERT INTO  AAA_ipcFF(FF_guid,FF_JK_guid,FF_yewuname,FF_name, FF_retype,FF_canshu,FF_shuoming,FF_open,FF_CorE,FF_log,FF_addtime,FF_edittime) VALUES (@FF_guid" + i + ",@FF_JK_guid,@FF_yewuname" + i + ",@FF_name" + i + ", @FF_retype" + i + ",@FF_canshu" + i + ",@FF_shuoming" + i + ",@FF_open" + i + ",@FF_CorE" + i + ",@FF_log" + i + ",@FF_addtime" + i + ",@FF_edittime" + i + ") ");
            }
        }

        //提交

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        Hashtable return_ht = I_DBL.RunParam_SQL(alsql, input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加好了！');window.location.href='addJK.aspx';</script>");
        }
        else
        {
            //读取数据库出错。。。
            lblerrmsg.Text = return_ht["return_errmsg"].ToString();
            lblerrmsg.Visible = true;
            //Response.Write(return_ht["return_errmsg"].ToString());
        }
    }

    protected void FF_chkAll_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < this.GridView1.Rows.Count; i++)
        {
            ((CheckBox)GridView1.Rows[i].FindControl("FF_chkItem")).Checked =
                ((CheckBox)this.GridView1.HeaderRow.FindControl("FF_chkAll")).Checked;
        }
    }

    //添加选中的方法
    protected void btnAddM_Click(object sender, EventArgs e)
    {
        Hashtable input = new Hashtable();
        ArrayList alsql = new ArrayList();
        string FF_addtime = DateTime.Now.ToString();
        for (int i = 0; i < GridView1.Rows.Count; i++)
        {
            CheckBox ck = (CheckBox)GridView1.Rows[i].FindControl("FF_chkItem");
            if (ck.Checked)
            {
                string FF_guid = Guid.NewGuid().ToString();
                string FF_JK_guid = lblJKguid.Text.Trim();
                string FF_yewuname = ((TextBox)GridView1.Rows[i].FindControl("FF_yewuname")).Text;
                string FF_name = ((TextBox)GridView1.Rows[i].FindControl("FF_name")).Text;
                string FF_retype = ((TextBox)GridView1.Rows[i].FindControl("FF_retype")).Text;
                string FF_canshu = ((TextBox)GridView1.Rows[i].FindControl("FF_canshu")).Text;
                string FF_shuoming = ((TextBox)GridView1.Rows[i].FindControl("FF_shuoming")).Text;
                string FF_open = ((ListBox)GridView1.Rows[i].FindControl("FF_open")).SelectedValue;
                string FF_CorE = ((ListBox)GridView1.Rows[i].FindControl("FF_CorE")).SelectedValue;
                string FF_Log = "";
                CheckBoxList cbl = (CheckBoxList)GridView1.Rows[i].FindControl("FF_Log");
                foreach (ListItem item in cbl.Items)
                {
                    if (item.Selected == true)
                        FF_Log += item.Text + "|";
                }
                if (FF_Log == "")
                {
                    FF_Log = "关闭";
                }

                if (FF_yewuname.Trim() == "" || FF_name.Trim() == "" || FF_retype.Trim() == "" || FF_canshu.Trim() == "" || FF_shuoming.Trim() == "" || FF_open.Trim() == "" || FF_CorE.Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加失败，方法资料不全！');</script>");
                    return;
                }

                input["@FF_guid" + i] = FF_guid;
                input["@FF_JK_guid"] = FF_JK_guid;
                input["@FF_yewuname" + i] = FF_yewuname;
                input["@FF_name" + i] = FF_name;
                input["@FF_retype" + i] = FF_retype;
                input["@FF_canshu" + i] = FF_canshu;
                input["@FF_shuoming" + i] = FF_shuoming;
                input["@FF_open" + i] = FF_open;
                input["@FF_CorE" + i] = FF_CorE;
                input["@FF_log" + i] = FF_Log;
                input["@FF_addtime" + i] = FF_addtime;
                input["@FF_edittime" + i] = FF_addtime;
                alsql.Add(" INSERT INTO  AAA_ipcFF(FF_guid,FF_JK_guid,FF_yewuname,FF_name, FF_retype,FF_canshu,FF_shuoming,FF_open,FF_CorE,FF_log,FF_addtime,FF_edittime) VALUES (@FF_guid" + i + ",@FF_JK_guid,@FF_yewuname" + i + ",@FF_name" + i + ", @FF_retype" + i + ",@FF_canshu" + i + ",@FF_shuoming" + i + ",@FF_open" + i + ",@FF_CorE" + i + ",@FF_log" + i + ",@FF_addtime" + i + ",@FF_edittime" + i + ")");
            }
        }
        if (alsql.Count > 0)
        {
            //提交
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            Hashtable return_ht = I_DBL.RunParam_SQL(alsql, input);

            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('添加好了！');window.location.href='addJK.aspx';</script>");
            }
            else
            {
                //读取数据库出错。。。
                //Response.Write(return_ht["return_errmsg"].ToString());
                lblerrmsg.Text = return_ht["return_errmsg"].ToString();
                lblerrmsg.Visible = true;
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script>alert('没有要执行的sql语句！');</script>");
        }
    }


    //读取数据库，显示接口数据。
    private void beginshowJK()
    {
        string sql = " select distinct JK_host from AAA_ipcJK where JK_open = 1 order by JK_host asc ";

        I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
        DataSet dsJKlist = new DataSet();
        Hashtable input = new Hashtable();
        Hashtable return_ht = I_DBL.RunParam_SQL(sql, "接口列表", input);

        if ((bool)(return_ht["return_float"])) //说明执行完成
        {
            dsJKlist = (DataSet)(return_ht["return_ds"]);
            JK_host_selt.DataSource = dsJKlist.Tables[0].DefaultView;
            JK_host_selt.DataTextField = "JK_host";
            JK_host_selt.DataValueField = "JK_host";
            JK_host_selt.DataBind();
            JK_host_selt.Items.Insert(0, new ListItem("请选择", "请选择"));            
            TextBox1.Text = "请选择";
        }
        else
        {
            //读取数据库出错。。。
            TextBox1.Text = "";
            JK_host_selt.DataSource = null;
            JK_host_selt.DataBind();
            //Response.Write(return_ht["return_errmsg"].ToString());
            lblerrmsg.Text = return_ht["return_errmsg"].ToString();
            lblerrmsg.Visible = true;
        }
    }  

    //读取数据库
    private void beginshowjkpath()
    {
        Hashtable input = new Hashtable();
        input["@JK_host"] = JK_host_selt.SelectedValue.ToString(); 

        if (input["@JK_host"].ToString() == "请选择" || input["@JK_host"].ToString() == "")
        {
            JK_path_selt.Items.Clear();
            JK_path_selt.DataSource = null;
            JK_path_selt.DataBind();
            TextBox2.Text = "请选择";
        }
        else
        {
            string sql_host = " select JK_path from AAA_ipcJK where JK_open = 1 and JK_host = @JK_host order by JK_path asc ";
            I_Dblink I_DBL = (new DBFactory()).DbLinkSqlMain("");
            DataSet dsFFlist = new DataSet();
            Hashtable return_ht = I_DBL.RunParam_SQL(sql_host, "接口host", input);
            if ((bool)(return_ht["return_float"])) //说明执行完成
            {
                dsFFlist = (DataSet)(return_ht["return_ds"]);
                JK_path_selt.DataSource = dsFFlist.Tables[0].DefaultView;
                JK_path_selt.DataTextField = "JK_path";
                JK_path_selt.DataValueField = "JK_path";
                JK_path_selt.DataBind();
                JK_path_selt.Items.Insert(0, new ListItem("请选择", "请选择"));
                TextBox2.Text = "请选择";
            }
            else
            {
                //读取数据库出错。。。
                TextBox2.Text = "";
                JK_path_selt.DataSource = null;
                JK_path_selt.DataBind();
                //Response.Write(return_ht["return_errmsg"].ToString());
                lblerrmsg.Text = return_ht["return_errmsg"].ToString();
                lblerrmsg.Visible = true;
            }
        }

    }
    protected void JK_host_selt_SelectedIndexChanged(object sender, EventArgs e)
    {
        beginshowjkpath();
    }
}