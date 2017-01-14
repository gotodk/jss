using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
/// 万能复合菜单,标签无错设置方便 by 于海滨。
/// 使用方式:拖入一个TreeView，设置菜单。 在行生成事件中，调用BeginReSetHand即可。
/// 使用复合菜单会取消标题。
/// </summary>
public class YHBRowHeader
{
    
	public YHBRowHeader()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    #region 万能复合菜单,标签无错设置方便 by 于海滨。使用方式:拖入一个TreeView，设置菜单。 在行生成事件中，调用BeginReSetHand即可。使用复合菜单会取消标题。

    /// <summary>
    /// 复合列最大层数，用于确定最大行数
    /// </summary>
    int yhbmaxlevel;
    /// <summary>
    /// 节点集合哈希表，实际类型是ArrayList.而ArrayList的实际类型是TreeNodeCollection
    /// </summary>
    Hashtable yhbALTreeNodeCollection;
    /// <summary>
    /// 某节点的末端菜单个数。
    /// </summary>
    int yhbMMMnum;
    /// <summary>
    /// 遍历所有菜单，获取生成列所需数据
    /// </summary>
    /// <param name="tn"></param>
    private void LookAllNode(TreeNodeCollection tn, TreeView TVhide)
    {
        TVhide.PathSeparator = '★';
        foreach (TreeNode node in tn)
        {
            string[] thisvp = node.ValuePath.Split('★');
            int thislevel = thisvp.Length;
            ArrayList AL = new ArrayList();
            if (yhbALTreeNodeCollection.ContainsKey(thisvp.Length - 1))
            {
                AL = (ArrayList)yhbALTreeNodeCollection[thisvp.Length - 1];
            }
            AL.Add(node);
            yhbALTreeNodeCollection[thisvp.Length - 1] = AL;
            if (yhbmaxlevel < thislevel)
            {
                yhbmaxlevel = thislevel;
            }
            if (node.ChildNodes.Count != 0)
            {
                LookAllNode(node.ChildNodes, TVhide);
            }
        }
    }
    /// <summary>
    /// 遍历指定菜单，获取某个节点的下属末端菜单个数
    /// </summary>
    /// <param name="tn"></param>
    private void LookNodeForMMM(TreeNodeCollection tn, TreeView TVhide)
    {
        TVhide.PathSeparator = '★';
        foreach (TreeNode node in tn)
        {
            string[] thisvp = node.ValuePath.Split('★');
            int thislevel = thisvp.Length;
            if (node.ChildNodes.Count != 0)
            {
                LookNodeForMMM(node.ChildNodes, TVhide);
            }
            else
            {
                yhbMMMnum++;
            }
        }
    }
    /// <summary>
    /// 开始重新设置复合表头
    /// </summary>
    /// <param name="GVR">e.Row不用动</param>
    /// <param name="TVhide">菜单控件</param>
    /// <param name="sender">sender不用动</param>
    public void BeginReSetHand(GridViewRow GVR, TreeView TVhide, object sender)
    {
        switch (GVR.RowType)
        {
            case DataControlRowType.Header:
                GridView GVthis = (GridView)sender;

                //清理原有表头
                GVR.Cells.Clear();//清理原列
                GVthis.Caption = "";//清理控件标题
                GVthis.CaptionAlign = TableCaptionAlign.NotSet;//清理控件标题
                //获取所有表头，准备生成
                yhbmaxlevel = 0;
                yhbALTreeNodeCollection = new Hashtable();
                LookAllNode(TVhide.Nodes, TVhide);
                //循环行
                for (int p = 0; p < yhbmaxlevel; p++)
                {
                    //判定往哪个里面放行
                    GridViewRow rowHeader;
                    if (p == yhbmaxlevel - 1)
                    {
                        rowHeader = GVR;//用自带的行
                    }
                    else
                    {
                        //添加新行
                        rowHeader = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                        GVthis.Controls[0].Controls.AddAt(p, rowHeader);
                    }
                    ArrayList tnc = (ArrayList)yhbALTreeNodeCollection[p];
                    for (int t = 0; t < tnc.Count; t++)
                    {
                        rowHeader.Cells.Add(new TableHeaderCell());
                        //当前菜单所有末端菜单个数
                        yhbMMMnum = 0;
                        LookNodeForMMM(((TreeNode)tnc[t]).ChildNodes, TVhide);
                        if (yhbMMMnum > 1)
                        {
                            rowHeader.Cells[t].Attributes.Add("colspan", yhbMMMnum.ToString());
                        }
                        int rowstr = yhbmaxlevel - p;//最大行数减当前行索引就是行跨度,这种算法是默认末级拓展
                        if (rowstr > 1 && yhbMMMnum == 0)
                        {
                            rowHeader.Cells[t].Attributes.Add("rowspan", rowstr.ToString());
                        }
                        rowHeader.Cells[t].Text = ((TreeNode)tnc[t]).Value;
                    }
                }
                break;
        }
    }
    #endregion

}