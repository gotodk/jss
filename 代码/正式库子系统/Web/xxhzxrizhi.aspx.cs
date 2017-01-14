using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FMOP.DB;
using Galaxy.ClassLib.DataBaseFactory;
using Hesion.Brick.Core.WorkFlow;
using Infragistics.WebUI.UltraWebGrid;
using Telerik.WebControls;

public partial class Web_xxhzxrizhi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //强制中文格式显示
        System.Globalization.CultureInfo ci = null;
        ci = System.Globalization.CultureInfo.CreateSpecificCulture("zh-CN");
        System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        if (!IsPostBack)
        {
            //判断是否有权限
            DefinedModule Dfmodule = new DefinedModule("CSTGZYYDKHZB");
            Authentication auth = Dfmodule.authentication;
            if (auth == null)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            if (!auth.GetAuthByUserNumber(User.Identity.Name).CanView)
            {
                //MessageBox.ShowAlertAndBack(this, "您无权访问该模块");
            }

            //开始一个分页绑定========
            //设置默认参数(调用时候不用修改)
            SetDefaultSQL();
            //更改特殊参数(根据需要修改默认值)
            //ViewState["search_str_where"] = " ";
            //开始绑定(调用时候不用修改)
            Begin_My_Binding_Pager();
            //==========================
        }
    }








    #region  设置默认参数以及特殊处理的函数。修更改默认值后使用。

    /// <summary>
    /// 设置默认分页参数
    /// </summary>
    private void SetDefaultSQL()
    {
        ViewState["page_index"] = 0;
        ViewState["page_count"] = 0;
        ViewState["record_count"] = 0;
        ViewState["page_size"] = 10;
        ViewState["serach_Row_str"] = " '编号'=number,'事项来源'=SXLY,'优先级'=YXJ,'创建时间'=CreateTime,'对接人'=DJR,'发起时间'=FQSJ,'应答人'=YDR,'应答时间'=YDSJ,'反馈人'=FKR,'反馈时间'=FKSJ ";
        ViewState["search_str_where"] = " ";
        ViewState["search_tbname"] = " XXHZXGZTJJLB ";
        ViewState["search_mainid"] = " number ";
        ViewState["search_paixu"] = " DESC ";
        ViewState["search_paixuZD"] = " CreateTime ";
    }

    /// <summary>
    /// 重设数据集，用于对搜索结果特殊处理
    /// </summary>
    private DataSet resetds(DataSet dstemp)
    {
        //进行处理
        return dstemp;
    }

    #endregion
    #region  分页数据绑定处理的函数集合。直接粘贴使用，一般情况无需进行任何修改。
    /// <summary>
    /// 开始绑定数据(更改参数后执行)
    /// </summary>
    private void Begin_My_Binding_Pager()
    {
        //开始绑定(调用时候不用修改)
        My_Binding_Pager((int)ViewState["page_index"], (int)ViewState["page_count"], (int)ViewState["record_count"], (int)ViewState["page_size"], (string)ViewState["serach_Row_str"], (string)ViewState["search_str_where"], (string)ViewState["search_tbname"], (string)ViewState["search_mainid"], (string)ViewState["search_paixu"], (string)ViewState["search_paixuZD"]);
        //============
    }



    /// <summary>
    /// 得到从工厂返回的各种数据，绑定数据表并完成分页显示
    /// </summary>
    private void My_Binding_Pager(int page_index, int page_count, int record_count, int page_size, string serach_Row_str, string search_str_where, string search_tbname, string search_mainid, string search_paixu, string search_paixuZD)
    {
        /// <summary>
        /// 连接工厂接口
        /// </summary>
        I_DBFactory I_DBF;
        /// <summary>
        /// 数据库连接接口
        /// </summary>
        I_Dblink I_DBL;
        /// <summary>
        /// 哈希表,存储存储过程参数
        /// </summary>
        Hashtable news_hashTb = new Hashtable();
        /// <summary>
        /// 用于填充DataGrid控件的数据集
        /// </summary>
        DataSet DataSet_Beuse = new DataSet();
        /// <summary>
        /// 用于传递给工厂类的哈希表,包含存储过程需要传入的参数
        /// </summary>
        Hashtable Hashtable_PutIn = new Hashtable();
        /// <summary>
        /// 用于获取工厂类返回结果的哈希表,包含存储过程需要传出的参数
        /// </summary>
        Hashtable Hashtable_PutOut = new Hashtable();
        /// <summary>
        /// 用于获取工厂类返回结果的哈希表,包含工厂类执行完成返回的结果集合
        /// </summary>
        Hashtable return_ht = new Hashtable();
        /// <summary>
        /// 用来初始化给存储过程需要的参数
        /// </summary>
        Hashtable init_ht = new Hashtable();
        DataSet_Beuse.Clear();
        return_ht.Clear();
        Hashtable_PutIn.Clear();
        Hashtable_PutOut.Clear();
        //给哈希表赋值
        Hashtable_PutIn.Add("@PageIndex", page_index);//页面索引
        Hashtable_PutIn.Add("@PageSize", page_size);//单页数量
        Hashtable_PutIn.Add("@strGetFields", serach_Row_str);//要查询的列
        Hashtable_PutIn.Add("@tableName", search_tbname);//表名称
        Hashtable_PutIn.Add("@ID", search_mainid); //主键
        Hashtable_PutIn.Add("@strWhere", search_str_where); //查询条件
        Hashtable_PutIn.Add("@sortName", search_paixu); //排序方式,前后空格
        Hashtable_PutIn.Add("@orderName", search_paixuZD); //父级查询排序方式,用于排序的字段
        Hashtable_PutOut.Add("@RecordCount", record_count); //返回记录总数
        Hashtable_PutOut.Add("@PageCount", page_count); //返回分页后页数
        //初始化数据工厂以及相关参数
        I_DBF = new DBFactory();
        I_DBL = I_DBF.DbLinkSqlMain(ConfigurationManager.ConnectionStrings["FMOPConn"].ToString());
        //获取数据
        return_ht = I_DBL.RunProc_CMD("GetCustomersDataPage", DataSet_Beuse, Hashtable_PutIn, ref Hashtable_PutOut);
        page_count = Convert.ToInt32(Hashtable_PutOut["@PageCount"]);
        ViewState["page_count"] = page_count;
        record_count = Convert.ToInt32(Hashtable_PutOut["@RecordCount"]);
        ViewState["record_count"] = record_count;
        if ((bool)return_ht["return_float"])
        {
            fy_tb_main.Visible = true;
            tb_tz.Text = (page_index + 1).ToString();
            DataSet_Beuse = (DataSet)return_ht["return_ds"];
            DataSet DataSet_Beuse_new = resetds(DataSet_Beuse);
            GV_show.DataSource = DataSet_Beuse_new.Tables[0].DefaultView;
            GV_show.DataBind();
            fymsg.InnerHtml = "共" + page_count + "页,当前第" + (page_index + 1).ToString() + "页,共" + record_count + "条数据,每页" + page_size + "条";
            //开始重置分页数列表
            Button[] barr_fy = { tb_ym1, tb_ym2, tb_ym3, tb_ym4, tb_ym5, tb_ym6, tb_ym7, tb_ym8, tb_ym9, tb_ym10 };
            int nnm = 2;
            int search_fyshow = barr_fy.Length / nnm;
            //小于分页数的一半
            if ((page_index + 1) <= search_fyshow)
            {
                //循环更新数码
                for (int i = 0; i < barr_fy.Length; i++)
                {
                    barr_fy[i].Text = (i + 1).ToString();
                    barr_fy[i].CommandArgument = (i + 1 - 1).ToString();
                    barr_fy[i].Visible = true;
                    //隐藏无效的页码
                    if (i + 1 > page_count)
                    {
                        barr_fy[i].Visible = false;
                    }
                    //是否当前页
                    if (page_index == i)
                    {
                        //当前页
                        barr_fy[i].CssClass = "supfy_current";
                    }
                    else
                    {
                        barr_fy[i].CssClass = "supfy";
                    }

                }
            }
            //大于分页数的一半
            if ((page_index + 1) > search_fyshow)
            {
                //循环更新数码
                for (int i = 0; i < barr_fy.Length; i++)
                {
                    barr_fy[i].Text = (page_index + 1 - search_fyshow + i).ToString();
                    barr_fy[i].CommandArgument = (page_index + 1 - search_fyshow + i - 1).ToString();
                    barr_fy[i].Visible = true;
                    //隐藏无效的页码
                    if (page_index + 1 - search_fyshow + i > page_count)
                    {
                        barr_fy[i].Visible = false;
                    }
                    //是否当前页
                    if (search_fyshow == i)
                    {
                        //当前页
                        barr_fy[i].CssClass = "supfy_current";
                    }
                    else
                    {
                        barr_fy[i].CssClass = "supfy";
                    }
                }
            }

        }
        else
        {
            fy_tb_main.Visible = false;
            GV_show.DataSource = null;
            GV_show.DataBind();
            //显示数据库读取操作的返回错误信息 = return_ht["return_errmsg"].ToString();
            //Response.Write(return_ht["return_errmsg"].ToString());
        }
    }

    //CommandEventArgs为command事件提供数据
    protected void B_fynumlist_one_Click(Object sender, CommandEventArgs e)
    {
        ViewState["page_index"] = System.Convert.ToInt32(e.CommandArgument);
        Begin_My_Binding_Pager();
    }
    protected void B_sy_Click(object sender, EventArgs e)
    {
        ViewState["page_index"] = 0;
        Begin_My_Binding_Pager();
    }
    protected void B_syy_Click(object sender, EventArgs e)
    {
        int page_index = (int)ViewState["page_index"];
        if (page_index - 1 <= 0)
        {
            ViewState["page_index"] = 0;
        }
        else
        {
            ViewState["page_index"] = page_index - 1;
        }
        Begin_My_Binding_Pager();
    }
    protected void B_xyy_Click(object sender, EventArgs e)
    {
        int page_index = (int)ViewState["page_index"];
        int page_count = (int)ViewState["page_count"];
        if (page_index + 1 >= page_count - 1)
        {
            ViewState["page_index"] = page_count - 1;
        }
        else
        {
            ViewState["page_index"] = page_index + 1;
        }
        Begin_My_Binding_Pager();
    }
    protected void B_sy0_Click(object sender, EventArgs e)
    {
        int page_count = (int)ViewState["page_count"];
        ViewState["page_index"] = page_count - 1;
        Begin_My_Binding_Pager();
    }
    protected void tb_gotz_Click(object sender, EventArgs e)
    {
        int page_index = (int)ViewState["page_index"];
        int page_count = (int)ViewState["page_count"];
        System.Text.RegularExpressions.Regex RegNumber = new System.Text.RegularExpressions.Regex("^[0-9]+$");
        System.Text.RegularExpressions.Match m = RegNumber.Match(tb_tz.Text);
        int tz = 0;
        if (m.Success)
        {
            tz = System.Convert.ToInt32(tb_tz.Text);
        }
        else
        {
            tb_tz.Text = (page_index + 1).ToString();
            return;
        }
        if (tz - 1 <= 0)
        {
            ViewState["page_index"] = 0;
        }
        else if (tz - 1 >= page_count - 1)
        {
            ViewState["page_index"] = page_count - 1;
        }
        else
        {
            ViewState["page_index"] = tz - 1;
        }
        Begin_My_Binding_Pager();
    }
    #endregion
    protected void Button1_Click(object sender, EventArgs e)
    {
        string key = tb_key.Text.Trim();
        string tm11 = tm1.Text.Trim();
        string tm22 = tm2.Text.Trim();
        string sqlwhere = " 1=1 ";
        if (key != "")
        {
            sqlwhere = sqlwhere + " and (number like '%" + key + "%' or SXLY  like '%" + key + "%' or YXJ like '%" + key + "%' or DJR like '%" + key + "%' or FQSJ like '%" + key + "%' or YDR like '%" + key + "%' or YDSJ like '%" + key + "%' or FKR like '%" + key + "%' or FKSJ like '%" + key + "%' or SXSMMS like '%" + key + "%' or SXGJBDQK like '%" + key + "%') ";
        }
        if (tm11 != "" && tm22 != "")
        {
            sqlwhere = sqlwhere + "and (CreateTime between  '" + tm11 + "' and  '" + tm22 + "')";
        }

        ViewState["search_str_where"] = sqlwhere;
        Begin_My_Binding_Pager();
    }
}