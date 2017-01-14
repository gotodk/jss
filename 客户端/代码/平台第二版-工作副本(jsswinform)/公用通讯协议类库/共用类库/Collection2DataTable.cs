using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace 公用通讯协议类库.共用类库
{
    public class Collection2DataTable
    {
        private UserCollection UserList;
        public Collection2DataTable(UserCollection UCtemp)
        {
            UserList = UCtemp;
        }

        public DataTable GetDataTable()
        {
            DataTable dttemp = new DataTable();
            dttemp.Columns.Add(new DataColumn("使用者帐号", typeof(string)));
            dttemp.Columns.Add(new DataColumn("广域网络终结点", typeof(string)));
            dttemp.Columns.Add(new DataColumn("局域网络终结点", typeof(string)));
            dttemp.Columns.Add(new DataColumn("私有连接状态", typeof(string)));
            dttemp.Columns.Add(new DataColumn("私有连接方式", typeof(string)));
            dttemp.Columns.Add(new DataColumn("私有有效终结点", typeof(string)));

            foreach (User user in UserList)
            {
                DataRow dr = dttemp.NewRow();
                dr["使用者帐号"] = user.UserName;
                if (user.NetPoint_G != null)
                {
                    dr["广域网络终结点"] = user.NetPoint_G.Address + ":" + user.NetPoint_G.Port;
                }
                else
                {
                    dr["广域网络终结点"] = "";
                }
                string J_str = "";
                if (user.NetPoint_J != null)
                {
                    for (int p = 0; p < user.NetPoint_J.Count; p++)
                    {
                        IPEndPoint IPEndPoint_temp = (IPEndPoint)(user.NetPoint_J[p.ToString()]);
                        J_str = J_str + IPEndPoint_temp.Address + ":" + IPEndPoint_temp.Port + " | ";
                    }
                }
                dr["局域网络终结点"] = J_str;
                dr["私有连接状态"] = user.Online_own;
                dr["私有连接方式"] = user.Type_own;
                if (user.NetPoint_own != null)
                {
                    dr["私有有效终结点"] = user.NetPoint_own.Address + ":" + user.NetPoint_own.Port;
                }
                dttemp.Rows.Add(dr);

            }
            return dttemp;
        }
    }
}
