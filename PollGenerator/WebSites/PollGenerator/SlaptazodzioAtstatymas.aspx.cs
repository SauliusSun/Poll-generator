using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Net.Mail;
using System.Net;

namespace PollGenerator.Shell.Views
{
    public partial class SlaptazodzioAtstatymas : Microsoft.Practices.CompositeWeb.Web.UI.Page, ISlaptazodzioAtstatymasView
    {
        private SlaptazodzioAtstatymasPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public SlaptazodzioAtstatymasPresenter Presenter
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

        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

            AppSettingsSection conSection = config.AppSettings;

            //string systemName = conSection.Settings["MasterHeader"].Value;

            //if (systemName.Length > 0)
            //{
            //    systemName += " - ";
            //}

            e.Message.Subject = string.Format("Vartotojo duomenų priminimas Klausiu.lt apklausų sistemoje");
            e.Message.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;

            e.Message.Body = e.Message.Body.Replace("<%PasswordQuestion%>", PasswordRecovery1.Question);
            e.Message.Body = e.Message.Body.Replace("<%PasswordAnswer%>", PasswordRecovery1.Answer);

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.EnableSsl = true;

            smtpClient.Credentials = new NetworkCredential("isgalvotas@gmail.com", "a39nikotinas");

            //smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;

            e.Message.From = new MailAddress("info@klausiu.lt", "Klausiu.lt");

            e.Message.Sender = new MailAddress("info@klausiu.lt");

            e.Message.ReplyTo = new MailAddress("info@klausiu.lt");

            smtpClient.Send(e.Message);

            e.Cancel = true;
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //		ms-help://MS.VSCC.v80/MS.VSIPCC.v80/ms.practices.wcsf.2007oct/wcsf/html/08da6294-8a4e-46b2-8bbe-ec94c06f1c30.html

    }
}

