using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project.Models;
using System.IO;
using Newtonsoft.Json;


namespace Project.Controllers
{
    public class ProductController : Controller
    {
        string connectionString = @"Data Source=.;Initial Catalog=Asp.NetMVC;Integrated Security=True"; //connection to DB
        


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




        // GET: Product/Edit/5 ->
        public ActionResult Edit(int id)
        {
            ProductModel productModel = new ProductModel(); //
            DataTable dtblProduct = new DataTable(); // 
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) // 
            {
                sqlConn.Open(); // 

                string query = "SELECT * FROM Product WHERE Id = @Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(query, sqlConn); //
                sqlDa.SelectCommand.Parameters.AddWithValue("@Id", id);
                sqlDa.Fill(dtblProduct); // 
            }
            if(dtblProduct.Rows.Count == 1)
            {
                productModel.Id = Convert.ToInt32(dtblProduct.Rows[0][0].ToString()); // 
                productModel.Name = dtblProduct.Rows[0][1].ToString();
                productModel.Description = dtblProduct.Rows[0][2].ToString();
                productModel.Category = dtblProduct.Rows[0][3].ToString();
                productModel.Manufacturer= dtblProduct.Rows[0][4].ToString();
                productModel.Supplier= dtblProduct.Rows[0][5].ToString();
                productModel.Price = Convert.ToDecimal(dtblProduct.Rows[0][6].ToString());

                return View(productModel); //
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Product/Edit/5 ->
        [HttpPost]
        public ActionResult Edit(ProductModel product)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) //by which connection
            {
                sqlConn.Open(); //open connection

                string query = "UPDATE Product SET Name = @Name, Description = @Description, Category = @Category, Manufacturer = @Manufacturer, Supplier = @Supplier, Price = @Price WHERE Id = @Id"; //query for edit new product

                SqlCommand sqlCmd = new SqlCommand(query, sqlConn); //handling query over connection

                sqlCmd.Parameters.AddWithValue("@Id", product.Id); //adding values
                sqlCmd.Parameters.AddWithValue("@Name", product.Name); 
                sqlCmd.Parameters.AddWithValue("@Description", product.Description);
                sqlCmd.Parameters.AddWithValue("@Category", product.Category);
                sqlCmd.Parameters.AddWithValue("@Manufacturer", product.Manufacturer);
                sqlCmd.Parameters.AddWithValue("@Supplier", product.Supplier);
                sqlCmd.Parameters.AddWithValue("@Price", product.Price);

                sqlCmd.ExecuteNonQuery(); //execute query

                sqlConn.Close(); //close connection

            }

            return RedirectToAction("Index");
        }




        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString)) //by which connection
            {
                sqlConn.Open(); //open connection

                string query = "DELETE FROm Product WHERE Id = @Id"; //query for delete new product

                SqlCommand sqlCmd = new SqlCommand(query, sqlConn); //handling query over connection

                sqlCmd.Parameters.AddWithValue("@Id", id); //passing parameter

                sqlCmd.ExecuteNonQuery(); //execute query

                sqlConn.Close(); //close connection
            }

            return RedirectToAction("Index");
        }


        //GET: Product from JSON file
        
        private myDemoEntities mde = new myDemoEntities();
        public ActionResult Import(HttpPostedFileBase jsonFile)
        {
            if(! Path.GetFileName(jsonFile.FileName).EndsWith(".json"))
            {
                ViewBag.Error = "Invalid file type";
            }
            else
            {
                jsonFile.SaveAs(Server.MapPath("~/JSONFiles/" + Path.GetFileName(jsonFile.FileName)) );
                StreamReader streamReader = new StreamReader(Server.MapPath("~/JSONFiles/" + Path.GetFileName(jsonFile.FileName)) );
                string data = streamReader.ReadToEnd();

                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(data);

                products.ForEach(p => {
                    Product product = new Product()
                    {
                        Name = p.Name,
                        Description = p.Description,
                        Category = p.Category,
                        Manufacturer = p.Manufacturer,
                        Supplier = p.Supplier,
                        Price = p.Price  
                    };
                    mde.Products.Add(product);
                    mde.SaveChanges();
                });
                ViewBag.Success = "Success";
            }
            return RedirectToAction("Index");
        }


        
    }
}
