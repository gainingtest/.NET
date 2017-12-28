using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using webapi.Models;
using webapi.Models.dto;



namespace webapi.Controllers
{
    public class TaxiController : Controller
    {

        String connectionString = "Data Source =YANFAZHONGXIN2-\\SQLEXPRESS; Initial Catalog = webapitest; Integrated Security = True; MultipleActiveResultSets = True";
        

        // GET: Taxi/taxiSelectAll
        public string taxiSelectAll()
        { 
            String sql="select * from taxi";
            IDbConnection connection = new SqlConnection(connectionString);
            List<Taxi> listTaxi = connection.Query<Taxi>(sql).ToList();
            int sum = 0;
            //循环算数
            foreach(Taxi taxi in listTaxi)
            {
                sum=sum+int.Parse(taxi.money);
            }
            Dictionary<string, object> summoney = new Dictionary<string, object>();
            summoney.Add("summoney", sum);
            Message message= new Message(true,"查询成功",listTaxi,summoney);
            return toJson(message);
        }

        //post:/Taxi/taxidelete
        public string taxidelete(int? id)
        {
            String sql = "delete from taxi where id=@id";
            if (id == null)
            {
                sql = "delete from taxi";
            }
            IDbConnection connection = new SqlConnection(connectionString);
            int result=connection.Execute(sql,new { id=id});
            if (result > 0)
            {
                //删除
                Message message = new Message(true, "删除成功", null, null);
                return toJson(message);
            }
            else {
                Message message = new Message(false, "删除失败", null, null);
                return toJson(message);
            }

        }

        //post:/Taxi/taxidosubmit
        public string taxidosubmit(int? carnum, string date, string time, string money)
        {
            if ("".Equals(date) || "".Equals(time) || "".Equals(money)||null== carnum)
            {
                Message message = new Message(false, "不能有为空项", null, null);
                return toJson(message);
            }
            //进行存储
            IDbConnection connection = new SqlConnection(connectionString);
            //逻辑判断1.车号不能重复；2.同一天只能2张票;3.同一天的票中不能有时间交叉的
            string sql = "select * from taxi where taxinum=@carnum";
            List<Taxi> listTaxi = connection.Query<Taxi>(sql,new { carnum=carnum}).ToList();
            int result1 = listTaxi.Count;
            if (result1 > 0) {
                Message message = new Message(false, "亲，车牌号重复", null, null);
                return toJson(message);
            }
            sql = "select * from taxi where date=@date";
            List<Taxi> listTaxi2=connection.Query<Taxi>(sql, new { date = date }).ToList();
            result1 = listTaxi2.Count;
            if (result1 == 2)
            {
                Message message = new Message(false, "亲，日期"+date+"已经存在两张了", null, null);
                return toJson(message);
            }
            if (result1 == 1)
            {
                //判断日期是否包含
                foreach (Taxi taxi in listTaxi2)
                {
                    if (!separatCompare(taxi.time, time))
                    {
                        Message message = new Message(false, "亲，再换一张吧，时间包含了", null, null);
                        return toJson(message);
                    }
                }
            }
            sql = "insert into taxi values(@carnum,@date,@time,@money)";
            int result = connection.Execute(sql, new { carnum = carnum, date = date, time = time, money = money });
            if (result > 0)
            {
                return toJson(new Message(true, "添加成功", null, null));
            }
            else
            {
                return toJson(new Message(false, "添加失败", null, null));
            }
        }
        public static String toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            // Console.WriteLine(str);
            //HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return str;
        }

        private static bool separatCompare(String time, String time2)
        {
            
            int req1 = int.Parse(time.Substring(0, 2));
            int req2 = int.Parse(time.Substring(2, 2));

            int old1 = int.Parse(time2.Substring(0, 2));
            int old2 = int.Parse(time2.Substring(2, 2));
            if (old1 >= req1 && old1 <= req2)
            {
                return false;
            }
            if (old2 >= req1 && req2 >= old2)
            {
                return false;
            }
            if (req1 >= old1 && req1 <= old2) {
                return false;
            }
            if (req2 >= old1 && req2 <= old2)
            {
                return false;
            }
            return true;
        }
    }
}