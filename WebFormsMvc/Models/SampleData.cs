using System;

namespace WebFormsMvc.Models
{
    public class SampleData
    { 
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime Born { get; set; }

        public virtual decimal Cost { get; set; }

        public virtual bool Good { get; set; }
    }
}