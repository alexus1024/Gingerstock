using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gingerstock2.Store.Models
{
    public abstract class Gingerstock2EntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id { get; set; }
    }
}