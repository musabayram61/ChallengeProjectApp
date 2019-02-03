using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace ChallengesApp.Models
{
    [Table("Games")]
    public class Game
    {
        //Veritabanı tablomuz için oluşturduğumuz sınıf
        public int id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ImgUrl { get; set; }
        /* 
         * Tablo kolonlarını istediğimiz sayıda çoğaltabiliriz. 
         * public string Category { get; set; }
           public string AgeRating { get; set; }
           public string Copyright { get; set; }
           public float Price { get; set; }
           */
    }


}