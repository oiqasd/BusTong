using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopJet.FrameWork.Helper;
using TopJet.FrameWork.Helper.Json;

namespace ConsoleApplication1
{
    public class shBusQuery
    {
        //http://webapp.shbustong.com:56008/MobileWeb/ForecastChange.aspx?stopid=xh001
        #region Instance

        private static shBusQuery _instance;
        private static object _syncobject = new object();

        public static shBusQuery Instance
        {
            get
            {
                if (_instance == null)
                    lock (_syncobject)
                    {
                        if (_instance == null)
                            _instance = new shBusQuery();
                    }
                return _instance;
            }
        }
        #endregion

         
        /// <summary>
        /// 路线站点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public BusRespose<T> BusRequestLine<T>(int type, string data)
        {
            string url = string.Empty;
            string SendData = string.Empty;

            switch (type)
            {

                case 1:
                    url = "http://webapp.shbustong.com:56008/MobileWeb/Handler.ashx";
                    SendData = "Method=station&roadline=" + data;
                    break;
                case 2:
                    url = "http://webapp.shbustong.com:56008/MobileWeb/ForecastChange.aspx";
                    SendData = "stopid=" + data;
                    break;
                case 3:
                    url = "http://webapp.shbustong.com:56008/WebService.asmx/getRoadLineInfoByRoadLineForMobile";
                    SendData = "{RoadLine:700,ToDirection:up}";
                    break;
            }

            BusRespose<T> response = new BusRespose<T>();
            HttpResquestEntity httpResquestEntity = new HttpResquestEntity();
            httpResquestEntity.Url = url;//reqUrl + messageType + "/" + actionName;
            httpResquestEntity.SendData = SendData;
            httpResquestEntity.MethodType = HttpMethodType.POST;
            httpResquestEntity.URLEncoding = Encoding.UTF8;
            httpResquestEntity.ContentType = HttpContentType.QueryString;
            httpResquestEntity.Timeout = 30000;
            HttpResponseEntity httpResponseEntity = HttpRequestHelper.SendValue(httpResquestEntity);
            if (httpResponseEntity.ResponseHttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                response.ResultCode = -99999;
                response.ResultMsg = "响应失败" + httpResponseEntity.ResponseHttpStatusCode;

                return response;
            }
            response.data = JsonHelper.Deserialize<T>(httpResponseEntity.ResponseContext);

            return response;
        }

    }


    public class BusRespose<T>
    {
        public int ResultCode { get; set; }
        public string ResultMsg { get; set; }
        public T data { get; set; }
    }
}
