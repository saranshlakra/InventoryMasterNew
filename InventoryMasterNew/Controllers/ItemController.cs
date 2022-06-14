using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using InventoryMasterNew.Models;
using System.Web.Script.Serialization;
using InventoryMasterNew.Models.ViewModels;

namespace InventoryMasterNew.Controllers
{
    public class ItemController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ItemController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44382/api/");
        }

        //objective: communicate with our item data api to retrieve a list of items 
        //curl https://localhost:44382/api/itemdata/listitems
        // GET: Item
        public ActionResult List()
        {
            //HttpClient client = new HttpClient() { };
            //string url = "https://localhost:44382/Itemdata/itemlist";
            //HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("THe response code is");
            //Debug.WriteLine(response.StatusCode);

            //IEnumerable<ItemDto> items = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            //Debug.WriteLine("Number of items");
            //Debug.WriteLine(items.Count());

            //objective: communicate with our item data api to retrieve a list of items
            //curl https://localhost:44324/api/itemdata/listitems


            string url = "itemdata/listitems";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ItemDto> items
                = response.Content.ReadAsAsync<IEnumerable<ItemDto>>().Result;
            //Debug.WriteLine("Number of item received : ");
            //Debug.WriteLine(items.Count());



            return View(items);
        }

        // GET: Item/Details/5
        public ActionResult Details(int id)
        {
            ItemDto ViewModel = new ItemDto();

            //objective: communicate with our Item data api to retrieve one Item
            //curl https://localhost:44382/api/itemdata/finditem/{id}

            string url = "itemdata/finditem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

          //  Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ItemDto SelectedItem = response.Content.ReadAsAsync<ItemDto>().Result;
          //  Debug.WriteLine("item received : ");
          //  Debug.WriteLine(SelectedItem.ItemName);

            ViewModel = SelectedItem;

            ////show associated aisle with this item
            //url = "aisledata/listaisleforitem/" + id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<AisleDto> ResponsibleAisle = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;

          //  ViewModel.ResponsibleKeepers = ResponsibleAisle;

            //url = "keeperdata/listkeepersnotcaringforitem/" + id;
            //response = client.GetAsync(url).Result;
            //IEnumerable<AisleDto> AvailableKeepers = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;

            //ViewModel.AvailableAisle = AvailableAisle;


            return View(ViewModel);
        }

        // GET: Item/Create
        public ActionResult New()
        {
            ItemViewModel item = new ItemViewModel();
            
            // it will find view with the name of method if view() is empty
            string url = "aisledata/ListAisle";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<AisleDto> Aisles = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;

            item.AisleList = Aisles;

            return View(item);
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create( Item item )
        {
            //Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(item.itemName);
            //objective: add a new item into our system using the API
            //curl -H "Content-Type:application/json" -d @item.json https://localhost:44324/api/animaldata/addanimal 
            string url = "itemdata/AddItem";


            string jsonpayload = jss.Serialize(item);
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

        // GET: Item/Edit/5
        public ActionResult Edit(int id)
        {

            ItemViewModel ViewModelDto = new ItemViewModel();

            
            string url = "itemdata/finditem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ItemDto SelectedItem = response.Content.ReadAsAsync<ItemDto>().Result;
            ViewModelDto.Id = SelectedItem.Id;
            ViewModelDto.ItemName = SelectedItem.ItemName;
            ViewModelDto.ItemType = SelectedItem.ItemType;
            ViewModelDto.BBD = SelectedItem.BBD;
            ViewModelDto.ItemCount = SelectedItem.ItemCount;


           url = "aisledata/ListAisle";
           response = client.GetAsync(url).Result;

            IEnumerable<AisleDto> Aisles = response.Content.ReadAsAsync<IEnumerable<AisleDto>>().Result;

            ViewModelDto.AisleList = Aisles;
            return View(ViewModelDto);
        }

        // POST: Item/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Item item)
        {
            string url = "itemdata/updateitem/" + id;
            string jsonpayload = jss.Serialize(item);
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

        // GET: Item/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "itemdata/finditem/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ItemDto selecteditem = response.Content.ReadAsAsync<ItemDto>().Result;
            return View(selecteditem);
        }

        // POST: Item/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {

            string url = "itemdata/deleteitem/" + id;
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
