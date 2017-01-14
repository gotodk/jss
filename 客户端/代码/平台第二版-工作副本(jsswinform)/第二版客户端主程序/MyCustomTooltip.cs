using System;

using Infragistics.UltraChart.Resources;

namespace  ChartSamplesExplorerCS.LabelsTitlesTooltips
{
	/// <summary>
	/// Summary description for MyCustomTooltip.
	/// </summary>
	public class MyCustomTooltip : IRenderLabel
	{
		public MyCustomTooltip()
		{
			
		}
		#region IRenderLabel Members

		public string ToString(System.Collections.Hashtable Context)
		{
			double dataValue = (double)Context["DATA_VALUE"];
            string htlx = Context["ITEM_LABEL"].ToString();
            string time = Convert.ToDateTime(Context["SERIES_LABEL"]).ToString("yyyy��MM��dd��HHʱmm��ss��");
            return "��ͬ���ͣ�" + htlx + "��ʱ�䣺" + time + "���б�۸�" + dataValue.ToString("#0.00")+"Ԫ";

            //if (dataValue > 75)
            //{ return dataValue.ToString() + "[Very High]"; }
            //else if (dataValue > 50)
            //{ return dataValue.ToString() + "[High]"; }
            //else if (dataValue > 25)
            //{ return dataValue.ToString() + "[Medium]"; }
            //else if (dataValue >= 0)
            //{ return dataValue.ToString() + "[Low]"; }
            //else
            //{ return dataValue.ToString() + "[Negative]"; }
		}

		#endregion
	}
}
