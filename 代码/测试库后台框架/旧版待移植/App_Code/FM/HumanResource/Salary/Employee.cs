using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Hesion.Brick.Core;
using FMOP.DB;

/// <summary>
/// Employee 的摘要说明
/// </summary>
namespace FM.HumanResource.Salary
{
	/// <summary>
	/// 员工薪酬类
	/// </summary>
	public class Employee
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        private string empNumber = string.Empty;

        /// <summary>
        /// 薪酬计算单号
        /// </summary>
        private int ruleId = 0;

        /// <summary>
        /// 执行的年份
        /// </summary>
        private string year = string.Empty;

        /// <summary>
        /// 执行的月份
        /// </summary>
        private string month = string.Empty;

        /// <summary>
        /// 员工岗位类型
        /// </summary>
		private string jobType;

        /// <summary>
        /// 标准薪资
        /// </summary>
		private float standardSalary;

        /// <summary>
        /// 业绩考核得分
        /// </summary>
		private float achievementScore;

        /// <summary>
        /// 素质考核
        /// </summary>
		private float qualityScore;

        /// <summary>
        /// 扣款
        /// </summary>
		private float penalty;

        /// <summary>
        /// 奖励
        /// </summary>
		private float bounty;

        /// <summary>
        /// 旷工
        /// </summary>
		private int leaveHours;

		/// <summary>
		/// 薪酬计算规则
		/// </summary>
        private SalaryRule empSalaryRule;

        /// <summary>
        /// 实际出勤天数
        /// </summary>
        private float factDays;
        
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmpNumber
        {
            get
            {
                return empNumber;
            }
            set
            {
                empNumber = value;
            }
        }

		/// <summary>
		/// 员工岗位类型
		/// </summary>
		public string JobType{	
			get{
				return this.jobType;
			}	
			set{
				this.jobType = value;
			}	
		}
	
		/// <summary>
		/// 业绩考核得分
		/// </summary>
		public float AchievementScore{	
			get{
				return this.achievementScore;
			}	
			set{
				this.achievementScore = value;
			}	
		}
	
		/// <summary>
		/// 个人素质考核得分
		/// </summary>
		public float QualityScore{	
			get{
				return this.qualityScore;
			}	
			set{
				this.qualityScore = value;
			}	
		}
	
		/// <summary>
		/// 奖励金额
		/// </summary>
		public float Penalty{	
			get{
				return this.penalty;
			}	
			set{
				this.penalty = value;
			}	
		}
	
		/// <summary>
		/// 罚款金额
		/// </summary>
		public float Bounty{	
			get{
				return this.bounty;
			}	
			set{
				this.bounty = value;
			}	
		}
	
		/// <summary>
		/// 请假时数
		/// </summary>
		public int LeaveHours{	
			get{
				return this.leaveHours;
			}	
			set{
				this.leaveHours = value;
			}	
		}

        /// <summary>
        /// 薪酬计算规则
        /// </summary>
        public SalaryRule EmpSalaryRule
        {
            set
            {
                empSalaryRule = value;
            }
            get
            {
                return empSalaryRule;
            }
        }

        /// <summary>
        /// 实际出勤天数
        /// </summary>
        public float FactDays
        {
            set
            {
                factDays = value;
            }
            get
            {
                return factDays;
            }
        }

