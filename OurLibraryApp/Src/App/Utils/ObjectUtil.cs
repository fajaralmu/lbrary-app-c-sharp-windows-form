using Newtonsoft.Json.Linq;
using OurLibrary.Annotation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OurLibraryApp.Src.App.Utils
{
    class ObjectUtil
    {
        public static object FillObjectWithMap(object Object, Dictionary<string, object> Map, string[] Keys)
        {
            foreach (string K in Keys)
            {
                if (Map.ContainsKey(K))
                {
                    JValue Value = (JValue)Map[K];
                    object Val = Map[K];
                    switch (Value.Type.ToString())
                    {
                        case "Integer":
                            Val = int.Parse(Val.ToString());
                            break;
                        case "String":
                            Val = Val.ToString();
                            break;
                        case "Null":
                            Val = null;
                            break;
                    }

                    if (HasProperty(K, Object))
                    {
                        Object.GetType().GetProperty(K).SetValue(Object, Val);
                    }
                }
            }
            return Object;
        }

        public static bool HasProperty(string PropName, object O)
        {
            foreach (PropertyInfo Prop in O.GetType().GetProperties())
            {
                if (Prop.Name.Equals(PropName))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CustomAttributesCount(Type O)
        {
            int count = 0;
            foreach (PropertyInfo Prop in O.GetProperties())
            {
                object[] Attributes = Prop.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    count++;
                }
            }
            return count;
        }

        public static List<object> ListToListObj(ICollection O)
        {
            List<object> List = new List<object>();
            foreach (var o in O)
            {
                List.Add(o);
            }
            return List;
        }
    }
}
