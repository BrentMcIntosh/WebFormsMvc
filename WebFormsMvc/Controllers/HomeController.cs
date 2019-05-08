using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography.X509Certificates;
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
            return View(AgGridModel.GetSample());
        }

        private static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static IQueryable<SampleData> Sort(IQueryable<SampleData> collection, string sortBy, bool reverse = false)
        {
            return collection.OrderBy(sortBy + (reverse ? " descending" : ""));
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

                    var columnName = FirstCharToUpper(sort.ColId);

                    var direction = sort.Sort;

                    items = new List<SampleData>(Sort(items.AsQueryable(), columnName, direction == "desc"));
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