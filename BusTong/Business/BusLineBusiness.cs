
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Business.Helper;
using Model;
using Entity;
using TopJet.FrameWork.Helper;
using System.Text.RegularExpressions;

namespace Business
{
    public class BusLineBusiness
    {
        #region Instance

        private static BusLineBusiness _instance;
        private static object _syncobject = new object();

        public static BusLineBusiness Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncobject)
                    {
                        if (_instance == null)
                            _instance = new BusLineBusiness();
                    }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// 线路站点
        /// </summary>
        /// <param name="roadline"></param>
        /// <returns></returns>
        public BusLineModel GetBusStation(string roadline)
        {
            BusLineModel model = new BusLineModel();
            var dt = BusDataAccess.Instance.GetRoadLine(roadline);
            if (dt == null || dt.Rows.Count <= 0)
            {
                var response = shBusQuery.Instance.BusRequestLine<BusLineModel>(1, roadline);
                var rnum = response.data.Count;

                if (response.ResultCode == 0)
                {
                    if (rnum > 0)
                    {
                        var roadID = DataAccess.BusDataAccess.Instance.AddRoadLine(roadline, rnum);

                        foreach (var m in response.data.data)
                        {
                            var roadStation = new StationInfo()
                            {
                                Area = m.Area,
                                District = m.District,
                                RoadName = m.RoadName,
                                StationAddress = m.StationAddress,
                                StationName = m.StationName,
                                PathDirection = m.PathDirection,
                                lineList = m.lineList,
                                stopid = m.stopid
                            };

                            var SnID = BusDataAccess.Instance.AddStationName(roadStation);
                            if (SnID > 0)
                            {
                                BusDataAccess.Instance.AddRoadStation(roadID, SnID);
                            }
                        }

                        model = response.data;
                    }
                    else
                    {
                        response.ResultCode = -1;
                        response.ResultMsg = "查无此路线";
                    }
                }
            }
            else
            {
                model.data = new ModelHelpler<StationInfo>().FillModel(dt);

                model.Count = dt.Rows.Count;
            }

            if (model != null && model.data != null)
            {
                model.data.Sort((x, y) =>
                {
                    return x.StationName.CompareTo(y.StationName);
                });
            }
            return model;

        }


        public string GetBusOnline(string stopid)
        {
            var response = shBusQuery.Instance.BusRequestLine<string>(2, stopid);

            var data = response.data;

            Regex rg = new Regex("(?<=(<form))[.\\s\\S]*?(?=(</form>))", RegexOptions.Multiline | RegexOptions.Singleline);

            var d = "<form" + rg.Match(data).Value + "</form>";

            return d;
        }
    }




}
