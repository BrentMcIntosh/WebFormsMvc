using System.Collections.Generic;

namespace WebFormsMvc.Models.AgGrid
{
    public class GetRowsResponse
    {
        public List<Dictionary<string, object>> Data { get; set; }

        public int? LastRow { get; set; }

        public  List<string> SecondaryColumnFields { get; set; }
    }
}