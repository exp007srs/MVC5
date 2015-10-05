using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5_Sample.Controllers
{
    public class MVC5Controller : Controller
    {
        // GET: MVC5
        //public string Index()
        //{
        //    return "<b>DEFAULT ACTION</b>";
        //    //return View();
        //}

        // GET: MVC5

        public ActionResult Index()
        {
           return View(); //Returns Razor view named Index.cshtml.
        }
        
        // GET: /MVC5/Welcome/

        // "name" and "numTimes" -> Query string paramters.
        // "ID" -> URL paramter.
        public string Welcome(string name, int numTimes=1, int ID=1)
        {
            //return "<b>WELCOME ACTION</b>";
            return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is:" + numTimes + " and ID: " + ID);
        }
    }
}