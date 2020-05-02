using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Yetki")]
    public class Yetki
    {
        [Key]
        public int YetkiId { get; set; }
        [Required,StringLength(50,ErrorMessage ="En fazla 50 karakter uzunluğunda olmalıdır.")]
        public string YetkiAd { get; set; }
        public DateTime KayitTarihi { get; set; }
        public ICollection<Kullanici> Kullanicis { get; set; }
    }
}