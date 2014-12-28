using System;
using System.Web;
using Microsoft.Practices.ObjectBuilder;
using DataAccess;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Web.UI;
using StatisticsUtilities;
using EncryptionUtility;
using StatisticsUtilities.Results;

namespace PollGenerator.Poll.Views
{
    public partial class ApklausosValdymas : Microsoft.Practices.CompositeWeb.Web.UI.Page, IApklausosValdymasView
    {
        private ApklausosValdymasPresenter _presenter;

        protected int removeIndex = 20;

        //----------ReOrder
        protected List<Question> ListDataItems;

        protected int NewListOrderNumber;

        //-------------

        // save control Id
        private List<string> AttributesIDCollection = new List<string>();

        // save control value
        private List<string> AttributesValueCollection = new List<string>();

        // saves controls count
        public int ControlsCount
        {
            get
            {
                if (ViewState["Count"] == null)
                {
                    return 0;
                }
                return (int)ViewState["Count"];
            }
            set
            {
                ViewState["Count"] = value;
            }
        }

        // Page load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this._presenter.OnViewInitialized();              
                
                // Apklausos nustatymai

                if (Request.QueryString["ID"] != null)
                {                    
                    string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                    int a = QuestionReorderList.Items.Count;

                    //QuestionReorderList.Items.Clear();

                    int b = QuestionReorderList.Items.Count;

                    // set PollId cookie
                    //Response.Cookies.Remove("PollIdCookie");

                    //Response.Cookies.Add(new HttpCookie("PollIdCookie", pollID));

                    
                    //QuestionReorderList.DataBind();

                    int c  = QuestionReorderList.Items.Count;


                    DbAccessManager manager = new DbAccessManager();

                    NameTextBox.Text = manager.GetPollName(int.Parse(pollID));

                    DescriptionTextBox.Text = manager.GetPollDescription(int.Parse(pollID));

                    EndDescTextBox.Text = manager.GetPollCompletedText(int.Parse(pollID));

                    PollLimitTextBox.Text = manager.GetPollLimit(int.Parse(pollID));

                    // get Poll url
                    this.RedirectUrlTextBox.Text = manager.GetPollUrl(int.Parse(pollID));

                    // show poll results?
                    bool showPollResults = manager.ShowPollResults(int.Parse(pollID));

                    if (showPollResults)
                    {
                        ShowResultsDropDownList.SelectedIndex = 0;
                    }
                    else
                    {
                        ShowResultsDropDownList.SelectedIndex = 1;
                    }

                    bool multipleAnswers = manager.IsMultipleAnswerChecked(int.Parse(pollID));

                    if (multipleAnswers)
                    {
                        MultipleAnswersDropDownList.SelectedIndex = 0;
                    }
                    else
                    {
                        MultipleAnswersDropDownList.SelectedIndex = 1;
                    }

                    bool publicPoll = manager.IsPollPublic(int.Parse(pollID));

                    if (publicPoll)
                    {
                        PublicPollDropDownList.SelectedIndex = 0;
                    }
                    else
                    {
                        PublicPollDropDownList.SelectedIndex = 1;
                    }

                    // encrypt pollId
                    string encryptedPollId = Encryption.Encrypt(pollID);

                    LinkLabel.Text = "http://klausiu.lt/Apklausa.aspx?ID=" + Server.UrlEncode(encryptedPollId);

                    LinkLabel.Font.Bold = true;

                    BBLabel.Text = string.Format("[url={0}]{1}[/url]", LinkLabel.Text, NameTextBox.Text);

                    BBLabel.Font.Bold = true;

                    HtmlLabel.Text = Server.HtmlEncode("<a href=" + LinkLabel.Text + ">" + NameTextBox.Text + "</a>");

                    HtmlLabel.Font.Bold = true;

                    // Generuojame apklausos statistiką                  
                    Statistics Stats = new Statistics();

                    Stats.Generate(int.Parse(pollID), Panel4);

                    // Enable results if is any answers
                    if (manager.HasPollAnswers(int.Parse(pollID)) == false || manager.HasPollQuestions(int.Parse(pollID)) == false)
                    {
                        ResultsButton.Enabled = false;       
                    }
                    else 
                    {
                        ResultsButton.Enabled = true;
                    }
                    
                }
                
            }
            this._presenter.OnViewLoaded();

