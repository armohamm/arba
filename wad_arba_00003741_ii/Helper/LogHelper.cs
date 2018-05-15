using ArbaShop.DAL.Entities;
using ArbaShop.DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using wad_arba_00003741_ii.Controllers;
using wad_arba_00003741_ii.Models;

namespace wad_arba_00003741_ii.Helper
{

    public class LogHelper
    {
        public static void log(BaseController baseController, Product productImport = null)
        {
            string ip = baseController.Request.UserHostAddress;
            string email = (string)(baseController.Session["User"] ?? "Anonymous");
            string action = baseController.ControllerContext.RouteData.Values["action"].ToString();
            string controller = baseController.ControllerContext.RouteData.Values["controller"].ToString();
            string httpMethod = baseController.ControllerContext.HttpContext.Request.HttpMethod;

            string param = "";

            if(productImport == null)
            {
                param = getParam(baseController);
            }
            else
            {
                param = getParam(baseController, productImport);
            }

            if (baseController.ModelState.Keys.Count > 0)
            {
                foreach(string key in baseController.ModelState.Keys){
                    string value = baseController.ModelState[key].Value.AttemptedValue;
                    param = param + "" + key + "=" + value + ",  ";
                }
            }
            
            DateTime now = DateTime.Now;
            long time = now.Ticks;

            var logRepo = new LogRepo();
            Log log = new Log
            {
                Id = 0,
                Date = time,
                Username = email,
                IP = ip,
                Controller = controller,               
                Action = action,
                HttpMethod = httpMethod,
                Params = param                                                                                   
            };

            logRepo.Create(log);
        }

        private static string getParam(BaseController baseController)
        {
            string param = "";

            if (baseController.ModelState.Keys.Count > 0)
            {
                foreach (string key in baseController.ModelState.Keys)
                {
                    string value = baseController.ModelState[key].Value.AttemptedValue;
                    param = param + "" + key + "=" + value + ",  ";
                }
            }

            return param;
        }

        private static string getParam(BaseController baseController, Product importProduct)
        {
            string param = "";

            param = param + "Name = " + importProduct.Name + ",  ";
            param = param + "Price = " + importProduct.Price + ",  ";
            param = param + "Category = " + importProduct.Category + ",  ";
            param = param + "Available = " + importProduct.Available + ",  ";

            return param;
        }

    }
}