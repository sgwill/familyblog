using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;

namespace WilliamsonFamily.Models.ShutterflyPhoto
{
    public class SampleCode
    {
        private byte[] CreateUploadData(Stream imageStream, string fileName, Dictionary<string, string> parameters, string boundary)
        {
            string[] keys = new string[parameters.Keys.Count];
            parameters.Keys.CopyTo(keys, 0);
            Array.Sort(keys);

            StringBuilder hashStringBuilder = new StringBuilder();// TODO: sharedSecret, 2 * 1024);
            StringBuilder contentStringBuilder = new StringBuilder();

            foreach (string key in keys)
            {
                hashStringBuilder.Append(key);
                hashStringBuilder.Append(parameters[key]);
                contentStringBuilder.Append("--" + boundary + "\r\n");
                contentStringBuilder.Append("Content-Disposition: form-data; name=\"" + key + "\"\r\n");
                contentStringBuilder.Append("\r\n");
                contentStringBuilder.Append(parameters[key] + "\r\n");
            }

            contentStringBuilder.Append("--" + boundary + "\r\n");
            contentStringBuilder.Append("Content-Disposition: form-data; name=\"api_sig\"\r\n");
            contentStringBuilder.Append("\r\n");
            contentStringBuilder.Append(MD5Hash(hashStringBuilder.ToString()) + "\r\n");

            // Photo
            contentStringBuilder.Append("--" + boundary + "\r\n");
            contentStringBuilder.Append("Content-Disposition: form-data; name=\"photo\"; filename=\"" + Path.GetFileName(fileName) + "\"\r\n");
            contentStringBuilder.Append("Content-Type: image/jpeg\r\n");
            contentStringBuilder.Append("\r\n");

            UTF8Encoding encoding = new UTF8Encoding();

            byte[] postContents = encoding.GetBytes(contentStringBuilder.ToString());

            byte[] photoContents = ConvertNonSeekableStreamToByteArray(imageStream);

            byte[] postFooter = encoding.GetBytes("\r\n--" + boundary + "--\r\n");

            byte[] dataBuffer = new byte[postContents.Length + photoContents.Length + postFooter.Length];

            Buffer.BlockCopy(postContents, 0, dataBuffer, 0, postContents.Length);
            Buffer.BlockCopy(photoContents, 0, dataBuffer, postContents.Length, photoContents.Length);
            Buffer.BlockCopy(postFooter, 0, dataBuffer, postContents.Length + photoContents.Length, postFooter.Length);

            return dataBuffer;
        }

        private byte[] ConvertNonSeekableStreamToByteArray(Stream nonSeekableStream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            int bytes;
            while ((bytes = nonSeekableStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, bytes);
            }
            byte[] output = ms.ToArray();
            return output;
        }

        private string MD5Hash(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider csp = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] hashedBytes = csp.ComputeHash(bytes, 0, bytes.Length);

            return BitConverter.ToString(hashedBytes).Replace("-", String.Empty).ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }

        public void UploadPhoto(Stream stream, string filename, string title, string descriptioSn, string tags)
        {
            // Url
            string url = "http://up1.shutterfly.com/images?oflyAppId=1c5723d356fbb8b97776563dcccbbcd5";
            var Uri = new Uri(url);


            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("AuthenticationID", "003062994768|1288472168744|9cb05b214539292c04671dcf13cef70e70d2250e");
            parameters.Add("Image.FolderName", "My Albums");
            parameters.Add("Image.AlbumName", "2010-10-30");

            UploadDataAsync(stream, filename, Uri, parameters);
        }


        private void UploadDataAsync(Stream imageStream, string fileName, Uri uploadUri, Dictionary<string, string> parameters)
        {
            string boundary = "----------------------12bfeed09d7";

            byte[] dataBuffer = CreateUploadData(imageStream, fileName, parameters, boundary);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uploadUri);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;
            req.Headers.Add("oflyHashMeth", "SHA1");
            req.Headers.Add("oflyApiSig", "5a8952f8c92519b76002dca15c2353596b0ba7bf");
            req.Headers.Add("Accept-Encoding", "gzip");
            req.Headers.Add("oflyTimestamp", DateTime.UtcNow.ToString());

            req.BeginGetRequestStream(
                r =>
                {
                    Stream s = req.EndGetRequestStream(r);
                    s.Write(dataBuffer, 0, dataBuffer.Length);
                    s.Close();

                    req.BeginGetResponse(
                        r2 =>
                        {
                            try
                            {
                                WebResponse res = req.EndGetResponse(r2);
                                StreamReader sr = new StreamReader(res.GetResponseStream());
                                string responseXml = sr.ReadToEnd();
                                sr.Close();

                                XmlReaderSettings settings = new XmlReaderSettings();
                                settings.IgnoreWhitespace = true;
                                XmlReader reader = XmlReader.Create(new StringReader(responseXml), settings);

                                if (!reader.ReadToDescendant("rsp"))
                                {
                                    throw new XmlException("Unable to find response element 'rsp' in Flickr response");
                                }
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.LocalName == "stat" && reader.Value == "fail")
                                        throw new Exception("blah");
                                    continue;
                                }

                                reader.MoveToElement();
                                reader.Read();

                            }
                            catch (Exception ex)
                            {
                            }


                        },
                        this);
                },
                this);

        }
    }
}