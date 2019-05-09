using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace WebFormsMvc.Models.AgGrid
{
    public class AgGridModel
    {
        public List<AgGridColumn> Columns { get; set; } = new List<AgGridColumn>();

        public string CallBackMethod { get; set; }

        public string ToJsonCamel()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }

        public static AgGridModel GetSample()
        {
            return new AgGridModel
            {
                Columns = new List<AgGridColumn>
                {
                    new AgGridColumn { HeaderName = "Id", Field = "id" },
                    new AgGridColumn { HeaderName = "Name", Field = "name", Filter = "agTextColumnFilter", FilterParams = new FilterParams { ApplyButton = true, ClearButton = true } },
                    new AgGridColumn { HeaderName = "Born", Field = "born", Filter = "agDateColumnFilter", FilterParams = new FilterParams { ApplyButton = true, ClearButton = true } },
                    new AgGridColumn { HeaderName = "Cost", Field = "cost", Filter = "agNumberColumnFilter", FilterParams = new FilterParams { ApplyButton = true, ClearButton = true } },
                    new AgGridColumn { HeaderName = "Good", Field = "good" }
                },

                CallBackMethod = "/Home/GetRows"
            };
        }
    }
}
