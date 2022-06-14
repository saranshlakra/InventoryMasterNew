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

        // GET: Aisle/Details/1
        public ActionResult Details(int id)
        {
            AisleDto ViewModel = new AisleDto();

            //objective: communicate with our aisle data api to retrieve one aisle info
            //curl https://localhost:44382/api/aisledata/findaisle/{id}

            string url = "aisledata/findaisle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

           AisleDto SelectedAisle = response.Content.ReadAsAsync<AisleDto>().Result;
            //Debug.WriteLine("aisle received : ");
            //Debug.WriteLine(SelectedAisle.Name);

            ViewModel = SelectedAisle;

            return View(ViewModel);
        }

       // [HttpGet]
        // GET: Aisle/Create
        public ActionResult New()
        {
            AisleViewModel aisle = new AisleViewModel();

            // it will find view with the name of method if view() is empty
            string url = "aisledata/ListAisle";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AisleViewModel> Aisles = response.Content.ReadAsAsync<IEnumerable<AisleViewModel>>().Result;

          //  aisle.AisleList = Aisles;

            return View(aisle);
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(Aisle aisle)
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(item.itemName);
            //objective: add a new item into our system using the API
            //curl -H "Content-Type:application/json" -d @item.json https://localhost:44382/api/aisledata/addaisle
            string url = "aisledata/addAisle";


            string jsonpayload = jss.Serialize(aisle);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Aisle/Edit/5
        public ActionResult Edit(int id)
        {

            AisleViewModel ViewModelDto = new AisleViewModel();


            string url = "aisledata/findaisle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AisleDto SelectedItem = response.Content.ReadAsAsync<AisleDto>().Result;
            ViewModelDto.AisleId = SelectedItem.AisleId;
            ViewModelDto.Name = SelectedItem.Name;
            ViewModelDto.Desc = SelectedItem.Desc;
            ViewModelDto.AisleCap = SelectedItem.AisleCap;


            url = "aisledata/ListAisle";
            response = client.GetAsync(url).Result;

            IEnumerable<AisleDto> Aisles = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;

            ViewModelDto.AisleList = Aisles;
            return View(ViewModelDto);
        }

        // POST: Aisle/Edit/5
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection, object aisle)
        {

            string url = "aisledata/updateaisle/" + id;
            string jsonpayload = jss.Serialize(aisle);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Aisle/Delete/1
        public ActionResult Delete(int id)
        {
            string url = "aisledata/findaisle/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AisleDto selectedaisle = response.Content.ReadAsAsync<AisleDto>().Result;
            return View(selectedaisle);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            string url = "aisledata/deleteaisle/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
