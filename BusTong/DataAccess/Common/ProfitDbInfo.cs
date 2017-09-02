using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public class ProfitDbInfo : BaseDbInfo
    { 
        public static ProfitDbInfo CreateInstance()
        {
            ProfitDbInfo instance = new ProfitDbInfo();
            return instance;
        }
        /// <summary>
        /// 根据链接构建实例
        /// </summary>
        private ProfitDbInfo()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            db = factory.Create(DBConfig.Instance.strProfitDBConnection);
            ExecuteResult = new DbResult();

            Procdbcomm = db.DbProviderFactory.CreateCommand();
            listDbParameter = new List<DbParameter>();
        }
    }
}
