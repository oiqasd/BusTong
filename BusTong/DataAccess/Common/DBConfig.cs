using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
    public class DBConfig
    {
        #region Instance
        private DBConfig()
        {
        }
        private static BaseConfig _instance;

        private static object _syncObject = new object();

        public static BaseConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObject)
                    {
                        if (_instance == null)
                        {
                            string env = ConfigurationManager.AppSettings["environment"];
                            switch (env)
                            {
                                case "fat":
                                    _instance = new FatDBConfig();
                                    break;
                                case "prd":
                                    _instance = new PrdDBConfig();
                                    break;
                                default:
                                    _instance = new DevDBConfig();
                                    break;
                            }
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion
    }

    public class BaseConfig
    {
        public string strDBConnection;

        public string strProfitDBConnection;
    }
    //开发环境
    public class DevDBConfig : BaseConfig
    {
        public DevDBConfig()
        {
            strDBConnection = "DevDBConnStr";
            strProfitDBConnection = "DevProfitDbConn";
        }
    }
    //测试环境
    public class FatDBConfig : BaseConfig
    {
        public FatDBConfig()
        {
            strDBConnection = "FatDBConnStr";
            strProfitDBConnection = "FatProfitDbConn";
        }
    }

    //生产环境
    public class PrdDBConfig : BaseConfig
    {
        public PrdDBConfig()
        {
            strDBConnection = "PrdDBConnStr";
            strProfitDBConnection = "PrdProfitDbConn";
        }
    }
}
