using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WilliamsonFamily.Models.Photo;
using System.IO;
using System.Net;
using System.Web;

namespace WilliamsonFamily.Models.ShutterflyPhoto
{
    public class ShutterflyPhotoRepository : IPhotoRepository
    {
        public IPhoto UploadPhoto(Stream stream, string filename, string title, string descriptioSn, string tags)
        {
            SampleCode sc = new SampleCode();
            sc.UploadPhoto(stream, filename, title, descriptioSn, tags);
            return null;
            //if (!Authenticate())
            //    return null;

            //Uri address = new Uri("http://up1.shutterfly.com/images");

            //// Create the web request  
            //HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            //// Set type to POST  
            //request.Method = "POST";
            //request.ContentType = "multipart/form-data";
            //request.Headers.Add("oflyHashMeth", "SHA1");
            //request.Headers.Add("oflyApiSig", "1c5723d356fbb8b97776563dcccbbcd5");
            //request.Headers.Add("Accept-Encoding", "gzip");
            //request.Headers.Add("oflyTimestamp", DateTime.UtcNow.ToFileTime().ToString());

            //// Create the data we want to send  
            //string appId = "";
            //string secret = "309caa89aab85e98";

            //StringBuilder data = new StringBuilder();
            
            //data.Append("?Image.FolderName=BlogUpload");
            //data.Append("&Image.AlbumName=Blog");
            

            //// Create a byte array of the data we want to send  
            //byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

            //// Set the content length in the request headers  
            //request.ContentLength = byteData.Length;

            //// Write data  
            //using (Stream postStream = request.GetRequestStream())
            //{
            //    postStream.Write(byteData, 0, byteData.Length);
            //}

            //// Get response  
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    // Get the response stream  
            //    StreamReader reader = new StreamReader(response.GetResponseStream());

            //    // Console application output  
            //    Console.WriteLine(reader.ReadToEnd());
            //}  
        }

        //private bool Authenticate()
        //{
        //    // Create the web request  
        //    HttpWebRequest request
        //        = WebRequest.Create("https://api.del.icio.us/v1/posts/recent") as HttpWebRequest;

        //    // Add authentication to request  
        //    request.Credentials = new NetworkCredential("username", "password");

        //    // Get response  
        //    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        //    {
        //        // Get the response stream  
        //        StreamReader reader = new StreamReader(response.GetResponseStream());

        //        // Console application output  
        //        Console.WriteLine(reader.ReadToEnd());
        //    }  
        //}
    }
}