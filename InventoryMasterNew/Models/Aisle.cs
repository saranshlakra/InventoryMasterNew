using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventoryMasterNew.Models
{
    public class Aisle
    {

        [Key]

        public int AisleId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int AisleCap { get; set; }

        // list of items
        public ICollection<Item> Item { get; set; }
    }

    public class AisleDto
    {
        public int AisleId { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int AisleCap { get; set; }

        // list of items
        public ICollection<Item> Item { get; set; }
    }
}