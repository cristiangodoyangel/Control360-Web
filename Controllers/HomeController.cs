using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Inventario360.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public ActionResult Index()
        {
            bool conexionExitosa = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    conn.Open();
                    conexionExitosa = (conn.State == ConnectionState.Open);
                }
            }
            catch (Exception ex)
            {
                conexionExitosa = false;
                ViewBag.Error = ex.Message; // opcional para debug
            }

            ViewBag.Conectado = conexionExitosa;
            return View();
        }
    }
}