            // re-create controls
            this.RestoreControls();
        }

        [CreateNew]
        public ApklausosValdymasPresenter Presenter
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

        protected void TabContainer_Load(object sender, EventArgs e)
        {
   
        }

        // save poll preferences
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                string name = NameTextBox.Text;

                string description = DescriptionTextBox.Text;

                string endDescription = EndDescTextBox.Text;

                string limit = PollLimitTextBox.Text;

                string url = RedirectUrlTextBox.Text;

                bool showResults;

                if (ShowResultsDropDownList.SelectedIndex == 0)
                {
                    showResults = true;
                }
                else
                {
                    showResults = false;
                }

                bool multipleAnswers;

                if (MultipleAnswersDropDownList.SelectedIndex == 0)
                {
                    multipleAnswers = true;
                }
                else
                {
                    multipleAnswers = false;
                }

                bool publicPoll;

                if (PublicPollDropDownList.SelectedIndex == 0)
                {
                    publicPoll = true;
                }
                else
                {
                    publicPoll = false;
                }
                

                DbAccessManager manager = new DbAccessManager();

                manager.UpdatePollPreferences(int.Parse(pollID), name, description, endDescription, int.Parse(limit), url, showResults, multipleAnswers, publicPoll);
            }
        }

        // re-order question list (after dragging)
        protected void QuestionReorderList_ItemReorder(object sender, AjaxControlToolkit.ReorderListItemReorderEventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                var NewOrder = e.NewIndex + 1;
                var OldOrder = e.OldIndex + 1;

                //var ReorderListItemID = Convert.ToInt32(((Label) e.Item.FindControl("Id")).Text);
                var ReorderListItemID = int.Parse(pollID);

                var db = new PollDataContext();

                var ListItemCount = 1;

                var ListData = from q in db.Questions
                               orderby q.Sequence
                               select q;

                foreach (var ListDataItem in ListData)
                {
                    // Move forward items in this range
                    if (OldOrder > NewOrder
                        && ListItemCount >= NewOrder
                        && ListItemCount <= OldOrder
                        )

                        ListDataItem.Sequence = ListItemCount + 1;
                        // Move backward items in this range
                    else if
                        (OldOrder < NewOrder
                         && ListItemCount <= NewOrder
                         && ListItemCount >= OldOrder
                        )
                        ListDataItem.Sequence = ListItemCount - 1;

                    ListItemCount++;

                    // Set the changed item into the newly numerical gap
                    if (ListDataItem.Id == ReorderListItemID)
                        ListDataItem.Sequence = NewOrder;
                }

                db.SubmitChanges();

                //GetListData(); //Get the list with the newly inserted item
                //BindData();
                QuestionReorderList.Controls.Clear();
                QuestionsLinqDataSource.DataBind();

            }
        }

        protected void QuestionReorderList_Init(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                //Request.Cookies.Remove("PollIdCookie");

                //Response.Cookies.Remove("PollIdCookie");

                //Response.Cookies.Add(new HttpCookie("PollIdCookie", pollID));

                //Request.Cookies.Add(new HttpCookie("PollIdCookie", pollID));
                
                Session.Add("PollIdSession", pollID);

                //QuestionReorderList.Items.Clear();
            }


        }

        // get question list
        protected void GetListData()
        {
            var db = new PollDataContext();

            ListDataItems = (from q in db.Questions
                             orderby q.Sequence
                             select q).ToList();

            NewListOrderNumber = ListDataItems.Count() + 1;
        }


        /// <summary>
        /// Restore dynamically created controls after every postback 
        /// </summary>
        public void RestoreControls()
        {
            AttributesIDCollection = (List<string>) ViewState["Attributes"];

            //AttributesValueCollection = (List<string>) ViewState["Value"];

            int valueIndex = 0;

            if (AttributesIDCollection != null)
            {
                foreach (string Id in AttributesIDCollection)
                {
                    TextBox AnswerTextBox = new TextBox();

                    AnswerTextBox.ID = Id;

                    AnswerTextBox.Width = 150;

                    //AnswerTextBox.Text = AttributesValueCollection[valueIndex];
                    
                    NewQPanel.Controls.AddAt(5, AnswerTextBox);

                    valueIndex++;
                }
            }
        }

        /// <summary>
        /// Deep search for dynamically created control
        /// </summary>
        /// <param name="Root"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Control FindControlRecursive(Control Root, string Id)
        {

            if (Root.ID == Id)

                return Root;

            foreach (Control Ctl in Root.Controls)
            {

                Control FoundCtl = FindControlRecursive(Ctl, Id);

                if (FoundCtl != null)

                    return FoundCtl;

            }

            return null;

        }

        // "Pridėti atsakymą" button click
        protected void ValueButton_Click(object sender, EventArgs e)
        {
            // get controls Id
            AttributesIDCollection = (List<string>)ViewState["Attributes"];        

            List<string> ValueList = new List<string>();

            ValueList.Add("");

            //// remove dynamically created controls
            if (AttributesIDCollection != null)
            {
                ValueList.RemoveAt(0);

                foreach (string Id in AttributesIDCollection)
                {
                    TextBox AnswerTextBox = this.FindControlRecursive(this.Master, Id) as TextBox;

                    if (AnswerTextBox != null)
                    {
                        ValueList.Add(AnswerTextBox.Text);
                    }     

                    NewQPanel.Controls.Remove(AnswerTextBox);

                }

            }

            //// restore ViewState
            ViewState["Attributes"] = "";

            ViewState["Value"] = "";

            ControlsCount += 1;

            List<string> IdList = new List<string>();

            ValueList.Add("");

            for (int i = 0; i < ControlsCount; i++)
            {                   
                NewQPanel.Controls.AddAt(6, new LiteralControl("<br/><br/><b>" + (i + 1).ToString() + ". </b>"));

                TextBox Box = new TextBox();

                Box.Width = 150;

                Box.ID = "control" + i.ToString();
                
                Box.Text = ValueList[i];
            
                NewQPanel.Controls.AddAt(7, Box);

                IdList.Add(Box.ID);

                ValueList.Add(Box.Text);
             
            }

            ViewState["Attributes"] = IdList;

            ViewState["Value"] = ValueList;

            // can add another text box
            ValueButton.Visible = true;

            // delete Panel space
            removeIndex++;
        }

        private void DeleteQuestionAnswers()
        {
            if (AttributesIDCollection != null)
            {
                foreach (string Id in AttributesIDCollection)
                {
                    TextBox box = this.FindControlRecursive(this.Master, Id) as TextBox;

                    NewQPanel.Controls.Remove(box);
                }
            }

            // clear ViewState
            ControlsCount = 0;

            ViewState["Attributes"] = null;

            ViewState["Value"] = null;
            
        }


        private void ClearFields()
        {
            this.RequiredDropDownList.Enabled = true;

            QuestionTextBox.Text = "";

            QuestionTypeDropDownList.SelectedIndex = 0;

            RequiredDropDownList.SelectedIndex = 0;

            AttributesIDCollection = (List<string>)ViewState["Attributes"];

            // remove dynamically created controls
            if (AttributesIDCollection != null)
            {
                foreach (string Id in AttributesIDCollection)
                {
                    TextBox box = this.FindControlRecursive(this.Master, Id) as TextBox;

                    NewQPanel.Controls.Remove(box);
                }
            }

            // clear ViewState
            ControlsCount = 0;

            ViewState["Attributes"] = null;

            ViewState["Value"] = null;
           
        }

        protected void AddQuestionButton_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string pollID = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                DbAccessManager manager = new DbAccessManager();

                int max = manager.GetMaxSequenceNumber(int.Parse(pollID));

                max += 1;

                // add question to the database
                manager.AddQuestion(int.Parse(pollID), QuestionTextBox.Text,
                                    int.Parse(QuestionTypeDropDownList.SelectedItem.Value),
                                    RequiredDropDownList.SelectedItem.Value, max);

                // get dynamically created controls IDs
                AttributesIDCollection = (List<string>) ViewState["Attributes"];

                if (AttributesIDCollection != null)
                {

                    foreach (string Id in AttributesIDCollection)
                    {
                        ContentPlaceHolder content = (ContentPlaceHolder)Master.FindControl("DefaultContent");
                       
                        TextBox Box = this.FindControlRecursive(this.Master, Id) as TextBox;
                      
                        manager.AddQuestionAnswer(Box.Text, QuestionTextBox.Text, int.Parse(pollID));
                    }
                }

                // restore Page settings
                this.ClearFields();

                // update question list with newly created question
                this.QuestionReorderList.Controls.Clear();
                this.QuestionReorderList.DataBind();
            }

        }

        // select question type (enable /disable multiple questions)
        protected void QuestionTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int questionType = int.Parse(QuestionTypeDropDownList.SelectedItem.Value);

            switch (questionType)
            {
                case 3:
                case 4:
                case 5:

                    this.RequiredDropDownList.Enabled = true;

                    ValueButton.Visible = true;

                    this.DeleteQuestionAnswers();

                    break;

                case 6:

                    this.RequiredDropDownList.Enabled = false;

                    break;

                default:

                    this.RequiredDropDownList.Enabled = true;

                    ValueButton.Visible = false;

                    this.DeleteQuestionAnswers();

                    break;

            }
           
        }

        // delete question from reorder list 
        protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton button = (ImageButton)sender;

            string questionId = button.CommandArgument;

            DbAccessManager manager = new DbAccessManager();

            manager.DeleteQuestion(int.Parse(questionId));

            //QDataSource.Update();

            if (Request.QueryString["ID"] != null)
            {
                string pollId = Encryption.Decrypt(Request.QueryString["ID"]);

                manager.ReorderQuestionsSequence(int.Parse(pollId));
            }
           
            QuestionReorderList.Controls.Clear();

            QuestionReorderList.DataBind();

            if (QuestionReorderList.Items.Count == 0)
            {
                ResultsButton.Enabled = false;
            }
        }

        // download file in Excel (.xls) format
        protected void ResultsButton_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string pollId = Convert.ToString(Encryption.Decrypt(Request.QueryString["ID"]));

                // poll results object
                StatisticsResults Results = new StatisticsResults();

                // save results in server and let user download
                Results.Export(pollId, Server, Response);
            }


        }   
        
}
}

