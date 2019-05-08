using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvc.Models.AgGrid
{
    /// <summary>
    /// pass a VO (Value Object) of the column and not the column itself,
    /// so the data can be converted to a JSON string and passed to server-side
    /// </summary>
    public class ColumnVo
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Field { get; set; }

        public string AggFunc { get; set; }
    }
}