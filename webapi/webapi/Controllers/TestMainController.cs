using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webapi.Models;


namespace webapi.Controllers
{
    public class TestMainController : Controller
    {

        //不行
        //String connectionString = ConfigurationManager.ConnectionStrings["sqlconnectionString"].ToString();

        //行
        String connectionString="Data Source =YANFAZHONGXIN2-\\SQLEXPRESS; Initial Catalog = webapitest; Integrated Security = True; MultipleActiveResultSets = True";

        // GET: TestMain/insert
        public ActionResult Insert()
        {
            
            return View();
        }

        // POST: TestMain/insert
        public void Insert2(String Name)
        {

            IDbConnection connection = new SqlConnection(connectionString);

            // var result = connection.Execute("Insert into Users values (@UserName, @Email, @Address)",
            //     new { UserName = "Tony", Email = "tony@qq.com", Address = "北京" });


            var result = connection.Execute("Insert into Book(Name) values (@Name)", Name);
                
            //return "测试";
        }





        //GET: TestMain/select
        public ActionResult select()
        {
            string query = "SELECT * FROM Book";
            IDbConnection connection = new SqlConnection(connectionString);
            List<Book> listbook=connection.Query<Book>(query).ToList();
            
            return View(listbook);
        }
        //GET: TestMain/details/5
        public ActionResult details(int id)
        {
            IDbConnection connection = new SqlConnection(connectionString);
            String query = "select * from book where Id=@id";
            
            Book book = connection.Query<Book>(query, new { id = id }).SingleOrDefault();

            return View(book);
        }


        //GET: TestMain/delete
        public String delete(int? id)
        {
            if (id == null)
            {
                return "传入的id值为空";
            }
           //删除（通过id ）
            String query = "DELETE FROM Book WHERE Id = @id";
            IDbConnection connection = new SqlConnection(connectionString);
           // connection.Execute(query, book);
            connection.Execute(query, new { id = id });
            return "删除成功";
        }
        




       
    }
}