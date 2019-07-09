using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurLibrary.Annotation
{
    public class ActionAttribute:Attribute
    {
        public string FieldType { get; set; }
        
    }
}