using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoyolBlog.Models.Entity
{
    public class Resimekle
    {
        public string icerik { get; set; }

        public int kategori_id { get; set; }

        public string baslik { get; set; }

        public IFormFile img { get; set; }
    }
}