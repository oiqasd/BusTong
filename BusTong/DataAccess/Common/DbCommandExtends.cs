using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public static class DbCommandExtends
    {
        public static void AddInOutParameter(this Database db, DbCommand Procdbcomm, string name, int size)
        {
            db.AddParameter(Procdbcomm, name, DbType.String, size, ParameterDirection.InputOutput, false, 0, 0, "", DataRowVersion.Default, "");
        }



    }
}
