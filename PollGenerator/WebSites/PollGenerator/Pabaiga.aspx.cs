using System;
using EncryptionUtility;
using Microsoft.Practices.ObjectBuilder;
using StatisticsUtilities;
using System.Web.UI.WebControls;
using DataAccess;

namespace PollGenerator.Shell.Views
{
    public partial class Pabaiga : Microsoft.Practices.CompositeWeb.Web.UI.Page, IPabaigaView
    {
        private PabaigaPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();

                if (Request.QueryString["Id"] != null)
                {
                    // descrypt poll Id
                    string pollId = Convert.ToString(Encryption.Decrypt(Request.QueryString["Id"]));

                    // database object
                    DbAccessManager DbManager = new DbAccessManager();

                    // content object
                    ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");

                    // register Ip address
                    string hostIpAddress = Request.UserHostAddress;

                    if (DbManager.IsIpAddressRegistered(hostIpAddress, int.Parse(pollId)) == false)
                    {
                        DbManager.InsertResponderIP(hostIpAddress, int.Parse(pollId));
                    }
               
                
                    // set poll name
                    Page.Title = DbManager.GetPollName(int.Parse(pollId));

                    // print poll completed text 
                    Label PollCompleted = new Label();

                    PollCompleted.Text = DbManager.GetPollCompletedText(int.Parse(pollId));

                    content.Controls.Add(PollCompleted);


                    // show poll results?
                    bool showResults = DbManager.ShowPollResults(int.Parse(pollId));
                    
                    if (showResults)
                    {
                           
                        // generate poll statistics                   
                        Statistics stat = new Statistics();

                        stat.Generate(int.Parse(pollId), content);              
                    }     

                }
            }
            this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public PabaigaPresenter Presenter
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

    }
}

