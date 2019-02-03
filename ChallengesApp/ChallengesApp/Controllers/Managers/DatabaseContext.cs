using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ChallengesApp.Models;
using ChallengesApp.Controllers;
using HtmlAgilityPack;
using System.Text;
using System.IO;
using System.Net;


namespace ChallengeApp.Controllers.Managers
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Game> Game { get; set; }

        public DatabaseContext()
        {
            // Database.SetInitializer(new VeritabaniOlusturucu());
            Database.SetInitializer(new VeritabaniOlusturucu());
        }
    }

    //Proje ilk çalıştığında oluşturulacak veritabanı için ayarlamaları burada yapıyoruz. İlgili linkteki içerikleri çekmeyi initializerde değilde sayfa get olurken de yapabilirdik.  
    //Bu sayede ekstaradan bir sql dosyası ile uğraşmayıp Projeyi Çalıştıracak arkadaşlar sadece web.config dosyasında connectionString i kendilerine uyarlayıp çalıştıracaklar. 
    public class VeritabaniOlusturucu : CreateDatabaseIfNotExists<DatabaseContext>
    {
        List<string> liste = new List<string>();

        protected override void Seed(DatabaseContext context)
        {

            string link = "https://itunes.apple.com/us/genre/ios-games/id6014?mt=8&letter=A&page=1#page";  //link değişkenine çekeceğimiz web sayafasının linkini yazıyoruz.

            Uri url = new Uri(link); //Uri tipinde değişeken linkimizi veriyoruz.

            WebClient client = new WebClient(); // webclient nesnesini kullanıyoruz bağlanmak için.
            client.Encoding = Encoding.UTF8; //türkçe karakter sorunu yapmaması için encoding utf8 yapıyoruz.

            string html = client.DownloadString(url); // siteye bağlanıp tüm sayfanın html içeriğini çekiyoruz.

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument(); //kütüphanemizi kullanıp htmldocument oluşturuyoruz.
            document.LoadHtml(html);//documunt değişkeninin html ine çektiğimiz htmli veriyoruz

            var seciliHtml = @"//*[@class=""column first""]/ul";//veriyi çekeceğimiz 1. div alanı kategori listesi xpath adresi
            var seciliHtml2 = @"//*[@class=""column""]/ul";//veriyi çekeceğimiz 2. div alanı kategori listesi xpath adresi
            var seciliHtml3 = @"//*[@class=""column last""]/ul";//veriyi çekeceğimiz 3. div alanı kategori listesi xpath adresi

            ////*[@id="selectedcontent"]/div[1]/ul
            var seciliHtmlList = document.DocumentNode.SelectNodes(seciliHtml); ////selectnodes methoduyla verdiğimiz xpathin htmlini getiriyoruz.
            var seciliHtmlList2 = document.DocumentNode.SelectNodes(seciliHtml2);
            var seciliHtmlList3 = document.DocumentNode.SelectNodes(seciliHtml3);

            foreach (var items in seciliHtmlList)
            {
                foreach (var innerItem in items.SelectNodes("li"))
                {
                    foreach (var item in innerItem.SelectNodes("a"))
                    {

                        var aValue = item.Attributes["href"] == null ? null : item.Attributes["href"].Value;
                        Game game = new Game();
                        game.Name = item.InnerText;

                        string GameLink = aValue; // Linklerin içeriğinde yer alan bilgileri çekmek adına her bir link için aynı işlemleri yapıyoruz.
                        Uri GameUrl = new Uri(GameLink);
                        WebClient GameClient = new WebClient();
                        GameClient.Encoding = Encoding.UTF8;
                        string GameHtml = client.DownloadString(GameUrl);

                        game.Url = aValue;
                        context.Game.Add(game);
                        HtmlAgilityPack.HtmlDocument GameDocument = new HtmlAgilityPack.HtmlDocument();
                        GameDocument.LoadHtml(GameHtml);

                        var GameSeciliHtml = @"//*[@class=""product-hero__media l-column small-5 medium-4 large-3 small-valign-top""]/picture";

                        //StringBuilder GameSt = new StringBuilder();
                        var GameSeciliHtmlList = GameDocument.DocumentNode.SelectNodes(GameSeciliHtml);

                        foreach (var gameItem in GameSeciliHtmlList)//Baslangıc
                        {
                            var ımgUrl = gameItem.SelectSingleNode("img").GetAttributeValue("src", null);
                            game.ImgUrl = ımgUrl;
                            context.Game.Add(game);
                        }//bitis
                        context.SaveChanges();
                    }
                }
            } // var seciliHtml = @"//*[@class=""column first""]/ul"; için bitiş
            context.SaveChanges();

            foreach (var items in seciliHtmlList2)
            {
                foreach (var innerItem in items.SelectNodes("li"))
                {
                    foreach (var item in innerItem.SelectNodes("a"))
                    {
                        //var classValue = item.Attributes["class"] == null ? null : item.Attributes["class"].Value;
                        var aValue = item.Attributes["href"] == null ? null : item.Attributes["href"].Value;
                        Game game = new Game();
                        game.Name = item.InnerText;

                        string GameLink = aValue;
                        Uri GameUrl = new Uri(GameLink);
                        WebClient GameClient = new WebClient();
                        GameClient.Encoding = Encoding.UTF8;
                        string GameHtml = client.DownloadString(GameUrl);


                        game.Url = aValue;
                        context.Game.Add(game);

                        HtmlAgilityPack.HtmlDocument GameDocument = new HtmlAgilityPack.HtmlDocument();
                        GameDocument.LoadHtml(GameHtml);

                        var GameSeciliHtml = @"//*[@class=""product-hero__media l-column small-5 medium-4 large-3 small-valign-top""]/picture";

                        //StringBuilder GameSt = new StringBuilder();
                        var GameSeciliHtmlList = GameDocument.DocumentNode.SelectNodes(GameSeciliHtml);

                        foreach (var gameItem in GameSeciliHtmlList)
                        {
                            var ımgUrl = gameItem.SelectSingleNode("img").GetAttributeValue("src", null);
                            game.ImgUrl = ımgUrl;
                            context.Game.Add(game);

                            // @//*[@class=""eproduct-header__identity app-header__identity""]/div/div[2]/header/h2/a

                            // string ValuesLink = aValue;
                            // var ValuesHtml = @"//*[@class=""eproduct-header__identity app-header__identity""]/h2";
                            // var ValuesSeciliHtmlList = GameDocument.DocumentNode.SelectNodes(ValuesHtml);
                            //foreach (var ValuesItem in ValuesSeciliHtmlList)
                            //{
                            //   var devops = ValuesItem.SelectSingleNode("a").InnerText;
                            //   game.Seller = devops;
                            //   context.Game.Add(game);
                            //}



                        }
                        context.SaveChanges();
                    }
                }
            } //var seciliHtml2 = @"//*[@class=""column""]/ul"; için bitiş
            context.SaveChanges();

            foreach (var items in seciliHtmlList3)
            {
                foreach (var innerItem in items.SelectNodes("li"))
                {
                    foreach (var item in innerItem.SelectNodes("a"))
                    {
                        //var classValue = item.Attributes["class"] == null ? null : item.Attributes["class"].Value;
                        var aValue = item.Attributes["href"] == null ? null : item.Attributes["href"].Value;
                        Game game = new Game();
                        game.Name = item.InnerText;

                        string GameLink = aValue;
                        Uri GameUrl = new Uri(GameLink);
                        WebClient GameClient = new WebClient();
                        GameClient.Encoding = Encoding.UTF8;
                        string GameHtml = client.DownloadString(GameUrl);


                        game.Url = aValue;
                        context.Game.Add(game);

                        HtmlAgilityPack.HtmlDocument GameDocument = new HtmlAgilityPack.HtmlDocument();
                        GameDocument.LoadHtml(GameHtml);

                        var GameSeciliHtml = @"//*[@class=""product-hero__media l-column small-5 medium-4 large-3 small-valign-top""]/picture";

                        //StringBuilder GameSt = new StringBuilder();
                        var GameSeciliHtmlList = GameDocument.DocumentNode.SelectNodes(GameSeciliHtml);

                        foreach (var gameItem in GameSeciliHtmlList)
                        {
                            var ımgUrl = gameItem.SelectSingleNode("img").GetAttributeValue("src", null);
                            game.ImgUrl = ımgUrl;
                            context.Game.Add(game);

                            // @//*[@class=""eproduct-header__identity app-header__identity""]/div/div[2]/header/h2/a

                            // string ValuesLink = aValue;
                            // var ValuesHtml = @"//*[@class=""eproduct-header__identity app-header__identity""]/h2";
                            // var ValuesSeciliHtmlList = GameDocument.DocumentNode.SelectNodes(ValuesHtml);
                            //foreach (var ValuesItem in ValuesSeciliHtmlList)
                            //{
                            //   var devops = ValuesItem.SelectSingleNode("a").InnerText;
                            //   game.Seller = devops;
                            //   context.Game.Add(game);
                            //}



                        }
                        context.SaveChanges();
                    }
                }
            }// var seciliHtml3 = @"//*[@class=""column last""]/ul" için bitiş
            context.SaveChanges();


        }


    }
}




