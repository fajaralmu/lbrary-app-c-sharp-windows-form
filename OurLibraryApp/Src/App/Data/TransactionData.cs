using OurLibrary.Models;
using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Gui.App.Home;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Src.App.Data
{
    class TransactionData : BaseData
    {

        private List<issue> Issues = new List<issue>();

        public TransactionData(AppUser AppUser) : base("issuesList")
        {
            this.AppUser= AppUser;
            ListObjServiceName = "issuesList";
            Entity = typeof(issue);
        }
        public TransactionData(string Name, AppUser AppUser) : base("issuesList")
        {
            this.AppUser = AppUser;
            Entity = typeof(issue);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> issueListInfo = ObjMapInfo;
            Issues = (List<issue>)issueListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Issues);
            EntityTotalCount = (int)issueListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            int TotalCount = 0;
            List<issue> issues = new List<issue>();
            List<Dictionary<string, object>> issueListMap = ObjectList;

            if (issueListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> issueMap in issueListMap)
            {
                if (issueMap.Keys.Count == 1 && issueMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)issueMap["count"];
                    break;
                }
               
                issue issue = (issue)ObjectUtil.FillObjectWithMap(new issue(), issueMap);
                issue.appUser = AppUser;
                issues.Add(issue);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",issues }
            };
        }

        protected override Panel ShowDetailPanel(object Object)
        {
            issue Issue = (issue)Object;
            Panel DetailPanel = new Panel();
            Control[] DetailsCol = new Control[7 * (Issue.book_issue.Count + 1)];
            //update
            string[] ColumnLabels = { "No", "IssueId", "RecId", "Title", "", "Returned", Issue.type.Trim() + " item id" };
            for (int i = 0; i < ColumnLabels.Length; i++)
            {
                DetailsCol[i] = new TitleLabel(11) { Text = ColumnLabels[i] };
            }
            int ControlIndex = 7;
            for (int i = 0; i < Issue.book_issue.Count; i++)
            {
                book_issue BS = Issue.book_issue[i];

                if (Issue.type.Trim().Equals("issue"))
                {
                    Dictionary<string, object> CheckReturnResponse = Transaction.FetchObj(0, 0, Transaction.URL, "checkReturnedBook", AppUser, new Dictionary<string, object>()
                        {
                            {"book_issue_id",BS.id }
                        });
                    if (CheckReturnResponse["result"].ToString() == "0")
                    {
                        BS.book_issue_id = StringUtil.JSONStringToMap(CheckReturnResponse["data"].ToString())["book_issue_id"].ToString();
                    }
                }

                DetailsCol[ControlIndex++] = new Label() { Text = (i + 1).ToString() };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.book_record_id };
                DetailsCol[ControlIndex++] = new TextBoxReadonly() { Text = BS.book_record.book.title };
                DetailsCol[ControlIndex++] = new BlankControl() { Reserved = ReservedFor.BEFORE_HOR };
                string Type = Issue.type.ToLower().Trim();
                if (Type == ("return"))
                {
                    DetailsCol[ControlIndex++] = null;
                }
                else
                {
                    DetailsCol[ControlIndex++] = new Label() { Text = BS.book_return == 1 ? "yes " + BS.ref_issue : "No" };

                }
                DetailsCol[ControlIndex++] = new Label() { Text = BS.book_issue_id };
            }
            //
            DetailPanel = ControlUtil.GeneratePanel(7, DetailsCol, 5, 80, 20, Color.Orange, 5, 250);

            student Student = UserClient.StudentById(Issue.student_id, AppUser);

            Panel StudentDetail = ControlUtil.GeneratePanel(1, new Control[]
            {
                new Label() {Text= Issue.type.ToUpper().Trim()+" ID"},
                new TextBoxReadonly(13) {Text=Issue.id },
                new Label() {Text= "Date" },
                new TextBoxReadonly(13) {Text=Issue.date.ToString() },
                new Label() {Text= "Student ID" },
                new TextBoxReadonly(13) {Text=Student.id },
                new Label() {Text= "Student Name" },
                new TextBoxReadonly(13) {Text=Student.name },
                new Label() {Text= "Student ClassId"},
                new TextBoxReadonly(13) {Text=Student.class_id },
            }, 5, 200, 17, Color.Yellow, 5, 5, 500);

            Panel Wrapper = new Panel();
            Wrapper.Controls.Add(StudentDetail);
            Wrapper.Controls.Add(DetailPanel);
            Wrapper.SetBounds(5, 5, 500, 500);

            Wrapper.AutoScroll = false;
            Wrapper.VerticalScroll.Visible = true;
            Wrapper.VerticalScroll.Enabled = true;
            Wrapper.AutoScroll = true;

            return Wrapper;
        }
    }
}
