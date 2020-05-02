using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("KitapTuru")]
    public class KitapTuru
    {
        [Key]
        public int KitapTurId { get; set; }
        [Required,StringLength(50,ErrorMessage ="En fazla 50 karakter uzunluğunda olmalıdır.")]
        public string TurAd { get; set; }
        public int? KullaniciId { get; set; }
        public DateTime KayitTarihi { get; set; }
        public ICollection<Kitaplar> Kitaplars { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}