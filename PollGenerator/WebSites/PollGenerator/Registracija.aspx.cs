using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace PollGenerator.Shell.Views
{
    public partial class NaujasVartotojas : Microsoft.Practices.CompositeWeb.Web.UI.Page, INaujasVartotojasView
    {
        private NaujasVartotojasPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public NaujasVartotojasPresenter Presenter
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

        protected void CreateUserWizard1_SendingMail(object sender, MailMessageEventArgs e)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

            AppSettingsSection conSection = config.AppSettings;

            

            //string systemName = conSection.Settings["MasterHeader"].Value;

            //if (systemName.Length > 0)
            //{
            //    systemName += " - ";
            //}

            //e.Message.Subject = string.Format("{0}Jūsų vartotojo duomenys", systemName);

            e.Message.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;

            e.Message.Body = e.Message.Body.Replace("<%PasswordQuestion%>", CreateUserWizard1.Question);
            e.Message.Body = e.Message.Body.Replace("<%PasswordAnswer%>", CreateUserWizard1.Answer);

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

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(@"~/Poll/ManoApklausos.aspx");
        }


        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
            //MembershipUser Vartotojas = sender(MembershipUser);
            Roles.AddUserToRole(CreateUserWizard1.UserName, "Registruotas");
        }
}
}

