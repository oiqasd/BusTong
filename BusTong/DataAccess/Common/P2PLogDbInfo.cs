using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    /// <summary>
    /// xujf 2016-05-20，封装部分数据库操作功能
    /// 碰到复杂的情况可以直接使用企业类库的数据库操作，或者添加功能
    /// 不必要拘泥于这个类，这个类是将之前代码的相同之处进行了提取
    /// </summary>
    public class P2PLogDbInfo : BaseDbInfo
    {
        /// <summary>
        /// 创建常规的数据库操作对象
        /// </summary>
        /// <returns></returns>
        public static P2PLogDbInfo CreateInstance()
        {
            P2PLogDbInfo instance = new P2PLogDbInfo();
            return instance;
        }
        /// <summary>
        /// 根据链接构建实例
        /// </summary>
        private P2PLogDbInfo()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            db = factory.Create("LogDBConnStr");
            ExecuteResult = new DbResult();

            Procdbcomm = db.DbProviderFactory.CreateCommand();
            listDbParameter = new List<DbParameter>();
        }
    }
}
