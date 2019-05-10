using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate.Linq;
using WebFormsMvc.Extensions;
using WebFormsMvc.Filters;
using WebFormsMvc.Models;
using WebFormsMvc.Models.AgGrid;

namespace WebFormsMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(AgGridModel.GetSample());
        }

        public string GetRows(GetRowsRequest request)
        {
            var data = new List<Dictionary<string, object>>();

            var count = 7;

            using (var session = NHibernateSession.OpenSession())
            {
                var items = session.Query<SampleData>().Take(count).ToList();

                if (request?.SortModel != null && request.SortModel.Count > 0)
                {
                    var sort = request.SortModel[0];

                    var columnName = sort.ColId.FirstCharToUpper();

                    var direction = sort.Sort;

                    items = new List<SampleData>(GeneralFilter.Sort(items.AsQueryable(), columnName, direction == "desc"));
                }

                if (request?.FilterModel != null && request.FilterModel.Count > 0)
                {
                    items = GeneralFilter.Filter(items.AsQueryable(), request.FilterModel);
                }

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

                LastRow = count
            };

            return JsonConvert.SerializeObject(response, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}