using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TelerikReportingSample.Model
{
    public class VideoFile
    {
        public int Id { get; set; }
        public string fileName { get; set; }
        public DateTime fileDate { get; set; }
        public long filesize { get; set; }
    }
}