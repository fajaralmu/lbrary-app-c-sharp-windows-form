using OurLibraryApp.Gui.App.Controls;
using OurLibraryApp.Src.App.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OurLibraryApp.Gui.App.Home
{
    class Loading : BaseForm
    {
        TitleLabel LoadingInfo = new TitleLabel();
        private delegate void SafeCallDelegate(string text);

        private Thread thread2 = null;

        public void SetLoadingMsg(string MSG)
        {

            Controls.Remove(LoadingInfo);
            LoadingInfo = new TitleLabel();
            LoadingInfo.Text = MSG;

            Controls.Add(LoadingInfo);
        }

        public void Init()
        {
            Width = 400;
            Height = 200;
            thread2 = new Thread(new ThreadStart(DoInit));
            thread2.Start();
         
        }


        public void DoInit()
        {

            var d = new SafeCallDelegate(SetLoadingMsg);
         //   Invoke(d, new object[] { "Please wait" });

           
            

        }
        public Loading(string info)
        {

            Text = "LOADING";
            //  Init();
            Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            Init();
            base.OnLoad(e);
        }
    }
}
