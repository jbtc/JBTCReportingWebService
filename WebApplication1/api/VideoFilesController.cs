using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.IO;
using System.Data;
using System.Net.Http.Headers;
using TelerikReportingSample.Model;

namespace WebApplication1.api
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VideoFilesController : ApiController
    {
        VideoFile[] videoFiles = new VideoFile[] {
            new VideoFile { Id = 1, fileDate = DateTime.Now, fileName = "video.mp4",filesize = 1 },
            new VideoFile { Id = 2, fileDate = DateTime.Now , fileName = "video2.mp4",filesize = 1 },
            new VideoFile { Id = 3, fileDate = DateTime.Now, fileName = "video3.mp4" ,filesize = 1}
        };

        List<VideoFile> videoFileList = new List<VideoFile>();
        private int idIndex;

        /// <summary>
        /// get video by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/VideoServiceById")]// videoservice/3ytrytr
        public string GetVideo(string szId = null)
        {
            
                int id = Convert.ToInt32(szId);
                GenerateFolderList();
                videoFiles = videoFileList.ToArray();
                var video = videoFiles.FirstOrDefault((p) => p.Id == id);
            if (video == null)
            {
                return null;
            }
            else
            {
                //videoFileList.Clear();
                //videoFileList.Add(video);
                //videoFiles = videoFileList.ToArray();
                //return videoFiles;
                string item = @"<video width=""320"" height=""240"" controls autoplay loop>";
                item += @"<source src=""/motion/" + video.fileName + @""" type=""video/mp4"">";
                item += @"Your browser does not support the video tag.</video>";
                return item;//video.Id +"||"+video.fileName+"||"+video.filesize.ToString();
            }
        }


        /// <summary>
        /// get required data from folder
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/VideoService")]
        public IEnumerable<VideoFile> GetAllVideoFiles()
        {
            GenerateFolderList();
            videoFiles = videoFileList.ToArray();
            return videoFiles;
        }



        #region unused
        //public static string ExtractFilename(string filepath)
        //{
        //    // If path ends with a "\", it's a path only so return String.Empty.
        //    if (filepath.Trim().EndsWith(@"\"))
        //        return String.Empty;

        //    // Determine where last backslash is.
        //    int position = filepath.LastIndexOf('\\');
        //    // If there is no backslash, assume that this is a filename.
        //    if (position == -1)
        //    {
        //        // Determine whether file exists in the current directory.
        //        if (File.Exists(Environment.CurrentDirectory + Path.DirectorySeparatorChar + filepath))
        //            return filepath;
        //        else
        //            return String.Empty;
        //    }
        //    else
        //    {
        //        // Determine whether file exists using filepath.
        //        if (File.Exists(filepath))
        //            // Return filename without file path.
        //            return filepath.Substring(position + 1);
        //        else
        //            return String.Empty;
        //    }
        //}
        #endregion

        /// <summary>
        /// go through the directory and get all mp4s
        /// </summary>
        private void GenerateFolderList2()
        {
            /*
             * // get directories
             * var directories = Directory.GetDirectories("your_directory_path");
             * 
             * // iiterate over files
             * if (Directory.Exists("C:\\Temp")) 
             * {
                   string[] files = Directory.GetFiles("C:\\Temp", "*.*", SearchOption.AllDirectories);    
                }
             */
            string subPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath.ToString() + @"\motion\";
            string[] directories = Directory.GetDirectories(subPath);
            foreach (string path in directories)
            {
                //if (!path.Contains("motion"))
                //{
                //    continue;
                //}
                if (Directory.Exists(path))
                {
                    DirectoryInfo di = new DirectoryInfo(path);

                    foreach (FileInfo fi in di.GetFiles())
                    {
                        VideoFile fd = new VideoFile();
                        string s = fi.FullName;
                        int position = s.LastIndexOf('\\');
                        string[] foldernames = s.Split('\\');
                        string subfolder = foldernames[foldernames.Length - 2];
                        string tag = "";
                        // If there is no backslash, assume that this is a filename.
                        if (position > -1)
                        {
                            if (s.Contains(".mp4"))
                            {
                                tag = s.Substring(position + 1);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }

                        VideoFile setVideofile = new VideoFile()
                        {
                            Id = idIndex,
                            fileDate = fi.CreationTime,
                            fileName = @"/" + subfolder + "/" + tag,
                            filesize = fi.Length
                        };
                        videoFileList.Add(setVideofile);
                        idIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// go through the directory and get all mp4s
        /// </summary>
        private void GenerateFolderList()
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath.ToString() + @"\motion\";
            if (Directory.Exists(path))
            {
                DirectoryInfo di = new DirectoryInfo(path);
                videoFileList = new List<VideoFile>();
                idIndex = 0;
                foreach (FileInfo fi in di.GetFiles())
                {

                    VideoFile fd = new VideoFile();
                    string s = fi.FullName;
                    int position = s.LastIndexOf('\\');
                    string tag = "";
                    // If there is no backslash, assume that this is a filename.
                    if (position > -1)
                    {
                        if (s.Contains(".mp4"))
                        {
                            tag = s.Substring(position + 1);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    VideoFile setVideofile = new VideoFile()
                    {
                        Id = idIndex,
                        fileDate = fi.CreationTime,
                        fileName = tag,
                        filesize = fi.Length
                    };
                    videoFileList.Add(setVideofile);
                    idIndex++;
                }
            }
            GenerateFolderList2();
        }

    }
}
