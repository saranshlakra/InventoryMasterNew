using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using InventoryMasterNew.Models;

namespace InventoryMasterNew.Controllers
{
    public class ItemDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all items in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all items in the database
        /// </returns>
        /// <example>
        /// GET: api/itemdata/listitem
        /// </example>

        [HttpGet]
        [ResponseType(typeof(ItemDto))]
        // GET: api/ItemData
        public IHttpActionResult ListItems()
        {
            List<Item> Items = db.Items.ToList();
            List<ItemDto> ItemDtos = new List<ItemDto>();

            
            Items.ForEach(a => ItemDtos.Add(new ItemDto()
            {
                Id = a.Id,
                ItemName = a.ItemName,
                ItemType = a.ItemType,
                ItemCount = a.ItemCount,
                BBD = a.BBD,
                AisleId = a.AisleId,
            }));

            return Ok(ItemDtos);
        }



        /// <summary>
        /// Gathers information about all items 
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all items in the database, 
        /// </returns>
        /// <param name="id">primary key of item.</param>
        /// <example>
        /// GET: api/itemData/finditems/3
        /// </example>
     
        [HttpGet]
        // GET: api/ItemData/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult FindItem(int id)
        
        {
                Item Item = db.Items.Find(id);
                ItemDto ItemDto = new ItemDto()
                {
                    Id = Item.Id,
                    ItemName = Item.ItemName,
                    ItemType = Item.ItemType,
                    ItemCount = Item.ItemCount,
                    AisleId = Item.AisleId,
                    BBD = Item.BBD,
                };
                if (Item == null)
                {
                    return NotFound();
                }

                return Ok(ItemDto);
        }



        /// <summary>
        /// Updates a particular item in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the item ID primary key</param>
        /// <param name="item">JSON FORM DATA of an item</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/itemData/Updateitem/5
        /// FORM DATA: item JSON Object
        /// </example>

        [HttpPost]
        // PUT: api/ItemData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest();
            }

            db.Entry(item).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        /// <summary>
        /// Adds an item to the system
        /// </summary>
        /// <param name="item">JSON FORM DATA of an item</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: item ID, item Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/itemData/Additem
        /// FORM DATA: item JSON Object
        /// </example>

        [HttpPost]
        // POST: api/ItemData
        [ResponseType(typeof(Item))]
        public IHttpActionResult AddItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Items.Add(item);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = item.Id }, item);
        }



        /// <summary>
        /// Delete an item from the system by using item ID.
        /// </summary>
        /// <param name="id">The primary key of the item</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/itemData/Deleteitem/5
        /// FORM DATA: (empty)
        /// </example>

        // DELETE: api/ItemData/5
        [ResponseType(typeof(Item))]
        public IHttpActionResult DeleteItem(int id)
        {
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            db.Items.Remove(item);
            db.SaveChanges();

            return Ok(item);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemExists(int id)
        {
            return db.Items.Count(e => e.Id == id) > 0;
        }
    }
}