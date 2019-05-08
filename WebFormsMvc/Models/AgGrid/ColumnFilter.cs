
namespace Model.AgGrid
{
    public class ColumnFilter
    {
        public string FilterType { set; get; }
        public string Type { set; get; }
        public string Condition { set; get; }
    }


    public class Condition1
    {
        public string type { get; set; }
        public string filter { get; set; }
        public string filterType { get; set; }
    }

    public class Condition2
    {
        public string type { get; set; }
        public string filter { get; set; }
        public string filterType { get; set; }
    }

    public class Name
    {
        public Condition1 condition1 { get; set; }
        public Condition2 condition2 { get; set; }
        public string @operator { get; set; }
    }

    public class Born
    {
        public object dateTo { get; set; }
        public string dateFrom { get; set; }
        public string type { get; set; }
        public string filterType { get; set; }
    }


    public class FilterModel
    {
        public Name name { get; set; }
        public Born born { get; set; }
    }
}