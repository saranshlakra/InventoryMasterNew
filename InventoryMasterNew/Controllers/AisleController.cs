using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using InventoryMasterNew.Models;
using System.Web.Script.Serialization;

namespace InventoryMasterNew.Controllers
{
    public class AisleController : Controller
    {
        // GET: Aisle

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AisleController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44382/api/");
        }

        //objective: communicate with our aisle data api to retrieve a list of aisle
        //curl https://localhost:44382/api/Aisledata/listAisle
        public ActionResult List()
        {
            string url = "aisledata/listaisle";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AisleDto> aisles = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;
            //IEnumerable<ItemDto> items
            //     = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            //Debug.WriteLine("Number of aisle received : ");
            //Debug.WriteLine(aisle.Count());
            return View(aisles);
        }

        // GET: Aisle/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Aisle/Create
        public ActionResult Create()
        {

           
            return View();
        }

        // POST: Aisle/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Aisle/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Aisle/Edit/5
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

        // GET: Aisle/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Aisle/Delete/5
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
