using MvcKutuphane.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcKutuphane.Models.DataContext
{
    public class KutuphaneDBContext:DbContext
    {
        public KutuphaneDBContext() : base ("MvcLibraryDB")
        {
            
        }
        public DbSet<Kitaplar> Kitaplar { get; set; }
        public DbSet<KitapTuru> KitapTuru { get; set; }
        public DbSet<Islem> Islem { get; set; }
        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Uye> Uye { get; set; }
        public DbSet<Yazar> Yazar { get; set; }
        public DbSet<Yetki> Yetki { get; set; }
        public DbSet<Kimlik> Kimlik { get; set; }
        public DbSet<Kasa> Kasa { get; set; }
    }
}