using Newtonsoft.Json.Linq;
using OurLibrary.Annotation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OurLibrary.Models;

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
                        case "Date":
                            Val = DateTime.Parse(Val.ToString());
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
            if (O == null)
            {
                O = new object();
            }

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

        public static object FillObjectWithMap(object OBJ, Dictionary<string, object> ObjMap)
        {
            foreach (string key in ObjMap.Keys)
            {
                object keyVal = ObjMap[key];
                if (null != keyVal && HasProperty(key, OBJ))
                {
                    object Value = null;
                    Type KeyType = keyVal.GetType();
                    if (KeyType.Equals(typeof(JArray)))
                    {
                        Value = ((JArray)ObjMap[key]).ToObject(OBJ.GetType().GetProperty(key).PropertyType);
                    }
                    else
                    if (KeyType.Equals(typeof(JValue)))
                    {
                        Value = ((JValue)ObjMap[key]).ToObject(OBJ.GetType().GetProperty(key).PropertyType);
                    }
                    else
                    if (KeyType.Equals(typeof(JObject)))
                    {
                        Value = ((JObject)ObjMap[key]).ToObject(typeof(Dictionary<string, object>));
                    }
                    else
                    if (KeyType.Equals(typeof(Int64)))
                    {
                        Value = Convert.ToInt32(ObjMap[key]);
                    }
                    else
                    {
                        Value = ObjMap[key];
                    }
                    if (null == Value)
                    {
                        continue;
                    }
                    if (Value.GetType().Equals(typeof(Dictionary<string, object>)))
                    {
                        object ObjValue = Activator.CreateInstance(OBJ.GetType().GetProperty(key).PropertyType);
                        Value = FillObjectWithMap(ObjValue, (Dictionary<string, object>)Value);
                    }
                    else if (Value.GetType().Equals(typeof(List<>)) || Value.GetType().Equals(typeof(ICollection)))
                    {
                        List<object> ObjList = new List<object>();
                        List<object> ValList = (List<object>)Value;
                        foreach (object o in ValList)
                        {
                            if (o.GetType().Equals(typeof(Dictionary<string, object>)))
                            {
                                object itemVal = null;
                                object ObjValue = Activator.CreateInstance(OBJ.GetType().GetProperty(key).PropertyType);
                                itemVal = FillObjectWithMap(null, (Dictionary<string, object>)o);
                                ObjList.Add(itemVal);
                            }
                        }
                        Value = ObjList;
                    }

                    OBJ.GetType().GetProperty(key).SetValue(OBJ, Value);
                }
            }
            return OBJ;
        }

        public static string[] ValidateProps(string[] Props, object Obj)
        {
            List<string> ValidProp = new List<string>();
            for (int j = 0; j < Props.Length; j++)
            {
                string Prop = Props[j];
                if (HasProperty(Prop, Obj))
                {
                    ValidProp.Add(Prop);
                }
            }
            return ValidProp.ToArray<string>();
        }

        public static object ConvertList(List<object> value, Type type)
        {
            IList list = (IList)Activator.CreateInstance(type);
            foreach (var item in value)
            {
                list.Add(item);
            }
            return list;
        }

        public static List<object> ICollectionToListObj(ICollection Collection)
        {
            List<object> List = new List<object>();
            foreach (var item in Collection)
            {
                if (item == null)
                    continue;
                List.Add(item);
            }
            return List;
        }

        public static string ListToDelimitedString(object ListOfObject, string Delimiter, string ValDelimiter, params string[] Props)
        {
            String ListString = "";
            if (ListOfObject == null)
                return "";
            List<object> List = ICollectionToListObj((ICollection)ListOfObject);

            string[] ValidProps = ValidateProps(Props, List.ElementAt(0));

            for (int i = 0; i < List.Count; i++)

            {
                object Obj = List.ElementAt(i);
                string Value = "";
                for (int j = 0; j < ValidProps.Length; j++)
                {
                    string Prop = ValidProps[j];
                    Value += GetValueFromProp(Prop, Obj).ToString();
                    if (j < ValidProps.Length - 1)
                    {
                        Value += ValDelimiter;
                    }
                }
                ListString += Value;
                if (i < List.Count - 1)
                {
                    ListString += Delimiter;
                }

            }
            return ListString;
        }

        public static object GetValueFromProp(string propname, object Object)
        {
            return Object.GetType().GetProperty(propname).GetValue(Object);
        }

        public static Dictionary<string, object> FillMap(object OBJ)
        {
            Dictionary<string, object> Map = new Dictionary<string, object>();
            foreach (PropertyInfo Prop in OBJ.GetType().GetProperties())
            {
                Map.Add(Prop.Name, GetValueFromProp(Prop.Name, OBJ));
            }
            return Map;
        }

        public static object GetObjectValues(string[] Props, object OriginalObj)
        {
            object NewObject = Activator.CreateInstance(OriginalObj.GetType());
            for (int i = 0; i < Props.Length; i++)
            {
                string PropName = Props[i];
                if (HasProperty(PropName, OriginalObj))
                {
                    object val = OriginalObj.GetType().GetProperty(PropName).GetValue(OriginalObj);
                    NewObject.GetType().GetProperty(PropName).SetValue(NewObject, val);
                }
            }
            return NewObject;
        }
    }
}

