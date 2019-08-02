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
    class StudentData : BaseData
    {

        private TextBox InputID = new TextBox();
        private TextBox InputName = new TextBox();
        // private MonthCalendar InputBOD= new MonthCalendar();
        private Panel BODPanel = new Panel();
        private ComboBox CbxYear = new ComboBox();
        private ComboBox CbxMonth = new ComboBox();
        private ComboBox CbxDay = new ComboBox();
        private TextBox InputEmail = new TextBox();
        private TextBox InputAddress = new TextBox();
        private ComboBox ClassList = new ComboBox();

        private List<@class> ClassDataList = new List<@class>();

        private List<student> Students = new List<student>();

        public StudentData(AppUser AppUser) : base("studentList")
        {
            this.AppUser = AppUser;
            ListObjServiceName = "studentList";
            Entity = typeof(student);
        }
        public StudentData(string Name, AppUser AppUser) : base("studentList")
        {
            this.AppUser = AppUser;
            Entity = typeof(student);
            this.Name = Name;
        }

        public override Panel UpdateListPanel(int Offset, int Limit, Dictionary<string, object> ObjMapInfo)
        {
            Dictionary<string, object> studentListInfo = ObjMapInfo;
            Students = (List<student>)studentListInfo["data"];
            EntityList = ObjectUtil.ListToListObj(Students);
            EntityTotalCount = (int)studentListInfo["totalCount"];
            EntityListPanel = base.GeneratePanel(Offset, Limit);
            return EntityListPanel;
        }

        public override Dictionary<string, object> ObjectList(int Offset, int Limit, List<Dictionary<string, object>> ObjectList)
        {
            int TotalCount = 0;
            List<Dictionary<string, object>> StudentListMap = ObjectList;
            List<student> Students = new List<student>();
            if (StudentListMap.Count == 0)
            {
                goto end;
            }

            foreach (Dictionary<string, object> StudentMap in StudentListMap)
            {
                if (StudentMap.Keys.Count == 1 && StudentMap.Keys.ElementAt(0).Equals("count"))
                {
                    TotalCount = (int)StudentMap["count"];
                    break;
                }
                student Student = (student)ObjectUtil.FillObjectWithMap(new student(), StudentMap);
                Student.AppUser = AppUser;
                Students.Add(Student);
            }

            end:
            return new Dictionary<string, object>() {
                {"totalCount",TotalCount },
                {"data",Students }
            };
        }

        protected override Panel ShowDetailPanel(object Object)
        {
            student Student = (student)Object;
            Panel Detail = new Panel();

            //TitleLabel issueTitle = new TitleLabel() { Text = "Issue List" };
            //issueTitle.setb
            Panel IssuePanel = StudentDetailPanel(Student, "issue", 5, 25, 600, 500);
            //TitleLabel returnTitle = new TitleLabel() { Text = "Return List" };
            Panel ReturnPanel = StudentDetailPanel(Student, "return", 5, 540, 600, 500);
            Detail.Controls.Add(new Label() { Text = "* = not returned" });
            Detail.Controls.Add(IssuePanel);
            Detail.Controls.Add(ReturnPanel);
            Detail.SetBounds(5, 5, 700, 1200);
            return Detail;
        }

        protected Panel StudentDetailPanel(student Student, string Type, int X, int Y, int W, int H)
        {
            List<book_issue> BookIssues = new List<book_issue>();
            //fetch obj
            Dictionary<string, object> FilterParams = new Dictionary<string, object>();
            FilterParams.Add("student_id", Student.id);
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
                    DetailsCol[i] = new TitleLabel(13) { Text = ("============" + Type.ToUpper() + " LIST============") };
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

                DetailsCol[ControlIndex++] = new Label() { Text = (i + 1).ToString() + (NotReturned ? "*" : "") };
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

        public override Panel ShowAddForm(object Object = null)
        {
            student EditStudent = (student)Object;
            bool EditState = EditStudent != null;
            ClearAllFields();
            UpdateList();
            Button BtnAdd = new Button() { Text = "Add" };

            BtnAdd.Click += (e, o) =>
            {
                if(ClassList.SelectedItem == null)
                {
                    ClassList.SelectedItem = ClassList.Items.IndexOf(0);
                }
                student Student = new student()
                {
                    id = EditState ? InputID.Text : null,
                    name = InputName.Text.Trim(),
                    email = InputEmail.Text.Trim(),
                    address = InputAddress.Text.Trim(),
                    class_id = ((ComboboxItem)ClassList.SelectedItem).Value.ToString(),
                    bod = getBODString(),
                    issues = null,
                    visits = null,
                    AppUser = null,
                    @class = null
                };
                if (null != UserClient.AddStudent(Student, AppUser))
                {
                    MessageBox.Show("Success");
                    EntityForm.Navigate(0, 0);
                }
                else
                {
                    MessageBox.Show("Failed");
                }
            };
            InputID.Enabled = false;
            InputID.Text = "GENERATED";

            if (EditState)
            {
                DateTime BOD = StringUtil.StringToDateTime(EditStudent.bod);
                InputID.Text = EditStudent.id;
                InputName.Text = EditStudent.name.Trim();
                InputEmail.Text = EditStudent.email.Trim();
                InputAddress.Text = EditStudent.address.Trim();
                ClassList.SelectedItem = EditStudent.class_id;
                CbxDay.SelectedValue = BOD.Day;
                CbxMonth.SelectedValue = BOD.Month;
                CbxYear.SelectedValue = BOD.Year;
            }

            Control[] Controls = new Control[]
            {
                new TitleLabel(20) {Text="Add Student" },new BlankControl(),
                 new Label() {Text="ID" }, InputID,
                new Label() {Text="Name" }, InputName,
                 new Label() {Text="BOD" }, BODPanel,
                  new Label() {Text="Email" }, InputEmail,
                   new Label() {Text="Address" }, InputAddress,
                    new Label() {Text="Class" }, ClassList,
                      BtnAdd, null
            };

            return ControlUtil.GeneratePanel(2, Controls, 5, 180, 30, Color.Aqua);
        }

        protected override void UpdateList()
        {
            FillDate();
            UserClient UserClient = new UserClient();
            ClassDataList = UserClient.ClassList(AppUser, 0, 0, null);
            foreach (@class ClassItem in ClassDataList)
            {
                ComboboxItem Item = new ComboboxItem(ClassItem.class_name.Trim(), ClassItem.id);
                ClassList.Items.Add(Item);
            }

        }

        private string getBODString()
        {
            try
            {
                return CbxYear.SelectedItem.ToString() + "-"
                    + CbxMonth.SelectedItem.ToString() + "-" +
                    CbxDay.SelectedItem.ToString();
            }catch(Exception ex)
            {
                return "2019-01-01";
            }
        }

        private void FillDate()
        {
            CbxDay = ControlUtil.FillComboInteger(1, 31);
            CbxMonth = ControlUtil.FillComboInteger(1, 12);
            CbxYear = ControlUtil.FillComboInteger(1900, DateTime.Now.Year);
            BODPanel = ControlUtil.GeneratePanel(3, new Control[]
            {
                CbxDay,CbxMonth,CbxYear
            }, 0, 90, 20, Color.Aquamarine);
        }

        protected override void ClearAllFields()
        {
            InputID.Clear();
            InputName.Clear();
            InputEmail.Clear();
            ClassList.Items.Clear();
            InputAddress.Clear();

        }

    }
}
