using System;

namespace WebFormsMvc.Models.AgGrid
{
    public class MyRow
    {
        public int ID { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}