using OurLibrary.Models;
using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Src.App.Access;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    class IssueForm : BaseForm
    {

        private Panel StudentInfoPanel = new Panel();
        private Panel IssueDetailPanel = new Panel();
        private Panel BookIssuesPanel = new Panel();
        private AppUser appUser;
        private student Student;
        private TextBox InputStudentId = new TextBox();
        private TextBox InputRecId = new TextBox();
        private Button BtnSearchStudent = new Button() { Text = "Search by id" };
        private Button BtnAddBook = new Button() { Text = "Add Book" };
        private Button BtnSubmitIssue = new Button() { Text = "Submit" };
        private Button BtnClear = new Button() { Text = "Clear" };
        private List<book_issue> BookIssues = new List<book_issue>();

        public IssueForm(AppForm RefForm)
        {
            Text = @"Issue";
            this.RefForm = RefForm;
            this.appUser = RefForm.TheUser;
            RefForm.Enabled = false;
            Init();
            Show();
        }
        private void Init()
        {
            Width = 600;
            Height = 700;

            //component

            BtnSearchStudent.Click += new EventHandler(SearchStudent);
            BtnAddBook.Click += new EventHandler(SearchBookRec);
            BtnClear.Click += new EventHandler(Clear);
            BtnSubmitIssue.Click += new EventHandler(SubmitIssue);
            //

            UpdateForm();
        }

        private void SubmitIssue(object sender, EventArgs e)
        {

            var Confirm = MessageBox.Show("Are You Sure To Submit Issue ??",
                                     "Confirm Issue!!",
                                     MessageBoxButtons.YesNo);
            if (Confirm == DialogResult.No)
            {
                return;
            }
            if (Student == null || BookIssues.Count == 0)
            {
                MessageBox.Show("Complete the field!");
                return;
            }
            issue Issue = UserClient.SubmitIssue(BookIssues, Student.id, appUser);
            if (Issue == null)
            {
                MessageBox.Show("Issue Failed");
                return;
            }
            BookIssues.ForEach(b =>
            {
                book_issue ExistInRecordedIssue = (GetByRecId(b.book_record_id, Issue.book_issue));
                if (ExistInRecordedIssue != null)
                {
                    b.id = ExistInRecordedIssue.id;
                }
                else
                {
                    b.id = "FAILED";
                }
            });

            MessageBox.Show("Issue Success with ID: " + Issue.id);
            UpdateForm();

        }

        private book_issue GetByRecId(string RecId, List<book_issue> List)
        {
            foreach (book_issue BS in List)
            {
                if (BS.book_record_id.Equals(RecId))
                {
                    return BS;
                }
            }
            return null;
        }

        private void Clear(object sender, EventArgs e)
        {
            InputRecId.Text = "";
            InputStudentId.Text = "";

            BookIssues.Clear();
            this.Student = null;
            UpdateForm();
        }

        private bool IsExistInList(string RecId, List<book_issue> BookIssues)
        {
            foreach (book_issue BS in BookIssues)
            {
                if (BS.book_record_id.Equals(RecId))
                    return true;
            }
            return false;
        }

        private void SearchBookRec(object sender, EventArgs e)
        {
            string RecId = InputRecId.Text.Trim();
            book_record BookRecord = UserClient.BookRecById(RecId, appUser);
            if (BookRecord != null && !IsExistInList(RecId, BookIssues))
            {
                BookIssues.Add(new book_issue() { book_record_id = BookRecord.id, book_record = BookRecord });
            }
            UpdateForm();
        }

        private Panel GenerateStudentInfoPanel(student Student, int x, int y)
        {
            return ControlUtil.GeneratePanel(1, new Control[]
            {
                new Label() {Text= "Student ID" },
                new TextBoxReadonly(13) {Text=Student.id },
                new Label() {Text= "Student Name" },
                new TextBoxReadonly(13) {Text=Student.name },
                new Label() {Text= "Student ClassId"},
                new TextBoxReadonly(13) {Text=Student.class_id },
            }, 5, 200, 17, Color.Yellow, x, y);
        }

        private void SearchStudent(object sender, EventArgs e)
        {
            string StudentId = InputStudentId.Text;
            student Student = UserClient.StudentById(StudentId, appUser);
            if (Student != null)
            {
                this.Student = Student;

            }
            else
            {
                this.Student = null;
                MessageBox.Show("Student not found");
            }
            UpdateForm();

        }

        private Panel GenerateBookIssuesTable(List<book_issue> BookIssues, int x, int y)
        {
            Control[] TableControls = new Control[5 * (BookIssues.Count + 1)];
            int ControlIdx = 0;
            TableControls[ControlIdx++] = new Label() { Text = "No" };
            TableControls[ControlIdx++] = new Label() { Text = "RecId" };
            TableControls[ControlIdx++] = new Label() { Text = "Title" };
            TableControls[ControlIdx++] = new Label() { Text = "Option" };
            TableControls[ControlIdx++] = new Label() { Text = "GeneratedId" };
            int No = 1;
            foreach (book_issue BS in BookIssues)
            {
                string RecId = BS.book_record.id;
                Button BtnRemove = new Button() { Text = "Remove" };
                BtnRemove.Click += new EventHandler((s, e) =>
                {
                    RemoveBookIssue(RecId);
                });
                TableControls[ControlIdx++] = new Label() { Text = No++.ToString() };
                TableControls[ControlIdx++] = new TextBoxReadonly() { Text = BS.book_record.id + "(" + BS.book_record.available + ")" };
                TableControls[ControlIdx++] = new TextBoxReadonly() { Text = BS.book_record.book.title };
                TableControls[ControlIdx++] = BtnRemove;
                TableControls[ControlIdx++] = new TextBoxReadonly() { Text = BS.id };
            }


            return ControlUtil.GeneratePanel(5, TableControls, 5, 100, 20, Color.Cornsilk, x, y, 550, 400);

        }

        private void RemoveBookIssue(string RecId)
        {
            foreach (book_issue BS in BookIssues)
            {
                if (BS.book_record.id.Equals(RecId))
                {
                    BookIssues.Remove(BS);
                    break;
                }
            }
            UpdateForm();
        }

        private void UpdateForm()
        {
            Controls.Clear();
            InputRecId.Text = "";
            if (Student != null)
            {
                StudentInfoPanel = GenerateStudentInfoPanel(Student, 370, 10);
                Controls.Add(StudentInfoPanel);
                //populate book issues
                BookIssuesPanel = GenerateBookIssuesTable(BookIssues, 10, 270);
                Controls.Add(BookIssuesPanel);
            }
            GenerateFields();
        }

        private void GenerateFields()
        {
            BtnAddBook.Enabled = Student != null;
            BtnClear.Enabled = Student != null;
            BtnSubmitIssue.Enabled = Student != null;
            Control[] FieldControls = new Control[]
            {
                new TitleLabel(20) {Text="Book Issue" }, new BlankControl() {Reserved=ReservedFor.BEFORE_HOR },
                new Label() {Text="Student ID" },  InputStudentId,
                BtnSearchStudent, null,
                new Label() {Text="Rec Id" }, InputRecId,
                BtnAddBook, BtnClear,
                BtnSubmitIssue, null

            };
            IssueDetailPanel = ControlUtil.GeneratePanel(2, FieldControls, 5, 150, 30, Color.Coral, 10, 10);
            Controls.Add(IssueDetailPanel);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.RefForm.Enabled = true;
        }
    }
}
