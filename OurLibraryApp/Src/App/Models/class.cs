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
    public partial class @class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public @class()
        {
            this.students = new HashSet<student>();
        }

        [FieldAttribute(AutoGenerated = true, Required = true, FieldType = AttributeConstant.TYPE_ID_STR_NUMB, FixSize = 6)]
        public string id { get; set; }
        [FieldAttribute(Required = true, FieldName = "Class Name", FieldType = AttributeConstant.TYPE_TEXTBOX)]
        public string class_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<student> students { get; set; }
    }
}
