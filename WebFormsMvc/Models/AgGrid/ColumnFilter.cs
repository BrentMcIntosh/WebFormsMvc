namespace WebFormsMvc.Models.AgGrid
{
    public class ColumnFilter : Condition
    {
        public Condition Condition1 { get; set; }

        public Condition Condition2 { get; set; }

        public string Operator { get; set; }
    }
}