﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWMS.Models
{
    public class Keeper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; }
        public bool Available { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
