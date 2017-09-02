using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAccess.Common;
using Entity;

namespace DataAccess
{
    public class BusDataAccess
    {
        #region Instance

        private static BusDataAccess _instance;
        private static object _syncobject = new object();

        public static BusDataAccess Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncobject)
                    {
                        if (_instance == null)
                            _instance = new BusDataAccess();
                    }
                return _instance;
            }
        }
        #endregion

        public DataTable GetRoadLine(string RoadLine)
        {
            BusDbInfo db = BusDbInfo.CreateInstance();
            db.GetSqlStringCommand("SELECT c.* FROM dbo.RoadLine a JOIN dbo.RoadStation b ON a.ID=b.RoadID JOIN dbo.StationInfo c ON c.ID = b.StationID WHERE a.RoadLine=@RoadLine");
            db.AddInParameter("@RoadLine", DbType.String, RoadLine);
            return db.ExecuteDataSet().Tables[0];
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="RoadLine"></param>
        /// <returns></returns>
        public long AddRoadLine(string RoadLine, int Count)
        {
            BusDbInfo db = BusDbInfo.CreateInstance();
            db.GetSqlStringCommand("INSERT INTO RoadLine(RoadLine,Count) VALUES (@RoadLine,@Count); SELECT @@IDENTITY;");
            db.AddInParameter("@RoadLine", DbType.String, RoadLine);
            db.AddInParameter("@Count", DbType.Int32, Count);

            return Convert.ToInt64(db.ExecuteScalar());

        }


        public long AddStationName(StationInfo entity)
        {
            BusDbInfo db = BusDbInfo.CreateInstance();
            string sql = @"INSERT INTO dbo.StationInfo
                        ( StationName ,
                          Area ,
                          District ,
                          RoadName ,
                          StationAddress ,
                          PathDirection ,
                          lineList ,
                          stopid
                        )
                    VALUES  ( @StationName
                              ,@Area
                              ,@District 
                              ,@RoadName  
		                      ,@StationAddress  
                              ,@PathDirection 
                              ,@lineList 
                              ,@stopid  
                              )
                    SELECT SCOPE_IDENTITY(); ";
            db.GetSqlStringCommand(sql);
            db.AddInParameter("@StationName", DbType.String, entity.StationName);
            db.AddInParameter("@Area", DbType.String, entity.Area);
            db.AddInParameter("@District", DbType.String, entity.District);
            db.AddInParameter("@RoadName", DbType.String, entity.RoadName);
            db.AddInParameter("@StationAddress", DbType.String, entity.StationAddress);
            db.AddInParameter("@PathDirection", DbType.String, entity.PathDirection);
            db.AddInParameter("@lineList", DbType.String, entity.lineList);
            db.AddInParameter("@stopid", DbType.String, entity.stopid);
            return Convert.ToInt64(db.ExecuteScalar());
            // return (long)db.ExecuteScalar();
        }


        public int AddRoadStation(long RoadID, long StationID)
        {
            BusDbInfo db = BusDbInfo.CreateInstance();
            db.GetSqlStringCommand("INSERT INTO RoadStation(RoadID,StationID) VALUES (@RoadID,@StationID)");
            db.AddInParameter("@RoadID", DbType.Int64, RoadID);
            db.AddInParameter("@StationID", DbType.Int64, StationID);
            return db.ExecuteNonQuery();
        }
    }
}
