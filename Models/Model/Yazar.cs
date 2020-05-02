using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Yazar")]
    public class Yazar
    {
        [Key]
        public int YazarId { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 100 karakter uzunluğunda olmalıdır.")]
        public string AdSoyad { get; set; }
        public int? KullaniciId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public ICollection<Kitaplar> Kitaplars { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}