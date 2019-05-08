using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvc.Models.AgGrid
{
    public class GridRequest
    {
        public int? id { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public bool searching { get; set; }
        public string sortColumn { get; set; }
        public string sortOrder { get; set; }
        public long cacheBuster { get; set; }
        public string filters { get; set; }
    }
}