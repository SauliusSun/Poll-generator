using System;
using Microsoft.Practices.ObjectBuilder;
using System.Web;
using System.Web.UI.WebControls;
using DataAccess;
using System.Web.UI;
using EncryptionUtility;

namespace PollGenerator.Poll.Views
{
    public partial class ManoApklausos : Microsoft.Practices.CompositeWeb.Web.UI.Page, IManoApklausosView
    {
        private ManoApklausosPresenter _presenter;

        //public ImageButton ViewButton;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                string userName = User.Identity.Name;

                Response.Cookies.Add(new HttpCookie("user", userName));

                MyPollsGridView.DataBind();

                if (MyPollsGridView.Rows.Count == 0)
                {
                    ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");

                    content.Controls.Add(new LiteralControl("Jūs neturite sukurtų apklausų. <a href=NaujaApklausa.aspx><b>Kurkite.</b></a>"));
                }
               
            }
            this._presenter.OnViewLoaded();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            MyPollsGridView.DataBind();
        }

        [CreateNew]
        public ManoApklausosPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html

        protected void MyPollsGridView_DataBinding(object sender, EventArgs e)
        {
            string userName = User.Identity.Name;          
        }

        protected void MyPollsGridView_DataBound(object sender, EventArgs e)
        {
            
        }

        

        protected void MyPollsDataSource_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            string userName = User.Identity.Name;

            HttpCookie cookie = new HttpCookie("user");

            cookie.Values.Add("user", userName);


        }

        // delete questions from reorder list 
        protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (ImageButton)sender;

            // get poll id
            string pollId = button.CommandArgument;

            DbAccessManager manager = new DbAccessManager();

            manager.DeletePoll(int.Parse(pollId));

            MyPollsGridView.DataSource = null;

            MyPollsGridView.DataBind();

            if (MyPollsGridView.Rows.Count == 0)
            {
                ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");

                content.Controls.Add(new LiteralControl("Jūs neturite sukurtų apklausų. <a href=NaujaApklausa.aspx><b>Kurkite.</b></a>"));
            }

        }

        // Preference button click
        protected void PreferenceButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton PreferenceButton = (ImageButton)sender;

            // pollId
            string pollId = PreferenceButton.CommandArgument;

            string encryptedId = Encryption.Encrypt(pollId);

            Response.Redirect("~/Poll/ApklausosValdymas.aspx?ID=" + Server.UrlEncode(encryptedId));

        }

        // ViewButton click
        protected void ViewButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ViewButton = (ImageButton)sender;

            // pollId
            string pollId = ViewButton.CommandArgument;

            string encryptedId = Encryption.Encrypt(pollId);

            Response.Redirect("~/Apklausa.aspx?ID=" + Server.UrlEncode(encryptedId));

        }

        protected void MyPollsGridView_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                HyperLink Url = (HyperLink)e.Row.FindControl("NameHyperlink");

                string pollId = Url.ImageUrl;

                string encodedPollId = Encryption.Encrypt(pollId); 

                Url.NavigateUrl = "~/Poll/ApklausosValdymas.aspx?ID=" + Server.UrlEncode(encodedPollId);
          
            }
            
        }
}
}

