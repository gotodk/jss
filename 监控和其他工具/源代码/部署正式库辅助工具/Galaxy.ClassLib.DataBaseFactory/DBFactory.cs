namespace Galaxy.ClassLib.DataBaseFactory
{
	/// <summary>
	/// �����������Դ��������,Ҳ���Ǿ��幤����ɫ
	/// </summary>
	public class DBFactory : I_DBFactory
	{
		/// <summary>
        /// ��ȡ���ݿ�������ʵ��,������Access���ݿ�
		/// </summary>
        public I_Dblink DbLinkAccess(string ConnConfig)
		{
            return new Dblink_Access_Main(ConnConfig);
		}

        /// <summary>
        /// ��ȡ���ݿ�������ʵ��,����sql���ݿ�
        /// </summary>
        public I_Dblink DbLinkSqlMain(string ConnConfig)
        {
            return new Dblink_Sql_Main(ConnConfig);
        }
	}
}
