using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
 
namespace MusicData
{
    class Program
    {
        // dotnet add package System.Runtime.Serialization.Json
        // dotnet add package System.Runtime.Serialization.Primitives
        // https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
 
        static void Main(string[] args)
        {
            // Get the data
            var artists = ProcessArtists().Result;  
 
            // View the results
            foreach (var artist in artists)
            {
                Console.WriteLine("Artist Name: " + artist.Name);
                Console.WriteLine("Artist Playcount: " + artist.Playcount);
                Console.WriteLine("Artist Listeners: " + artist.Listeners);
                Console.WriteLine("Artist Mbid: " + artist.Mbid);
                Console.WriteLine("Artist URL: " + artist.Url);
                Console.WriteLine(artist.Streamable);
                foreach (var image in artist.ImageList)
                {
                    Console.WriteLine("Image Size: " + image.Size);
                    Console.WriteLine("Image Text: " + image.Text);
                }
            }
 
            Console.ReadLine();
        }
 
        private static async Task<List<Artist>> ProcessArtists()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            var serializer = new DataContractJsonSerializer(typeof(ArtistData));
            var streamTask = client.GetStreamAsync("http://ws.audioscrobbler.com/2.0/?method=chart.gettopartists&api_key=87bdb2c24f5d7ea2e34ac5d1bdc419f1&format=json&limit=10");
            var artistData = serializer.ReadObject(await streamTask) as ArtistData;
            return artistData.Artists.ArtistList;
        }
 
        #region payload data
 
        // These classes will represent the info coming back
        [DataContract(Name = "ArtistData")]
        public class ArtistData
        {
            [DataMember(Name = "artists")]
            public Artists Artists { get; set; }
        }
 
        [DataContract(Name = "artists")]
        public class Artists
        {
            public Artists()
            {
                ArtistList = new List<Artist>();
            }
 
            [DataMember(Name = "artist")]
            public List<Artist> ArtistList { get; set; }
        }
 
        [DataContract(Name = "artist")]
        public class Artist
        {
            public Artist()
            {
                ImageList = new List<Image>();
            }
 
            [DataMember(Name = "name")]
            public string Name { get; set; }
 
            [DataMember(Name = "playcount")]
            public string Playcount { get; set; }
 
            [DataMember(Name = "listeners")]
            public string Listeners { get; set; }
 
            [DataMember(Name = "mbid")]
            public string Mbid { get; set; }
 
            [DataMember(Name = "url")]
            public string Url { get; set; }
 
            [DataMember(Name = "streamable")]
            public string Streamable { get; set; }
 
            [DataMember(Name = "image")]
            public List<Image> ImageList { get; set; }
        }
 
        [DataContract(Name = "image")]
        public class Image
        {
            [DataMember(Name = "#text")]
            public string Text { get; set; }
 
            [DataMember(Name = "size")]
            public string Size { get; set; }
        }
 
        #endregion
    }
}