        /// <summary>
        // * 通过月度薪酬ID和员工号初始化员工薪酬类
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="RuleId"></param>
        public Employee(string Number, int RuleId)
        {
            //设定员工编号值
            empNumber = Number;

            //设定薪酬计算单号值
            ruleId = RuleId;

            if (empNumber != "" && ruleId.ToString() != "")
            {
                //设定年,月
                GetDate();

                //查询员工岗位类型
                jobType = GetJobs();

                //查询员工标准薪资
                standardSalary = GetStandardSalary();

                //薪酬计算规则
                empSalaryRule = SalaryRuleManager.GetRule(ruleId, jobType);

                //获取个人业绩考核得分
                achievementScore = GetAchievmentScore();

                //获取个人素质考核得分
                qualityScore = GetQualityScore();

                //获取奖励
                penalty = GetKoukuan(0);

                //获取扣款
                bounty = GetKoukuan(1);

                //获取请假时数
                leaveHours = GetLeaveHours();

                //实际出勤天数
                factDays = GetFactDays();
            }
            //else
            //{
            //    new Employee();   
            //}
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        private Employee()
        {

        }

        /// <summary>
        /// 获取员工岗位名称
        /// </summary>
        /// <returns></returns>
        private string GetJobs()
        {
            string type = string.Empty;
            string cmdText = "select YGZT,RYLB from HR_Employees where Number='" + empNumber + "'";
            DataSet result = new DataSet();
            try
            {
                result = DbHelperSQL.Query(cmdText);
                if (result != null && result.Tables[0] != null && result.Tables[0].Rows.Count > 0)
                {
                    type = result.Tables[0].Rows[0]["YGZT"].ToString();
                    if (type == "正式工")
                    {
                        type = result.Tables[0].Rows[0]["RYLB"].ToString();
                    }
                    else
                    {
                        type = "非正式人员";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

            return type;
        }

        /// <summary>
        /// 获取员工标准薪资
        /// </summary>
        /// <returns></returns>
        private float GetStandardSalary()
        {
            float pay = 0;
            string cmdText = "SELECT BZGZ FROM HR_Employees WHERE Number='" + empNumber + "'";
            SqlDataReader dr = null;
            
            try
            {
                dr = DbHelperSQL.ExecuteReader(cmdText);

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr[0].ToString() != "")
                        {
                            pay = float.Parse(dr[0].ToString());
                        }
                    }
                    closeReader(dr);
                }
            }
            catch (Exception ex)
            {
                closeReader(dr);
                Log.Error(ex.Message);
            }

            return pay;
        }

		/// <summary>
		// * 获取员工的业绩考核得分
		/// </summary>
		/// <returns></returns>
		private float GetAchievmentScore()
        {
            string ID = string.Empty;
            float score = 0;
            SqlDataReader dr = null;
            try
            {
                ID = GetTableId("HR_Achievement_Assess");
                if (ID != "" && empNumber != "")
                {
                    string cmdText = @"SELECT convert(numeric(6,2),convert(numeric(6,2),SUM(Hr_Achivement_History_Criterions.score)) / count(*) ) as Score
                                    FROM Hr_Achivement_History_Criterions INNER JOIN HR_Achievement_Assess ON Hr_Achivement_History_Criterions.AssessId=HR_Achievement_Assess.id
                                    INNER JOIN Hr_Achivement_History_Criterions_People ON Hr_Achivement_History_Criterions.id = Hr_Achivement_History_Criterions_People.History_Criterions_Id
                                    WHERE Hr_Achivement_History_Criterions_People.EmoployeeNo='" + empNumber + "' AND HR_Achievement_Assess.id=" + ID;
                    dr = DbHelperSQL.ExecuteReader(cmdText);
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString() != "")
                            {
                                score = float.Parse(dr[0].ToString());
                            }
                        }
                        closeReader(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                closeReader(dr);

                Log.Error(ex.Message);
            }

            return score;
		}
	
		/// <summary>
		// * 获取员工的个人素质考核得分
		/// </summary>
		/// <returns></returns>
		private float GetQualityScore()
        {
            string ID = string.Empty;
            float score = 0;
            SqlDataReader dr = null;
            try
            {
                ID = GetTableId("HR_Quality_Assess");
                if (ID != "" && empNumber != "")
                {
                    string cmdText = @"SELECT convert(numeric(6,2),convert(numeric(6,2),SUM(Hr_Quality_People.score)) / count(*) ) as Score
                               FROM Hr_Quality_People INNER JOIN HR_Quality_Assess ON Hr_Quality_People.AssessId=HR_Quality_Assess.id
                               WHERE Hr_Quality_People.Employee='" + empNumber + "' AND HR_Quality_Assess.id=" + ID;
                    dr = DbHelperSQL.ExecuteReader(cmdText);

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (dr[0].ToString() != "")
                            {
                                score = float.Parse(dr[0].ToString());
                            }
                        }
                        closeReader(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                closeReader(dr);
                Log.Error(ex.Message);
            }

            return score;
		}
	
		/// <summary>
		// * 获取员工的请假小时数
		/// </summary>
		/// <returns></returns>
		private int GetLeaveHours()
        {
            int Total = 0;
            int sj = 0;
            int bj = 0;
            int kg = 0;
            int addHours = 0;
            SqlDataReader dr = null;
            string cmdText = @"SELECT SJT,BJT,KGT,JBXS from XCGL_KQHZB_KQHZBXX INNER JOIN  XCGL_KQHZB ON XCGL_KQHZB_KQHZBXX.parentNumber = XCGL_KQHZB.Number
                               WHERE XCGL_KQHZB.NF=" + year + " AND XCGL_KQHZB.YF=" + month + " AND XCGL_KQHZB_KQHZBXX.YGBH='" + empNumber + "'";
            try
            {
                dr = DbHelperSQL.ExecuteReader(cmdText);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["SJT"].ToString() != "")
                        {
                            sj = int.Parse(dr["SJT"].ToString()) * 8;
                        }

                        if (dr["BJT"].ToString() != "")
                        {
                            bj = int.Parse(dr["BJT"].ToString()) * 8;
                        }

                        if (dr["KGT"].ToString() != "")
                        {
                            kg = int.Parse(dr["KGT"].ToString()) * 8;
                        }

                        if (dr["JBXS"].ToString() != "")
                        {
                            addHours = int.Parse(dr["JBXS"].ToString());
                        }
                    }
                    closeReader(dr);
                }
                
                Total = sj + bj + kg - addHours;
            }
            catch (Exception ex)
            {
                closeReader(dr);

                Log.Error(ex.Message);
            }

