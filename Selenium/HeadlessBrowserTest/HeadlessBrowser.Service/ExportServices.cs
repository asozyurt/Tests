using HeadlessBrowser.Common.Dto;
using System.Collections.Generic;
using System.IO;

namespace HeadlessBrowser.Service
{
    public class ExportServices
    {
        public static string WriteToFile(List<ImaginaryUserDto> users)
        {
            string fileName = @"ImaginartyUsers.txt";
            // Delete if exists
            DeleteFile(fileName);

            using (StreamWriter file = new StreamWriter(fileName))
            {
                users.ForEach(x => file.WriteLine(x));
            }
            return fileName;
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void ExportToFtp(string file)
        {
            //TODO: Export ftp
        }
    }
}
