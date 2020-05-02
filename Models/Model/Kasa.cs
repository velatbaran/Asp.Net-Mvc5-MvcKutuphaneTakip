using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Kasa")]
    public class Kasa
    {
        [Key]
        public int KasaId { get; set; }
        public int? UyeId { get; set; }
        public int? KullaniciId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public Nullable<decimal> Tutar { get; set; }
        public Uye Uye { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}