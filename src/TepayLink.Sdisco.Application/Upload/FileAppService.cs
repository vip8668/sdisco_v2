
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TepayLink.Sdisco.Upload
{
   public class FileAppService: SdiscoAppServiceBase
    {
        private readonly IHostingEnvironment _hostingEnvironment;
    
        public FileAppService(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
          
        }


        /// <summary>
        /// Upload file:
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<List<string>> Upload(IEnumerable<IFormFile> files)
        {




            List<string> urls = new List<string>();
            var folder = DateTime.Now.ToString("ddMMyy");

            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "upload/" + folder);

            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            int i = 0;
            foreach (var file in files)
            {
                i++;
                var extension = Path.GetExtension(file.FileName).ToLower();
                ValidateFile(file);
                var fileName = DateTime.Now.ToString("ddMMyyyyHHmmssfff") + "_" + i + extension;
                string path = AppConsts.Domain + $"/upload/{folder}/" + fileName;

                if (file.Length > 0)
                {
                    var filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                urls.Add(path);
            }

            return urls;
        }

        public bool ValidateFile(IFormFile file)
        {
            var extensionTypes = ".jpg,.gif,.png,.pdf,.dox,.docx,.xls,.xlsx".Split(',').ToList(); //ConfigurationManager.AppSettings["ExtensionFileTye"].ToString().ToLower().Split(',', ';', '|').ToList();
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!extensionTypes.Contains(extension))
            {
                throw new UserFriendlyException("File không hợp lệ");

            }

            var knownTypes = "image/jpeg, image/png,image/gif,image/png,application/pdf,application/msword,application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,application/vnd.openxmlformats-officedocument.wordprocessingml.document".Split(',').ToList();// ConfigurationManager.AppSettings["AcceptedFileTye"].ToString().ToLower().Split(',', ';', '|').ToList();
            if (file != null && file.Length > 0)
            {
                if (!knownTypes.Contains(file.ContentType.ToString().ToLower()))
                {
                    throw new UserFriendlyException("File không hợp lệ");
                }
                if (file.Length >= 20000000)
                {
                    throw new UserFriendlyException("File quá lớn");
                }
                return true;
            }
            throw new UserFriendlyException("File không hợp lệ");
            return false;
        }

    }
}
