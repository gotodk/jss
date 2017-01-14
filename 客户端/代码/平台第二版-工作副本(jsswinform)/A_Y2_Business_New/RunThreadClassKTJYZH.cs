using System.Collections;
using System.Data;
using System.Threading;
using 客户端主程序.DataControl;
using 客户端主程序.Support;

namespace 客户端主程序.NewDataControl
{
    public class RunThreadClassKTJYZH
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
        public RunThreadClassKTJYZH(Hashtable PHT, delegateForThread DFT)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
            DForThread = DFT;
            InPutHT = PHT;
        }

 

        /// <summary>
        ///提交开通交易账户的信息
        /// </summary>
        public void SubmitJYZHXX()
        {
            /*已移植不再需要
            string strMethodName = "";
            switch (InPutHT["方法类别"].ToString())
            {
                case "经纪人单位":
                    strMethodName = "SubmitJJRDW";
                    break;
                case "经纪人个人":
                    strMethodName = "SubmitJJRGR";
                    break;
                case "买卖家单位":
                    strMethodName = "SubmitMMJDW";
                    break;
                case "买卖家个人":
                    strMethodName = "SubmitMMJGR";
                    break;
            }
             */
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
                        
            object[] objParams = { dataTable };

            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("strMethodName", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C开通交易账户", objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);


        }

        /// <summary>
        /// 更行交易账户信息
        /// </summary>
        public void UpdateJYZHXX()
        {
            /*已移植不再需要
             string strMethodName = "";
            switch (InPutHT["方法类别"].ToString())
            {
                
                case "经纪人单位":
                    strMethodName = "UpdateJJRDW";
                    break;
                case "经纪人个人":
                    strMethodName = "UpdateJJRGR";
                    break;
                case "买卖家单位":
                    strMethodName = "UpdateMMJDW";
                    break;
                case "买卖家个人":
                    strMethodName = "UpdateMMJGR";
                    break;
            }
             */

            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
                       
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices(strMethodName, objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C修改交易账户", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 提交经纪人审核买卖家账户的信息
        /// </summary>
        public void SubmitJJRSHXX()
        {
            string strMethodName = "";
            switch (InPutHT["方法类别"].ToString())
            {
                case "买卖家审核通过":
                    //已移植可替换(已替换)
                    //strMethodName = "JJRPassMMJ";
                    strMethodName = "C经纪人通过买卖家";
                    break;
                case "买卖家驳回":
                    //已移植可替换(已替换)
                    //strMethodName = "JJRRejectMMJ";
                    strMethodName = "C经纪人驳回买卖家";
                    break;
            }
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
           
            object[] objParams = { dataTable };
            DataSet dsreturn = WSC2013.RunAtServices(strMethodName, objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
        /// <summary>
        ///得到买卖、卖家数据的基本信息
        /// </summary>
        public void GetMMJData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetMMJData", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C获取买卖家信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);


        }

        /// <summary>
        /// 获取账号信息的基础数据
        /// </summary>
        public void GetBasicData()
        {
            string strMethodName = "";
            switch (InPutHT["方法类别"].ToString())
            {
                case "经纪人交易账户":
                    //已移植可替换(已替换)
                    //strMethodName = "GetJJRData";
                    strMethodName = "C经纪人账户信息";
                    break;
                case "买家卖家交易账户":
                    //已移植可替换(已替换)
                    //strMethodName = "GetMMJZHZLData";
                    strMethodName = "C买卖家账户信息";
                    break;
            }
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            DataSet dsreturn = WSC2013.RunAtServices(strMethodName, objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 获取更换经纪人时获取的经纪人交易账户数据信息    
        /// </summary>
        public void GetSiftJJRData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetSiftJJRData", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C获取更换经纪人信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 当选择经纪人时获取要判断的数据信息，判断后执行相应的操作
        /// </summary>
        public void GetSelectJudgeJJRData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetSelectJudgeJJRData", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C选择经纪人", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }



        /// <summary>
        /// 获取交易账户审核的数据信息
        /// </summary>
        public void GetJYZHSHData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetJYZHSHData", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C交易账户审核数据信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }


        /// <summary>
        /// 获取经纪人资格证书路径
        /// </summary>
        public void GetJJRZGZSData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);

            dataTable.Columns.Add("买家角色编号");
            dataTable.Rows[0]["买家角色编号"] = PublicDS.PublisDsUser.Tables["用户信息"].Rows[0]["买家角色编号"].ToString();            
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("GetJJRZGZS", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C获取经纪人资格证书", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }

        /// <summary>
        /// 暂停、恢复新用户审核  仅用于经纪人交易账户的时候
        /// </summary>
        /// <param name="dsreturn"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public void ZT_HF_XYHSH()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("ZT_HF_XYHSH", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C暂停恢复新用户审核", objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
        /// <summary>
        /// 暂停、恢复新用户审核  初始状态
        /// </summary>
        public void ZT_HF_XYHSHCSZT()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("ZT_HF_XYHSHCSZT", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C暂停恢复新用户审核初始状态", objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);
        }
        /// <summary>
        /// 暂停恢复用户的新业务
        /// </summary>
        /// <param name="dsreturn"></param>
        /// <param name="dtInfor"></param>
        /// <returns></returns>
        public void SetZTHFYHXYW()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("SetZTHFYHXYW", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C暂停恢复用户的新业务", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 得到当前交易账户的当前状态
        /// </summary>
        public void GetJYZHDQZT()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //C当前交易账户状态
            //DataSet dsreturn = WSC2013.RunAtServices("GetJYZHDQZT", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C当前交易账户状态", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }


        /// <summary>
        /// 得到与电子购货合同匹配的数据信息
        /// 这个方法不再使用
        /// </summary>
        public void GetDZGHHTXX()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            DataSet dsreturn = WSC2013.RunAtServices("GetDZGHHTXX", objParams);            
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 判断是否签订承诺书
        /// </summary>
        public void JudgeSFQDCNS()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("JudgeSFQDCNS", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C判断是否签订承诺书", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }
        /// <summary>
        /// 交易账户签订承诺书
        /// </summary>
        public void JYZHQDCNS()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
            //DataSet dsreturn = WSC2013.RunAtServices("JYZHQDCNS", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C交易账户签订承诺书", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 得到投标单的数据信息
        /// </summary>
        public void Get_TBDData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换)
           // DataSet dsreturn = WSC2013.RunAtServices("Get_TBDData", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C得到投标单的信息HT", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 得到预订单的数据信息
        /// </summary>
        public void Get_YDDData()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            // DataSet dsreturn = WSC2013.RunAtServices("Get_YDDData", objParams); C得到预订单的信息HT

            //已移植可替换(已替换)
            DataSet dsreturn = WSC2013.RunAtServices("C得到预订单的信息HT", objParams); //C得到预订单的信息HT
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 提交银行员工信息
        /// </summary>
        public void SubmitYHUserInfor()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换) wyh 2014.0711
            DataSet dsreturn = WSC2013.RunAtServices("C提交银行员工信息", objParams);

            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }

        /// <summary>
        /// 修改银行员工信息
        /// </summary>
        public void ModifyYHUserInfor()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换) wyh 2014.0711
           // DataSet dsreturn = WSC2013.RunAtServices("ModifyYHYHInfor", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C修改银行人员信息表", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }


        /// <summary>
        /// 删除数据信息
        /// </summary>
        public void DeleteYHUserInfor()
        {
            //防卡死，停100毫秒
            Thread.Sleep(100);
            //访问远程服务
            NewDataControl.WebServicesCenter2013 WSC2013 = new NewDataControl.WebServicesCenter2013();
            //InPutHT可以选择灵活使用

            DataTable dataTable = StringOP.GetDataTableFormHashtable(InPutHT);
            object[] objParams = { dataTable };
            //已移植可替换(已替换) wyh 2014.0711
           // DataSet dsreturn = WSC2013.RunAtServices("DeleteYHYHInfor", objParams);
            DataSet dsreturn = WSC2013.RunAtServices("C删除银行员工信息", objParams);
            //填充传入参数哈希表
            Hashtable OutPutHT = new Hashtable();
            OutPutHT["返回值"] = dsreturn;
            DForThread(OutPutHT);

        }




    }
}
