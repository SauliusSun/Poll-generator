using System;
using Microsoft.Practices.ObjectBuilder;
using DataAccess;
using System.Web.UI.WebControls;
using EncryptionUtility;


namespace PollGenerator.Poll.Views
{
    public partial class NaujaApklausa : Microsoft.Practices.CompositeWeb.Web.UI.Page, INaujaApklausaView
    {
        private NaujaApklausaPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public NaujaApklausaPresenter Presenter
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

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // adding Database manager
                DbAccessManager manager = new DbAccessManager();

                // getting content place holder
                ContentPlaceHolder content = (ContentPlaceHolder) Master.FindControl("DefaultContent");

                // get information form TextBox
                string pollName = NameTextBox.Text;

                string pollDesc = DescriptionTextBox.Text;

                string pollCompleted = EndDescTextBox.Text;

                string userName = User.Identity.Name;

                // insert new Poll
                manager.InsertPoll(pollName, pollDesc, pollCompleted, userName);

                // get Poll ID
                int pollId = manager.GetPollID(pollName, pollDesc, pollCompleted, userName);

                Response.Redirect("~/Poll/ApklausosValdymas.aspx?ID=" + Server.UrlEncode(Encryption.Encrypt(pollId.ToString())));

                this.ClearFields();
            }

        }

        private void ClearFields()
        {
            NameTextBox.Text = "";

            DescriptionTextBox.Text = "";

            EndDescTextBox.Text = "";

        }

        protected void PollNameCustomValidation_ServerValidate(object source, ServerValidateEventArgs args)
        {
            DbAccessManager dbManager = new  DbAccessManager();

            // if Poll name for user exist
            if(dbManager.IsPollName(args.Value, User.Identity.Name))
            {
                args.IsValid = false;
            }
            else // if not...
            {
                args.IsValid = true;
            }
            
        }

    }
}

