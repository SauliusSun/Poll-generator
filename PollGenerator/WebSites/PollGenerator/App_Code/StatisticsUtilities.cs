using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using DataAccess;
using am.Charts;
using AjaxControlToolkit;


namespace StatisticsUtilities
{
    /// <summary>
    /// Summary description for StatisticsUtilities
    /// </summary>
    public class Statistics
    {
        public Statistics()
        {   
        }

        /// <summary>
        /// function for loading statistics from Db
        /// </summary>
        /// <param name="pollID"></param>
        /// <param name="content"></param>
        public void Generate(int pollID, ContentPlaceHolder content)
        {
            DbAccessManager manager = new DbAccessManager();

            // if poll response limit reached
            this.CheckResponseLimit(pollID, manager, content);

            content.Controls.Add(new LiteralControl("<br/><br/>"));
            

            // get poll name
            string poll = manager.GetPollName(pollID);

            // print poll name
            content.Controls.Add(new LiteralControl("<center><h1>" + poll + "</h1></center>"));

            

            // getting question names from particular poll
            List<string> questionsNames = new List<string>();

            questionsNames = manager.GetQuestionsNames(pollID);

            // iterate through question names collection
            foreach (string q in questionsNames)
            {
                

                // secondly, detect question type 
                int questionType = manager.GetQuestionType(q, pollID);

                // question ID
                int questionID = manager.GetQuestionID(q, pollID);

                switch (questionType)
                {   // TextBox, do nothing for now
                    case 1:
                        //TextBox Box = new TextBox();

                        //Box.Width = 400;

                        //Box.ID = q;

                        //content.Controls.Add(Box);

                        //content.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;

                    case 2:
                        // Big TextBox, do nothing for now
                        //TextBox BigBox = new TextBox();

                        //BigBox.Width = 400;

                        //BigBox.Height = 150;

                        //BigBox.TextMode = TextBoxMode.MultiLine;

                        //BigBox.ID = q;

                        //content.Controls.Add(BigBox);

                        //content.Controls.Add(new LiteralControl("<br/><br/>"));
                        break;
                    case 3:
                        // firstly, print question name
                        //Label NameLabel = new Label();
                        //NameLabel.Text = q;
                        //content.Controls.Add(NameLabel);
                        //content.Controls.Add(new LiteralControl("<br/><br/>"));

                        //ColumnChart chart = new ColumnChart();

                        //chart.AreaFillAlpha = 40;

                        //chart.Bullet = ColumnChartBulletTypes.RoundOutline;

                        //chart.ColumnBorderColor = Color.Black;

                        //chart.ColumnBorderAlpha = 90;

                        //chart.ColumnGrowTime = 5;

                        //chart.Depth = 15;

                        //chart.PlotAreaBackgroundAlpha = 10;

                        //chart.PlotAreaBackgroundColor = Color.Yellow;
                        
                        //chart.LegendWidth = 400;

                        //PieChart pie = new PieChart();

                        //ChartLabel Label = new ChartLabel();

                        //Label.Text = q;

                        //Label.Align = LabelAlignments.Center;

                        //Label.TextSize = 14;
                        
                        //pie.Labels.Add(Label);

                        PieChart DropDownPieChart = this.GenerateChart(q);
                        
                        List<string> Attributes = new List<string>();

                        Attributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in Attributes)
                        {
                            
                            //Label AnswerLabel = new Label();

                            //AnswerLabel.Text = attribute;

                            //AnswerLabel.Font.Bold = true;

                            //content.Controls.Add(AnswerLabel);
                           

                            //content.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                            //// get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                            //Label CountLabel = new Label();

                            //CountLabel.Text = count.ToString();

                            //CountLabel.Font.Bold = true;

                            //content.Controls.Add(CountLabel);

                            //content.Controls.Add(new LiteralControl("<br/><br/>"));

                            //--------------Pie Chart

                            //PieChartDataItem item = new PieChartDataItem();

                            //item.Value = count.ToString();

                            //item.Title = attribute;

                            //pie.Items.Add(item);

                            this.AddValueToChart(attribute, count.ToString(), DropDownPieChart);

                            //------------- End of Pie Chart

                            //-----------------Column Chart

                            //ColumnChartSeriesDataItem item = new ColumnChartSeriesDataItem();

                            //item.SeriesItemID = count.ToString();
                            //item.Value = count.ToString();
                            
                            //ColumnChartGraph graph = new ColumnChartGraph();

                            //graph.DataSeriesItemIDField = count.ToString();

                            //graph.DataValueField = count.ToString();

                            //graph.GraphType = ColumnChartGraphTypes.Line;

                            //graph.Title = attribute;

                            //chart.Series.Add(item);

                            //chart.Graphs.Add(graph);
                           
                         
                            //collection.Add(item);

                            //-----------------End of Column Chart
                            
                        }

                        content.Controls.Add(new LiteralControl("<center>"));
                        content.Controls.Add(DropDownPieChart);
                        content.Controls.Add(new LiteralControl("</center>"));
                        
                        content.Controls.Add(new LiteralControl("<br/><br/>"));


                        break;
                    case 4:
                        // firstly, print question name
                        //Label NameLabel2 = new Label();
                        //NameLabel2.Text = q;
                        //content.Controls.Add(NameLabel2);
                        //content.Controls.Add(new LiteralControl("<br/><br/>"));

                        //PieChart pie2 = new PieChart();

                        //ChartLabel Label2 = new ChartLabel();

                        //Label2.Text = q;

                        //Label2.Align = LabelAlignments.Center;

                        //Label2.TextSize = 14;

                        //pie2.Labels.Add(Label2);

                        PieChart CheckPieChart = this.GenerateChart(q);
                        
                        List<string> CheckAttributes = new List<string>();

                        CheckAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in CheckAttributes)
                        {
                            //Label AnswerLabel = new Label();

                            //AnswerLabel.Text = attribute;

                            //AnswerLabel.Font.Bold = true;

                            //content.Controls.Add(AnswerLabel);

                            //content.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                            // get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                            //Label CountLabel = new Label();

                            //CountLabel.Text = count.ToString();

                            //CountLabel.Font.Bold = true;

                            //content.Controls.Add(CountLabel);

                            //content.Controls.Add(new LiteralControl("<br/><br/>"));

                            //PieChartDataItem item2 = new PieChartDataItem();

                            //item2.Value = count.ToString();

                            //item2.Title = attribute;

                            //pie2.Items.Add(item2);

                            this.AddValueToChart(attribute, count.ToString(), CheckPieChart);



                        
                        }

                        content.Controls.Add(new LiteralControl("<center>"));
                        content.Controls.Add(CheckPieChart);
                        content.Controls.Add(new LiteralControl("</center>"));

                        content.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;

                    case 5:
                        // firstly, print question name
                        //Label NameLabel3 = new Label();
                        //NameLabel3.Text = q;
                        //content.Controls.Add(NameLabel3);
                        //content.Controls.Add(new LiteralControl("<br/><br/>"));

                        PieChart RadioPieChart = this.GenerateChart(q);

                       

                        List<string> RadioAttributes = new List<string>();

                        RadioAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in RadioAttributes)
                        {
                            //Label AnswerLabel = new Label();

                            //AnswerLabel.Text = attribute;

                            //AnswerLabel.Font.Bold = true;

                            //content.Controls.Add(AnswerLabel);

                            //content.Controls.Add(new LiteralControl("&nbsp;&nbsp;"));

                            // get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                            //Label CountLabel = new Label();

                            //CountLabel.Text = count.ToString();

                            //CountLabel.Font.Bold = true;

                            //content.Controls.Add(CountLabel);

                            this.AddValueToChart(attribute, count.ToString(), RadioPieChart);

                            content.Controls.Add(new LiteralControl("<br/><br/>"));
                     
                        }

