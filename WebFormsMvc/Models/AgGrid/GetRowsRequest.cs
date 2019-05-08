using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.AgGrid;

namespace WebFormsMvc.Models.AgGrid
{
    public class GetRowsRequest
    {
        public int StartRow { get; set; }

        public int EndRow { get; set; }
         
        public List<ColumnVo> RowGroupCols { get; set; }

        public List<ColumnVo> ValueCols { get; set; }

        public List<ColumnVo> PivotCols { get; set; }

        public bool PivotMode { get; set; }

        public List<string> GroupKeys { get; set; }

        public FilterModel FilterModel { get; set; }

        public List<SortModel> SortModel { get; set; }
    }
}