using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using Demo1.Models;

namespace Demo1.Controllers
{
    public class ProductController : Controller
    {
        private readonly string cs = ConfigurationManager.ConnectionStrings["InventoryDB"].ConnectionString;
        // GET: Product
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ProductId, ProductName, Quantity, Price FROM dbo.Products ORDER BY ProductName", con);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        products.Add(ReadProduct(dr));
                    }
                }
            }

            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Products (ProductName, Quantity, Price) VALUES (@Name, @Quantity, @Price)", con);
                AddProductParameters(cmd, p);
                cmd.ExecuteNonQuery();
            }
            TempData["SuccessMessage"] = "Product added successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product product = FindProduct(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE dbo.Products SET ProductName=@Name, Quantity=@Quantity, Price=@Price WHERE ProductId=@Id", con);
                AddProductParameters(cmd, p);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = p.Id;
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return HttpNotFound();
                }
            }
            TempData["SuccessMessage"] = "Product updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            Product product = FindProduct(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Products WHERE ProductId=@Id", con);
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                if (cmd.ExecuteNonQuery() == 0)
                {
                    return HttpNotFound();
                }
            }
            TempData["SuccessMessage"] = "Product deleted successfully.";
            return RedirectToAction("Index");
        }

        private Product FindProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("SELECT ProductId, ProductName, Quantity, Price FROM dbo.Products WHERE ProductId=@Id", con))
            {
                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return null;
                    return ReadProduct(reader);
                }
            }
        }

        private static Product ReadProduct(SqlDataReader reader)
        {
            return new Product
            {
                Id = Convert.ToInt32(reader["ProductId"]),
                Name = Convert.ToString(reader["ProductName"]),
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToInt32(reader["Price"])
            };
        }

        private static void AddProductParameters(SqlCommand command, Product product)
        {
            command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = product.Name;
            command.Parameters.Add("@Quantity", SqlDbType.Int).Value = product.Quantity;
            command.Parameters.Add("@Price", SqlDbType.Int).Value = product.Price;
        }
    }
}
