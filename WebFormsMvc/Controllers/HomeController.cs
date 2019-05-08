using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Linq;
using WebFormsMvc.Models;
using WebFormsMvc.Models.AgGrid;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new AgGridModel
            {
                Columns = new List<AgGridColumn>
                {
                    new AgGridColumn { HeaderName = "Id", Field = "id" },
                    new AgGridColumn { HeaderName = "Name", Field = "name" },
                    new AgGridColumn { HeaderName = "Born", Field = "born" },
                    new AgGridColumn { HeaderName = "Cost", Field = "cost" },
                    new AgGridColumn { HeaderName = "Good", Field = "good" }
                },

                CallBackMethod = "/Home/GetRows"
            };

            return View(model);
        }

        public string GetRows(GetRowsRequest request)
        {
            var data = new List<Dictionary<string, object>>();

            using (var session = NHibernateSession.OpenSession())
            {
                var items = session.Query<SampleData>().ToList();

                foreach (var item in items)
                {
                    data.Add(new Dictionary<string, object>
                    {
                        ["id"] = item.Id,
                        ["name"] = item.Name,
                        ["born"] = item.Born,
                        ["cost"] = item.Cost,
                        ["good"] = item.Good
                    });
                }
            }

            var response = new GetRowsResponse
            {
                Data = data,

                LastRow = 1000
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}