﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventario360.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.NombreUsuario = User.Identity.IsAuthenticated ? User.Identity.Name : "Invitado";
            ViewBag.EsAdmin = User.IsInRole("Administrador");
            ViewBag.EsProyectos = User.IsInRole("Proyectos");

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}