                        content.Controls.Add(new LiteralControl("<center>"));
                        content.Controls.Add(RadioPieChart);
                        content.Controls.Add(new LiteralControl("</center>"));


                        content.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;
                  

                }

            }

            
        }

        public void Generate(int pollId, TabPanel panel)
        {
            DbAccessManager manager = new DbAccessManager();

            // get poll name
            string poll = manager.GetPollName(pollId);

            // print poll name
            panel.Controls.Add(new LiteralControl("<center><h2>" + poll + "</h2></center>"));

            // getting question names from particular poll
            List<string> questionsNames = new List<string>();

            questionsNames = manager.GetQuestionsNames(pollId);

            // iterate through question names collection
            foreach (string q in questionsNames)
            {
                

                // secondly, detect question type 
                int questionType = manager.GetQuestionType(q, pollId);

                // question ID
                int questionID = manager.GetQuestionID(q, pollId);

                switch (questionType)
                {   
                    case 1:

                        // TextBox, do nothing for now
                     
                        break;

                    case 2:

                        // Big TextBox, do nothing for now
                       
                        break;

                    case 3:
                  
                        PieChart DropDownPieChart = this.GenerateChart(q);
                        
                        List<string> Attributes = new List<string>();

                        Attributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in Attributes)
                        {
                            
                         

                            //// get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                          
                            //--------------Pie Chart


                            this.AddValueToChart(attribute, count.ToString(), DropDownPieChart);

                            //------------- End of Pie Chart

                            //-----------------Column Chart

                         
                            //collection.Add(item);

                            //-----------------End of Column Chart
                            
                        }

                        panel.Controls.Add(new LiteralControl("<center>"));
                        panel.Controls.Add(DropDownPieChart);
                        panel.Controls.Add(new LiteralControl("</center>"));   
                        panel.Controls.Add(new LiteralControl("<br/><br/>"));


                        break;
                    case 4:
           
                        PieChart CheckPieChart = this.GenerateChart(q);
                        
                        List<string> CheckAttributes = new List<string>();

                        CheckAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in CheckAttributes)
                        {
                         
                            // get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                            this.AddValueToChart(attribute, count.ToString(), CheckPieChart);
                        
                        }

                        panel.Controls.Add(new LiteralControl("<center>"));
                        panel.Controls.Add(CheckPieChart);
                        panel.Controls.Add(new LiteralControl("</center>"));
                        panel.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;

                    case 5:
                      
                        PieChart RadioPieChart = this.GenerateChart(q);           

                        List<string> RadioAttributes = new List<string>();

                        RadioAttributes = manager.GetQuestionAnswerChoices(questionID);

                        foreach (string attribute in RadioAttributes)
                        {

                            // get answer count how many time particular answers were selected
                            int count = manager.GetAnswerChoseCount(questionID, attribute);

                            this.AddValueToChart(attribute, count.ToString(), RadioPieChart);

                            panel.Controls.Add(new LiteralControl("<br/><br/>"));
                     
                        }

                        panel.Controls.Add(new LiteralControl("<center>"));
                        panel.Controls.Add(RadioPieChart);
                        panel.Controls.Add(new LiteralControl("</center>"));
                        panel.Controls.Add(new LiteralControl("<br/><br/>"));

                        break;
                  

                }

            }

            
        


        }

        public void AddValueToChart(string answer, string value, PieChart chart)
        {
            PieChartDataItem item = new PieChartDataItem();

            item.Value = value;

            item.Title = answer;

            chart.Items.Add(item);
            
        }

        public PieChart GenerateChart(string questionName)
        {
            PieChart Pie = new PieChart();

            ChartLabel Label = new ChartLabel();

            Label.Text = questionName;

            Label.Align = LabelAlignments.Center;

            Label.TextSize = 18;

            Label.Top = 35;

            Pie.Labels.Add(Label);
            
            return Pie;
        }

        /// <summary>
        /// Returns true if all required answers in the poll are marked
        /// </summary>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public bool IsRequiredAnswersMarked(int pollID, ContentPlaceHolder content)
        {
            DbAccessManager manager = new DbAccessManager();

            foreach (Control control in content.Controls)
            {
                
                if (manager.IsAnswerRequired(pollID, control.ID))
                {
                    string type = control.GetType().ToString();

                    switch (type)
                    {
                        case "System.Web.UI.WebControls.TextBox":

                            string answer = ((TextBox)control).Text;

                            if (answer == string.Empty)
                            {
                                return false;
                            }

                            break;
                           
                            
                        case "System.Web.UI.WebControls.DropDownList":

                            answer = ((DropDownList)control).SelectedValue;

                            if (answer == string.Empty)
                            {
                                return false;
                            }

                            break;

                        case "System.Web.UI.WebControls.CheckBoxList":

                            CheckBoxList List = (CheckBoxList)control;

                            bool CheckBoxListChecked = false; 

                            foreach (ListItem item in List.Items)
                            {
                                if (item.Selected)
                                {
                                    CheckBoxListChecked = true;
                        
                                }
                              
                            }

                            if (CheckBoxListChecked == true)
                            {
                                break;
                            }

                            return false;
                                                                              
                        case "System.Web.UI.WebControls.RadioButtonList":

                            RadioButtonList Radio = (RadioButtonList)control;

                            bool RadioButtonListChecked = false;

                            foreach (ListItem item in Radio.Items)
                            {
                                if (item.Selected)
                                {
                                    RadioButtonListChecked = true;
                                }

                            }

                            if (RadioButtonListChecked == true)
                            {
                                break;
                            }

                            return false;
                    }



                }
            }

            return true;
        }

        public void CheckResponseLimit(int pollId, DbAccessManager manager, ContentPlaceHolder content)
        {
            int responseLimit = manager.GetResponseLimit(pollId);

            int responseCount = manager.GetResponseCount(pollId);

            if (responseLimit == 0)
            {
                return;
            }

            if (responseCount >= responseLimit)
            {
                content.Controls.Add(new LiteralControl("<br/>"));
                content.Controls.Add(new LiteralControl("<b>PASTABA: Apklausiamų respodentų skaičius pasiekė limitą.</b>"));
                
            }

        }

      
    }
}