using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;
using DataAccess;
using StatisticsUtilities;
using System.Drawing;
using EncryptionUtility;


namespace PollGenerator.Shell.Views
{
    public partial class Apklausa : Microsoft.Practices.CompositeWeb.Web.UI.Page, IApklausaView
    {
        private ApklausaPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();
            }
            this._presenter.OnViewLoaded();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
            if (Request.QueryString["ID"] != null)
            {
                DbAccessManager manager = new DbAccessManager();

                // descrypt pollId
                string pollId = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                // check for mutiple answers
                string ResponderIpAddress = Request.UserHostAddress;

                this.CheckPollMultipleAnswers(ResponderIpAddress, int.Parse(pollId), manager); 

                // check if ResponseCount reached ResponseLimit
                this.CheckResponseLimit(int.Parse(pollId), manager);

                ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");
                
                content.Controls.Remove(CompleteButton);
                
                PollGeneratorUtility gen = new PollGeneratorUtility();

                gen.GeneratePoll(int.Parse(pollId), content);

                Page.Title = manager.GetPollName(int.Parse(pollId));

                content.Controls.Add(CompleteButton);

                CompleteButton.Visible = true;

                // required label to mark questions
                Label requireLabel = new Label();

                requireLabel.Text = "*";

                requireLabel.ForeColor = Color.Red;

                // print info text about required answers
                content.Controls.Add(new LiteralControl("<br/><br/>"));
                content.Controls.Add(requireLabel);
                Label InfoLabel = new Label();
                InfoLabel.Text = " - privalomi atsakyti klausimai.";
                content.Controls.Add(InfoLabel);


            }
        }

        protected void CompleteButton_Click(object sender, EventArgs e)
        {
            string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

            ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");

            DbAccessManager manager = new DbAccessManager();

            Statistics Stats = new Statistics();

            // do cheching if all required questions are answered
            if (Stats.IsRequiredAnswersMarked(int.Parse(pollID), content))
            {
                // add answers to the Db
                foreach (Control control in content.Controls)
                {
                    string answer = "";
                    string questionName = "";
                    int questionID;
                    string type = control.GetType().ToString();


                    switch (type)
                    {
                        case "System.Web.UI.WebControls.TextBox":

                            answer = ((TextBox)control).Text;

                            if (answer != "")
                            {

                                questionName = ((TextBox)control).ID;

                                questionID = manager.GetQuestionIDByName(int.Parse(pollID), questionName);

                                manager.AddStatistics(int.Parse(pollID), questionID, answer);
                            }

                            break;

                        case "System.Web.UI.WebControls.DropDownList":

                            answer = ((DropDownList)control).SelectedValue;

                            questionName = ((DropDownList)control).ID;


                            questionID = manager.GetQuestionIDByName(int.Parse(pollID), questionName);

                            manager.AddStatistics(int.Parse(pollID), questionID, answer);

                            break;

                        case "System.Web.UI.WebControls.CheckBoxList":
                            CheckBoxList List = (CheckBoxList)control;

                            foreach (ListItem item in List.Items)
                            {
                                if (item.Selected)
                                {
                                    answer = item.Text;

                                    questionName = List.ID;

                                    questionID = manager.GetQuestionIDByName(int.Parse(pollID), questionName);

                                    manager.AddStatistics(int.Parse(pollID), questionID, answer);

                                }


                            }

                            break;

                        case "System.Web.UI.WebControls.RadioButtonList":
                            RadioButtonList Radio = (RadioButtonList)control;

                            foreach (ListItem item in Radio.Items)
                            {
                                if (item.Selected)
                                {
                                    answer = item.Text;

                                    questionName = Radio.ID;

                                    questionID = manager.GetQuestionIDByName(int.Parse(pollID), questionName);

                                    manager.AddStatistics(int.Parse(pollID), questionID, answer);



                                }

                            }

                            break;
                    }

                }

                // add +1 to response count
                manager.AddResponse(int.Parse(pollID));

                this.PollEnd(int.Parse(pollID));

            }
            else
            {
                content.Controls.Add(new LiteralControl("<br/><br/>"));

                Label ErrorInfoLabel = new Label();

                ErrorInfoLabel.Text =
                    "Ne į visus privalomus klausimus atsakėte. Prašome peržiūrėti atsakymus dar kartą.";

                ErrorInfoLabel.ForeColor = Color.Red;

                content.Controls.Add(ErrorInfoLabel);
            }
        }

        public void PollEnd(int pollId)
        {
            DbAccessManager DbManager = new DbAccessManager();

            string hostIpAddress = Request.UserHostAddress;

            if (DbManager.IsIpAddressRegistered(hostIpAddress, pollId) == false)
            {
                DbManager.InsertResponderIP(hostIpAddress, pollId);
            }

            // check if url is set for poll
            string url = DbManager.GetPollUrl(pollId);

            if (url != null)
            {
                if (url != string.Empty)
                {
                    Response.Redirect(url);              
                }
            }
            
            // else redirect to completed poll page
            
            //string serverUrl = Request.ServerVariables["HTTP_HOST"];

            Response.Redirect("~/Pabaiga.aspx?Id=" +
                              Server.UrlEncode(Convert.ToString(Encryption.Encrypt(pollId.ToString()))));
       }
            
        

        public void CheckResponseLimit(int pollId, DbAccessManager manager)
        {
            int responseLimit = manager.GetResponseLimit(pollId);

            int responseCount = manager.GetResponseCount(pollId);

            if (responseLimit == 0)
            {
                return;
            }

            if (responseCount >= responseLimit)
            {              
                this.PollEnd(pollId);
            }
            
        }

        public void CheckPollMultipleAnswers(string hostIpAddress, int pollId, DbAccessManager manager)
        {
            if (manager.IsMultipleAnswerChecked(pollId) == false)
            {
                if (manager.IsIpAddressRegistered(hostIpAddress, pollId))
                {
                    this.PollEnd(pollId);
                }
                
            }
            
        }

        [CreateNew]
        public ApklausaPresenter Presenter
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

