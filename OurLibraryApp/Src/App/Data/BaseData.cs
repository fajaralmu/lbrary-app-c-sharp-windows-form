using OurLibrary.Annotation;
using OurLibraryApp.Gui.App.Home;
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
        public Type Entity;
        public Panel EntityListPanel;
        public List<object> EntityList { get; set; }
        public Panel DetailPanel;
        public int EntityTotalCount = 0;
        public EntityForm EntityForm;
        public string Name { get; set; }
        public BaseData()
        {

        }

        public void SetEntityForm(EntityForm EntityForm)
        {
            this.EntityForm = EntityForm;
        }

        public virtual Panel UpdateListPanel(int Offset, int Limit)
        {
            return new Panel();
        }

        protected Panel GeneratePanel(int Offset, int Limit)
        {
            int CustomedProp = ObjectUtil.CustomAttributesCount(Entity);
            Control[] TableControls = new Control[(CustomedProp + 2) * (EntityList.Count + 1)];
            //HEADER//

            TableControls[0] = new Label() { Text = "No" };

            PropertyInfo[] Props = Entity.GetProperties();
            int ColNameIdx = 1;
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
                        TableControls[ColNameIdx] = new Label() { Text = FieldName.ToUpper() };
                        ColNameIdx++;
                    }
                }
            }
            TableControls[CustomedProp + 1] = new Label() { Text = "Option" };
            int ControlIndex = CustomedProp + 2; ;

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
                                VAL = ClassRefConverterValue.ToString();
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

                Button BtnDetail = new Button() { Text = "Detail" , Enabled = false};

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
                                Panel DetailPanel =(Panel) Method.Invoke(obj, null);
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
            return ControlUtil.PopulatePanel(CustomedProp + 2, TableControls, 5, 90, 30, Color.White, 5, 130, 760, 500);

        }

      
    }
}
