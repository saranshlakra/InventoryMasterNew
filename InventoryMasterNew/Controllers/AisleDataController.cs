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
    public class AisleDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Aisle in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Aisle in the database
        /// </returns>
        /// <example>
        /// GET: api/AisleData/listaisle
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AisleDto))]
        // GET: api/ItemData
        public IHttpActionResult ListItems()
        {
            List<Aisle> Aisles = db.Aisles.ToList();
            List<AisleDto> AislesDtos = new List<AisleDto>();

            Aisles.ForEach(a => AislesDtos.Add(new AisleDto()
            {
                AisleId = a.AisleId,
                Name = a.Name,
                Desc = a.Desc,
                AisleCap = a.AisleCap,
            }));

            return Ok(AislesDtos);
        }

        // PUT: api/AisleData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateAisle(int id, Aisle aisle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aisle.AisleId)
            {
                return BadRequest();
            }

            db.Entry(aisle).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AisleExists(id))
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

        // POST: api/AisleData
        [ResponseType(typeof(Aisle))]
        public IHttpActionResult AddAisle(Aisle aisle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Aisles.Add(aisle);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aisle.AisleId }, aisle);
        }

        // DELETE: api/AisleData/5
        [ResponseType(typeof(Aisle))]
        public IHttpActionResult DeleteAisle(int id)
        {
            Aisle aisle = db.Aisles.Find(id);
            if (aisle == null)
            {
                return NotFound();
            }

            db.Aisles.Remove(aisle);
            db.SaveChanges();

            return Ok(aisle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AisleExists(int id)
        {
            return db.Aisles.Count(e => e.AisleId == id) > 0;
        }
    }
}