using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MotoyolBlog.Models.Entity;

namespace MotoyolBlog.Models
{
    public class ViewModel
    {
        public IEnumerable<Makaleler> Makale_model { get; set; }
         
        public IEnumerable<MakaleListesi_Result> Makale_Listesi_model { get; set; }

        public IEnumerable <Yorumlar> yorum_model { get; set; }

        public IEnumerable<Favoriler> favori_model { get; set; }

    }
}