using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FMOP.DB;
using System.Web.UI;
using System.Runtime.Remoting.Contexts;
using System.Globalization;
/// <summary>
/// KTJYZHClass 的摘要说明
/// </summary>
public class KTJYZHClass
{
    public KTJYZHClass()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 获取平台管理机构
    /// </summary>
    /// <param name="dataSet"></param>
    /// <returns></returns>
    public DataSet GetGLJG(DataSet dsreturn)
    {
        DataTable dataFGSname = DbHelperSQL.Query("select FGSname,BSCname from AAA_CityList_FGS where BSCname !='其他办事处' Group by BSCname,FGSname ").Tables[0];
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台管理机构数据成功！";
        DataTable dataTable1 = dataFGSname.Copy();
        dsreturn.Tables.Add(dataTable1);
        dsreturn.Tables[1].TableName = "平台管理机构";
        return dsreturn;
    }
    #region //交易账户信息提交
    /// <summary>
    /// 提交，开通【经纪人、单位】的交易账户信息
    /// </summary>
    /// <returns></returns>
    public DataSet SubmitJJRDW(DataSet dsreturn, DataTable dataTable)
    {

        ArrayList arrayList = new ArrayList();

        //如果用户没有选择管理机构 根据省、市、区自动选择平台管理机构
        if (dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() == "请选择平台管理机构")
        {

            Hashtable hashTable = new Hashtable();
            hashTable["省份"] = dataTable.Rows[0]["经纪人单位所属省份"].ToString();
            hashTable["地市"] = dataTable.Rows[0]["经纪人单位所属地市"].ToString();
            string strGLJG = GetPTGLJG(hashTable);
            if (strGLJG == "没有对应的数据")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台管理机构数据失败！";
                return dsreturn;
            }
            else
            {
                dataTable.Rows[0]["经纪人单位平台管理机构"] = strGLJG;
            }
        }
        //判断该登录邮箱是否开通交易账户
        DataTable dataTableExits = DbHelperSQL.Query("select B_JSZHLX '交易账户类型',J_SELJSBH '卖家角色编号',J_BUYJSBH '买家角色编号',J_JJRJSBH '经纪人资格证书编号' from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "'").Tables[0];
        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            Object objectName = dataTableExits.Rows[0]["交易账户类型"];
            if (objectName.ToString() != "")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已经提交过，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";
                DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());
                DataTable dataTableYHXX = dataSet.Tables["用户信息"];
                DataTable dataTableGLXX = dataSet.Tables["关联信息"];
                DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
                DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables[1].TableName = "用户信息";
                dsreturn.Tables.Add(dataTableGLXX_Copy);
                dsreturn.Tables[2].TableName = "关联信息";
                return dsreturn;
            }
        }

        //生成买家角色编号
        string strBuyerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString(), "C");
        //string strBuyerJSBH = "C'+Number +'";
        //生成经纪人角色编号
        //  string strJJRJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString(), "J");
        string strJJRJSBH = strBuyerJSBH.Replace("C", "J");
        //string strJJRJSBH = "J'+Number+'";
        //经纪人资格证书编号  此处先空着,暂用这这个W开头代替   此处经纪人资格证是系统生成的 
        // string strJJRZGZSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString(), "D");
        //string strJJRZGZSBH = "D'+Number+'";
        string strJJRZGZSBH = strBuyerJSBH.Replace("C", "D");
        //获取关联表的Number值
        string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");


        //登录表
        string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX='" + dataTable.Rows[0]["经纪人单位交易账户类别"].ToString() + "',J_BUYJSBH='" + strBuyerJSBH + "',J_JJRJSBH='" + strJJRJSBH + "',J_JJRSFZTXYHSH='否',J_JJRZGZSBH='" + strJJRZGZSBH + "',JJRZGZS='',S_SFYBJJRSHTG='是',S_SFYBFGSSHTG='否',I_ZCLB='" + dataTable.Rows[0]["经纪人单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["经纪人单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人单位银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() + "',I_ZLTJSJ='" + DateTime.Now.ToString() + "',B_JSZHMM='" + dataTable.Rows[0]["经纪人单位证券资金密码"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人单位院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人单位经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "'";
        arrayList.Add(strDLZHXXB);
        //插入关联表   
        string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人单位交易账户类别"].ToString() + "','" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "','" + strJJRJSBH + "','" + dataTable.Rows[0]["经纪人单位用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
        arrayList.Add(strGLB);
        bool bolTag = DbHelperSQL.ExecSqlTran(arrayList);
        if (bolTag)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息提交成功！";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] += "\n\r您的资料已提交，正在审核，交易账户将在1工作日资料审核通过后开通。\n\r纸质资料请邮寄至总部服务中心处，邮寄信息：\n\r公司名称：富美科技集团有限公司中国商品批发交易平台事业部服务中心\n\r地址：南经济开发区（长清平安）富美路 \n\r 邮政编码：250306  电话：400-688-8844 ";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }

    /// <summary>
    /// 提交，开通【经纪人、个人】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet SubmitJJRGR(DataSet dsreturn, DataTable dataTable)
    {

        ArrayList arrayList = new ArrayList();

        //如果用户没有选择管理机构 根据省、市、区自动选择平台管理机构
        if (dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() == "请选择平台管理机构")
        {

            Hashtable hashTable = new Hashtable();
            hashTable["省份"] = dataTable.Rows[0]["经纪人个人所属省份"].ToString();
            hashTable["地市"] = dataTable.Rows[0]["经纪人个人所属地市"].ToString();
            string strGLJG = GetPTGLJG(hashTable);
            if (strGLJG == "没有对应的数据")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取平台管理机构数据失败！";
                return dsreturn;
            }
            else
            {
                dataTable.Rows[0]["经纪人个人平台管理机构"] = strGLJG;
            }
        }

        //判断该登录邮箱是否开通交易账户
        DataTable dataTableExits = DbHelperSQL.Query("select B_JSZHLX '交易账户类型',J_SELJSBH '卖家角色编号',J_BUYJSBH '买家角色编号',J_JJRJSBH '经纪人资格证书编号' from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "'").Tables[0];
        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            Object objectName = dataTableExits.Rows[0]["交易账户类型"];
            if (objectName.ToString() != "")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已经提交过，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";
                DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());
                DataTable dataTableYHXX = dataSet.Tables["用户信息"];
                DataTable dataTableGLXX = dataSet.Tables["关联信息"];
                DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
                DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables[1].TableName = "用户信息";
                dsreturn.Tables.Add(dataTableGLXX_Copy);
                dsreturn.Tables[2].TableName = "关联信息";
                return dsreturn;
            }
        }

        //生成买家角色编号
        string strBuyerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), "C");
        // string strBuyerJSBH = "C'+Number+'";

        //生成经纪人角色编号
        //  string strJJRJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), "J");
        // string strJJRJSBH = "J'+Number+'";
        string strJJRJSBH = strBuyerJSBH.Replace("C", "J");
        //经纪人资格证书编号  此处先空着,暂用这这个W开头代替   此处经纪人资格证是系统生成的 
        // string strJJRZGZSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), "D");
        // string strJJRZGZSBH = "D'+Number+'";
        string strJJRZGZSBH = strBuyerJSBH.Replace("C", "D");
        //获取关联表的Number值
        string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
        //登录表 
        try
        {
            string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX='" + dataTable.Rows[0]["经纪人个人交易账户类别"].ToString() + "',J_BUYJSBH='" + strBuyerJSBH + "',J_JJRJSBH='" + strJJRJSBH + "',J_JJRSFZTXYHSH='否',J_JJRZGZSBH='" + strJJRZGZSBH + "',JJRZGZS='',S_SFYBJJRSHTG='是',S_SFYBFGSSHTG='否',I_ZCLB='" + dataTable.Rows[0]["经纪人个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["经纪人个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人个人银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() + "' ,I_ZLTJSJ='" + DateTime.Now.ToString() + "',B_JSZHMM='" + dataTable.Rows[0]["经纪人个人证券资金密码"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人个人院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人个人经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "'";




            arrayList.Add(strDLZHXXB);
            //插入关联表   
            string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人个人交易账户类别"].ToString() + "','" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "','" + strJJRJSBH + "','" + dataTable.Rows[0]["经纪人个人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
            arrayList.Add(strGLB);
            bool bolTag = DbHelperSQL.ExecSqlTran(arrayList);

            if (bolTag)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息提交成功！";
                //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] += "\n\r您的资料已提交，正在审核，交易账户将在1工作日资料审核通过后开通。\n\r纸质资料请邮寄至总部服务中心处，邮寄信息：\n\r公司名称：富美科技集团有限公司中国商品批发交易平台事业部服务中心\n\r地址：南经济开发区（长清平安）富美路 \n\r 邮政编码：250306  电话：400-688-8844 ";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

                DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());
                DataTable dataTableYHXX = dataSet.Tables["用户信息"];
                DataTable dataTableGLXX = dataSet.Tables["关联信息"];
                DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
                DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables[1].TableName = "用户信息";
                dsreturn.Tables.Add(dataTableGLXX_Copy);
                dsreturn.Tables[2].TableName = "关联信息";
                return dsreturn;
            }
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
            return dsreturn;
        }
        catch (Exception exec)
        {

            throw exec;
        }
    }

    /// <summary>
    /// 提交，开通【买卖家、单位】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet SubmitMMJDW(DataSet dsreturn, DataTable dataTable)
    {

        //判断该登录邮箱是否开通交易账户
        DataTable dataTableExits = DbHelperSQL.Query("select B_JSZHLX '交易账户类型',J_SELJSBH '卖家角色编号',J_BUYJSBH '买家角色编号',J_JJRJSBH '经纪人资格证书编号' from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'").Tables[0];
        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            Object objectName = dataTableExits.Rows[0]["交易账户类型"];
            if (objectName.ToString() != "")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已经提交过，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";
                DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
                DataTable dataTableYHXX = dataSet.Tables["用户信息"];
                DataTable dataTableGLXX = dataSet.Tables["关联信息"];
                DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
                DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables[1].TableName = "用户信息";
                dsreturn.Tables.Add(dataTableGLXX_Copy);
                dsreturn.Tables[2].TableName = "关联信息";
                return dsreturn;
            }
        }


        ArrayList arrayList = new ArrayList();
        //生成买家角色编号
        string strBuyerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString(), "C");
        // string strBuyerJSBH = "C'+Number+'";

        //生成卖家角色编号
        // string strSelerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString(), "B");
        //  string strSelerJSBH = "B'+Number+'";
        string strSelerJSBH = strBuyerJSBH.Replace("C", "B");
        //获取关联表的Number值
        string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
        DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家单位经纪人资格证书编号"].ToString());
        if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
            return dsreturn;
        }
        //登录表I_PTGLJG

        string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX='" + dataTable.Rows[0]["买卖家单位交易账户类别"].ToString() + "',J_SELJSBH='" + strSelerJSBH + "',J_BUYJSBH='" + strBuyerJSBH + "',S_SFYBJJRSHTG='否',S_SFYBFGSSHTG='否',I_ZCLB='" + dataTable.Rows[0]["买卖家单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["买卖家单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家单位银行账号"].ToString() + "' ,I_ZLTJSJ='" + DateTime.Now.ToString() + "',B_JSZHMM='" + dataTable.Rows[0]["买卖家单位证券资金密码"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家单位关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家单位业务服务部门"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'";

        arrayList.Add(strDLZHXXB);
        //插入关联表   
        string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "','" + dataTable.Rows[0]["买卖家单位交易账户类别"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";

        arrayList.Add(strGLB);
        bool bolTag = DbHelperSQL.ExecSqlTran(arrayList);
        if (bolTag)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息提交成功！";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] += "\n\r您的资料已提交，正在审核，交易账户将在1工作日资料审核通过后开通。\n\r纸质资料请邮寄至总部服务中心处，邮寄信息：\n\r公司名称：富美科技集团有限公司中国商品批发交易平台事业部服务中心\n\r地址：南经济开发区（长清平安）富美路 \n\r 邮政编码：250306  电话：400-688-8844 ";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";


            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";


        return dsreturn;
    }


    /// <summary>
    /// 提交，开通【买卖家、个人】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet SubmitMMJGR(DataSet dsreturn, DataTable dataTable)
    {

        //判断该登录邮箱是否开通交易账户
        DataTable dataTableExits = DbHelperSQL.Query("select B_JSZHLX '交易账户类型',J_SELJSBH '卖家角色编号',J_BUYJSBH '买家角色编号',J_JJRJSBH '经纪人资格证书编号' from AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "'").Tables[0];
        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            Object objectName = dataTableExits.Rows[0]["交易账户类型"];
            if (objectName.ToString() != "")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已经提交过，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";
                DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());
                DataTable dataTableYHXX = dataSet.Tables["用户信息"];
                DataTable dataTableGLXX = dataSet.Tables["关联信息"];
                DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
                DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
                dsreturn.Tables.Add(dataTableYHXX_Copy);
                dsreturn.Tables[1].TableName = "用户信息";
                dsreturn.Tables.Add(dataTableGLXX_Copy);
                dsreturn.Tables[2].TableName = "关联信息";
                return dsreturn;
            }
        }


        ArrayList arrayList = new ArrayList();
        //生成买家角色编号
        string strBuyerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString(), "C");
        // string strBuyerJSBH = "C'+Number+'";
        //生成卖家角色编号
        //  string strSelerJSBH = PublicClass2013.GetNumberPrefix(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString(), "B");
        //  string strSelerJSBH = "B'+Number+'";
        string strSelerJSBH = strBuyerJSBH.Replace("C", "B");
        DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家个人经纪人资格证书编号"].ToString());
        //获取关联表的Number值
        string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
        if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
            return dsreturn;
        }
        //登录表
        string strDLZHXXB = "update AAA_DLZHXXB set B_JSZHLX='" + dataTable.Rows[0]["买卖家个人交易账户类别"].ToString() + "',J_SELJSBH='" + strSelerJSBH + "',J_BUYJSBH='" + strBuyerJSBH + "',S_SFYBJJRSHTG='否',S_SFYBFGSSHTG='否',I_ZCLB='" + dataTable.Rows[0]["买卖家个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["买卖家个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家个人银行账号"].ToString() + "' ,I_ZLTJSJ='" + DateTime.Now.ToString() + "',B_JSZHMM='" + dataTable.Rows[0]["买卖家个人证券资金密码"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家个人关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家个人业务服务部门"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "'";


        arrayList.Add(strDLZHXXB);
        //插入关联表   
        string strGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "','" + dataTable.Rows[0]["买卖家个人交易账户类别"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";

        arrayList.Add(strGLB);
        bool bolTag = DbHelperSQL.ExecSqlTran(arrayList);
        if (bolTag)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息提交成功！";
            //dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] += "\n\r您的资料已提交，正在审核，交易账户将在1工作日资料审核通过后开通。\n\r纸质资料请邮寄至总部服务中心处，邮寄信息：\n\r公司名称：富美科技集团有限公司中国商品批发交易平台事业部服务中心\n\r地址：南经济开发区（长清平安）富美路 \n\r 邮政编码：250306  电话：400-688-8844 ";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请已提交，平台将在2个工作日内完成审核，审核通过后予以开通；\n\r账户开通后，请执交易方编号及相关资料到您选定的银行开立交易资金第三方存管账户。 ";

            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";

            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }

    #endregion


    /// <summary>
    /// 开通同完结算账户后，重新获取基础表数据
    /// </summary>
    /// <returns>返回用户相关数据，或登陆失败数据</returns>
    public DataSet checkLoginIn(string user)
    {

        DataSet dsReturn = new DataSet();

        #region 用户信息查询
        string sql = "SELECT Number,B_DLYX AS 登录邮箱,B_DLMM AS 登录密码,B_JSZHMM AS 结算账户密码,B_YHM AS 用户名,B_JSZHLX AS 结算账户类型,B_SFYZYX AS 是否验证邮箱, B_SFYXDL AS 是否允许登录, B_SFDJ AS 是否冻结, B_SFXM AS 是否休眠, B_ZCSJ AS 注册时间,B_ZHYCDLSJ AS 最后一次登录时间,isnull(B_ZHDQXYFZ,0) '账户当前信用分值', B_ZHDQKYYE AS 账户当前可用余额,B_YZMGQSJ AS 验证码过期时间,B_YXYZM AS 邮箱验证码, ";//用户帐号基础信息部分

        sql += " J_SELJSBH AS 卖家角色编号, J_BUYJSBH AS 买家角色编号, J_JJRJSBH AS 经纪人角色编号, J_JJRSFZTXYHSH AS 经纪人是否暂停新用户审核, J_JJRZGZSBH AS 经纪人资格证书编号, JJRZGZS AS 经纪人资格证书, ";//平台角色部分

        sql += " S_SFYBJJRSHTG AS 是否已被经纪人审核通过, S_SFYBFGSSHTG AS 是否已被分公司审核通过, ";//是否可进行业务flag

        sql += " I_ZCLB AS 注册类别, I_JYFMC AS 交易方名称, I_YYZZZCH AS 营业执照注册号, I_YYZZSMJ AS 营业执照扫描件, I_SFZH AS 身份证号, I_SFZSMJ AS 身份证扫描件,I_SFZFMSMJ 身份证反面扫描件, I_ZZJGDMZDM AS 组织机构代码证代码, I_ZZJGDMZSMJ AS 组织机构代码证扫描件, I_SWDJZSH AS 税务登记证税号, I_SWDJZSMJ AS 税务登记证扫描件, I_YBNSRZGZSMJ AS 一般纳税人资格证扫描件, I_KHXKZH AS 开户许可证号, I_KHXKZSMJ AS 开户许可证扫描件, I_FDDBRXM AS 法定代表人姓名, I_FDDBRSFZH AS 法定代表人身份证号, I_FDDBRSFZSMJ AS 法定代表人身份证扫描件,I_FDDBRSFZFMSMJ 法定代表人身份证反面扫描件, I_FDDBRSQS AS 法定代表人授权书, I_JYFLXDH AS 交易方联系电话, I_SSQYS AS 省, I_SSQYSHI AS 市, I_SSQYQ AS 区, I_XXDZ AS 详细地址, I_LXRXM AS 联系人姓名, I_LXRSJH AS 联系人手机号, I_KHYH AS 开户银行, I_YHZH AS 银行账号, I_PTGLJG AS 平台管理机构,I_DSFCGZT AS '第三方存管状态',(case charindex('-',I_ZQZJZH) when '0' then I_ZQZJZH else ''  end) AS '证券资金账号',I_JJRFL AS 经纪人分类 FROM AAA_DLZHXXB ";//提交资质详细信息

        sql += " WHERE B_DLYX=@B_DLYX  ";//登录帐号&密码
        SqlParameter[] sp = { 
                                new SqlParameter("@B_DLYX",user)
                            };
        #endregion
        DataSet ds = DbHelperSQL.Query(sql, sp);//查询出来的用户信息
        //开始验证
        if (ds != null && ds.Tables[0].Rows.Count == 1)
        {
            DataTable dtUInfo = new DataTable();
            dtUInfo = ds.Tables[0].Copy();//用户信息表
            dtUInfo.TableName = "用户信息";
            DataTable dtUGL = new DataTable();
            DataTable dtUGL_Copys = DbHelperSQL.Query("SELECT Number,DLYX AS 登录邮箱,JSZHLX AS 结算账号类型,GLJJRDLZH AS 关联经纪人登陆账号,GLJJRBH AS 关联经纪人编号, GLJJRYHM AS 关联经纪人用户名, SQSJ AS 申请时间, JJRSHZT AS 经纪人审核状态, JJRSHSJ AS 经纪人审核时间, JJRSHYJ AS 经纪人审核意见, FGSSHZT AS 分公司审核状态, FGSSHR AS 分公司审核人, FGSSHSJ AS 分公司审核时间, FGSSHYJ AS 分公司审核意见, SFZTYHXYW AS 是否暂停用户新业务 FROM AAA_MJMJJYZHYJJRZHGLB WHERE SFDQMRJJR='是' and DLYX='" + user + "' and SFYX='是' ").Tables[0];//关联信息表
            dtUGL = dtUGL_Copys.Copy();
            dtUGL.TableName = "关联信息";
            dsReturn.Tables.Add(dtUInfo);
            dsReturn.Tables.Add(dtUGL);
        }
        return dsReturn;
    }

    #region//交易账户信息修改

    /// <summary>
    /// 修改【经纪人、单位】的交易账户信息
    /// </summary>
    /// <returns></returns>
    public DataSet UpdateJJRDW(DataSet dsreturn, DataTable dataTable)
    {

        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString());
 

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        ArrayList arrayList = new ArrayList();

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表
        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["经纪人单位是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = "";
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["经纪人单位登录邮箱"].ToString(), hashTableVierfy))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;

        }

        if (dataTable.Rows[0]["经纪人单位是否更换平台管理机构"].ToString() == "是")//更换了平台管理机构
        {
            //1、作废关联表
            strSqlZFGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否', SFYX='否' where DLYX='" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "' and JSZHLX='经纪人交易账户'";
            arrayList.Add(strSqlZFGLB);
            //2、获取关联表的Number值
            string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
            //3、插入关联表   
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人单位交易账户类别"].ToString() + "','" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString() + "','" + dataTable.Rows[0]["经纪人单位用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
            if (dataTable.Rows[0]["经纪人单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString() + "',I_SFZH='',I_SFZSMJ='',I_ZZJGDMZDM='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["经纪人单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人单位银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人单位院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人单位经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["经纪人单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人单位银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人单位院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人单位经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
        }
        else//没有更换平台管理机构
        {
            if (dataTable.Rows[0]["经纪人单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString() + "',I_SFZH='',I_SFZSMJ='',I_ZZJGDMZDM='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["经纪人单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人单位银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人单位院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人单位经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString() + "'";

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["经纪人单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["经纪人单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["经纪人单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["经纪人单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["经纪人单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["经纪人单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["经纪人单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["经纪人单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["经纪人单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["经纪人单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["经纪人单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["经纪人单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["经纪人单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人单位银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人单位平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人单位院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人单位经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人单位业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人单位经纪人角色编号"].ToString() + "'";
            }
            arrayList.Add(strSqlGXDLB);
            //判断当前交易账户的审核状态
            DataTable dataTabeUser = DbHelperSQL.Query("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是' ").Tables[0];
            if (dataTabeUser != null && dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回")
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX='" + dataTable.Rows[0]["经纪人单位登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

        }


        bool tag = DbHelperSQL.ExecSqlTran(arrayList);

        if (tag == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息修改成功！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }

    /// <summary>
    /// 修改【经纪人、个人】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet UpdateJJRGR(DataSet dsreturn, DataTable dataTable)
    {
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString());
 

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion



        ArrayList arrayList = new ArrayList();

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["经纪人个人是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = "";
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["经纪人个人登录邮箱"].ToString(), hashTableVierfy))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }


        if (dataTable.Rows[0]["经纪人个人是否更换平台管理机构"].ToString() == "是")//更换了平台管理机构
        {
            //1、作废关联表
            strSqlZFGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否', SFYX='否' where DLYX='" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "' and JSZHLX='经纪人交易账户'";
            arrayList.Add(strSqlZFGLB);
            //2、获取关联表的Number值
            string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
            //3、插入关联表   
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,JJRSHSJ,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人个人交易账户类别"].ToString() + "','" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "','" + dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString() + "','" + dataTable.Rows[0]["经纪人个人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核通过','" + DateTime.Now.ToString() + "','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
            if (dataTable.Rows[0]["经纪人个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                //登录表 
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["经纪人个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString() + "',I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='',I_JYFLXDH='" + dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人个人银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人个人院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人个人经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["经纪人个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人个人银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人个人院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人个人经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
        }
        else//没有更换平台管理机构
        {
            if (dataTable.Rows[0]["经纪人个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["经纪人个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString() + "',I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='',I_JYFLXDH='" + dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人个人银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人个人院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人个人经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString() + "'";

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["经纪人个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["经纪人个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["经纪人个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["经纪人个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["经纪人个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["经纪人个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["经纪人个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["经纪人个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["经纪人个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["经纪人个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["经纪人个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["经纪人个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["经纪人个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["经纪人个人银行账号"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["经纪人个人平台管理机构"].ToString() + "',I_YXBH='" + dataTable.Rows[0]["经纪人个人院系编号"].ToString() + "',I_JJRFL='" + dataTable.Rows[0]["经纪人个人经纪人分类"].ToString() + "',I_YWGLBMFL='" + dataTable.Rows[0]["经纪人个人业务管理部门分类"].ToString() + "'  where J_JJRJSBH='" + dataTable.Rows[0]["经纪人个人经纪人角色编号"].ToString() + "'";
            }
            arrayList.Add(strSqlGXDLB);
            DataTable dataTabeUser = DbHelperSQL.Query("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是' ").Tables[0];
            if (dataTabeUser != null && dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回")
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX='" + dataTable.Rows[0]["经纪人个人登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }
        }


        bool tag = DbHelperSQL.ExecSqlTran(arrayList);
        if (tag == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息修改成功！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["经纪人个人登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }

    /// <summary>
    /// 修改【买卖家、单位】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet UpdateMMJDW(DataSet dsreturn, DataTable dataTable)
    {
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["买卖家单位卖家角色编号"].ToString());
  

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        ArrayList arrayList = new ArrayList();

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["买卖家单位是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = dataTable.Rows[0]["买卖家单位是否已被经纪人审核通过"].ToString();
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["买卖家单位登录邮箱"].ToString(), hashTableVierfy))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您开通交易账户的申请已经审核通过，可以进行相关业务操作。若有需要，请重新修改资料！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }

        #region//验证是否已经更换经纪人


        string SelJSBH = dataTable.Rows[0]["买卖家单位卖家角色编号"].ToString().Trim();
        string strSqlTwo = "select a.Number from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + SelJSBH + "'  and a.SFDQMRJJR='是'";
        object MMJGLBNumber= DbHelperSQL.GetSingle(strSqlTwo);
        if (MMJGLBNumber != null && MMJGLBNumber.ToString().Trim() != "")
        {
            if (MMJGLBNumber.ToString().Trim() != dataTable.Rows[0]["买卖家单位买家卖家与经纪人关联表Number"].ToString().Trim())
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败，已更换经纪人！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败，已更换经纪人！";
            return dsreturn;
        }

        #endregion

        if (dataTable.Rows[0]["买卖家单位是否更换经纪人资格证书编号"].ToString() == "是")//更换了经纪人资格证书
        {
            //1、作废关联表
            strSqlZFGLB = "update  AAA_MJMJJYZHYJJRZHGLB set  SFDQMRJJR='否',SFYX='否'  where DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是'  ";
            arrayList.Add(strSqlZFGLB);
            //2、获取关联表的Number值
            string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");

            DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家单位经纪人资格证书编号"].ToString());
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                return dsreturn;
            }

            if (dataTable.Rows[0]["买卖家单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString() + "',I_SFZH='',I_SFZSMJ='',I_ZZJGDMZDM='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["买卖家单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家单位银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家单位关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家单位业务服务部门"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["买卖家单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家单位银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "' ,I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家单位关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家单位业务服务部门"].ToString() + "'  where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            //3、插入关联表 
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "','" + dataTable.Rows[0]["买卖家单位交易账户类别"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
        }
        else//没有更换平台管理机构
        {
            DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家单位经纪人资格证书编号"].ToString());
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                return dsreturn;
            }
            if (dataTable.Rows[0]["买卖家单位是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString() + "',I_SFZH='',I_SFZSMJ='',I_ZZJGDMZDM='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["买卖家单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家单位银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家单位关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家单位业务服务部门"].ToString() + "'   where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'";

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家单位交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家单位交易方名称"].ToString() + "',I_YYZZZCH='" + dataTable.Rows[0]["买卖家单位营业执照注册号"].ToString() + "',I_YYZZSMJ='" + dataTable.Rows[0]["买卖家单位营业执照扫描件"].ToString() + "',I_ZZJGDMZDM='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证"].ToString() + "',I_ZZJGDMZSMJ='" + dataTable.Rows[0]["买卖家单位组织机构代码证代码证扫描件"].ToString() + "',I_SWDJZSH='" + dataTable.Rows[0]["买卖家单位税务登记证税号"].ToString() + "',I_SWDJZSMJ='" + dataTable.Rows[0]["买卖家单位税务登记证扫描件"].ToString() + "',I_YBNSRZGZSMJ='" + dataTable.Rows[0]["买卖家单位一般纳税人资格证扫描件"].ToString() + "',I_KHXKZH='" + dataTable.Rows[0]["买卖家单位开户许可证号"].ToString() + "',I_KHXKZSMJ='" + dataTable.Rows[0]["买卖家单位开户许扫描件"].ToString() + "',I_YLYJK='" + dataTable.Rows[0]["买卖家单位预留印鉴卡扫描件"].ToString() + "',I_FDDBRXM='" + dataTable.Rows[0]["买卖家单位法定代表人姓名"].ToString() + "',I_FDDBRSFZH='" + dataTable.Rows[0]["买卖家单位法定代表人身份证号"].ToString() + "',I_FDDBRSFZSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证扫描件"].ToString() + "',I_FDDBRSFZFMSMJ='" + dataTable.Rows[0]["买卖家单位法定代表人身份证反面扫描件"].ToString() + "',I_FDDBRSQS='" + dataTable.Rows[0]["买卖家单位法定代表人授权书"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家单位交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家单位所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家单位所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家单位所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家单位详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家单位联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家单位联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家单位开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家单位银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家单位关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家单位业务服务部门"].ToString() + "'   where B_DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'";
            }
            arrayList.Add(strSqlGXDLB);
            DataTable dataTabeUser = DbHelperSQL.Query("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是' ").Tables[0];
            if (dataTabeUser != null && (dataTabeUser.Rows[0]["经纪人审核状态"].ToString() == "驳回" || dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回"))
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

            DataTable dataTabeBHHSFXG = DbHelperSQL.Query("select  Number,FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["买卖家单位登录邮箱"].ToString() + "'  and SFSCGLJJR='否'  and SFYX='是' ").Tables[0];
            if (dataTabeBHHSFXG.Rows.Count>0)
            {
                for (int i = 0; i < dataTabeBHHSFXG.Rows.Count; i++)
                {
                    if (dataTabeBHHSFXG.Rows[i]["经纪人审核状态"].ToString() == "驳回")
                    {
                        string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where Number='" + dataTabeBHHSFXG.Rows[i]["Number"].ToString()+ "' ";
                        arrayList.Add(strGXGLB);
                    }
                }
            }

            string strUpdateMMJGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='" + dataTable.Rows[0]["买卖家单位银行工作人员工号"].ToString() + "' where Number='" + dataTable.Rows[0]["买卖家单位买家卖家与经纪人关联表Number"].ToString().Trim() + "'";
            arrayList.Add(strUpdateMMJGLB_FZB);


        }
        bool Tag = DbHelperSQL.ExecSqlTran(arrayList);
        if (Tag == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息修改成功！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家单位登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }


    /// <summary>
    /// 修改【买卖家、个人】的交易账户信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet UpdateMMJGR(DataSet dsreturn, DataTable dataTable)
    {
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString());
 

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion

        ArrayList arrayList = new ArrayList();

        string strSqlZFGLB = "";//作废关联表  "是否有效"字段
        string strSqlCRGLB = "";//插入关联表
        string strSqlGXDLB = "";//更新登录表

        Hashtable hashTableVierfy = new Hashtable();
        hashTableVierfy["是否已经被分公司审核通过"] = dataTable.Rows[0]["买卖家个人是否已被分公司审核通过"].ToString();
        hashTableVierfy["是否已经被经纪人审核通过"] = dataTable.Rows[0]["买卖家个人是否已被经纪人审核通过"].ToString();
        if (IsVierifySame("经纪人交易账户", dataTable.Rows[0]["买卖家个人登录邮箱"].ToString(), hashTableVierfy))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您开通交易账户的申请已审核通过，可以进行相关业务操作，若需要请重新修改资料！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        #region//验证是否已经更换经纪人


        string SelJSBH = dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString().Trim();
        string strSqlTwo = "select a.Number from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + SelJSBH + "'  and a.SFDQMRJJR='是'";
        object MMJGLBNumber = DbHelperSQL.GetSingle(strSqlTwo);
        if (MMJGLBNumber != null && MMJGLBNumber.ToString().Trim() != "")
        {
            if (MMJGLBNumber.ToString().Trim() != dataTable.Rows[0]["买卖家个人买家卖家与经纪人关联表Number"].ToString().Trim())
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败，已更换经纪人！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "操作失败，已更换经纪人！";
            return dsreturn;
        }

        #endregion
        if (dataTable.Rows[0]["买卖家个人是否更换经纪人资格证书编号"].ToString() == "是")//更换了经纪人资格证书编号
        {
            //1、作废关联表
            strSqlZFGLB = "update  AAA_MJMJJYZHYJJRZHGLB set  SFDQMRJJR='否',SFYX='否'  where DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是'  ";
            arrayList.Add(strSqlZFGLB);
            //2、获取关联表的Number值
            string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
            //3、插入关联表  
            DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家个人经纪人资格证书编号"].ToString());
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                return dsreturn;
            }
           
            if (dataTable.Rows[0]["买卖家个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                //登录表 
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["买卖家个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString() + "',I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='',I_JYFLXDH='" + dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家个人银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家个人关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家个人业务服务部门"].ToString() + "'  where J_SELJSBH='" + dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["买卖家个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家个人银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家个人关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家个人业务服务部门"].ToString() + "'  where J_SELJSBH='" + dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString() + "'";
                arrayList.Add(strSqlGXDLB);
            }
            strSqlCRGLB = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR)values ('" + strNumber + "','" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "','" + dataTable.Rows[0]["买卖家个人交易账户类别"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人登录邮箱"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人角色编号"].ToString() + "','" + dataGLJJR.Rows[0]["关联经纪人用户名"].ToString() + "','是','" + DateTime.Now.ToString() + "','审核中','审核中','否','是','是')";
            arrayList.Add(strSqlCRGLB);
        }
        else//没有更换经纪人资格证书编号
        {
            DataTable dataGLJJR = GetJJRYHXX(dataTable.Rows[0]["买卖家个人经纪人资格证书编号"].ToString());
            if (dataGLJJR == null || dataGLJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "根据关联的经纪人资格证书编号，无法获取相应的经纪人信息！";
                return dsreturn;
            }
            if (dataTable.Rows[0]["买卖家个人是否更换注册类别"].ToString() == "是")//更换了注册类别
            {
                //4、更新登录表
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["买卖家个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString() + "',I_YYZZZCH='',I_YYZZSMJ='',I_ZZJGDMZDM='',I_ZZJGDMZSMJ='',I_SWDJZSH='',I_SWDJZSMJ='',I_YBNSRZGZSMJ='',I_KHXKZH='',I_KHXKZSMJ='',I_FDDBRXM='',I_FDDBRSFZH='',I_FDDBRSFZSMJ='',I_FDDBRSQS='',I_JYFLXDH='" + dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家个人银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataTable.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "',I_GLYH='" + dataGLJJR.Rows[0]["买卖家个人关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家个人业务服务部门"].ToString() + "'  where J_SELJSBH='" + dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString() + "'";

            }
            else//没有更换注册类别
            {
                strSqlGXDLB = "update AAA_DLZHXXB set I_ZCLB='" + dataTable.Rows[0]["买卖家个人交易注册类别"].ToString() + "',I_JYFMC='" + dataTable.Rows[0]["买卖家个人交易方名称"].ToString() + "',I_SFZH='" + dataTable.Rows[0]["买卖家个人身份证号"].ToString() + "',I_SFZSMJ='" + dataTable.Rows[0]["买卖家个人身份证扫描件"].ToString() + "',I_SFZFMSMJ='" + dataTable.Rows[0]["买卖家个人身份证反面扫描件"].ToString() + "',I_JYFLXDH='" + dataTable.Rows[0]["买卖家个人交易方联系电话"].ToString() + "',I_SSQYS='" + dataTable.Rows[0]["买卖家个人所属省份"].ToString() + "',I_SSQYSHI='" + dataTable.Rows[0]["买卖家个人所属地市"].ToString() + "',I_SSQYQ='" + dataTable.Rows[0]["买卖家个人所属区县"].ToString() + "',I_XXDZ='" + dataTable.Rows[0]["买卖家个人详细地址"].ToString() + "',I_LXRXM='" + dataTable.Rows[0]["买卖家个人联系人姓名"].ToString() + "',I_LXRSJH='" + dataTable.Rows[0]["买卖家个人联系人手机号"].ToString() + "',I_KHYH='" + dataTable.Rows[0]["买卖家个人开户银行"].ToString() + "',I_YHZH='" + dataTable.Rows[0]["买卖家个人银行账号"].ToString() + "',I_YWGLBMFL='" + dataGLJJR.Rows[0]["业务管理部门分类"].ToString() + "',I_PTGLJG='" + dataGLJJR.Rows[0]["平台管理机构"].ToString() + "',I_GLYHGZRYGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "',I_GLYH='" + dataTable.Rows[0]["买卖家个人关联银行"].ToString() + "',I_YWFWBM='" + dataTable.Rows[0]["买卖家个人业务服务部门"].ToString() + "'  where J_SELJSBH='" + dataTable.Rows[0]["买卖家个人卖家角色编号"].ToString() + "'";
            }
            arrayList.Add(strSqlGXDLB);
            DataTable dataTabeUser = DbHelperSQL.Query("select FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是' ").Tables[0];
            if (dataTabeUser != null && (dataTabeUser.Rows[0]["经纪人审核状态"].ToString() == "驳回" || dataTabeUser.Rows[0]["分公司审核状态"].ToString() == "驳回"))
            {
                string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "'  and SFDQMRJJR='是' and SFYX='是'";
                arrayList.Add(strGXGLB);
            }

            DataTable dataTabeBHHSFXG = DbHelperSQL.Query("select  Number,FGSSHZT '分公司审核状态',JJRSHZT '经纪人审核状态' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + dataTable.Rows[0]["买卖家个人登录邮箱"].ToString() + "'  and SFSCGLJJR='否'  and SFYX='是' ").Tables[0];
            if (dataTabeBHHSFXG.Rows.Count > 0)
            {
                for (int i = 0; i < dataTabeBHHSFXG.Rows.Count; i++)
                {
                    if (dataTabeBHHSFXG.Rows[i]["经纪人审核状态"].ToString() == "驳回")
                    {
                        string strGXGLB = "update AAA_MJMJJYZHYJJRZHGLB set SFXG='是',ZHXGSJ='" + DateTime.Now.ToString() + "' where Number='" + dataTabeBHHSFXG.Rows[i]["Number"].ToString() + "' ";
                        arrayList.Add(strGXGLB);
                    }
                }
            }

            string strUpdateMMJGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='" + dataTable.Rows[0]["买卖家个人关联银行工作人员工号"].ToString() + "' where Number='" + dataTable.Rows[0]["买卖家个人买家卖家与经纪人关联表Number"].ToString().Trim() + "'";
            arrayList.Add(strUpdateMMJGLB_FZB);



        }
        bool Tag = DbHelperSQL.ExecSqlTran(arrayList);
        if (Tag == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户信息修改成功！";
            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["买卖家个人登录邮箱"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[1].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[2].TableName = "关联信息";
            return dsreturn;
        }
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户信息提交失败！";
        return dsreturn;
    }


    /// <summary>
    /// 判断当前账户的审核状态和数据库中当前的状态是否一致
    /// </summary>
    /// <param name="strLB">账户类别</param>
    /// <param name="hashTableVerify">当前状态</param>
    /// <returns>不一致为true，一致为false</returns>
    public bool IsVierifySame(string strLB, string strDLYX, Hashtable hashTableVerify)
    {
        DataTable dataTable = DbHelperSQL.Query("select S_SFYBJJRSHTG '是否已经被经纪人审核通过',S_SFYBFGSSHTG '是否已经被分公司审核通过',B_JSZHLX '结算账户类型' from dbo.AAA_DLZHXXB  where  B_DLYX='" + strDLYX + "' ").Tables[0];


        if (strLB == "经纪人交易账户")
        {
            if (hashTableVerify["是否已经被分公司审核通过"].ToString() == "否" && dataTable.Rows[0]["是否已经被分公司审核通过"].ToString() == "是")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else//买家卖家交易账户
        {
            if (hashTableVerify["是否已经被经纪人审核通过"].ToString() == "否" || hashTableVerify["是否已经被分公司审核通过"].ToString() == "否")
            {
                if (dataTable.Rows[0]["是否已经被经纪人审核通过"].ToString() == "是" && dataTable.Rows[0]["是否已经被分公司审核通过"].ToString() == "是")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }






    #endregion





    /// <summary>
    /// 经纪人审核 买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JJRPassMMJ(DataSet dsreturn, DataTable dataTable)
    {
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["GLJJRBH"].ToString());
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
 

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        ArrayList arrayList = new ArrayList();
        if (IsGHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))//当前执行审核操作的经纪人是  选择的经纪人 
        {
            string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号',c.B_SFDJ '是否冻结',c.B_SFXM '是否休眠' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";
            DataTable dataTable_GH = DbHelperSQL.Query(strSqlMRJJR_GH).Tables[0];
            if (dataTable_GH.Rows.Count <= 0 || dataTable_GH.Rows[0]["是否冻结"].ToString() == "是" || dataTable_GH.Rows[0]["是否休眠"].ToString() == "是")//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作，或者//当前更换的默认经纪人已经被冻结或者休眠
            {

                //1、更新原来交易账户的默认经纪人为否 （处理的是第一次关联的经纪人）
                string strSqlFirst = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是' and JJRSHZT='审核通过'";
                arrayList.Add(strSqlFirst);
                //2、更新所有更换的经纪人在关联表中的数据位作废状态（根据【是否首次关联的经纪人】字段来确认）
                string strSqlSecond = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否',SFYX='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核中','驳回') ";
                arrayList.Add(strSqlSecond);
                //2.1更新所有已经关联过的审核通过的经纪人
                string strSqlSecond1 = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核通过') ";
                arrayList.Add(strSqlSecond1);
                //3、跟新当前关联表中Number值的记录为有效状态，并且设为默人经纪人，并且数据是有效状态
                string strSqlThird = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='是',SFYX='是',JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "' where Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
                arrayList.Add(strSqlThird);

                //4、更新当前买卖家账户的“业务服务部门”字段值为：当前关联经纪人的经纪人分类 ；
                string strJJRFL = "select I_JJRFL,I_JYFMC from AAA_DLZHXXB where J_JJRJSBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "'";
                DataSet dsJJRFL = DbHelperSQL.Query(strJJRFL);
                string strUpdateYWFWBM = "";
                string strUpdateGLB_FZB = "";
                if (dsJJRFL != null && dsJJRFL.Tables[0].Rows.Count>0)
                {
                    string jjrfl = dsJJRFL.Tables[0].Rows[0]["I_JJRFL"].ToString();
                    string jjfmc = dsJJRFL.Tables[0].Rows[0]["I_JYFMC"].ToString();
                    if (jjrfl == "银行")
                    {
                        strUpdateYWFWBM = "update AAA_DLZHXXB set I_YWFWBM='" + jjrfl.Trim() + "',I_GLYH='" + jjfmc.Trim() + "',I_GLYHGZRYGH='无' where J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";

                        #region//变更经纪人的时候，往买卖家关联表里插入了数据，此时写入关联表辅助表员工工号的数据是从登陆表里取得，而这时登陆表里员工工号还是原来默认经纪人时的数据，所以在这个节点往买卖家关联表辅助表插入的员工工号是错误的，所以在经纪人审核的的时候把辅助表里员工工号更新成正确的（“即如果审核的经纪人是银行类型的，则员工工号为‘无’；如果审核的经纪人是一般经纪人，则员工工号是空”）

                        strUpdateGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='无' where number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
                        #endregion
                    }
                    else 
                    {
                        strUpdateYWFWBM = "update AAA_DLZHXXB set I_YWFWBM='" + jjrfl.Trim() + "',I_GLYH='',I_GLYHGZRYGH='' where J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
                        #region//变更经纪人的时候，往买卖家关联表里插入了数据，此时写入关联表辅助表员工工号的数据是从登陆表里取得，而这时登陆表里员工工号还是原来默认经纪人时的数据，所以在这个节点往买卖家关联表辅助表插入的员工工号是错误的，所以在经纪人审核的的时候把辅助表里员工工号更新成正确的（“即如果审核的经纪人是银行类型的，则员工工号为‘无’；如果审核的经纪人是一般经纪人，则员工工号是空”）
                        strUpdateGLB_FZB = "update AAA_MJMJJYZHYJJRZHGLB_FZB set GLJJRXSYGGH='' where number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
                        #endregion
                    }
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未查询到当前关联经纪人的信息！";
                    return dsreturn;
                }
                
                arrayList.Add(strUpdateYWFWBM);
                arrayList.Add(strUpdateGLB_FZB);

                if (DbHelperSQL.ExecSqlTran(arrayList))
                {
                    object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
                    Hashtable ht = new Hashtable();
                    ht["type"] = "集合集合经销平台";
                    ht["提醒对象登陆邮箱"] = dataTable.Rows[0]["DLYX"].ToString();
                    ht["提醒对象用户名"] = dataTable.Rows[0]["JYFMC"].ToString();
                    ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                    ht["提醒对象角色编号"] = dataTable.Rows[0]["JSBH"].ToString();
                    ht["提醒对象角色类型"] = "卖家";
                    //ht["提醒内容文本"] = " 尊敬的" + dataTable.Rows[0]["JYFMC"].ToString() + "，您选择的经纪人" + objName.ToString() + "已审核通过，请进行业务操作！";
                    ht["提醒内容文本"] = " 您选择的经纪人" + objName.ToString() + "申请审核通过，请进行其它业务操作！";
                    ht["创建人"] = objName;
                    List<Hashtable> list = new List<Hashtable>();
                    list.Add(ht);
                    PublicClass2013.Sendmes(list);

                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交成功！";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交失败！";
                }
                return dsreturn;
            }
            //else if (dataTable_GH.Rows[0]["是否冻结"].ToString() == "是" || dataTable_GH.Rows[0]["是否休眠"].ToString() == "是")//当前更换的默认经纪人已经被冻结或者休眠
            //{
            //    //1、更新原来交易账户的默认经纪人为否 （更新更换的默认经纪人为否）
            //    string strSqlFirst = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and JJRSHZT='审核通过'";
            //    arrayList.Add(strSqlFirst);
            //    //2、更新所有更换的经纪人在关联表中的数据位作废状态（根据【是否首次关联的经纪人】字段来确认）
            //    string strSqlSecond = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否',SFYX='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核中','驳回') ";
            //    arrayList.Add(strSqlSecond);
            //    //3、跟新当前关联表中Number值的记录为有效状态，并且设为默人经纪人，并且数据是有效状态
            //    string strSqlThird = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='是',SFYX='是',JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "' where Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
            //    arrayList.Add(strSqlThird);
            //}
            else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过，无需审核！";
                return dsreturn;
            }
            else if (dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被审核通过
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已审核通过该交易账户，无需审核！";
                return dsreturn;
            }
        }
        else  //当前的经纪人是首次关联的经纪人
        {
            if (IsMMJSHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                return dsreturn;
            }
            else if (IsGHZCLB(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString(), dataTable.Rows[0]["注册类别"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "Reject";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换注册类别，请重新审核！";
                return dsreturn;
            }
            else if (IsJJRPassMMJ(dataTable.Rows[0]["DLYX"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能再次进行审核操作！";
                return dsreturn;
            }
            else
            {
                string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='是'";

                object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
                object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
                string strSql1 = "";
                string strSql2 = "";

                strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG='是' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  ";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是'  ";
                arrayList.Add(strSql1);
                arrayList.Add(strSql2);
            }
        }
        #region
        /*
        if (IsMMJSHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人！";
            return dsreturn;
        }
        else if (IsJJRPassMMJ(dataTable.Rows[0]["DLYX"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能再次进行审核操作！";
            return dsreturn;
        }
        else
        {
            string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='是'";

            object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
            object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
            string strSql1 = "";
            string strSql2 = "";
         
            if (dataTable.Rows[0]["GLJJRBH"].ToString().Equals(objJSBH.ToString()))//第一次的经纪人   
            {

                strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG='是' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  ";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是'  ";
                arrayList.Add(strSql1);
                arrayList.Add(strSql2);
            }
            else //更换的经纪人
            {
                string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";
              DataTable dataTable_GH=DbHelperSQL.Query(strSqlMRJJR_GH).Tables[0];
              if (dataTable_GH.Rows.Count <= 0)//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作
              {
             
                //1、更新原来交易账户的默认经纪人为否 （处理的是第一次关联的经纪人）
                  string strSqlFirst = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='是' and JJRSHZT='审核通过'";
                  arrayList.Add(strSqlFirst);
                 //2、更新所有更换的经纪人在关联表中的数据位作废状态（根据【是否首次关联的经纪人】字段来确认）
                  string strSqlSecond = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='否',SFYX='否' where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and JSZHLX='买家卖家交易账户' and SFSCGLJJR='否' and  JJRSHZT in('审核中','驳回') ";
                  arrayList.Add(strSqlSecond);
                 //3、跟新当前关联表中Number值的记录为有效状态，并且设为默人经纪人，并且数据是有效状态
                  string strSqlThird = "update AAA_MJMJJYZHYJJRZHGLB set SFDQMRJJR='是',SFYX='是',JJRSHZT='审核通过',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "' where Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
                  arrayList.Add(strSqlThird);
              }
              else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
              {
                  dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                  dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过！";
                  return dsreturn;
             }
            }
         }
            */
        #endregion
        if (DbHelperSQL.ExecSqlTran(arrayList))//审核通过
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交失败！";

        }

        return dsreturn;


    }

    /// <summary>
    /// 经纪人驳回  买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JJRRejectMMJ(DataSet dsreturn, DataTable dataTable)
    {

        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(dataTable.Rows[0]["GLJJRBH"].ToString());
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
 

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        ArrayList arrayList = new ArrayList();
        if (IsGHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))//当前执行审核操作的经纪人是  选择的经纪人 
        {
            string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号',c.B_SFDJ '是否冻结',c.B_SFXM '是否休眠' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";
            DataTable dataTable_GH = DbHelperSQL.Query(strSqlMRJJR_GH).Tables[0];
            string strSql1 = "";
            if (dataTable_GH.Rows.Count <= 0)//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作
            {
                strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
            }
            else if (dataTable_GH.Rows[0]["是否冻结"].ToString() == "是" || dataTable_GH.Rows[0]["是否休眠"].ToString() == "是")//当前更换的默认经纪人已经被冻结或者休眠
            {
                strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否',JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'  ";
            }
            else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过，无需审核！";
                return dsreturn;
            }
            else if (dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被审核通过
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已审核通过该交易账户，无需审核！";
                return dsreturn;
            }

            arrayList.Add(strSql1);
            if (DbHelperSQL.ExecSqlTran(arrayList))
            {
                object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
                Hashtable ht = new Hashtable();
                ht["type"] = "集合集合经销平台";
                ht["提醒对象登陆邮箱"] = dataTable.Rows[0]["DLYX"].ToString();
                ht["提醒对象用户名"] = dataTable.Rows[0]["JYFMC"].ToString();
                ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                ht["提醒对象角色编号"] = dataTable.Rows[0]["JSBH"].ToString();
                ht["提醒对象角色类型"] = "卖家";
                // ht["提醒内容文本"] = " 尊敬的" + dataTable.Rows[0]["JYFMC"].ToString() + "，您未能通过选择的经纪人" + objName + "的审核，请与经纪人联系！";
                ht["提醒内容文本"] = " 您选择的经纪人" + objName.ToString() + "申请审核未通过，请您进入“选择经纪人”模块重新选择！";
                ht["创建人"] = objName;
                List<Hashtable> list = new List<Hashtable>();
                list.Add(ht);
                PublicClass2013.Sendmes(list);

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }
            return dsreturn;


        }
        else   //当前的经纪人是首次关联的经纪人
        {
            if (IsMMJSHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                return dsreturn;
            }
            else if (IsGHZCLB(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString(), dataTable.Rows[0]["注册类别"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "Reject";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换注册类别，请重新审核！";
                return dsreturn;
            }
            else if (IsJJRPassMMJ(dataTable.Rows[0]["DLYX"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能进行驳回操作！";
                return dsreturn;
            }
            else
            {
                string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' ";

                object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
                object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
                string strSql1 = "";
                string strSql2 = "";
                strSql1 = "update AAA_DLZHXXB set S_SFYBJJRSHTG='否' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回',SFXG='否' ,JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是' ";
                arrayList.Add(strSql1);
                arrayList.Add(strSql2);

                // 自身开户申请	[x1]被经纪人审核时驳回	-0.5	减项	经纪人审核时    
                jhjx_JYFXYMX jhjxFYFXMYX = new jhjx_JYFXYMX();
                string[] strArray = jhjxFYFXMYX.ZSKHSQ_BJJRSHSBH(dataTable.Rows[0]["DLYX"].ToString(), "卖家", dataTable.Rows[0]["GLJJRBH"].ToString());
                arrayList.AddRange(strArray);



            }
            if (DbHelperSQL.ExecSqlTran(arrayList))
            {
                object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
                Hashtable ht = new Hashtable();
                ht["type"] = "集合集合经销平台";
                ht["提醒对象登陆邮箱"] = dataTable.Rows[0]["DLYX"].ToString();
                ht["提醒对象用户名"] = dataTable.Rows[0]["JYFMC"].ToString();
                ht["提醒对象结算账户类型"] = "买家卖家交易账户";
                ht["提醒对象角色编号"] = dataTable.Rows[0]["JSBH"].ToString();
                ht["提醒对象角色类型"] = "卖家";
                //ht["提醒内容文本"] = " 尊敬的" + dataTable.Rows[0]["JYFMC"].ToString() + "，您提交的开通交易账户的资料未能通过经纪人的审核，请与您的经纪人联系！";
                ht["提醒内容文本"] = " 您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请。";
                ht["创建人"] = objName;
                List<Hashtable> list = new List<Hashtable>();
                list.Add(ht);
                PublicClass2013.Sendmes(list);

                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交成功！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的审核信息提交失败！";
            }
            return dsreturn;

        }
        #region
        /*
        if (IsMMJSHJJR(dataTable.Rows[0]["JSBH"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人！";
            return dsreturn;
        }
        else if (IsJJRPassMMJ(dataTable.Rows[0]["DLYX"].ToString(), dataTable.Rows[0]["GLJJRBH"].ToString()))
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经审核通过该交易账户，不能进行驳回操作！";
            return dsreturn;
        }
        else
        {
            string strSqlMRJJR = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' ";

            object objJSBH = DbHelperSQL.GetSingle(strSqlMRJJR);
            object objName = dataTable.Rows[0]["当前经纪人名称"].ToString();
            string strSql1 = "";
            string strSql2 = "";
            if (dataTable.Rows[0]["GLJJRBH"].ToString().Equals(objJSBH.ToString()))//第一次的经纪人
            {
                strSql1 = "update AAA_DLZHXXB set S_SFYBFGSSHTG='否' where B_JSZHLX='买家卖家交易账户' and  J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
                strSql2 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回', JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是' ";
                arrayList.Add(strSql1);
                arrayList.Add(strSql2);
            }
            else //更换的经纪人
            {
                string strSqlMRJJR_GH = "select a.GLJJRBH '关联经纪人编号' from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.SFDQMRJJR='是' and a.SFYX='是' and a.SFSCGLJJR='否'";
                DataTable dataTable_GH = DbHelperSQL.Query(strSqlMRJJR_GH).Tables[0];
                if (dataTable_GH.Rows.Count <= 0)//该买卖家账户关联的所的经纪人账户尚未对该账户进行审核操作，或者有些经济人对此账户执行了驳回操作
                {
                    strSql1 = "update AAA_MJMJJYZHYJJRZHGLB set JJRSHZT='驳回', JJRSHSJ='" + DateTime.Now.ToString() + "',JJRSHYJ='" + dataTable.Rows[0]["SHYJ"].ToString() + "'    where JSZHLX='买家卖家交易账户' and DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and  GLJJRBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "' and SFYX='是' and SFSCGLJJR='否'  ";
                }
                else if (!dataTable_GH.Rows[0]["关联经纪人编号"].ToString().Equals(dataTable.Rows[0]["GLJJRBH"].ToString()))//该买卖家交易账户已被其他经纪人审核通过
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "warr";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此交易账户已被其它经纪人审核通过！";
                    return dsreturn;
                }
             
                arrayList.Add(strSql1);
            }
           */
        #endregion




    }

    /// <summary>
    /// 买卖家是否更换经纪人
    /// </summary>
    /// <returns></returns>
    public bool IsMMJSHJJR(string strSelerJSBH, string strGLJJRBH)
    {
        string strSql = "select * from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH='" + strGLJJRBH + "' and b.J_SELJSBH='" + strSelerJSBH + "' and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 判断当前买卖家交易账户 是否更换了注册类别
    /// </summary>
    /// <param name="strSelerJSBH">卖家角色编号</param>
    /// <param name="strGLJJRBH">关联经纪人角色编号</param>
    /// <param name="strSqlZCLB">当前注册类别</param>
    /// <returns>false 没有更换注册类别，true 已经更换了注册类别</returns>
    public bool IsGHZCLB(string strSelerJSBH, string strGLJJRBH, string strSqlZCLB)
    {
        string strSql = "select b.I_ZCLB '注册类别',* from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH='" + strGLJJRBH + "' and b.J_SELJSBH='" + strSelerJSBH + "' and a.SFSCGLJJR='是' and a.SFDQMRJJR='是'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];

        if (dataTable.Rows[0]["注册类别"].ToString() == strSqlZCLB)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 判断当前交易账户是否选择更换了经纪人
    /// </summary>
    /// <param name="strSelerJSBH"></param>
    /// <param name="strGLJJRBH"></param>
    /// <returns>true 更换了 false 未更换</returns>
    public bool IsGHJJR(string strSelerJSBH, string strGLJJRBH)
    {
        string strSql = "select * from  AAA_MJMJJYZHYJJRZHGLB  as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where  b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.GLJJRBH='" + strGLJJRBH + "' and b.J_SELJSBH='" + strSelerJSBH + "' and a.SFSCGLJJR='否' and a.FGSSHZT='审核通过'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }

    }





    /// <summary>
    /// 判断当前经纪人是否已经把当前买卖家审核通过
    /// </summary>
    /// <param name="strMMJDLYX"></param>
    /// <param name="strGLJJRBH"></param>
    /// <returns></returns>
    public bool IsJJRPassMMJ(string strMMJDLYX, string strGLJJRBH)
    {
        string strSql = "select * from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + strMMJDLYX + "' and GLJJRBH='" + strGLJJRBH + "' and SFYX='是' and JJRSHZT='审核通过' and SFSCGLJJR='是' and SFDQMRJJR='是'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 得到买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetMMJData(DataSet dsreturn, DataTable dataTable)
    {
        #region//zhouli 作废 2014.08.06 变更新的SQL，去掉where语句中a.SFYX='是'条件，增加查询字段 a.SFYX，原因：验证是否变更经纪人
        //string strSql = "select b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case JJRSHZT when  '审核中' then '--' else CONVERT(varchar(10),ZHXGSJ,120) end) '驳回修改时间'   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";
        #endregion

        string strSql = "select  a.SFYX,b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',(case JJRSHZT when  '审核中' then '--' else CONVERT(varchar(10),ZHXGSJ,120) end) '驳回修改时间'   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "' and a.Number='" + dataTable.Rows[0]["关联表Number"].ToString() + "'";

        DataTable dataTableONE = DbHelperSQL.Query(strSql).Tables[0];
        string strSqlTwo = "select J_JJRZGZSBH,JJRZGZS,I_JYFMC,I_JYFLXDH from AAA_DLZHXXB  where J_JJRJSBH='" + dataTable.Rows[0]["GLJJRBH"].ToString() + "'";
        DataTable dataTableTWO = DbHelperSQL.Query(strSqlTwo).Tables[0];
        if (dataTableONE != null && dataTableONE.Rows.Count > 0 && dataTableTWO != null && dataTableTWO.Rows.Count > 0)
        {

            #region//zhouli 2014.08.06 add 验证是否变更经纪人
            if (dataTableONE.Rows[0]["SFYX"].ToString().Trim() != "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前交易账户已更换经纪人，无需审核！";
                return dsreturn;
            }
            #endregion

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "买家卖家交易账户基本信息";
            DataTable dataTableTWOCopy = dataTableTWO.Copy();
            dsreturn.Tables.Add(dataTableTWOCopy);
            dsreturn.Tables[2].TableName = "买家卖家关联经纪人账户基本信息";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取失败！";
        }
        return dsreturn;
    }




    /// <summary>
    /// 得到买卖家的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetMMJZHZLData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "select b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',I_YXBH,I_JJRFL,I_YWGLBMFL,I_GLYH,I_GLYHGZRYGH,I_YWFWBM  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and a.SFSCGLJJR='是'";
        DataTable dataTableONE = DbHelperSQL.Query(strSql).Tables[0];
        string strSqlTwo = "select a.Number,c.B_DLYX, c.J_JJRZGZSBH,c.JJRZGZS,c.I_JYFMC,c.I_JYFLXDH,c.J_JJRJSBH,c.I_YXBH,c.I_JJRFL,c.I_YWGLBMFL from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where b.B_JSZHLX='买家卖家交易账户' and b.J_SELJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'  and a.SFDQMRJJR='是'";



        DataTable dataTableTWO = DbHelperSQL.Query(strSqlTwo).Tables[0];
        if (dataTableONE != null && dataTableONE.Rows.Count > 0 && dataTableTWO != null && dataTableTWO.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "买家卖家交易账户基本信息";
            DataTable dataTableTWOCopy = dataTableTWO.Copy();
            dsreturn.Tables.Add(dataTableTWOCopy);
            dsreturn.Tables[2].TableName = "买家卖家关联经纪人账户基本信息";

            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["DLYX"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[3].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[4].TableName = "关联信息";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "买卖家的数据信息获取失败！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 得到经纪人的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetJJRData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "select b.Number '登录_Number',B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),a.CreateTime,120) '资料提交时间',I_YXBH,I_JJRFL,I_YWGLBMFL   from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='经纪人交易账户' and a.SFYX='是' and b.J_JJRJSBH='" + dataTable.Rows[0]["JSBH"].ToString() + "'";
        DataTable dataTableONE = DbHelperSQL.Query(strSql).Tables[0];

        if (dataTableONE != null && dataTableONE.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人的数据信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "经纪人交易账户基本信息";


            DataSet dataSet = checkLoginIn(dataTable.Rows[0]["DLYX"].ToString());
            DataTable dataTableYHXX = dataSet.Tables["用户信息"];
            DataTable dataTableGLXX = dataSet.Tables["关联信息"];
            DataTable dataTableYHXX_Copy = dataTableYHXX.Copy();
            DataTable dataTableGLXX_Copy = dataTableGLXX.Copy();
            dsreturn.Tables.Add(dataTableYHXX_Copy);
            dsreturn.Tables[2].TableName = "用户信息";
            dsreturn.Tables.Add(dataTableGLXX_Copy);
            dsreturn.Tables[3].TableName = "关联信息";



        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人的数据信息获取失败！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 通过经纪人资格证书，得到经纪人用户信息
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <returns></returns>
    public DataSet GetJJRYHXX(DataSet dsreturn, string strJJRZGZ)
    {

        string strSql = "select I_JYFMC '经纪人名称',I_LXRSJH '经纪人联系电话',S_SFYBFGSSHTG as '分公司开通审核状态',B_JSZHLX '角色类型',J_JJRSFZTXYHSH '经纪人暂停接受新用户',B_SFYXDL '是否允许登陆',B_SFDJ '是否冻结账号',B_SFXM '是否休眠',CONVERT(varchar(10),J_JJRZGZSYXQKSSJ,120) '经纪人资格证书有效期开始时间',convert(varchar(10),J_JJRZGZSYXQJSSJ,120) '经纪人资格证书有效期结束时间' ,I_JJRFL 经纪人分类 from AAA_DLZHXXB  where J_JJRZGZSBH='" + strJJRZGZ + "' and I_JJRFL='一般经纪人'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取经纪人用户信息成功！";
        DataTable dataTableCpoy = dataTable.Copy();
        dsreturn.Tables.Add(dataTableCpoy);
        dsreturn.Tables[1].TableName = "经纪人用户信息";
        return dsreturn;
    }

    /// <summary>
    /// 判断身份证号是否重复
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <returns></returns>
    public DataSet JudgeSFZHXX(DataSet dsreturn, string strZHLX, string strSFZH)
    {
        string strSql = "";
        switch (strZHLX)
        {
            case "个人身份证号":
                strSql = " select * from dbo.AAA_DLZHXXB where I_SFZH='" + strSFZH + "'  or I_FDDBRSFZH='" + strSFZH + "' ";
                break;
            case "法定代表人身份证号":
                strSql = " select * from dbo.AAA_DLZHXXB where I_FDDBRSFZH='" + strSFZH + "'  or I_SFZH='" + strSFZH + "' ";
                break;
        }
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTable.Rows.Count == 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此身份证可以注册";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "此身份证已经被注册";
        }
        return dsreturn;
    }





    /// <summary>
    ///根据买卖家登录邮箱、获取关联表中有关买卖家的数据信息
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <returns></returns>
    public DataSet GetMMJJYZHData(DataSet dsreturn, DataTable dtInfor)
    {

        string strSql = "select a.DLYX '登录邮箱',b.I_JYFMC '交易方名称', b.B_JSZHLX '结算账户类型' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where DLYX='" + dtInfor.Rows[0]["买卖家登录邮箱"].ToString() + "' and SFYX='是' and SFDQMRJJR='是' and GLJJRBH='" + dtInfor.Rows[0]["关联经纪人角色编号"].ToString() + "'   and JJRSHZT='审核通过' and FGSSHZT='审核通过'  and b.B_JSZHLX='买家卖家交易账户'";
        DataTable dataTable = DbHelperSQL.Query(strSql).Tables[0];

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取买卖家数据信息！";
        DataTable dataTableCpoy = dataTable.Copy();
        dsreturn.Tables.Add(dataTableCpoy);
        dsreturn.Tables[1].TableName = "买卖家数据信息";
        return dsreturn;
    }

    /// <summary>
    /// 暂停恢复用户的新业务  仅用于买卖家交易账户
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dtInfor"></param>
    /// <returns></returns>
    public DataSet SetZTHFYHXYW(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string MMJDLYX = dataTable.Rows[0]["买卖家登录邮箱"].ToString();//买卖家登录邮箱
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型
        string CZLX = dataTable.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复
        string strLY = dataTable.Rows[0]["理由"].ToString();//理由信息
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }

        DataSet dsDJXX = jhjx_yhdongjie.getYHdjinfo(DLYX, "经纪人暂停用户新业务");
        if (dsDJXX.Tables[0].Rows[0]["执行结果"].ToString() == "err")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "未找到该交易账户的用户信息，请与平台服务人员联系！";
            return dsreturn;
        }
        else if (dsDJXX.Tables[0].Rows[0]["执行结果"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r" + dsDJXX.Tables[0].Rows[0]["提示文本"].ToString();
            return dsreturn;
        }


        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        DataTable dataTableInfor = DbHelperSQL.Query("select SFZTYHXYW '是否暂停用户新业务' from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是' and JSZHLX='买家卖家交易账户' and JJRSHZT='审核通过' and FGSSHZT='审核通过'").Tables[0];
        if (dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "是" && CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该用户的新业务处于暂停状态，请不要再执行暂停操作！";
                return dsreturn;
            }
            else if (dataTableInfor.Rows[0]["是否暂停用户新业务"].ToString() == "否" && CZLX == "恢复")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该用户的新业务处于恢复状态，请不要再执行恢复操作！";
                return dsreturn;
            }

        }

        Object obj = DbHelperSQL.GetSingle("select ZTYHXYWBZ from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是' ");
        string strSourceValue = "";//原始值
        if (obj == null)
        {
            strSourceValue = "";
        }
        else
        {
            strSourceValue = obj.ToString();
        }

        ArrayList arrayList = new ArrayList();
        if (CZLX == "暂停")
        {
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='是', ZTXYWSJ='" + DateTime.Now.ToString() + "'   where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停该用户新业务 操作理由：" + strLY + "◆";
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ='" + strCZ + "' where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }
        else
        {
            strSql = "update  AAA_MJMJJYZHYJJRZHGLB set  SFZTYHXYW='否',ZTXYWSJ=null   where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是'  and JJRSHZT='审核通过' and FGSSHZT='审核通过'";
            arrayList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复该用户新业务  操作理由：" + strLY + "◆";
            string strCZValue = "update AAA_MJMJJYZHYJJRZHGLB set ZTYHXYWBZ='" + strCZ + "' where DLYX='" + MMJDLYX + "' and GLJJRBH='" + JSBH + "' and SFYX='是' and SFDQMRJJR='是' ";
            arrayList.Add(strCZValue);
        }
        bool boolTag = DbHelperSQL.ExecSqlTran(arrayList);

        if (boolTag)
        {
            if (CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功暂停该用户的新业务！";

            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功恢复该用户的新业务！";
            }

        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前操作失败！";

        }

        return dsreturn;



    }



    /// <summary>
    /// 获取更换经纪人时获取的经纪人交易账户数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetSiftJJRData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string JSBH = dataTable.Rows[0]["卖家角色编号"].ToString();//卖家角色编号
        string MMJDLYX = dataTable.Rows[0]["买卖家登录邮箱"].ToString();//买卖家登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型 内容为 买家卖家交易账户
        string strWhere = dataTable.Rows[0]["搜索经纪人资格证书编号"].ToString();//搜所经济人角色编号内容
        #region//基础验证
        DataTable dataTableBasic=DbHelperSQL.Query("select b.Number '登录_Number', B_DLYX,B_YHM,B_JSZHLX,J_SELJSBH,J_BUYJSBH,J_JJRJSBH,J_JJRSFZTXYHSH,J_JJRZGZSBH,JJRZGZS,J_JRZTXYHBZ,S_SFYBJJRSHTG,S_SFYBFGSSHTG,I_ZCLB,I_JYFMC,I_YYZZZCH,I_YYZZSMJ,I_SFZH,I_SFZSMJ,I_SFZFMSMJ,I_ZZJGDMZDM,I_ZZJGDMZSMJ,I_SWDJZSH,I_SWDJZSMJ,I_YBNSRZGZSMJ,I_KHXKZH,I_KHXKZSMJ,I_YLYJK,I_FDDBRXM,I_FDDBRSFZH,I_FDDBRSFZSMJ,I_FDDBRSFZFMSMJ,I_FDDBRSQS,I_JYFLXDH,I_SSQYS,I_SSQYSHI,I_SSQYQ,I_XXDZ,I_LXRXM,I_LXRSJH,I_KHYH,I_YHZH,I_PTGLJG,a.Number '关联_Number',GLJJRBH,JJRSHZT,JJRSHSJ,JJRSHYJ,FGSSHZT,FGSSHR,FGSSHSJ,FGSSHYJ,convert(varchar(10),b.I_ZLTJSJ,120) '资料提交时间',I_YXBH,I_JJRFL,I_YWGLBMFL  from AAA_MJMJJYZHYJJRZHGLB as a inner join  AAA_DLZHXXB as b on a.DLYX=b.B_DLYX   where b.B_JSZHLX='买家卖家交易账户' and a.SFYX='是' and a.SFSCGLJJR='是' and b.J_SELJSBH='" + JSBH + "'  ").Tables[0];
        if (dataTable.Rows.Count > 0)
        {
            if (dataTableBasic.Rows[0]["JJRSHZT"].ToString() == "驳回" || dataTableBasic.Rows[0]["FGSSHZT"].ToString()=="驳回")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的开户申请审核未通过，请您进入“账户维护”界面查询详情并重新提交申请。";
                return dsreturn;
            
            }
        }
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }
        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
      
        #endregion

        //查询当前经纪人的状态信息
        string strSqlDQJJRZT = "select c.I_PTGLJG '经纪人_平台管理机构',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',c.I_JYFMC '经纪人交易方名称',b.I_SSQYS '买卖家_所属区域省',b.I_SSQYSHI '买卖家_所属区域市',b.I_SSQYQ '买卖家_所属区域区'  from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where a.JSZHLX='" + JSZHLX + "' and a.SFDQMRJJR='是'  and a.SFYX='是' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.DLYX='" + MMJDLYX + "'";
        DataTable dataTableDQJJRZT = DbHelperSQL.Query(strSqlDQJJRZT).Tables[0];
        DataTable dataTableXZJJR = null;
        if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count > 0)
        {
            string strSqlAll = "";



            //选择与当前关联经纪人所属分公司相同的经纪人
            if (strWhere != "")
            {
                //strSqlAll = "select top 15 * from ( select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and b.I_PTGLJG='" + dataTableDQJJRZT.Rows[0]["经纪人_平台管理机构"].ToString() + "' and b.B_SFDJ='否' and b.B_SFXM='否' and b.B_SFYXDL='是' and b.J_JJRSFZTXYHSH='否' and b.J_JJRZGZSBH='" + strWhere + "'  ) as tab  order by case 经纪人_所属区域省市 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + dataTableDQJJRZT.Rows[0]["买卖家_所属区域市"].ToString() + "' then 1  else 3 end asc,case 经纪人_所属区域省 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + "' then 1  else 3 end asc,分公司审核时间 asc";
                strSqlAll = "select  * from ( select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间',a.FGSSHZT '分公司审核状态',b.B_SFDJ '是否冻结', b.B_SFXM '是否休眠',b.B_SFYXDL '是否允许登录',b.J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.SFYX='是' and b.J_JJRZGZSBH='" + strWhere + "'  ) as tab";
            }
            else
            {
                strSqlAll = "select top 15 * from ( select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and b.I_PTGLJG='" + dataTableDQJJRZT.Rows[0]["经纪人_平台管理机构"].ToString() + "' and b.B_SFDJ='否' and b.B_SFXM='否' and b.B_SFYXDL='是' and b.J_JJRSFZTXYHSH='否'   ) as tab  order by case 经纪人_所属区域省市 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + dataTableDQJJRZT.Rows[0]["买卖家_所属区域市"].ToString() + "' then 1  else 3 end asc,case 经纪人_所属区域省 when '" + dataTableDQJJRZT.Rows[0]["买卖家_所属区域省"].ToString() + "' then 1  else 3 end asc,分公司审核时间 asc";
            }




            dataTableXZJJR = DbHelperSQL.Query(strSqlAll).Tables[0];
        }
        DataTable dataTableGLGJJR = DbHelperSQL.Query("select a.DLYX '买卖家登录邮箱',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人交易方名称',c.B_DLYX '经纪人登录邮箱',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',a.JJRSHZT '经纪人审核状态',a.JJRSHSJ '经纪人审核时间',a.JJRSHYJ '经纪人审核意见',a.SQSJ '申请时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where a.JSZHLX='买家卖家交易账户' and a.SFYX='是' and  a.DLYX='" + MMJDLYX + "' and a.SFSCGLJJR='否' and a.JJRSHZT='审核通过' and a.SFDQMRJJR='是' order by a.SQSJ desc").Tables[0];
        if (strWhere != "" && dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableGLGJJR != null)//点击搜索按钮时的信息处理
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okErr";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "";
            string strTag = "";
            if (dataTableXZJJR.Rows.Count <= 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请填写正确的经纪人资格证书编号！";
            }
            else if (!dataTableXZJJR.Rows[0]["平台管理机构"].ToString().Equals(dataTableDQJJRZT.Rows[0]["经纪人_平台管理机构"].ToString()))
            {
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "请选择与您同一个业务管理部门的经纪人！";
            }
            else
            {

                List<string> listStrArray = new List<string>();

                if (dataTableXZJJR.Rows[0]["是否冻结"].ToString() == "是")
                {
                    listStrArray.Add("冻结状态");
                }
                if (dataTableXZJJR.Rows[0]["是否休眠"].ToString() == "是")
                {
                    listStrArray.Add("休眠状态");
                }
                if (dataTableXZJJR.Rows[0]["是否允许登录"].ToString() == "否")
                {
                    listStrArray.Add("不允许登录状态");
                }
                if (dataTableXZJJR.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是")
                {
                    listStrArray.Add("暂停新用户审核状态");
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：暂停新用户审核状态，请重新选择！";
                }
                if (listStrArray.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：" +string.Join("|",listStrArray.ToArray()).ToString()+ "，请重新选择！";
                }
                else
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取可选经纪人数据信息！";
                }
            }



            DataTable dataTableDQJJRZTCopy = dataTableDQJJRZT.Copy();
            dsreturn.Tables.Add(dataTableDQJJRZTCopy);
            dsreturn.Tables[1].TableName = "买卖家关联的当前经纪人信息";
            DataTable dataTableXZJJRCopy = dataTableXZJJR.Copy();
            dsreturn.Tables.Add(dataTableXZJJRCopy);
            dsreturn.Tables[2].TableName = "买卖家可选经纪人信息";
            DataTable dataTableGLGJJRCopy = dataTableGLGJJR.Copy();
            dsreturn.Tables.Add(dataTableGLGJJRCopy);
            dsreturn.Tables[3].TableName = "买卖家关联过的经纪人信息";

        }
        else if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableXZJJR != null && dataTableGLGJJR != null)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取可选经纪人数据信息！";
            DataTable dataTableDQJJRZTCopy = dataTableDQJJRZT.Copy();
            dsreturn.Tables.Add(dataTableDQJJRZTCopy);
            dsreturn.Tables[1].TableName = "买卖家关联的当前经纪人信息";
            DataTable dataTableXZJJRCopy = dataTableXZJJR.Copy();
            dsreturn.Tables.Add(dataTableXZJJRCopy);
            dsreturn.Tables[2].TableName = "买卖家可选经纪人信息";
            DataTable dataTableGLGJJRCopy = dataTableGLGJJR.Copy();
            dsreturn.Tables.Add(dataTableGLGJJRCopy);
            dsreturn.Tables[3].TableName = "买卖家关联过的经纪人信息";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取可选经纪人数据信息失败！";

        }
        return dsreturn;
    }

    /// <summary>
    /// 当选择经纪人时获取要判断的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetSelectJudgeJJRData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string JSBH = dataTable.Rows[0]["卖家角色编号"].ToString();//卖家角色编号
        string MMJDLYX = dataTable.Rows[0]["买卖家登录邮箱"].ToString();//买卖家登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型 内容为 买家卖家交易账户
        string strWhere = dataTable.Rows[0]["搜索经纪人资格证书编号"].ToString();//搜所经济人角色编号内容
        string strGLJJRDLYX = dataTable.Rows[0]["关联经纪人登录邮箱"].ToString();//关联经纪人登录邮箱
        string strGLJJRJSBH = dataTable.Rows[0]["关联经纪人角色编号"].ToString();//关联经纪人角色编号
        string strGLJJRYHM = dataTable.Rows[0]["关联经纪人用户名"].ToString();//关联经纪人用户名
        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
 
        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }
        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion

        //查询当前经纪人的状态信息
        string strSqlDQJJRZT = "select c.I_PTGLJG '经纪人_平台管理机构',c.B_SFDJ '经纪人_冻结状态',c.B_SFXM '经纪人_休眠状态',b.I_SSQYS '买卖家_所属区域省',b.I_SSQYSHI '买卖家_所属区域市',b.I_SSQYQ '买卖家_所属区域区'  from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH   where a.JSZHLX='" + JSZHLX + "' and a.SFDQMRJJR='是'  and a.SFYX='是' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.DLYX='" + MMJDLYX + "'";
        DataTable dataTableDQJJRZT = DbHelperSQL.Query(strSqlDQJJRZT).Tables[0];
        string strSqlJJRSHZT = "select a.DLYX '买卖家登录邮箱',c.J_JJRZGZSBH '经纪人资格证书编号',c.I_JYFMC '经纪人交易方名称',c.B_DLYX '经纪人登录邮箱',a.JJRSHZT '经纪人审核状态',a.JJRSHSJ '经纪人审核时间',a.JJRSHYJ '经纪人审核意见',a.SQSJ '申请时间' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX inner join AAA_DLZHXXB as c on a.GLJJRBH=c.J_JJRJSBH where a.JSZHLX='买家卖家交易账户' and a.SFYX='是' and  a.DLYX='" + MMJDLYX + "' and a.SFSCGLJJR='否' and c.B_SFDJ='否' and c.B_SFXM='否' and a.JJRSHZT='审核通过' and a.SFDQMRJJR='是' order by a.SQSJ desc";
        DataTable dataTableGLGJJR = DbHelperSQL.Query(strSqlJJRSHZT).Tables[0];

        //查询当前关联的经纪人的记录信息，用于防止重复选择单个经纪人
        string strSqlFZCFXZJJR = "select * from AAA_MJMJJYZHYJJRZHGLB where DLYX='" + MMJDLYX + "' and GLJJRBH='" + strGLJJRJSBH + "' and SFDQMRJJR='否' and SFSCGLJJR='否' and SFYX='是' and JJRSHZT in('审核中','驳回')";

        DataTable dataFZCFXZJJR = DbHelperSQL.Query(strSqlFZCFXZJJR).Tables[0];
          //查询当前经纪人的数据信息
        string strSqlJJR = "select a.DLYX '经纪人登录邮箱',b.J_JJRZGZSBH '经纪人资格证书编号',b.I_JYFMC '经纪人名称',b.J_JJRJSBH '经纪人角色编号',b.B_YHM '经纪人用户名','是' '是否当前分公司下经纪人','是' '是否阶段一筛选结果', b.I_PTGLJG '平台管理机构',b.I_SSQYS '经纪人_所属区域省',b.I_SSQYSHI '经纪人_所属区域市',b.I_SSQYQ '经纪人_所属区域区',b.I_SSQYS+b.I_SSQYSHI  '经纪人_所属区域省市',a.FGSSHSJ '分公司审核时间',a.FGSSHZT '分公司审核状态',b.B_SFDJ '是否冻结', b.B_SFXM '是否休眠',b.B_SFYXDL '是否允许登录',b.J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_MJMJJYZHYJJRZHGLB as a inner join AAA_DLZHXXB as b on a.DLYX=b.B_DLYX where a.JSZHLX='经纪人交易账户' and a.JJRSHZT='审核通过' and a.FGSSHZT='审核通过' and a.SFYX='是' and b.J_JJRJSBH='" + strGLJJRJSBH + "' ";
        DataTable dataTableJJR = DbHelperSQL.Query(strSqlJJR).Tables[0];

        if (dataTableDQJJRZT != null && dataTableDQJJRZT.Rows.Count >= 0 && dataTableGLGJJR != null)
        {
            if (dataTableDQJJRZT.Rows[0]["经纪人_冻结状态"].ToString() == "否" && dataTableDQJJRZT.Rows[0]["经纪人_休眠状态"].ToString() == "否")//判断当前关联经纪人的中表状态
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前关联的经纪人账户状态正常，不能选择其它经纪人。";
                return dsreturn;
            }
            if (dataTableGLGJJR.Rows.Count > 0)
            {
                if (dataTableGLGJJR.Rows[0]["经纪人审核状态"].ToString() == "审核通过")//获取当前卖家家关联经纪人的审核问题
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已被关联过的经纪人审核通过，不能选择其它经纪人。";
                    return dsreturn;
                }
            }
            if (dataFZCFXZJJR.Rows.Count > 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已经关联过此经纪人，不能再选择此经纪人。";
                return dsreturn;

            }

            List<string> listStrArray = new List<string>();
            if (dataTableJJR.Rows.Count > 0)
            {
                if (dataTableJJR.Rows[0]["是否冻结"].ToString() == "是")
                {
                    listStrArray.Add("冻结状态");
                }
                if (dataTableJJR.Rows[0]["是否休眠"].ToString() == "是")
                {
                    listStrArray.Add("休眠状态");
                }
                if (dataTableJJR.Rows[0]["是否允许登录"].ToString() == "否")
                {
                    listStrArray.Add("不允许登录状态");
                }
                if (dataTableJJR.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是")
                {
                    listStrArray.Add("暂停新用户审核状态");
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：暂停新用户审核状态，请重新选择！";
                }
                if (listStrArray.Count > 0)
                {
                    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您选择的经纪人当前状态为：" + string.Join("|", listStrArray.ToArray()).ToString() + "，请重新选择！";
                    return dsreturn;
                }
            }







            string strNumber = PublicClass2013.GetNextNumberZZ("AAA_MJMJJYZHYJJRZHGLB", "");
            string strExecSql = "insert AAA_MJMJJYZHYJJRZHGLB(Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR,SQSJ,JJRSHZT,FGSSHZT,SFZTYHXYW,SFYX,SFSCGLJJR,CreateTime,CheckLimitTime)values ('" + strNumber + "','" + MMJDLYX + "','" + JSZHLX + "','" + strGLJJRDLYX + "','" + strGLJJRJSBH + "','" + strGLJJRYHM + "','否','" + DateTime.Now.ToString() + "','审核中','审核通过','否','是','否','" + DateTime.Now.ToString() + "','" + DateTime.Now.ToString() + "')";

            int Tag = DbHelperSQL.ExecuteSql(strExecSql);
            if (Tag > 0)
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功关联此经纪人！";
                return dsreturn;
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取经纪人数据信息失败！";
            return dsreturn;

        }
        return dsreturn;
    }
    /// <summary>
    /// 通过经纪人资格证书，得到经纪人用户信息
    /// </summary>
    /// <param name="strJJRZGZ"></param>
    /// <returns></returns>
    public DataTable GetJJRYHXX(string strJJRZGZ)
    {

        DataTable dataTable = DbHelperSQL.Query("select B_DLYX '关联经纪人登录邮箱',B_YHM '关联经纪人用户名',J_JJRJSBH '关联经纪人角色编号',I_YWGLBMFL '业务管理部门分类',I_PTGLJG '平台管理机构',* from AAA_DLZHXXB  where J_JJRZGZSBH='" + strJJRZGZ + "'").Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return dataTable;
        }
        return null;
    }


    /// <summary>
    /// 根据省市获取平台管理机构
    /// </summary>
    /// <param name="hashTable"></param>
    /// <returns></returns>
    public string GetPTGLJG(Hashtable hashTable)
    {
        DataTable dataTable = DbHelperSQL.Query("select Pname,Cname,FGSname,BSCname from AAA_CityList_FGS where Pname like '" + hashTable["省份"] + "%' and Cname like '" + hashTable["地市"] + "%'").Tables[0];
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            return dataTable.Rows[0]["FGSname"].ToString();
        }
        return "没有对应的数据";

    }

    /// <summary>
    /// 得到交易账户审核的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetJYZHSHData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        if (dataTable.Rows[0]["JSZHLX"].ToString() == "买家卖家交易账户")
        {
            strSql = "select AAA_MJMJJYZHYJJRZHGLB.Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR '是否当前默认经纪人',CONVERT(varchar(20),b.I_ZLTJSJ,120) '申请时间',JJRSHZT '经纪人审核状态',CONVERT(varchar(20),JJRSHSJ,120)  '经纪人审核时间',JJRSHYJ '经纪人审核意见',FGSSHZT '分公司审核状态',FGSSHR '分公司审核人',CONVERT(varchar(20),FGSSHSJ,120) '分公司审核时间',FGSSHYJ '分公司审核意见' from AAA_MJMJJYZHYJJRZHGLB inner join AAA_DLZHXXB as b on AAA_MJMJJYZHYJJRZHGLB.DLYX=b.B_DLYX  where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and SFYX='是' and SFSCGLJJR='是' and  JSZHLX='" + dataTable.Rows[0]["JSZHLX"].ToString() + "'   order by SQSJ desc";

        }
        else  //经纪人交易账户
        {
            strSql = "select AAA_MJMJJYZHYJJRZHGLB.Number,DLYX,JSZHLX,GLJJRDLZH,GLJJRBH,GLJJRYHM,SFDQMRJJR '是否当前默认经纪人',CONVERT(varchar(20),b.I_ZLTJSJ,120) '申请时间',JJRSHZT '经纪人审核状态',CONVERT(varchar(20),JJRSHSJ,120)  '经纪人审核时间',JJRSHYJ '经纪人审核意见',FGSSHZT '分公司审核状态',FGSSHR '分公司审核人',CONVERT(varchar(20),FGSSHSJ,120) '分公司审核时间',FGSSHYJ '分公司审核意见' from AAA_MJMJJYZHYJJRZHGLB inner join AAA_DLZHXXB as b on AAA_MJMJJYZHYJJRZHGLB.DLYX=b.B_DLYX  where DLYX='" + dataTable.Rows[0]["DLYX"].ToString() + "' and SFYX='是' and  JSZHLX='" + dataTable.Rows[0]["JSZHLX"].ToString() + "'   order by SQSJ desc";
        }
        DataTable dataTableONE = DbHelperSQL.Query(strSql).Tables[0];
        if (dataTableONE != null && dataTableONE.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "交易账户审核信息获取成功！";
            DataTable dataTableCopy = dataTableONE.Copy();
            dsreturn.Tables.Add(dataTableCopy);
            dsreturn.Tables[1].TableName = "交易账户审核数据信息";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "该交易账户审核信息获取失败！";
        }
        return dsreturn;
    }


    /// <summary>
    /// 暂停、恢复新用户审核  仅用于经纪人交易账户的时候
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet ZT_HF_XYHSH(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型
        string CZLX = dataTable.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复

        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
 

        //2013.07.29 WYH dele
        if (JBXX["冻结功能项"].ToString().Trim().IndexOf("经纪人暂停代理新业务") >= 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系。\n\r被冻结功能：" + JBXX["冻结功能项"].ToString().Trim();
            return dsreturn;
        }

        if (JBXX["是否休眠"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }
        #endregion
        DataTable dataTableInfor = DbHelperSQL.Query("select J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_DLZHXXB where B_DLYX='" + DLYX + "' and  B_JSZHLX='" + JSZHLX + "'").Tables[0];
        if (dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是" && CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是暂停新用户审核审核状态，请不要再执行暂停操作！";
                return dsreturn;
            }
            else if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "否" && CZLX == "恢复")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您当前已经是恢复新用户审核审核状态，请不要再执行恢复操作！";
                return dsreturn;
            }

        }

        Object obj = DbHelperSQL.GetSingle("select J_JRZTXYHBZ from AAA_DLZHXXB  where B_DLYX='" + DLYX + "' and B_JSZHLX='" + JSZHLX + "'");
        string strSourceValue = "";//原始值
        if (obj is DBNull || obj == null)
        {
            strSourceValue = "";
        }
        else
        {
            strSourceValue = obj.ToString();
        }
        ArrayList arryList = new ArrayList();



        if (CZLX == "暂停")
        {
            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='是',J_JJRZTXYHSHSJ='" + DateTime.Now.ToString() + "' where B_DLYX='" + DLYX + "' and B_JSZHLX='" + JSZHLX + "'";
            arryList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：暂停新用户审核" + "◆";
            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ='" + strCZ + "' where  B_DLYX='" + DLYX + "' and B_JSZHLX='" + JSZHLX + "'";
            arryList.Add(strCZValue);
        }
        else
        {
            strSql = "update AAA_DLZHXXB set J_JJRSFZTXYHSH='否',J_JJRZTXYHSHSJ=null where B_DLYX='" + DLYX + "' and B_JSZHLX='" + JSZHLX + "'";
            arryList.Add(strSql);
            string strCZ = strSourceValue + "时间：" + DateTime.Now.ToString() + "  操作：恢复新用户审核" + "◆";
            string strCZValue = "update AAA_DLZHXXB set J_JRZTXYHBZ='" + strCZ + "' where  B_DLYX='" + DLYX + "' and B_JSZHLX='" + JSZHLX + "'";
            arryList.Add(strCZValue);
        }
        bool boolTag = DbHelperSQL.ExecSqlTran(arryList);

        if (boolTag == true)
        {
            if (CZLX == "暂停")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功执行暂停新交易方审核操作！";
            }
            else
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
                dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已成功执行恢复新交易方审核操作！";
            }
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "当前操作失败！";

        }

        return dsreturn;
    }

    /// <summary>
    /// 暂停、恢复新用户审核  初始状态
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet ZT_HF_XYHSHCSZT(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型
        string CZLX = dataTable.Rows[0]["操作类型"].ToString();//操作类型  暂停、恢复

        #region//基础验证
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "未开通交易账户";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已开通交易账户，能进行交易操作。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "已开通交易账户";
        }
        #endregion
        DataTable dataTableInfor = DbHelperSQL.Query("select J_JJRSFZTXYHSH '经纪人是否暂停新用户审核' from AAA_DLZHXXB where B_DLYX='" + DLYX + "' and  B_JSZHLX='" + JSZHLX + "'").Tables[0];
        if (dataTableInfor.Rows.Count > 0)
        {

            if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "是")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "交易账户当前为暂停状态";
            }
            else if (dataTableInfor.Rows[0]["经纪人是否暂停新用户审核"].ToString() == "否")
            {
                dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okOther";
                dsreturn.Tables["返回值单条"].Rows[0]["附件信息2"] = "交易账户当前为恢复状态";
            }
        }
        return dsreturn;
    }

    /// <summary>
    /// 得到当前交易账户的当前状态
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetJYZHDQZT(DataSet dsreturn, DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();//经纪人角色编号
        string DLYX = dataTable.Rows[0]["登录邮箱"].ToString();//登录邮箱
        string JSZHLX = dataTable.Rows[0]["结算账户类型"].ToString();//结算账户类型

        #region//基础验证
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okJudge";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "未开通交易账户";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "okJudge";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您已开通交易账户，能进行交易操作。";
            dsreturn.Tables["返回值单条"].Rows[0]["附件信息1"] = "已开通交易账户";
        }
        #endregion

        return dsreturn;
    }
    #region//执行每月经纪人扣税后台监控


    #region

    /// <summary>
    /// 执行每月经纪人扣税后台监控
    /// </summary>
    /// <param name="dsReturn"></param>
    /// <returns></returns>
    //public DataSet ServerMYJJRKS111111(DataSet dsreturn)
    //{
    //    ////1、匹配每月2号凌晨
    //    //if (DateTime.Now.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-02")))
    //    //{
    //    //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //    //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人扣税后台监控只有每月2号执行，当前时间为" + DateTime.Now.ToString("yyyy年MM月dd日") + "不能执行该监控！";
    //    //    return dsreturn;
    //    //}       

    //    string sqlCount = "select COUNT(*) from AAA_JJRSYKSMXB";
    //   object obj= DbHelperSQL.GetSingle(sqlCount);
    //   int countNumber = Convert.ToInt32(obj);
    //    //扣税公式
    //    string strKSGS="1、您的经纪人收益属于劳务报酬所得，劳务报酬所得，属于同一项目连续性收入的，以一个月内取得的收入为一次。2、自然月度未满的经纪人收益，由于无法确定应纳税额和税后所得，暂不计入您的经纪人收益余额。因此，你的经纪人交易账户目前可支取的收益余额未包含本月的经纪人收益。3、劳务所得税率计算标准及收益计算公式A、0<收入<=800，应纳税额=0，税后所得=收入，B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额C、4000<收入<=20000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额D、20000<收入<=50000，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额E、收入>50000 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额";

    //    DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where XZ in ('自然人月收益','所得税扣缴')");

    //    DataRow[] dr_ZRRYSY = ds_ZKMX.Tables[0].Select("number='1304000045'");//自然人月收益--实
    //    DataRow[] dr_SDSKJ = ds_ZKMX.Tables[0].Select("number='1304000044'");//所得税扣缴--实
    //   if (countNumber == 0)//首次执行
    //   {
    //       Hashtable hashTable = GetMonthStartEnd(DateTime.Now);
    //       ArrayList arrayList = new ArrayList();


    //       try
    //       {
    //           DateTime dataTimeSYSJ = Convert.ToDateTime(hashTable["月初时间"]);//获取收益月初的时间


    //         DataSet dataSet=ServerMYJJRKS_HQSY(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString());

    //           for(int i=0;i<dataSet.Tables[0].Rows.Count;i++)
    //           {
    //               Hashtable hashTableJE=ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));

    //         //获取经纪人收益扣税明细表的Number值
    //         string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
    //         //向经纪人收益扣税明细表中插入数据
    //         string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "','" + strKSGS + "','不确定','" + strNumber + "')";
    //         arrayList.Add(strSqlAddJJRSYKSMXB);
    //              // 自然人月收益
    //         dr_ZRRYSY[0]["ZY"] = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", dataTimeSYSJ.Year.ToString()).Replace("[x2]", dataTimeSYSJ.Month.ToString());
    //         //获取账款流水明细表（的Number值   
    //         string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
    //         string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + dr_ZRRYSY[0]["ZY"].ToString() + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
    //         arrayList.Add(strSqlZRRYSY);
    //               //所得税扣缴
    //         dr_SDSKJ[0]["ZY"] = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", dataTimeSYSJ.Year.ToString()).Replace("[x2]", dataTimeSYSJ.Month.ToString());
    //         string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
    //         string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + dr_SDSKJ[0]["ZY"].ToString() + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
    //         arrayList.Add(strSqlSDSKJ);
    //           }

    //       DbHelperSQL.ExecSqlTran(arrayList);

    //             dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
    //             dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行" + hashTable["月初时间"].ToString() + "到" + hashTable["月末时间"].ToString() + "经纪人扣税监控成功！";
    //             return dsreturn;
    //       }
    //       catch (Exception ex)
    //       {
    //           dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //           dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取" + hashTable["月初时间"].ToString() + "到" + hashTable["月末时间"].ToString() + "经纪人收益基础数据出错！ 详情：" + ex.ToString();
    //           return dsreturn;

    //       }
    //   }
    //   else//非首次执行  
    //   {


    //       Hashtable hashTable = GetMonthStartEnd(DateTime.Now);
    //       ArrayList arrayList = new ArrayList();
    //       try
    //       {

    //           while(GetJJRSYKSMXB(Convert.ToDateTime(hashTable["月初时间"]).Year.ToString(),Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()))
    //           {
    //               DateTime dataTimeSYSJ = Convert.ToDateTime(hashTable["月初时间"]);//获取收益月初的时间
    //               DataSet dataSet = ServerMYJJRKS_HQSY(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString());
    //               for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
    //               {
    //                   Hashtable hashTableJE = ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));
    //                   //获取经纪人收益扣税明细表的Number值
    //                   string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
    //                   //向经纪人收益扣税明细表中插入数据
    //                   string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + DateTime.Now.Year.ToString() + "','" + DateTime.Now.Month.ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "','" + strKSGS + "','不确定','" + strNumber + "')";
    //                   arrayList.Add(strSqlAddJJRSYKSMXB);
    //                   // 自然人月收益
    //                   dr_ZRRYSY[0]["ZY"] = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", dataTimeSYSJ.Year.ToString()).Replace("[x2]", dataTimeSYSJ.Month.ToString());
    //                   //获取账款流水明细表（的Number值   
    //                   string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
    //                   string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + dr_ZRRYSY[0]["ZY"].ToString() + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
    //                   arrayList.Add(strSqlZRRYSY);
    //                   //所得税扣缴
    //                   dr_SDSKJ[0]["ZY"] = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", dataTimeSYSJ.Year.ToString()).Replace("[x2]", dataTimeSYSJ.Month.ToString());
    //                   string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
    //                   string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + dr_SDSKJ[0]["ZY"].ToString() + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
    //                   arrayList.Add(strSqlSDSKJ);
    //               }

    //               hashTable = GetMonthStartEnd(Convert.ToDateTime(hashTable["月初时间"]));//此时在看上一个月的数据
    //           }
    //           DbHelperSQL.ExecSqlTran(arrayList);
    //           dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
    //           dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行" + hashTable["月初时间"].ToString() + "到" + hashTable["月末时间"].ToString() + "经纪人扣税监控成功！";
    //           return dsreturn;
    //       }
    //       catch (Exception ex)
    //       {
    //           dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
    //           dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行" + hashTable["月初时间"].ToString() + "到" + hashTable["月末时间"].ToString() + "经纪人扣税监控失败！ 详情：" + ex.ToString();
    //           return dsreturn;

    //       }
    //   }
    //}

    #endregion

    /// <summary>
    /// 执行每月经纪人扣税后台监控
    /// </summary>
    /// <param name="dsReturn"></param>
    /// <returns></returns>
    public DataSet ServerMYJJRKS(DataSet dsreturn)
    {
        ////1、匹配每月2号凌晨
        //if (!DateTime.Now.ToString("yyyy-MM-dd").Equals(DateTime.Now.ToString("yyyy-MM-02")))
        //{
        //    dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
        //    dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "经纪人扣税后台监控只有每月2号执行，当前时间为" + DateTime.Now.ToString("yyyy年MM月dd日") + "不能执行该监控！";
        //    return dsreturn;
        //}       

        string sqlCount = "select COUNT(*) from AAA_JJRSYKSMXB";
        object obj = DbHelperSQL.GetSingle(sqlCount);
        int countNumber = Convert.ToInt32(obj);
        //扣税公式
        string strKSGS = "1、您的经纪人收益属于劳务报酬所得，劳务报酬所得，属于同一项目连续性收入的，以一个月内取得的收入为一次。2、自然月度未满的经纪人收益，由于无法确定应纳税额和税后所得，暂不计入您的经纪人收益余额。因此，你的经纪人交易账户目前可支取的收益余额未包含本月的经纪人收益。3、.劳务所得税率计算标准及收益计算公式A、0<收入<=800，应纳税额=0，税后所得=收入，B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额C、4000<收入<=25000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额D、25000<收入<=62500，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额E、收入>62500 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额";

        DataSet ds_ZKMX = DbHelperSQL.Query("select * from AAA_moneyDZB where XZ in ('自然人月收益','所得税扣缴')");

        DataRow[] dr_ZRRYSY = ds_ZKMX.Tables[0].Select("number='1304000045'");//自然人月收益--实
        DataRow[] dr_SDSKJ = ds_ZKMX.Tables[0].Select("number='1304000044'");//所得税扣缴--实




        #region //月度经纪人收益调整方法更改
        try
        {
            DateTime dateTimeStart = Convert.ToDateTime("2012-05-01");//确定初始时间
            ArrayList arrayList = new ArrayList();
            //for (DateTime dateEnd = DateTime.Now.AddMonths(-1); dateEnd.CompareTo(dateTimeStart) >= 0; dateEnd = dateEnd.AddMonths(-1))
            //{
            for (DateTime dateEnd = DateTime.Now; dateEnd.CompareTo(dateTimeStart) >= 0; dateEnd = dateEnd.AddMonths(-1))
            {
                Hashtable hashTable = GetMonthStartEnd(dateEnd);
                bool isExists = GetJJRSYKSMXB(Convert.ToDateTime(hashTable["月初时间"]).Year.ToString(), Convert.ToDateTime(hashTable["月初时间"]).Month.ToString());
                 
                if (isExists)//此月份收益数据没有计算
                {
                    DataSet dataSet = ServerMYJJRKS_HQSY(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString());
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            if (dataSet.Tables[0].Rows[i]["第三方存管状态"].ToString() == "开通")//第三方存管状态为“已开通”
                            {
                                Hashtable hashTableJE = ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));
                                //获取经纪人收益扣税明细表的Number值
                                string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH,SFYJJSWC) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Year.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Month.ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "','" + strKSGS + "','确定','" + strNumber + "','已计算')";
                                arrayList.Add(strSqlAddJJRSYKSMXB);
                                // 自然人月收益
                                string str_dr_ZRRYSY = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //获取账款流水明细表（的Number值   
                                string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + str_dr_ZRRYSY + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlZRRYSY);
                                //所得税扣缴
                                string str_SDSKJ = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + str_SDSKJ + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlSDSKJ);
                                //更新登陆账号信息表 账户资金余额
                                string strZHZJYE = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE+'" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "' where B_DLYX='" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "'";
                                arrayList.Add(strZHZJYE);


                            }
                            else
                            {
                                //获取经纪人收益扣税明细表的Number值
                                string strNumber = PublicClass2013.GetNextNumberZZ("AAA_JJRSYKSMXB", "");
                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "insert AAA_JJRSYKSMXB(Number,DLYX,JSZHLX,JJRJSBH,SYKSZHRQ,SYKSNF,SYKSYF,KSSYJE,SKJE,SHJE,KSBZ,SFCGZRZH,JKLSH,SFYJJSWC) values('" + strNumber + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','" + DateTime.Now.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Year.ToString() + "','" + Convert.ToDateTime(hashTable["月初时间"]).Month.ToString() + "','0','0','0','" + strKSGS + "','不确定','" + strNumber + "','未计算')";
                                arrayList.Add(strSqlAddJJRSYKSMXB);

                            }
                        }
                    }
                }
                else//此月份收益数据已经计算过
                {
                    DataSet dataSet = ServerMYJJRKS_HQSYMX(hashTable["月初时间"].ToString(), hashTable["月末时间"].ToString(), Convert.ToDateTime(hashTable["月初时间"]).Year.ToString(), Convert.ToDateTime(hashTable["月初时间"]).Month.ToString());
                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            if (dataSet.Tables[0].Rows[i]["第三方存管状态"].ToString() == "开通")
                            {
                                Hashtable hashTableJE = ServerMYJJRKS_JXKS(Convert.ToDouble(dataSet.Tables[0].Rows[i]["金额"]));
                                string strNumber = dataSet.Tables[0].Rows[i]["Number"].ToString();
                                //向经纪人收益扣税明细表中插入数据
                                string strSqlAddJJRSYKSMXB = "update AAA_JJRSYKSMXB set KSSYJE='" + hashTableJE["收益"].ToString() + "',SKJE='" + hashTableJE["税额"].ToString() + "',SHJE='" + hashTableJE["税后金额"].ToString() + "',SFYJJSWC='已计算',JKLSH='" + strNumber + "',SFCGZRZH='确定' where Number='" + strNumber + "'";
                                arrayList.Add(strSqlAddJJRSYKSMXB);
                                // 自然人月收益
                                string str_dr_ZRRYSY = dr_ZRRYSY[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                //获取账款流水明细表（的Number值   
                                string strNumberZRRYSY = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strSqlZRRYSY = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberZRRYSY + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_ZRRYSY[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["收益"]).ToString("#0.00") + "','" + dr_ZRRYSY[0]["XM"].ToString() + "','" + dr_ZRRYSY[0]["XZ"].ToString() + "','" + str_dr_ZRRYSY + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlZRRYSY);
                                //所得税扣缴
                                string str_SDSKJ = dr_SDSKJ[0]["ZY"].ToString().Replace("[x1]", Convert.ToDateTime(hashTable["月初时间"]).Year.ToString()).Replace("[x2]", Convert.ToDateTime(hashTable["月初时间"]).Month.ToString()).ToString();
                                string strNumberSDSKJ = PublicClass2013.GetNextNumberZZ("AAA_ZKLSMXB", "");
                                string strSqlSDSKJ = "insert AAA_ZKLSMXB(Number,DLYX,JSZHLX,JSBH,LYYWLX,LYDH,LSCSSJ,YSLX,JE,XM,XZ,ZY,JKBH,QTBZ,SJLX) values('" + strNumberSDSKJ + "','" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "','" + dataSet.Tables[0].Rows[i]["结算账户类型"].ToString() + "','" + dataSet.Tables[0].Rows[i]["角色编号"].ToString() + "','AAA_JJRSYKSMXB','" + strNumber + "','" + DateTime.Now.ToString() + "','" + dr_SDSKJ[0]["YSLX"].ToString() + "','" + Convert.ToDouble(hashTableJE["税额"]).ToString("#0.00") + "','" + dr_SDSKJ[0]["XM"].ToString() + "','" + dr_SDSKJ[0]["XZ"].ToString() + "','" + str_SDSKJ + "','未确定','无','" + dr_ZRRYSY[0]["SJLX"].ToString() + "')";
                                arrayList.Add(strSqlSDSKJ);
                                //更新登陆账号信息表 账户资金余额
                                string strZHZJYE = "update AAA_DLZHXXB set B_ZHDQKYYE=B_ZHDQKYYE+'" + Convert.ToDouble(hashTableJE["税后金额"]).ToString("#0.00") + "' where B_DLYX='" + dataSet.Tables[0].Rows[i]["登录邮箱"].ToString() + "'";
                                arrayList.Add(strZHZJYE);
                            }
                        }
                    }


                }
            }
            DbHelperSQL.ExecSqlTran(arrayList);

            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控成功，时间" + DateTime.Now.ToString() + "！";
            return dsreturn;
        }
        catch (Exception ex)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "执行经纪人扣税监控失败！ 详情：" + ex.ToString();
            return dsreturn;

        }
        #endregion


    }



    /// <summary>
    /// 根据年份和月份获取经纪人收益扣税明细表的总数据，判断次年份和月份的数据是否存在，不存在为true存在为false
    /// </summary>
    /// <param name="strYear"></param>
    /// <param name="strMonth"></param>
    /// <returns></returns>
    public bool GetJJRSYKSMXB(string strYear, string strMonth)
    {
        string strSql = "select COUNT(*) from AAA_JJRSYKSMXB where SYKSNF='" + strYear + "' and SYKSYF='" + strMonth + "'";
        object objNum = DbHelperSQL.GetSingle(strSql);
        if (Convert.ToInt32(objNum) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    ///获取相对于传入时间的月初时间和月末时间
    /// </summary>
    /// <param name="dataTime">传入的时间</param>
    /// <returns></returns>
    public Hashtable GetMonthStartEnd(DateTime dataTime)
    {
        Hashtable hash = new Hashtable();
        //获取相对于传入时间的上月月初
        DateTime dataTimeLastMonthStart = DateTime.Parse(dataTime.AddMonths(-1).ToString("yyyy-MM-01"));
        //获取相对于传入时间的上月月末
        DateTime dataTimeLastMonthEnd = dataTimeLastMonthStart.AddMonths(1).AddDays(-1);
        hash["月初时间"] = dataTimeLastMonthStart.ToString("yyyy-MM-dd");
        hash["月末时间"] = dataTimeLastMonthEnd.ToString("yyyy-MM-dd");
        return hash;
    }


    /// <summary>
    /// 根据开始时间和结束时间查找经纪人的收益数据信息
    /// </summary>
    /// <param name="timeStart">月度开始时间</param>
    /// <param name="timeEnd">月度结束时间</param>
    /// <returns></returns>
    public DataSet ServerMYJJRKS_HQSY(string timeMonthStart, string timeMonthEnd)
    {
        string strSql = "select tab.*,isnull(DD.I_DSFCGZT,'') '第三方存管状态' from (select a.DLYX '登录邮箱',a.JSZHLX '结算账户类型',a.JSBH '角色编号',sum(a.JE) '金额' from AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.XZ=b.XZ inner join AAA_DLZHXXB as c on a.DLYX=c.B_DLYX where CONVERT(varchar(10),LSCSSJ,120)>='" + timeMonthStart + "' and CONVERT(varchar(10),LSCSSJ,120)<='" + timeMonthEnd + "' and a.SJLX='预' and c.I_ZCLB='自然人' and  b.xz in('来自卖方收益','来自买方收益')  and a.JSZHLX='经纪人交易账户' group by a.DLYX,a.JSZHLX,a.JSBH) as tab inner join AAA_DLZHXXB as DD on tab.登录邮箱=DD.B_DLYX";
        //这个sql滤掉AAA_JJRSYKSMXB表里，成功的
        DataSet dataSet = DbHelperSQL.Query(strSql);
        return dataSet;
    }

    /// <summary>
    /// 获取经纪人收益扣税明细表中“未计算”的数据和应获取的收益
    /// </summary>
    /// <param name="strDLYX">登陆邮箱</param>
    /// <param name="timeMonthStart">月初时间</param>
    /// <param name="timeMonthEnd">月末时间</param>
    /// <param name="strYear">年份</param>
    /// <param name="strMonth">月份</param>
    /// <returns>返回的数据</returns>
    public DataSet ServerMYJJRKS_HQSYMX(string timeMonthStart, string timeMonthEnd, string strYear, string strMonth)
    {
        string strSql = "select  MX.Number,MX.DLYX '登录邮箱',MX.JSZHLX '结算账户类型',MX.JJRJSBH '角色编号',isnull(MX.SFYJJSWC,'') '是否计算',isnull(DD.I_DSFCGZT,'') '第三方存管状态',(select sum(a.JE)  from AAA_ZKLSMXB as a inner join AAA_moneyDZB as b on a.XZ=b.XZ  where  a.DLYX=DD.B_DLYX and CONVERT(varchar(10),LSCSSJ,120)>='" + timeMonthStart + "' and CONVERT(varchar(10),LSCSSJ,120)<='" + timeMonthEnd + "' and a.SJLX='预' and DD.I_ZCLB='自然人' and  b.xz in('来自卖方收益','来自买方收益')  and a.JSZHLX='经纪人交易账户') '金额' from AAA_JJRSYKSMXB as MX inner join AAA_DLZHXXB as DD on MX.DLYX=DD.B_DLYX where MX.SYKSNF='" + strYear + "' and MX.SYKSYF='" + strMonth + "' and MX.JSZHLX='经纪人交易账户' and MX.SFYJJSWC='未计算'";
        DataSet dataSet = DbHelperSQL.Query(strSql);
        return dataSet;
    }

    /// <summary>
    /// 对经纪人的收益执行扣税的操作
    /// </summary>
    /// <param name="douSK">收益金额</param>
    /// <returns></returns>
    public Hashtable ServerMYJJRKS_JXKS(double douSK)
    {
        Hashtable hashSK = new Hashtable();
        hashSK["收益"] = douSK;
        #region//作废
        /*
         A、0<收入<=800，应纳税额=0，税后所得=收入，
B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额
C、4000<收入<=20000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额
D、20000<收入<=50000，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额
E、收入>50000 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额
         */
        //double SJ = 0.00;
        //if (douSK >= 0 && douSK <= 800)
        //{
        //    SJ = 0;
        //}
        //else if (douSK >800 && douSK <= 4000)
        //{
        //    SJ = Math.Round(((douSK - 800) * 0.20),2);
        //}
        //else if (douSK > 4000 && douSK<=20000)
        //{
        //    SJ = Math.Round((douSK*0.80* 0.20), 2);
        //}
        //else if (douSK > 20000 && douSK <= 50000)
        //{
        //    SJ = Math.Round((douSK * 0.80 * 0.30 - 2000), 2);
        //}
        //else
        //{
        //    SJ = Math.Round((douSK * 0.80 * 0.40 - 7000), 2);
        //}
        //hashSK["税额"] = SJ;//此处暂时这样做

        //hashSK["税后金额"] = Math.Round((douSK - SJ),2);
        #endregion

        /*
         .劳务所得税率计算标准及收益计算公式
A、0<收入<=800，应纳税额=0，税后所得=收入，
B、800<收入<=4000，应纳税额=（收入-800）*0.2，税后所得=收入-应纳税额
C、4000<收入<=25000，应纳税额=收入*0.8*0.2，税后所得=收入-应纳税额
D、25000<收入<=62500，应纳税额=收入*0.8*0.3-2000，税后所得=收入-应纳税额
E、收入>62500 ，应纳税额=收入*0.8*0.4-7000，税后所得=收入-应纳税额
         */
        double SJ = 0.00;
        if (douSK >= 0 && douSK <= 800)
        {
            SJ = 0;
        }
        else if (douSK > 800 && douSK <= 4000)
        {
            SJ = Math.Round(((douSK - 800) * 0.20), 2);
        }
        else if (douSK > 4000 && douSK <= 25000)
        {
            SJ = Math.Round((douSK * 0.80 * 0.20), 2);
        }
        else if (douSK > 25000 && douSK <= 62500)
        {
            SJ = Math.Round((douSK * 0.80 * 0.30 - 2000), 2);
        }
        else
        {
            SJ = Math.Round((douSK * 0.80 * 0.40 - 7000), 2);
        }
        hashSK["税额"] = SJ;//此处暂时这样做

        hashSK["税后金额"] = Math.Round((douSK - SJ), 2);
        return hashSK;
    }
    #endregion

    /// <summary>
    /// 得到电子购货合同相关的数据信息
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet GetDZGHHTXX(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string strNumber = dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        DataTable dataTableSeller = DbHelperSQL.Query("select Seller.I_ZCLB '注册类别',Seller.I_YYZZSMJ '营业执照扫描件',Seller.I_SFZSMJ '身份证扫描件',Seller.I_SFZFMSMJ '身份证反面扫描件',Seller.I_ZZJGDMZSMJ '组织机构代码证扫描件',Seller.I_SWDJZSMJ '税务登记证扫描件',Seller.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Seller.I_KHXKZSMJ '开户许可证扫描件',Seller.I_YLYJK '预留印鉴卡扫描件',Seller.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Seller.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件',Seller.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Seller on ZZ.T_YSTBDMJJSBH=Seller.J_SELJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableSellerCopy = dataTableSeller.Copy();
        dataTableSellerCopy.TableName = "卖家资质";
        dsreturn.Tables.Add(dataTableSellerCopy);

        DataTable dataTableBuyer = DbHelperSQL.Query("select Buyer.I_ZCLB '注册类别',Buyer.I_YYZZSMJ '营业执照扫描件',Buyer.I_SFZSMJ '身份证扫描件',Buyer.I_SFZFMSMJ '身份证反面扫描件',Buyer.I_ZZJGDMZSMJ '组织机构代码证扫描件',Buyer.I_SWDJZSMJ '税务登记证扫描件',Buyer.I_YBNSRZGZSMJ '一般纳税人资格证明扫描件',Buyer.I_KHXKZSMJ '开户许可证扫描件',Buyer.I_YLYJK '预留印鉴卡扫描件',Buyer.I_FDDBRSFZSMJ '法定代表人身份证扫描件',Buyer.I_FDDBRSFZFMSMJ '法定代表人身份证反面扫描件' ,Buyer.I_FDDBRSQS '法定代表人授权书' from AAA_ZBDBXXB as ZZ left join AAA_DLZHXXB as Buyer on ZZ.Y_YSYDDMJJSBH=Buyer.J_BUYJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableBuyerCopy = dataTableBuyer.Copy();
        dataTableBuyerCopy.TableName = "买家资质";
        dsreturn.Tables.Add(dataTableBuyerCopy);

        DataTable dataTableTBD = DbHelperSQL.Query("select  DD.I_JYFMC '卖家名称',TT.TJSJ '发布时间',TT.Number '投标单号',TT.GHQY '可供货区域',TT.SPMC '拟卖出商品名称',TT.GG '规格',TT.JJDW '计价单位',TT.TBNSL '投标拟售量',TT.TBJG '投标价格',TT.TBJE '投标金额',TT.DJTBBZJ '冻结的投标保证金',TT.PTSDZDJJPL '平台设定最大经济批量',TT.MJSDJJPL '卖家设定的经济批量',TT.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number left join AAA_DLZHXXB as DD on TT.MJJSBH=DD.J_SELJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableTBDCopy = dataTableTBD.Copy();
        dataTableTBDCopy.TableName = "投标单";
        dsreturn.Tables.Add(dataTableTBDCopy);

        DataTable dataTableYDD = DbHelperSQL.Query("select  DD.I_JYFMC '买家名称',YY.TJSJ '下单时间',YY.Number '预订单号',YY.SHQY '收货区域',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableYDDCopy = dataTableYDD.Copy();
        dataTableYDDCopy.TableName = "预订单";
        dsreturn.Tables.Add(dataTableYDDCopy);

        DataTable dataTableTBDZZ = DbHelperSQL.Query("select  TT.ZLBZYZM '质量标准与证明',TT.CPJCBG '产品检测报告',TT.PGZFZRFLCNS '品管总负责人法律承诺书',TT.FDDBRCNS '法定代表人承诺书',TT.SHFWGDYCN '售后服务规定与承诺',TT.CPSJSQS '产品送检授权书',TT.SLZM '税率证明',TT.ZZ01 '资质01',TT.ZZ02 '资质02',TT.ZZ03 '资质03',TT.ZZ04 '资质04',TT.ZZ05 '资质05',TT.ZZ06 '资质06',TT.ZZ07 '资质07',TT.ZZ08 '资质08',TT.ZZ09 '资质09',TT.ZZ10 '资质10',SP.SCZZYQ as '上传资质要求' from AAA_ZBDBXXB as ZZ inner join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number inner join AAA_PTSPXXB as SP on TT.SPBH=SP.SPBH where ZZ.Number='" + strNumber + "'").Tables[0];

        DataTable dataTableTBDZZCopy = dataTableTBDZZ.Copy();
        dataTableTBDZZCopy.TableName = "投标单资质";
        dsreturn.Tables.Add(dataTableTBDZZCopy);

        DataTable dataTableTHDYFHD = DbHelperSQL.Query("select (select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货数量', (select isnull(sum(T_THSL*ZBDJ),0.00)  from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT<>'撤销') '已提货金额',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT not in('撤销','未生成发货单','已生成发货单')) '已发货数量',(select isnull(sum(T_THSL),0) from AAA_THDYFHDXXB where ZBDBXXBBH=ZZ.Number  and F_DQZT  in('无异议收货','默认无异议收货','补发货物无异议收货','有异议收货后无异议收货')) '无异议收货数量', 'T'+TT.Number '提货单编号',TT.T_THSL '提货数量',isnull(TT.T_THSL*TT.ZBDJ,0.00) '提货金额',TT.F_DQZT '当前状态','F'+TT.Number '发货单编号' from  AAA_THDYFHDXXB as TT left join AAA_ZBDBXXB as ZZ   on ZZ.Number=TT.ZBDBXXBBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableTHDYFHDCopy = dataTableTHDYFHD.Copy();
        dataTableTHDYFHDCopy.TableName = "提货单发货单";
        dsreturn.Tables.Add(dataTableTHDYFHDCopy);
        //以后处理限制条件，加载这个地方的SQL语句里
        DataTable dataTableQTZZ = DbHelperSQL.Query("select Number '保函编号' ,Q_ZMWJLJ '履约争议证明文件路径' from AAA_ZBDBXXB where Number='" + strNumber + "'").Tables[0];
        DataTable dataTableQTZZCopy = dataTableQTZZ.Copy();
        dataTableQTZZCopy.TableName = "其他资质";
        dsreturn.Tables.Add(dataTableQTZZCopy);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功！";

        return dsreturn;
    }

    /// <summary>
    ///得到投标单的信息
    /// </summary>
    /// <param name="dsreturn">返回值</param>
    /// <param name="dataTable">传递的参数</param>
    /// <returns></returns>
    public DataSet Get_TBDData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string strNumber = dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        DataTable dataTableTBD = DbHelperSQL.Query("select TT.Number '投标单号',TT.TJSJ '发布时间',DD.I_JYFMC '卖方名称',TT.SPMC '拟卖出商品名称',TT.GG '规格',TT.JJDW '计价单位',TT.TBNSL '投标拟售量',TT.TBJG '投标价格',TT.TBJE '投标金额',TT.DJTBBZJ '冻结的投标保证金',TT.PTSDZDJJPL '平台设定最大经济批量',TT.MJSDJJPL '卖方设定的经济批量',TT.HTQX '合同期限',TT.GHQY '可供货区域' from AAA_ZBDBXXB as ZZ left join AAA_TBD as TT on ZZ.T_YSTBDBH=TT.Number left join AAA_DLZHXXB as DD on TT.MJJSBH=DD.J_SELJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableTBDCopy = dataTableTBD.Copy();
        dataTableTBDCopy.TableName = "投标单";
        dsreturn.Tables.Add(dataTableTBDCopy);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功获取投标单信息！";
        return dsreturn;

    }

    /// <summary>
    /// 得到预订单的信息
    /// </summary>
    /// <param name="dsreturn">返回值</param>
    /// <param name="dataTable">传递的参数</param>
    /// <returns></returns>
    public DataSet Get_YDDData(DataSet dsreturn, DataTable dataTable)
    {
        string strSql = "";
        string strNumber = dataTable.Rows[0]["中标定标_Number"].ToString();//中标定标信息表
        DataTable dataTableYDD = DbHelperSQL.Query("select YY.Number '预订单号',YY.TJSJ '下单时间',DD.I_JYFMC '买方名称',YY.SPMC '拟买入商品名称',YY.GG '规格',YY.JJDW '计价单位',YY.NDGSL '拟订购数量',YY.NMRJG '拟买入价格',YY.NDGJE '拟订购金额',YY.DJDJ '冻结的订金',YY.HTQX '合同期限',YY.SHQY '收货区域' from AAA_ZBDBXXB as ZZ left join AAA_YDDXXB as YY on ZZ.Y_YSYDDBH=YY.Number left join AAA_DLZHXXB as DD on YY.MJJSBH=DD.J_BUYJSBH where ZZ.Number='" + strNumber + "'").Tables[0];
        DataTable dataTableYDDCopy = dataTableYDD.Copy();
        dataTableYDDCopy.TableName = "预订单";
        dsreturn.Tables.Add(dataTableYDDCopy);
        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "陈宫获取预订单的信息！";
        return dsreturn;
    }



    /// <summary>
    /// 判断是否签订承诺书
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JudgeSFQDCNS(DataSet dsreturn, DataTable dataTable)
    {

        //当前买卖是一家
        string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "'";
        DataTable dataTableMMSYJ = DbHelperSQL.Query(strMMSYJ).Tables[0];
        //当前是卖家
        string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',T_MJCNSQDSJ '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ";
        DataTable dataTableDQSSeller = DbHelperSQL.Query(strDQSSeller).Tables[0];
        //当前是买家
        string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',Y_MJCNSQDSJ '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ";
        DataTable dataTableDQSBuyer = DbHelperSQL.Query(strDQSBuyer).Tables[0];

        string strSFQDCNS = "";//是否签订承诺书
        DateTime dateQDSJ = DateTime.Now;//承诺书签订时间
        if (dataTableMMSYJ != null && dataTableMMSYJ.Rows.Count > 0)  //当前买卖是一家
        {
            strSFQDCNS = dataTableMMSYJ.Rows[0]["卖家是否签订承诺书"].ToString();
            object obj = dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"];
            if (obj.ToString() == "")
            {
                dateQDSJ = DateTime.Now;
            }
            else
            {
                dateQDSJ = Convert.ToDateTime(dataTableMMSYJ.Rows[0]["卖家承诺书签订时间"].ToString());
            }
        }
        else if (dataTableDQSSeller != null && dataTableDQSSeller.Rows.Count > 0)  //当前是卖家
        {
            strSFQDCNS = dataTableDQSSeller.Rows[0]["卖家是否签订承诺书"].ToString();
            object obj = dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"];
            if (obj.ToString() == "")
            {
                dateQDSJ = DateTime.Now;
            }
            else
            {
                dateQDSJ = Convert.ToDateTime(dataTableDQSSeller.Rows[0]["卖家承诺书签订时间"].ToString());
            }
        }
        else if (dataTableDQSBuyer != null && dataTableDQSBuyer.Rows.Count > 0)//当前是买家
        {
            strSFQDCNS = dataTableDQSBuyer.Rows[0]["买家是否签订承诺书"].ToString();
            object obj = dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"];
            if (obj.ToString() == "")
            {
                dateQDSJ = DateTime.Now;
            }
            else
            {
                dateQDSJ = Convert.ToDateTime(dataTableDQSBuyer.Rows[0]["买家承诺书签订时间"].ToString());
            }

        }

        dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = strSFQDCNS;
        dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "获取数据成功";

        return dsreturn;
    }

    /// <summary>
    /// 交易账户签订承诺书
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet JYZHQDCNS(DataSet dsreturn, DataTable dataTable)
    {

        //当前买卖是一家
        string strMMSYJ = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',isnull(T_MJCNSQDSJ,'') '卖家承诺书签订时间',isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',isnull(Y_MJCNSQDSJ,'') '买家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "'";
        DataTable dataTableMMSYJ = DbHelperSQL.Query(strMMSYJ).Tables[0];
        //当前是卖家
        string strDQSSeller = "select Number,isnull(T_MJSFQDCNS,'') '卖家是否签订承诺书',isnull(T_MJCNSQDSJ,'') '卖家承诺书签订时间',T_YSTBDBH '原始投标单号',T_YSTBDDLYX '卖家登录邮箱',T_YSTBDJSZHLX '卖家结算账户类型',T_YSTBDMJJSBH '卖家角色编号' from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ";
        DataTable dataTableDQSSeller = DbHelperSQL.Query(strDQSSeller).Tables[0];
        //当前是买家
        string strDQSBuyer = "select Number,isnull(Y_MJSFQDCNS,'') '买家是否签订承诺书',isnull(Y_MJCNSQDSJ,'') '买家承诺书签订时间',Y_YSYDDBH '原始预订单号',Y_YSYDDDLYX '买家登录邮箱',Y_YSYDDJSZHLX '买家结算账户类型',Y_YSYDDMJJSBH '买家角色编号'  from  dbo.AAA_ZBDBXXB where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ";
        DataTable dataTableDQSBuyer = DbHelperSQL.Query(strDQSBuyer).Tables[0];


        bool b = false;
        if (dataTableMMSYJ != null && dataTableMMSYJ.Rows.Count > 0)  //当前买卖是一家
        {
            b = DbHelperSQL.ExecuteSql("update AAA_ZBDBXXB set T_MJSFQDCNS='已签',T_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',Y_MJSFQDCNS='已签',Y_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "'") > 0;
        }
        else if (dataTableDQSSeller != null && dataTableDQSSeller.Rows.Count > 0)  //当前是卖家
        {
            b = DbHelperSQL.ExecuteSql("update AAA_ZBDBXXB set T_MJSFQDCNS='已签',T_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and T_YSTBDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ") > 0;
        }
        else if (dataTableDQSBuyer != null && dataTableDQSBuyer.Rows.Count > 0)//当前是买家
        {
            b = DbHelperSQL.ExecuteSql("update AAA_ZBDBXXB set Y_MJSFQDCNS='已签',Y_MJCNSQDSJ='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where Number='" + dataTable.Rows[0]["中标定标_Number"].ToString() + "' and Y_YSYDDDLYX='" + dataTable.Rows[0]["登录邮箱"].ToString() + "' ") > 0;

        }
        if (b == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "成功签订承诺书！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "承诺书签订失败！";
        }

        return dsreturn;

    }



    /// <summary>
    ///提交银行人员信息表
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet SubmitYHYHInfor(DataSet dsreturn, DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();
        string BankDLYX = dataTable.Rows[0]["银行登录账号"].ToString();

        #region//基础验证
      
        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }
        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion

        #region//判断同一银行登录账号下是否已存在此员工工号
        string strYGGH = "select * from AAA_YHRYXXB where YHDLZH ='" + BankDLYX.Trim() + "' and YGGH='" + YGGH.Trim() + "'";
       DataTable dataTableRepeat= DbHelperSQL.Query(strYGGH).Tables[0];
       if (dataTableRepeat!=null&&dataTableRepeat.Rows.Count > 0)
       {
           dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
           dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "员工工号" + YGGH + "已经被占用，请修改员工工号后重新提交！";
           return dsreturn;
       }
        #endregion



        string strNumber = PublicClass2013.GetNextNumberZZ("AAA_YHRYXXB", "");
        //插入数据
        string strSql = "insert AAA_YHRYXXB(Number,YHDLZH,YGLSJG,YGXM,YGGH,LXFS,LXDZ,SFYX,TJSJ) values('" + strNumber + "','" + dataTable.Rows[0]["银行登录账号"].ToString() + "','" + dataTable.Rows[0]["员工隶属机构"].ToString() + "','" + dataTable.Rows[0]["员工姓名"].ToString() + "','" + dataTable.Rows[0]["员工工号"].ToString() + "','" + dataTable.Rows[0]["联系方式"].ToString() + "','" + dataTable.Rows[0]["联系地址"].ToString() + "','是','"+DateTime.Now.ToString()+"') ";

        bool b = DbHelperSQL.ExecuteSql(strSql)>0;
       
        if (b == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息提交成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息提交失败！";
        }

        return dsreturn;

    }



    /// <summary>
    ///提交银行人员信息表
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet ModifyYHYHInfor(DataSet dsreturn, DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();
        string DLYX = dataTable.Rows[0]["银行登录账号"].ToString();

        #region//基础验证

        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }
        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion
        
        //插入数据
        string strSql = "update AAA_YHRYXXB set YHDLZH='" + dataTable.Rows[0]["银行登录账号"].ToString() + "',YGLSJG='" + dataTable.Rows[0]["员工隶属机构"].ToString() + "',YGXM='" + dataTable.Rows[0]["员工姓名"].ToString() + "',LXFS='" + dataTable.Rows[0]["联系方式"].ToString() + "',LXDZ='" + dataTable.Rows[0]["联系地址"].ToString() + "' where  Number='" + dataTable.Rows[0]["银行人员表Number"].ToString() + "'";

        bool b = DbHelperSQL.ExecuteSql(strSql) > 0;

        if (b == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息修改成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息修改失败！";
        }

        return dsreturn;

    }



    /// <summary>
    ///删除银行人员信息表
    /// </summary>
    /// <param name="dsreturn"></param>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    public DataSet DeleteYHYHInfor(DataSet dsreturn, DataTable dataTable)
    {
        string JSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
        string BankDLYX = dataTable.Rows[0]["银行登录账号"].ToString();
        string YGGH = dataTable.Rows[0]["员工工号"].ToString();

        #region//基础验证

        Hashtable JCYZ = PublicClass2013.GetParameterInfo();
        Hashtable JBXX = PublicClass2013.GetUserInfo(JSBH);
        if (JBXX["交易账户是否开通"].ToString() == "否")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您尚未开通交易账户，不能进行交易操作。";
            return dsreturn;
        }
        if (JBXX["是否休眠"].ToString().Trim() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于休眠状态，请进入账户维护中激活休眠账户！";
            return dsreturn;
        }

        if (JBXX["是否冻结"].ToString() == "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "您的交易账户处于冻结状态，请与平台服务人员联系！\n\r被冻结功能：" + JBXX["冻结功能项"].ToString();
            return dsreturn;
        }
        if (JCYZ["是否在服务时间内(工作时)"].ToString() != "是")
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "非交易时间，暂停交易！";
            return dsreturn;
        }

        #endregion

        #region//判断此员工是否有关联交易方
        string strSqlExits = "select * from dbo.AAA_DLZHXXB where I_GLYH = (select top 1 I_JYFMC from dbo.AAA_DLZHXXB where B_DLYX='" + dataTable.Rows[0]["银行登录账号"].ToString() + "') and I_GLYHGZRYGH =(select YGGH from AAA_YHRYXXB where Number='" + dataTable.Rows[0]["银行人员表Number"].ToString() + "')";
        DataTable dataTableExits = DbHelperSQL.Query(strSqlExits).Tables[0];
        if (dataTableExits != null && dataTableExits.Rows.Count > 0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已经有交易方关联此员工，不能删除！";
            return dsreturn;

        }
        #endregion

        #region//判断此员工是否已经产生业务
        string strYW = "select fzb.GLJJRXSYGGH,ydd.GLJJRYX from AAA_TBDAndYDD_FZB fzb left join AAA_YDDXXB ydd on fzb.Number=ydd.Number where fzb.GLSJBM='预订单' and ydd.ZT!='撤销' and fzb.GLJJRXSYGGH='" + YGGH.Trim() + "' and ydd.GLJJRYX='" + BankDLYX.Trim() + "' 	union all select fzb.GLJJRXSYGGH,tbd.GLJJRYX from AAA_TBDAndYDD_FZB fzb left join AAA_TBD tbd on fzb.Number=tbd.Number where fzb.GLSJBM='投标单' and tbd.ZT!='撤销' and fzb.GLJJRXSYGGH='" + YGGH.Trim() + "' and tbd.GLJJRYX='" + BankDLYX.Trim() + "'";
        DataSet dsYW = DbHelperSQL.Query(strYW);
        if (dsYW!=null&&dsYW.Tables[0].Rows.Count>0)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "已经有交易方关联此员工，不能删除！";
            return dsreturn;
        }
        #endregion

        //删除数据
        string strSql = "delete from AAA_YHRYXXB where Number='" + dataTable.Rows[0]["银行人员表Number"].ToString() + "'";

        bool b = DbHelperSQL.ExecuteSql(strSql) > 0;

        if (b == true)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "ok";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息删除成功！";
        }
        else
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "服务人员信息删除失败！";
        }

        return dsreturn;

    }

    //经纪人收益-收益详情 根据经纪人角色编号及员工工号获取不同交易方编号的收益累计
    public DataSet GetSYLJ(DataSet dsreturn, DataTable dataTable)
    {
        try
        {
            string JJRJSBH = dataTable.Rows[0]["经纪人角色编号"].ToString();
            string YGGH = dataTable.Rows[0]["员工工号"].ToString();

            string strSQL = "select 交易方编号,isnull(sum(收益金额),0.00) 收益累计 from ( select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自卖方收益' and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH.Trim() + "' union all  select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_THDYFHDXXB thd  on thd.number = zkmx.LYDH left join AAA_ZBDBXXB_FZB fzb on fzb.Number=thd.ZBDBXXBBH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_THDYFHDXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH.Trim() + "'  union all   select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_SELJSBH=fzb.YSTBDSelJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSTBDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自卖方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSTBDGLJJRXSYGGH='" + YGGH.Trim() + "' union all     select zkmx.Number, zkmx.LSCSSJ 收益产生时间,'交易方名称'=(select I_JYFMC from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),'交易方编号'=(select J_BUYJSBH from AAA_DLZHXXB where J_BUYJSBH=fzb.YSYDDBuyJSBH),zkmx.XZ 交易性质,isnull(zkmx.JE,0.00) 收益金额, isnull( fzb.YSYDDGLJJRXSYGGH ,'--')中标定标信息表辅助表员工工号 from AAA_ZKLSMXB zkmx left join AAA_ZBDBXXB_FZB fzb on fzb.Number=zkmx.LYDH  where  XM='经纪人收益' and SJLX='预' and LYYWLX='AAA_ZBDBXXB' and zkmx.XZ='来自买方收益'and JSBH='" + JJRJSBH.Trim() + "' and fzb.YSYDDGLJJRXSYGGH='" + YGGH.Trim() + "' ) as tab group by 交易方编号";
            DataSet ds = DbHelperSQL.Query(strSQL);
            DataTable table = ds.Tables[0].Copy();
            table.TableName="主表";
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "OK";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "查询成功!";
            dsreturn.Tables.Add(table);
            return dsreturn;
        }
        catch(Exception)
        {
            dsreturn.Tables["返回值单条"].Rows[0]["执行结果"] = "err";
            dsreturn.Tables["返回值单条"].Rows[0]["提示文本"] = "系统繁忙，请稍后再试！";
            return dsreturn;
        }
    }
}