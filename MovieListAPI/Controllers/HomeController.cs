using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using DataAccessLayer.Context;

namespace MovieListAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
               return View();
        }
    }
}
