using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace WebFormsMvc.Models.AgGrid
{
    public class AgGridColumn
    {
        public string HeaderName { get; set; }
        public string Field { get; set; }
    }

    public class AgGridModel
    {
        public List<AgGridColumn> Columns { get; set; } = new List<AgGridColumn>();

        public string CallBackMethod { get; set; }

        public string ToJsonCamel()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
