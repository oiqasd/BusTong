using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Common
{
   public class SqlHelper
    {
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="strTable">查询表</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="steSelect">查询内容</param>
        /// <param name="strOrderby">排序字段</param>
        /// <param name="strSort">排序</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
       public static DataSet SqlParamter(string strTable, string strWhere, string strSelect, string strOrderby, string strSort, int pageIndex, int pageSize)
        {
            BaseDbInfo dbInfo = BusDbInfo.CreateInstance();
            //存储过程中的where条件
            StringBuilder whereSb = new StringBuilder();
            //查询数据的语句
            StringBuilder dataListSB = new StringBuilder();
            //查询统计信息的语句
            StringBuilder totalInfoSB = new StringBuilder();
            whereSb.AppendFormat(" from {0} where 1=1 ", strTable);

            //-----------------------组织查询条件开始---------------------//

            whereSb.Append(strWhere);
            //-----------------------组织查询条件结束---------------------//
            //-----------------------组织查询数据开始---------------------//
            dataListSB.Append("select * from (");
            dataListSB.AppendFormat(@"select {0}", strSelect);
            //排序
            dataListSB.AppendFormat(" ROW_NUMBER() OVER(ORDER BY {0} {1}) as rowno ", strOrderby, strSort);
            dataListSB.Append(whereSb);
            int page_begin = (pageIndex - 1) * pageSize + 1;
            int page_end = page_begin + pageSize - 1;
            dataListSB.AppendFormat(") tb where rowno between {0} and {1}  order by rowno;", page_begin, page_end);
            //-----------------------组织查询数据结束---------------------//

            //-----------------------组织查询统计信息开始---------------------//
            totalInfoSB.AppendFormat("select *, ((allrows-1)/{0} + 1) allpages from( select count(*)  allrows ", pageSize);
            totalInfoSB.Append(whereSb);
            totalInfoSB.Append(") tb ");
            //-----------------------组织查询统计信息结束---------------------//
            DataSet ds = DbHelper.ExecuteDataSet(dbInfo, totalInfoSB.ToString(), dataListSB.ToString());
            return ds;
        }
    }
}