            return Total;
		}

        /// <summary>
        /// 获取实际出勤天数
        /// </summary>
        /// <returns></returns>
        private float GetFactDays()
        {
            float days = 0;
            string cmdText = @"SELECT SJCQT FROM XCGL_KQHZB_KQHZBXX INNER JOIN  XCGL_KQHZB ON XCGL_KQHZB_KQHZBXX.parentNumber = XCGL_KQHZB.Number
                               WHERE XCGL_KQHZB.NF=" + year + " AND XCGL_KQHZB.YF=" + month + " AND XCGL_KQHZB_KQHZBXX.YGBH='" + empNumber + "'";
            SqlDataReader dr = null;

            try
            {
                dr = DbHelperSQL.ExecuteReader(cmdText);
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if(dr["SJCQT"].ToString() != "")
                        {
                            days = float.Parse (dr["SJCQT"].ToString());
                        }
                    }
                    closeReader(dr);
                }
            }
            catch (Exception ex)
            {
               closeReader(dr);
                Log.Error(ex.Message);
            }

            return days;
        }
	
		/// <summary>
		// * 获取员工奖励扣款金额
		/// </summary>
		/// <param name="type">0:奖励,1,罚款</param>
		/// <returns></returns>
		private float GetKoukuan(Int16 type)
        {
            float totalMoney = 0;
            string cmdText = @"SELECT JEY FROM XCGL_JLKKHZB_JLKKHZBMX INNER JOIN XCGL_JLKKHZB ON XCGL_JLKKHZB_JLKKHZBMX.parentNumber = XCGL_JLKKHZB.Number WHERE
                               XCGL_JLKKHZB.NF=" + year + " AND XCGL_JLKKHZB.YF=" + month + " AND XCGL_JLKKHZB_JLKKHZBMX.YGBH='" + empNumber + "'";
            SqlDataReader dr = null;

            try
            {
                if (type == 0)
                {
                    cmdText += " AND LX='奖'";
                }
                else
                {
                    cmdText += " AND LX='扣'";
                }

                dr = DbHelperSQL.ExecuteReader(cmdText);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (dr["JEY"].ToString() != "")
                        {
                            totalMoney = float.Parse(dr["JEY"].ToString());
                        }
                    }

                    closeReader(dr);
                }
            }
            catch (Exception ex)
            {
                closeReader(dr);
                Log.Error(ex.Message);
            }

            return totalMoney;
		}

        /// <summary>
        ///* 获取执行的日期
        /// </summary>
        private void GetDate()
        {
            string cmdText = string.Empty;
            SqlDataReader dr = null;

            try
            {
                cmdText = "select year,month from Hr_SalaryRule where ID='" + ruleId + "'";
                dr = DbHelperSQL.ExecuteReader(cmdText);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        year = dr["year"].ToString();
                        month = dr["month"].ToString();
                    }

                    closeReader(dr);
                }
            }
            catch (Exception ex)
            {
                closeReader(dr);
                Log.Error(ex.Message);
            }
        }

        /// <summary>
        ///* 通过表名，查询指定年月的ID
        /// </summary>
        /// <param name="table"></param>
        private string GetTableId(string tblName)
        {
            string ID = string.Empty;
            string cmdText = string.Empty;
            SqlDataReader dr = null;

            try
            {
                cmdText = "select id from " + tblName + " where year=" + year + " AND month=" + month;
                dr = DbHelperSQL.ExecuteReader(cmdText);
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ID = dr[0].ToString();
                    }
                    closeReader(dr);
                }
            }
            catch(Exception ex)
            {
                closeReader(dr);
                Log.Error(ex.Message);
            }

            return ID;
        }

        /// <summary>
        /// 关闭sqlDataReader连接
        /// </summary>
        /// <param name="dr"></param>
        private void closeReader(SqlDataReader dr)
        {
            if (dr != null && dr.IsClosed == false)
            {
                dr.Close();
            }
        }
    }
}