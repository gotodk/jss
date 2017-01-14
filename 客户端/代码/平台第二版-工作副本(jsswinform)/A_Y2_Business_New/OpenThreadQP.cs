using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using 客户端主程序.DataControl;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
   public  class OpenThreadQP
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
        public OpenThreadQP(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

        /// <summary>
        /// 开始执行线程
        /// </summary>
        public void BeginRun()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);

            if (!dt.Columns.Contains("买家角色编号"))
            {
                dt.Columns.Add("买家角色编号");
                dt.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            }

            object[] cs = { dt };
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C获取清盘数据", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        public void BeginRunDWControversial()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);

            if (!dt.Columns.Contains("买家角色编号"))
            {
                dt.Columns.Add("买家角色编号");
                dt.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            }

            object[] cs = { dt };
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C用户上传证明文件", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        public void BeginRunDWConfirm()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);

            if (!dt.Columns.Contains("买家角色编号"))
            {
                dt.Columns.Add("买家角色编号");
                dt.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();
            }

            object[] cs = { dt };
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C对方确认", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        public void BeginRunGetNewUserState()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用
            DataTable dt = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] cs = { dt };
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C用户基本信息", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        public void BeginRunGetSHWJinfo()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用            
            object[] cs = { InPutHT["number"].ToString() };
            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C获取中标定标信息表上传文件的信息", cs);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
    }
}
