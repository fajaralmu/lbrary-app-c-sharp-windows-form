//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OurLibrary.Models
{
    using Annotation;
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class visit
    {
        [FieldAttribute(AutoGenerated = true, Required = true, FieldType = AttributeConstant.TYPE_ID_AI)]

        public long id { get; set; }
        [FieldAttribute( FieldType = AttributeConstant.TYPE_READONLY)]

        public string student_id { get; set; }
        [FieldAttribute(FieldType = AttributeConstant.TYPE_READONLY)]

        public System.DateTime date { get; set; }
        [FieldAttribute( FieldType = AttributeConstant.TYPE_TEXTAREA)]

        public string info { get; set; }
    
        public virtual student student { get; set; }
    }
}
