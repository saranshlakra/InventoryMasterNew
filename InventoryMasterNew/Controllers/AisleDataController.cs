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
        /// GET: api/aisledata/listaisle
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AisleDto))]
        // GET: api/AisleData
        public IHttpActionResult ListAisle()
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

        /// <summary>
        /// Updates a particular aisle in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the aisle ID primary key</param>
        /// <param name="aisle">JSON FORM DATA of an aisle</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/aisledata/updateaisle/2
        /// </example>

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





        /// <summary>
        /// Adds an aisle to the system
        /// </summary>
        /// <param name="aisle">JSON FORM DATA of an aisle</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: aisle ID, aisle Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/aisleData/Addaisle
        /// </example>
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




        /// <summary>
        /// Deletes an aisle from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the aisle</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/aisleData/Deleteaisle/2
        /// </example>

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