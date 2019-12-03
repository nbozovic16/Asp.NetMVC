using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Asp.NetMVC;Integrated Security=True"; //connectio to DB
        


        // GET: Product -> this route get all products from DB and pass it to the view
        [HttpGet]
        public ActionResult Index()
        {
            DataTable dtblProdcut = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) //by which connection 
            {
                sqlConn.Open(); //open connection 

                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT * FROM Product", sqlConn); //query for select all products
                sqlDa.Fill(dtblProdcut); //filling object with data

                sqlConn.Close(); //close connection
            }
                return View(dtblProdcut); //passing data to View 
        }

       

        // GET: Product/Create -> this route get the form for insert new prodcut
        [HttpGet]
        public ActionResult Create()
        {
            return View(new ProductModel());
        }

        // POST: Product/Create -> this route post data to DB
        [HttpPost]
        public ActionResult Create(ProductModel product)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) //by which connection
            {
                sqlConn.Open(); //open connection

                string query = "INSERT INTO Product VALUES(@Name, @Description, @Category, @Manufacturer, @Supplier, @Price)"; //query for insert new product

                SqlCommand sqlCmd = new SqlCommand(query, sqlConn); //handling query over connection
                sqlCmd.Parameters.AddWithValue("@Name", product.Name); //adding values
                sqlCmd.Parameters.AddWithValue("@Description", product.Description);
                sqlCmd.Parameters.AddWithValue("@Category", product.Category);
                sqlCmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
                sqlCmd.Parameters.AddWithValue("@Supplier", product.Supplier);
                sqlCmd.Parameters.AddWithValue("@Price", product.Price);

                sqlCmd.ExecuteNonQuery(); //execute query

                sqlConn.Close(); //close connection

            }
                return RedirectToAction("Index"); //redirection 
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
