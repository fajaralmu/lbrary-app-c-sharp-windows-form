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
    class VisitForm : BaseForm
    {
        TextBox TxtInputID = new TextBox();
        Button RecordButton = new Button();
        UserClient userClient = new UserClient();

        Panel StudentInfoPanel = new Panel();
        AppForm RefForm;
        public VisitForm(AppForm RefForm)
        {
            
            this.RefForm = RefForm;
            RefForm.Enabled = false;
            Init();
            Show();
        }

        private void Init()
        {
            Name = "Visit Recorder";
            Text = @"Visit Recorder";
            Width = 600;
            Height = 500;
            RecordButton.Text = "Submit";
            RecordButton.Click += new EventHandler(SubmitVisit);
            Control[] ControlsList =
            {
                new Label() {Text="Input Student Id" },TxtInputID,
                null,RecordButton
            };
            Panel InputPanel = ControlUtil.PopulatePanel(2, ControlsList, 5, 200, 50, System.Drawing.Color.Yellow);
            this.Controls.Add(InputPanel);
            Controls.Add(StudentInfoPanel);
        }

        private void SubmitVisit(object sender, EventArgs e)
        {
            string Id = TxtInputID.Text;
            student Student = userClient.StudentVisit(Id);
        //    StudentInfoPanel.Controls.Clear();
            Controls.Remove(StudentInfoPanel);
            if (Student != null)
            {
                Control[] ControlsList =
                {
                    new Label() {Text="Visit ID" }, new Label() {Text=Student.visits.ElementAt(0).id.ToString() },
                    new Label() {Text="Time" }, new Label() {Text=Student.visits.ElementAt(0).date.ToString() },
                    null,null,
                    new Label() {Text="ID" }, new Label() {Text=Student.id },
                    new Label() {Text="Name" }, new Label() {Text=Student.name },
                    new Label() {Text="Class Id" },new Label() {Text=Student.class_id }
                };
                
                StudentInfoPanel = ControlUtil.PopulatePanel(2, ControlsList, 5, 200, 20, Color.Azure,5,200);

            }else
            {
                Control[] ControlsList =
                {
                    new TitleLabel(20) {Text="Student Not Found" }, null
                };
                StudentInfoPanel = ControlUtil.PopulatePanel(2, ControlsList, 5, 200, 70, Color.Azure, 5, 200);

            }

            Controls.Add(StudentInfoPanel);
        }

        protected override void OnClosed(EventArgs e)
        {
            RefForm.Enabled = true;
            base.OnClosed(e);
        }


    }
}
