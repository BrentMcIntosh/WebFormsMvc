namespace WebFormsMvc.Models.AgGrid
{
    public class Condition
    {
        public string Type { get; set; }
        public string Filter { get; set; }
        public string FilterType { get; set; }

        public string DateTo { get; set; }
        public string DateFrom { get; set; }

        public decimal FilterTo { get; set; }
    }
}