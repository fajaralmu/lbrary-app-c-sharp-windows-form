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
    using OurLibraryApp.Gui.App.Controls;
    using OurLibraryApp.Src.App.Access;
    using OurLibraryApp.Src.App.Utils;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public partial class student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public student()
        {
            this.issues = new HashSet<issue>();
        }

        [FieldAttribute(AutoGenerated = true, Required = true, FieldType = AttributeConstant.TYPE_ID_NUMB, FixSize = 7)]
        public string id { get; set; }

        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_TEXTBOX)]
        public string name { get; set; }

        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_DATE, FieldName = "Birth Date")]
        public string bod { get; set; }

        public string class_id { get; set; }

        [FieldAttribute(Required = false, FieldType = AttributeConstant.TYPE_TEXTBOX)]
        public string email { get; set; }

        [FieldAttribute(Required = false, FieldType = AttributeConstant.TYPE_TEXTAREA)]
        public string address { get; set; }

        [FieldAttribute(Required = true, FieldType = AttributeConstant.TYPE_DROPDOWN, ClassReference = "class", ClassAttributeConverter = "class_name", FieldName = "Kelas")]
        public virtual @class @class { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<issue> issues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<visit> visits { get; set; }

        public AppUser AppUser { get; set; }

        [ActionAttribute(FieldType = AttributeConstant.TYPE_DETAIL_CLICK)]
        public Panel DetailPanel()
        {
            Panel Detail = new Panel();

            //TitleLabel issueTitle = new TitleLabel() { Text = "Issue List" };
            //issueTitle.setb
            Panel IssuePanel = DetailPanel("issue", 5, 25, 600, 500);
            //TitleLabel returnTitle = new TitleLabel() { Text = "Return List" };
            Panel ReturnPanel = DetailPanel("return", 5, 540, 600, 500);
            Detail.Controls.Add(new Label() { Text="* = not returned"});
            Detail.Controls.Add(IssuePanel);
            Detail.Controls.Add(ReturnPanel);
            Detail.SetBounds(5, 5, 700, 1200);
            return Detail;
        }

        public Panel DetailPanel(string Type, int X, int Y, int W, int H)
        {
            List<book_issue> BookIssues = new List<book_issue>();
            //fetch obj
            Dictionary<string, object> FilterParams = new Dictionary<string, object>();
            FilterParams.Add("student_id", this.id);
            FilterParams.Add("orderby", "book_issue.book_return");
            FilterParams.Add("ordertype", "asc");
            FilterParams.Add("issue_type", Type);

            List<Dictionary<string, object>> ObjectMapList = Transaction.MapList(0, 0, Transaction.URL, "bookIssueList", AppUser, FilterParams);

            //generate panel
            Panel DetailPanel = new Panel();
            int ListCount = ObjectMapList == null ? 0 : ObjectMapList.Count;
            Control[] DetailsCol = new Control[8 * (ListCount + 2)];
            //update
            string[] ColumnLabels = { "No", "Id", "Trx Id", "RecId", /*"Title",*/ "", "Returned", "Book Issue Ref", "" };

            //generate title
            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    DetailsCol[i] = new TitleLabel(13) { Text = ("============"+Type.ToUpper() + " LIST============") };
                }
                else
                {
                    DetailsCol[i] = new BlankControl() { Reserved = ReservedFor.BEFORE_HOR };
                }
            }
            //generate column name
            for (int i = 8; i < ColumnLabels.Length + 8; i++)
            {
                if (ColumnLabels[i - 8] == "")
                {
                    DetailsCol[i] = new BlankControl() { Reserved = ReservedFor.BEFORE_HOR };
                }
                else
                    DetailsCol[i] = new Label() { Text = ColumnLabels[i - 8] };
            }
            int ControlIndex = 16;
            if (ObjectMapList == null)
            {
                goto End;
            }

            foreach (Dictionary<string, object> book_issueMap in ObjectMapList)
            {

                book_issue book_issue = (book_issue)ObjectUtil.FillObjectWithMap(new book_issue(), book_issueMap);
                BookIssues.Add(book_issue);
            }

            for (int i = 0; i < BookIssues.Count; i++)
            {
                book_issue BS = BookIssues[i];
                if (BS == null) continue;
                if (BS.id == null) continue;
                //if (type.Trim().Equals("issue"))
                //{
                //    Dictionary<string, object> CheckReturnResponse = Transaction.FetchObj(0, 0, Transaction.URL, "checkReturnedBook", new Dictionary<string, object>()
                //        {
                //            {"book_issue_id",BS.id }
                //        });
                //    if (CheckReturnResponse["result"].ToString() == "0")
                //    {
                //        BS.book_issue_id = StringUtil.JSONStringToMap(CheckReturnResponse["data"].ToString())["book_issue_id"].ToString();
                //    }
                //}
                bool NotReturned = Type == "issue" && BS.book_return == 0;

                DetailsCol[ControlIndex++] = new Label() { Text = (i + 1).ToString()+(NotReturned?"*":"") };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.issue_id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.book_record_id };
                //DetailsCol[ControlIndex++] = new Label() { Text = BS.book_record.book.title };
                DetailsCol[ControlIndex++] = new BlankControl() { Reserved = ReservedFor.BEFORE_HOR };
                //string Type = this.type.ToLower().Trim();
                //if (Type == ("return"))
                //{
                //    DetailsCol[ControlIndex++] = null;
                //}
                //else
                //{
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.book_return.ToString() };

                //}
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.book_issue_id };
                DetailsCol[ControlIndex++] = new BlankControl() { Reserved = ReservedFor.BEFORE_HOR };
            }
            //
            End:
            DetailPanel = ControlUtil.GeneratePanel(8, DetailsCol, 5, 70, 20, Color.Orange, X, Y, W, H);
            return DetailPanel;
        }
    }
}
