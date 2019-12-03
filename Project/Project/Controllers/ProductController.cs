using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Asp.NetMVC;Integrated Security=True"; //connectio to DB


        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblProdcut = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) //by what connection 
            {
                sqlConn.Open(); //open connection 
                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product", sqlConn); //query for select all products
                sqlDa.Fill(dtblProdcut); //filling object with data
            }
                return View(dtblProdcut); //passing data to View 
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
