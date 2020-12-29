using BenFatto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BenFatto.Controllers {

    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            string FilterIp = HttpContext.Request.Query["Ip"].ToString();
            string FilterUser = HttpContext.Request.Query["User"].ToString();
            string FilterHora = HttpContext.Request.Query["Hora"].ToString();

            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectLista(FilterIp, FilterUser, FilterHora);

            return View(Lista);
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file) {
            DataLayer dl = new DataLayer();

            string Result = "";
            using (StreamReader reader = new StreamReader(file.OpenReadStream())) {
                string fileContent = reader.ReadToEnd();
                string[] Line = fileContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                Result = dl.InsertLista(Line);
            }

            return Ok(Result);
        }

        public IActionResult Change() {
            string FilterId = HttpContext.Request.Query["Id"].ToString();

            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectValue(FilterId);

            return View(Lista);
        }

        public JsonResult Salvar(Table d) {
            DataLayer dl = new DataLayer();
            bool x = dl.Salvar(d);
            if (x)
                return new JsonResult("True");
            else
                return new JsonResult("False");
        }

        public JsonResult Delete(Table d) {
            DataLayer dl = new DataLayer();
            bool x = dl.Delete(d);
            if (x)
                return new JsonResult("True");
            else
                return new JsonResult("False");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}