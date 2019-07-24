using OurLibrary.Annotation;
using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Gui.App.Home;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Src.App.Data
{
    class BaseData
    {
        public Dictionary<string, object> FilterParmas = new Dictionary<string, object>();
        public Type Entity;
        public Panel EntityListPanel;
        public List<object> EntityList { get; set; }
        public Panel DetailPanel;
        public int EntityTotalCount = 0;
        public EntityForm EntityForm;
        protected string ListObjServiceName;
        public string Name { get; set; }
        public BaseData(string ListSvcName)
        {
            ListObjServiceName = ListSvcName;
        }

        public void SetEntityForm(EntityForm EntityForm)
        {
            this.EntityForm = EntityForm;
        }

        public virtual Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMap)
        {
            return new Panel();
        }

        protected Panel GeneratePanel(int Offset, int Limit)
        {
            int CustomedProp = ObjectUtil.CustomAttributesCount(Entity);
            Control[] TableControls = new Control[(CustomedProp + 2) * (EntityList.Count + 4)];
            //HEADER//

            PropertyInfo[] Props = Entity.GetProperties();
            int ControlIndex = 0;
            TableControls[ControlIndex++] = new Label() { Text = "No" };
            TableControls[ControlIndex + 2 * (CustomedProp + 2)] = new Label() { Text = "==" }; ;
            for (int i = 0; i < Props.Length; i++)
            {
                PropertyInfo PropsInfo = Props[i];
                object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                if (Attributes.Length > 0)
                {
                    FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                    if (Attribute.FieldType != null)
                    {
                        string FieldName = Attribute.FieldName != null && Attribute.FieldName != "" ? Attribute.FieldName : PropsInfo.Name;
                        TableControls[ControlIndex++] = new Label() { Text = FieldName.ToUpper() };

                        TextBox FilterTxtBox = new TextBox() { Name = "FILTER_00_BY_" + PropsInfo.Name, Text = FilterParmas.ContainsKey(PropsInfo.Name) ? FilterParmas[PropsInfo.Name].ToString() : "" };
                        Button FilterBtn = new Button() { Text = "Search" };
                        Button ASCBtn = new Button() { Text = "ASC" };
                        Button DESCBtn = new Button() { Text = "DESC" };

                        string OrderBy = Entity.Name + "." + PropsInfo.Name;
                        if (Attribute.ClassReference != null)
                        {
                            OrderBy = Attribute.ClassReference + "." + Attribute.ClassAttributeConverter;
                        }


                        FilterBtn.Click += new EventHandler((o, e) =>
                        {
                            if (this.FilterParmas.ContainsKey(PropsInfo.Name))
                            {
                                FilterParmas.Remove(PropsInfo.Name);
                            }
                            FilterParmas.Add(PropsInfo.Name, FilterTxtBox.Text);
                            EntityForm.Navigate(0, 0);
                        });
                        ASCBtn.Click += new EventHandler((o, e) =>
                         {
                             if (this.FilterParmas.ContainsKey("orderby"))
                             {
                                 FilterParmas.Remove("orderby");
                             }
                             if (this.FilterParmas.ContainsKey("ordertype"))
                             {
                                 FilterParmas.Remove("ordertype");
                             }
                             FilterParmas.Add("orderby", OrderBy);
                             FilterParmas.Add("ordertype", "asc");
                             EntityForm.Navigate(0, 0);
                         });
                        DESCBtn.Click += new EventHandler((o, e) =>
                        {
                            if (this.FilterParmas.ContainsKey("orderby"))
                            {
                                FilterParmas.Remove("orderby");
                            }
                            if (this.FilterParmas.ContainsKey("ordertype"))
                            {
                                FilterParmas.Remove("ordertype");
                            }
                            FilterParmas.Add("orderby", OrderBy);
                            FilterParmas.Add("ordertype", "desc");
                            EntityForm.Navigate(0, 0);
                        });

                        Panel SortPanel = ControlUtil.PopulatePanel(false, 2, new Control[] { ASCBtn, DESCBtn }, 0, 45, 17, Color.Coral);

                        Panel FilterPanel = ControlUtil.PopulatePanel(1, new Control[] { FilterTxtBox ,
                                FilterBtn,SortPanel }, 5, 90, 18, Color.LightYellow);
                        //FilterPanel.MinimumSize = new Size(90, 80);
                        TableControls[ControlIndex + CustomedProp + 1] = FilterPanel;
                        TableControls[ControlIndex + 2 * (CustomedProp) + 3] = new BlankControl() { Reserved = ReservedFor.BEFORE_VER };

                    }
                }
            }
            TableControls[ControlIndex++] = new Label() { Text = "Option" };
            TableControls[ControlIndex + 2 * (CustomedProp + 2)] = new Label() { Text = "==" }; ;
            ControlIndex += 2 * (CustomedProp + 2);

            int No = Offset * Limit;
            foreach (object obj in EntityList)
            {
                No++;
                string NoStr = Convert.ToString(No);
                TableControls[ControlIndex++] = new Label() { Text = (No).ToString() };
                for (int i = 0; i < Props.Length; i++)
                {
                    PropertyInfo PropsInfo = Props[i];
                    object[] Attributes = PropsInfo.GetCustomAttributes(typeof(FieldAttribute), true);
                    if (Attributes.Length > 0)
                    {
                        FieldAttribute Attribute = (FieldAttribute)Attributes[0];
                        if (Attribute.FieldType != null)
                        {
                            object PropValue = obj.GetType().GetProperty(PropsInfo.Name).GetValue(obj);
                            string VAL = "-";
                            if (PropValue != null && Attribute.ClassReference != null && Attribute.ClassAttributeConverter != null)
                            {
                                object ClassReff = PropValue;
                                object ClassRefConverterValue = ClassReff.GetType().GetProperty(Attribute.ClassAttributeConverter).GetValue(ClassReff);
                                VAL = ClassRefConverterValue == null?null: ClassRefConverterValue.ToString();
                            }
                            else
                            if (PropValue != null && Attribute.DropDownItemName != null && Attribute.DropDownValues != null && PropValue != null
                                && Attribute.DropDownItemName.Length > 0 && Attribute.DropDownValues.Length > 0)
                            {
                                string index = PropValue.ToString();
                                int intindex = 0;
                                int.TryParse(index, out intindex);
                                VAL = Attribute.DropDownItemName[intindex].ToString();
                            }
                            else if (PropValue != null && Attribute.FieldType.Equals(AttributeConstant.TYPE_COUNT))
                            {
                                VAL = ((ICollection)PropValue).Count.ToString();
                            }
                            else if (PropValue != null)
                            {
                                VAL = PropValue.ToString();
                            }
                            TableControls[ControlIndex++] = new Label() { Text = VAL };

                        }

                        /*if (Attribute.ClassReference != null)
                        {
                            Type refType = Type.GetType(ModelParameter.NameSpace + Attribute.ClassReference);
                            string refConverter = Attribute.ClassAttributeConverter;
                            PropertyInfo AttributeConverter = refType.GetProperty(refConverter);

                        }*/
                    }

                }

                Button BtnDetail = new Button() { Text = "Detail", Enabled = false };

                foreach (MethodInfo Method in Entity.GetMethods())
                {
                    object[] Attributes = Method.GetCustomAttributes(typeof(ActionAttribute), true);
                    if (Attributes.Length > 0)
                    {
                        ActionAttribute ActionAttr = (ActionAttribute)Attributes[0];
                        if (ActionAttr != null && ActionAttr.FieldType.Equals(AttributeConstant.TYPE_DETAIL_CLICK))
                        {
                            BtnDetail.Click += (o, e) =>
                            {
                                Panel DetailPanel = (Panel)Method.Invoke(obj, null);
                                EntityForm.ShowDetail(DetailPanel);
                            };
                            BtnDetail.Enabled = true;
                            goto next;
                        }
                    }
                }
                next:
                TableControls[ControlIndex++] = BtnDetail;

            }
            return ControlUtil.PopulatePanel(CustomedProp + 2, TableControls, 5, 100, 30, Color.White, 5, 130, Constant.ENTITY_PANEL_WIDTH, Constant.ENTITY_PANEL_HEIGHT);

        }

        internal Panel UpdateData(int offset, int limit)
        {
            Dictionary<string, object> ObjMap = this.GetList(offset, limit, this.FilterParmas, this.ListObjServiceName);
            if(ObjMap == null)
            {
                return null;
            }
            return UpdateListPanel(offset, limit, ObjMap);
        }

        public Dictionary<string, object> GetList(int Offset, int Limit, Dictionary<string, object> FilterParams, string ServiceName)
        {
            List<Dictionary<string, object>> ObjectMapList = Transaction.MapList(Offset, Limit, Transaction.URL, ServiceName, FilterParams);
            if(ObjectMapList == null)
            {
                return null;
            }

            return ObjectList(Offset, Limit, ObjectMapList);
        }

        public virtual Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            return new Dictionary<string, object>();
        }


    }
}
