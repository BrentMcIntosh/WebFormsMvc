using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFormsMvc.Models
{
    public class RequiredDocumentModel
    {
        public int MemberID { get; set; }
        public int RequiredDocumentID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public bool Approved { get; set; }
        public bool IsHardCopy { get; set; }
        public bool HasFile { get; set; }
        public String FileURL { get; set; }
        public String FilePath { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateSubmitted { get; set; }
    }
}