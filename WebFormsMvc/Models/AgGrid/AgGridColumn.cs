namespace WebFormsMvc.Models.AgGrid
{
    public class AgGridColumn
    {
        public string HeaderName { get; set; }

        public string Field { get; set; }

        public string Filter { get; set; }

        public FilterParams FilterParams { get; set; }

        public bool Sortable { get; set; } = true;
    }
}