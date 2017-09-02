using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BusLineModel
    {
        public int Count { get; set; }

        public List<StationInfo> data { get; set; }
    }
}
