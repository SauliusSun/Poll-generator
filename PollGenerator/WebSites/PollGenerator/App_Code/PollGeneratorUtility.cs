using System.Collections.Generic;
using DataAccess;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace PollGenerator
{

    /// <summary>
    /// Utility for poll serialization to Database and on demand loading to web page
    /// </summary>
    public class PollGeneratorUtility 
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PollGeneratorUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }

       

        /// <summary>
        /// Algorithm  for loading poll from Database
        /// </summary>
        public void GeneratePoll(int pollId, ContentPlaceHolder content )
        {
            // data access layer
            DbAccessManager manager = new DbAccessManager();

            // poll name by pollID
            string pollName = manager.GetPollName(pollId);
            
            // print poll name
            //content.Controls.Add(new LiteralControl("<h1>" + pollName + "</h1>"));
            
            // variable for storing question names 
            List<string> questionsNames = new List<string>();

            // attaching question names
            questionsNames = manager.GetQuestionsNamesBySequenceNumber(pollId);
            
            foreach (string q in questionsNames)
            {
                // firstly, print question name
                Label NameLabel = new Label();
                NameLabel.Text = q;
                content.Controls.Add(NameLabel);

                // if question answer is required
                if (manager.IsAnswerRequired(pollId, q))
                {
                    this.PrintRequiredMark(content);
                    
                }

                content.Controls.Add(new LiteralControl("<br/><br/>"));

                

                // secondly, detect question type and print neccessary control

                int questionType = manager.GetQuestionType(q, pollId);
                
                int questionID = manager.GetQuestionID(q, pollId);

                switch (questionType)
                {
                    case 1: // standard text box for short answer
                        TextBox Box = new TextBox();

                        Box.Width = 400;

                        Box.ID = q;

                        content.Controls.Add(Box);

                        content.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;

                    case 2: // big text box for long answers
                        TextBox BigBox = new TextBox();

                        BigBox.Width = 400;

                        BigBox.Height = 150;

                        BigBox.TextMode = TextBoxMode.MultiLine;

                        BigBox.ID = q;

                        content.Controls.Add(BigBox);

                        content.Controls.Add(new LiteralControl("<br/><br/>"));
                        break;
                    case 3: // drop down list
                        DropDownList List = new DropDownList();

                        List.ID = q;

                        List.Width = 150;

                        List<string> Attributes = new List<string>();

                        Attributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in Attributes)
                        {
                            ListItem item = new ListItem();
                            item.Text = attribute;
                            
                            List.Items.Add(item);
                            
                        }

                        content.Controls.Add(List);
                        

                        content.Controls.Add(new LiteralControl("<br/><br/>"));
                        
                        
                        break;
                    case 4: // check box list
                        
                        CheckBoxList Check = new CheckBoxList();

                        Check.ID = q;

                        Check.RepeatDirection = RepeatDirection.Horizontal;

                        Check.TextAlign = TextAlign.Right;

                        Check.RepeatLayout = RepeatLayout.Flow;

                        List<string> CheckAttributes = new List<string>();

                        CheckAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in CheckAttributes)
                        {
                            ListItem item = new ListItem();

                            item.Text = attribute;

                            Check.Items.Add(item);
                            
                        }

                        content.Controls.Add(Check);

                        content.Controls.Add(new LiteralControl("<br/><br/>"));
                        
                        break;

                    case 5: // radio button list

                        RadioButtonList Radio = new RadioButtonList();

                        Radio.ID = q;

                        Radio.RepeatDirection = RepeatDirection.Horizontal;

                        Radio.TextAlign = TextAlign.Right;

                        Radio.RepeatLayout = RepeatLayout.Flow;

                        List<string> RadioAttributes = new List<string>();

                        RadioAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in RadioAttributes)
                        {
                            ListItem item = new ListItem();

                            item.Text = attribute;

                            Radio.Items.Add(item);

                        }

                        content.Controls.Add(Radio);

                       
                        content.Controls.Add(new LiteralControl("<br/><br/>"));
                        
                        break;
                    case 6: // info text no need for any control
                        
                   
                        break;
                        
                }
                
            }

        

            



          

            // The end, generate End button
            //Button CompleteButton = new Button();
            //CompleteButton.ID = "CompleteButton";
            //CompleteButton.Text = "Baigti";
            ////CompleteButton.Click += new EventHandler(CompleteButton_Click);
            //CompleteButton.Command += new System.Web.UI.WebControls.CommandEventHandler(CompleteButton_Click);
 
            
            
            //content.Controls.Add(CompleteButton);
            
        }

        private void PrintRequiredMark(ContentPlaceHolder content)
        {
            // required label to mark questions
            Label requireLabel = new Label();

            requireLabel.Text = "*";

            requireLabel.ForeColor = Color.Red;

            content.Controls.Add(requireLabel);
            
        }



        

       
    }
}