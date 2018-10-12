using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace UploadFile
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public KeyValuePair<bool, string> UploadFile()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var httpPostedFile = HttpContext.Current.Request?.Files[0];

                    if (httpPostedFile != null)
                    {
                        //TODO: Scan the file
                        
                        var fileSavePath = HostingEnvironment.MapPath("~/UploadedFiles");
                        var fileName = GetFileName(httpPostedFile.FileName);

                        //Get the byte array
                        byte[] fileByteArray = new byte[httpPostedFile.ContentLength];
                        httpPostedFile.InputStream.Read(fileByteArray, 0, httpPostedFile.ContentLength);
                        
                        if (fileSavePath != null && !string.IsNullOrEmpty(fileName))
                        {
                            httpPostedFile.SaveAs(fileSavePath + "\\" + fileName);

                            return new KeyValuePair<bool, string>(true, "File uploaded successfully.");
                        }
                    }

                    return new KeyValuePair<bool, string>(false, "Could not get the uploaded file.");
                }

                return new KeyValuePair<bool, string>(false, "No file found to upload.");
            }
            catch (Exception ex)
            {
                return new KeyValuePair<bool, string>(false, "An error occurred while uploading the file. Error Message: " + ex.Message);
            }
        }

        private string GetFileName(string postedFile)
        {
            string fileName;

            var lastIndex = postedFile.LastIndexOf('\\');

            fileName = postedFile.Remove(0, lastIndex+1);

            return fileName;
        }

    }
}