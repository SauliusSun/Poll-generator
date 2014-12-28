using System;
using System.Web.UI.WebControls;
using EncryptionUtility;
using Microsoft.Practices.ObjectBuilder;

namespace PollGenerator.Shell.Views
{
    public partial class Atsakyk : Microsoft.Practices.CompositeWeb.Web.UI.Page, IAtsakykView
    {
        private AtsakykPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public AtsakykPresenter Presenter
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

        protected void PublicPollsGridView_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HyperLink Url = (HyperLink)e.Row.FindControl("NameHyperlink");

                string pollId = Url.ImageUrl;

                string encodedPollId = Encryption.Encrypt(pollId);

                Url.NavigateUrl = "~/Apklausa.aspx?ID=" + Server.UrlEncode(encodedPollId);

            }

        }

       

    }
}

