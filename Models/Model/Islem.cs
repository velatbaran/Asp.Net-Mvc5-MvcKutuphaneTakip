using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Islem")]
    public class Islem
    {
        [Key]
        public int IslemId { get; set; }
        public int? KitapId { get; set; }
        public int? UyeId { get; set; }
        public int? KullaniciId { get; set; }
        [Required,StringLength(250,ErrorMessage ="En fazla 250 karakter olmalıdır.")]
        public string Aciklama { get; set; }
        [Required]
        public DateTime VerilisTarihi { get; set; }
        [Required]
        public DateTime IadeTarihi { get; set; }
        public DateTime UyeGetirTarihi { get; set; }
        public string GecGelenGunSayisi { get; set; }
        public Kitaplar Kitaplar { get; set; }
        public Uye Uye { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}