using System;
using System.Windows.Forms;
using System.Collections;
using 客户端主程序.SubForm.NewCenterForm.GZZD;

namespace 客户端主程序.SubForm.NewCenterForm.MuBan
{
    public partial class UCFMPTZHKTFJ : UserControl
    {

        public UCFMPTZHKTFJ()
        {
            InitializeComponent();
        }

        private void UCdayindemo_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// 协议查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lablRemJJRLXDH_Click(object sender, EventArgs e)
        {
            Label exampleLable = (Label)sender;
            Hashtable htcs = new Hashtable();
            XSRM xs;
            switch (exampleLable.Text.Replace("\r\n", "").Trim())
            {
                case "1、《富美集团中国商品批发交易平台章程》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/YJPTZC/JYPTZC.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台章程", htcs);
                    xs.ShowDialog();
                    break;
                case "2、《富美集团中国商品批发交易平台交易规则》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTJYGZ.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易规则", htcs);
                    xs.ShowDialog();
                    break;
                case "3、《富美集团中国商品批发交易平台交易商品管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTSPGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易商品管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "4、《富美集团中国商品批发交易平台交易账户管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTJYZHGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易账户管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "5、《富美集团中国商品批发交易平台业务资料管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/YWZLGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台业务资料管理规定", htcs);
                    xs.ShowDialog();
                    break;              
                case "6、《富美集团中国商品批发交易平台经纪人管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JJRGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台经纪人管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "7、《富美集团中国商品批发交易平台交易方管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTMMJGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易方管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "8、《富美集团中国商品批发交易平台交易方资金管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYZJGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易方资金管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "9、《富美集团中国商品批发交易平台用户信用等级评价管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/YHXYDJPJGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台用户信用等级评价管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "10、《富美集团中国商品批发交易平台<电子购货合同>管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/DAGHHTGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台《电子购货合同》管理规定", htcs);
                    xs.ShowDialog();
                    break;
              
                case "11、《富美集团中国商品批发交易平台货款收付管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/HKSFGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台货款收付管理规定", htcs);
                    xs.ShowDialog();
                    break;

                case "12、《富美集团中国商品批发交易平台违规与处罚管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTCFYWGGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台违规与处罚管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "13、《富美集团中国商品批发交易平台费用收取管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/FYSQGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台费用收取管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "14、《富美集团中国商品批发交易平台清盘管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/QPGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台清盘管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "15、《富美集团中国商品批发交易平台信息发布管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/JYPTXXFBGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台信息发布管理规定", htcs);
                    xs.ShowDialog();
                    break;
                case "16、《富美集团中国商品批发交易平台交易安全管理规定》":
                    htcs["数据地址"] = "http://" + DataControl.XMLConfig.GetConfig_NoENC("基本配置", "MSS").Split('|')[2] + "/pingtaiservices/PTSXWB/GLGD/YIAQGLGD.htm";
                    xs = new XSRM("富美集团中国商品批发交易平台交易安全管理规定", htcs);
                    xs.ShowDialog();
                    break;

            }
        }
    }
}
