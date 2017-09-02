using Business;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class DefaultController : Controller
    {
        //
        // GET: /Default/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult LineStation(string RoadLine)
        {
            if (string.IsNullOrEmpty(RoadLine))
                return View("Index");
            BusLineModel list = new BusLineModel();
            list = BusLineBusiness.Instance.GetBusStation(RoadLine);
            return View(list);
        }

        public string Query(string stopid)
        {
            if (string.IsNullOrEmpty(stopid))
                return "站点不能为空";
            var msg = BusLineBusiness.Instance.GetBusOnline(stopid);
            return msg;
        }
    }
}
