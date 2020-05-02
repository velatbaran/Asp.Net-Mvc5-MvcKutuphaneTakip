using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Kitaplar")]
    public class Kitaplar
    {
        [Key]
        public int KitapId { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Ad { get; set; }
        public int? KitapTurId { get; set; }
        public int? YazarId { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Yayınevi { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string BasimYili { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Sayfa { get; set; }
        public int? KullaniciId { get; set; }
        public bool KitapDurum { get; set; }
        public DateTime KayitTarihi { get; set; }
        public Yazar Yazar { get; set; }
        public KitapTuru KitapTuru { get; set; }
        public Kullanici Kullanici { get; set; }
        public ICollection<Islem> Islems { get; set; }

    }
}