using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Uye")]
    public class Uye
    {
        [Key]       
        public int UyeId { get; set; }
        [Required, StringLength(11, ErrorMessage = "En fazla 11 karakter olmalıdır")]
        public string TC { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olmalıdır")]
        public string Ad { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olmalıdır")]
        public string Soyad { get; set; }
        [Required, StringLength(50, ErrorMessage = "En fazla 50 karakter olmalıdır")]
        public string Email { get; set; }
        [Required, StringLength(11, ErrorMessage = "En fazla 11 karakter olmalıdır")]
        public string Telefon { get; set; }
        [Required, StringLength(200, ErrorMessage = "En fazla 200 karakter olmalıdır")]
        public string Adres { get; set; }
        public int? KullaniciId { get; set; }
        public DateTime UyelikTarihi { get; set; }
        public Kullanici Kullanici { get; set; }
        public ICollection<Islem> Islems { get; set; }
        public ICollection<Kasa> Kasas { get; set; }
    }
}