﻿using Microsoft.AspNetCore.Mvc;

namespace Anahtar_oluşturma.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }

        public IActionResult Error()
        {
            return View(); 
        }
    }
}
