using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.Model
{
    [Table("Kullanici")]
    public class Kullanici
    {
        [Key]
        public int KullaniciId { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Ad { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Soyad { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Email { get; set; }
        [Required, StringLength(50, ErrorMessage = "50 karakter olmalıdır")]
        public string Sifre { get; set; }
        public int? YetkiId { get; set; }
        public string ResimURL { get; set; }
        public DateTime KayitTarihi { get; set; }
        public Yetki Yetki { get; set; }
        public ICollection<Islem> Islems { get; set; }
        public ICollection<Yazar> Yazars { get; set; }
        public ICollection<Uye> Uyes { get; set; }
        public ICollection<KitapTuru> KitapTurus { get; set; }
        public ICollection<Kitaplar> Kitaplars  { get; set; }
        public ICollection<Kimlik> Kimliks { get; set; }
        public ICollection<Kasa> Kasas { get; set; }
    }
}