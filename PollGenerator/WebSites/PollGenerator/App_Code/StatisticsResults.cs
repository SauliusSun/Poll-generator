using System.Collections.Generic;
using System.Web;
using CarlosAg.ExcelXmlWriter;
using DataAccess;


namespace StatisticsUtilities.Results
{

    /// <summary>
    /// Poll statistics detail results for download
    /// </summary>
    public class StatisticsResults
    {
        private Workbook ResultsBook;

        public StatisticsResults()
        {
            // creating Excel workbook object
            ResultsBook = new Workbook();
        }

        private Worksheet FillWorksheet(string pollId)
        {
            Worksheet ResultsSheet = ResultsBook.Worksheets.Add("Rezultatai");

            // creating access to Database
            DbAccessManager DbManager = new DbAccessManager();

            List<string> pollQuestionNames = new List<string>();

            pollQuestionNames = DbManager.GetQuestionsNamesBySequenceNumber(int.Parse(pollId));

            // indexes
            int questionAnswerIndex = -2;

            int questionAnswerCountIndex = -1;

            // create columns 
            this.CreateColumns(pollQuestionNames.Count, ResultsSheet);

            

            // n - question name
            foreach (string n in pollQuestionNames)
            {
                // +1 index
                //questionAnswerIndex += 2;
                //questionAnswerCountIndex += 2;

                int questionType = DbManager.GetQuestionType(n, int.Parse(pollId));

                // skip information question
                if (questionType == 6)
                    break;

                int questionId = DbManager.GetQuestionID(n, int.Parse(pollId));

                // Question Label row
                WorksheetRow QuestionLabelsRow = new WorksheetRow();

                ResultsSheet.Table.Rows.Add(QuestionLabelsRow);

               // question label cell
                WorksheetCell questionLabelCell = new WorksheetCell();

                questionLabelCell.Data.Text = n;

                //questionLabelCell.Index = questionAnswerIndex;

                QuestionLabelsRow.Cells.Add(questionLabelCell);

                // question answer count label cell
                WorksheetCell QuestionAnswerCountLabel = new WorksheetCell();

                QuestionAnswerCountLabel.Data.Text = "Atsakymas pasirinktas (įrašytas) kartų";

                //QuestionAnswerCountLabel.Index = questionAnswerCountIndex;

                QuestionLabelsRow.Cells.Add(QuestionAnswerCountLabel);   

                switch (questionType)
                {
                    case 1: // single line text box    
         
                        AddQuestionAnswers(questionId, ResultsSheet);
                    
                        break;
                    case 2: // multiple line text box
                        AddQuestionAnswers(questionId, ResultsSheet);
                        break;
                    case 3: // dropdown list
                        AddQuestionAnswers(questionId, ResultsSheet);
                        break;
                    case 4: // check box list
                        AddQuestionAnswers(questionId, ResultsSheet);
                        break;
                    case 5: // radio button list
                        AddQuestionAnswers(questionId, ResultsSheet);
                        break;
                    // 
                    //case 6: // information for user label text
                    //    break;

                }
                
            }

            return ResultsSheet;
            
        }


        public void Export(string pollId, HttpServerUtility Server, HttpResponse Response)
        {

            FillWorksheet(pollId);

            ResultsBook.Save(Server.MapPath("~/Uploads/rezultatai_" + pollId + ".xls"));

            Response.Redirect("~/Uploads/rezultatai_" + pollId + ".xls");
            
        }

        private void AddQuestionAnswers(int questionId, Worksheet ResultsSheet)
        {
            DbAccessManager DbManager = new DbAccessManager();

            List<string> answers = DbManager.GetQuestionAnswers(questionId);

            //int index = -1;

            foreach (string a in answers)
            {
                //index += 1;
                WorksheetRow row = new WorksheetRow();

                //row.Index = index;

                // answer

                WorksheetCell cell = new WorksheetCell();

                cell.Data.Text = a;

                row.Cells.Add(cell);
                
                

                // answer count 

                // how many times answer have beenn chosen
                int answerChose = DbManager.QuestionAnswerChoseCount(questionId, a); 
  
                WorksheetCell cell2 = new WorksheetCell();

                cell2.Data.Text = answerChose.ToString();

                cell2.Data.Type = DataType.Number;

                row.Cells.Add(cell2);

                //ResultsSheet.Table.Columns[3].Table.Rows.Add(row);
                ResultsSheet.Table.Rows.Add(row);
                
            }


            
        }

        private void CreateColumns(int questionCount, Worksheet ResultsSheet)
        {
            //int columnsCount = questionCount*2;

            //for (int i = 0; i < columnsCount; i++)
            //{
                ResultsSheet.Table.Columns.Add(new WorksheetColumn());
            ResultsSheet.Table.Columns.Add(new WorksheetColumn());
            //}
        }
    




    }

}