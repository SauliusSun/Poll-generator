using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Data;
using System.Reflection;

namespace DataAccess
{

    /// <summary>
    /// Summary description for DbAccessManager
    /// </summary>
    public class DbAccessManager
    {
        //private DataContext db;
        private PollDataContext db;

        public DbAccessManager()
        {
            db = (PollDataContext) new DataContext(@"Server=(local);Database=PollsDb;Integrated Security=TRUE;");
            //db = new PollDataContext();

            //Table<Poll> table = db.GetTable<Poll>();

        }

        public  void InsertPoll(string name, string description, string endDesc, string userName)
        {
            Poll poll = new Poll();

            poll.Name = name;
            poll.Description = description;
            poll.EndDesc = endDesc;
            poll.ResponseLimit = 0;
            poll.ResponseCount = 0;
            poll.Created = DateTime.Now;
            poll.UserName = userName;
            poll.Url = "";
            poll.ShowResults = true;
            poll.MultipleAnswers = false;
            poll.PublicPoll = false;

            db.Polls.InsertOnSubmit(poll);

            db.SubmitChanges();

            //Table<Poll> polls = db.GetTable<Poll>();

            //PollClassesDataContext 


        }

        /// <summary>
        /// Get all Polls by user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataTable GetPollsByUserName(string userName)
        {
            DataTable PollsTable = new DataTable();

            var p = (from poll in db.Polls
                     where poll.UserName == userName
                     select new
                                {
                                    poll.Name,
                                    poll.Description,
                                    poll.Created

                                }
                    ).ToList();

            PollsTable = ListToDataTable(p);

            return PollsTable;          
            
            
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            DataTable dt = new DataTable();

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

       /// <summary>
       /// Adds question to Database
       /// </summary>
       /// <param name="pollId"></param>
       /// <param name="questionName"></param>
       /// <param name="questionType"></param>
        public void AddQuestion(int pollId, string questionName, int questionType, string required, int max)
        {
            Question question = new Question();

            question.Name = questionName;

            question.Type = questionType;

           question.Sequence = max;

            question.Required = required;

            question.PollId = pollId;

            db.Questions.InsertOnSubmit(question);

            db.SubmitChanges();
            
        }

        public int GetQuestionID(string questionName, int pollID)
        {
            Question q = (from question in db.Questions
                          where question.Name == questionName && question.PollId == pollID
                          select question).Single();

            return q.Id;
            
        }

        public void DeleteQuestion(int questionId)
        {
            //List <ChoiceField> fields = new List<ChoiceField>();
            //fields = (from answers in db.ChoiceFields where answers.QuestionId == questionId select answers); 

            Question q = (from questions in db.Questions where questions.Id == questionId select questions).SingleOrDefault();

            if (q != null)
            {
                //db.ChoiceFields.De
                db.Questions.DeleteOnSubmit(q);

                db.SubmitChanges();
            }
        }

        public void AddQuestionAnswer(string name, string questionName, int pollID)
        {
            int questionID = this.GetQuestionID(questionName, pollID);

            Answer answer = new Answer();

            answer.Name = name;

            answer.QuestionId = questionID;

            db.Answers.InsertOnSubmit(answer);

            db.SubmitChanges();
            
        }

        /// <summary>
        /// Load questions from db
        /// </summary>
        /// <param name="pollId"></param>
        public List<string> GetQuestionsNames(int pollId)
        {
            List<string> questions = new List<string>();

            questions = (from question in db.Questions where question.PollId == pollId select question.Name).ToList();

            return questions;
            
        }

        // get questions and order by sequence
        public List<string> GetQuestionsNamesBySequenceNumber(int pollId)
        {
            List<string> questions = new List<string>();

            questions = (from question in db.Questions where question.PollId == pollId orderby question.Sequence select question.Name ).ToList();

            return questions;

        }

        public void ReorderQuestionsSequence(int pollId)
        {
            //List<string> questions = new List<string>();

            var questions = (from question in db.Questions where question.PollId == pollId orderby question.Sequence select question).ToList();

            int sequance = 1;

            foreach (Question q in questions)
            {
                q.Sequence = sequance;

                db.SubmitChanges();

                sequance++;
            }  
        
            

        }

        public string GetPollName(int pollID)
        {
            var name = (from poll in db.Polls where poll.Id == pollID select poll.Name).Single();

            return name.ToString();
        }

        // poll description
        public string GetPollDescription(int pollID)
        {
            string description = (from poll in db.Polls where poll.Id == pollID select poll.Description).SingleOrDefault();

            return description;

        }

        // how many times can be poll completed
        public string GetPollLimit(int pollID)
        {
            var limit = (from p in db.Polls where p.Id == pollID select p.ResponseLimit).SingleOrDefault();

            return limit.ToString();
        }

        // when poll is complete the url for redirection
        public string GetPollUrl(int pollID)
        {
            string url = (from p in db.Polls where p.Id == pollID select p.Url).SingleOrDefault();

            return url;
        }
      

        public int GetQuestionType(string questionName, int pollId)
        {


            //var types = from question in db.Questions
            //           where question.PollId == pollId && question.Name == questionName
            //           select question.Type;

            Question q = (from question in db.Questions
                          where question.PollId == pollId && question.Name == questionName
                          select question).Single();

            return q.Type;

        }

        public List<string> GetQuestionAnswerChoices(int questionID)
        {
            List<string> AnswerChoices = new List<string>();

            AnswerChoices =
                (from answer in db.Answers where answer.QuestionId == questionID select answer.Name).ToList();

            return AnswerChoices;
        }


        /// <summary>
        /// Add statistics
        /// </summary>
        /// <param name="pollID"></param>
        /// <param name="questionID"></param>
        /// <param name="answer"></param>
        public void AddStatistics(int pollID, int questionID, string answer)
        {


            var stats = (from stat in db.Statistics
                         where
                             stat.PollID == pollID && stat.QuestionID == questionID &&
                             stat.AnswerName == answer
                         select stat).SingleOrDefault();

            Statistic statistic = (Statistic) stats;


            if (statistic == null)
            {
                Statistic statis = new Statistic();

                statis.PollID = pollID;

                statis.QuestionID = questionID;

                statis.AnswerName = answer;
                
                statis.AnswerChose += 1;

                db.Statistics.InsertOnSubmit(statis);

                db.SubmitChanges();
                
            }

            else
            {
                statistic.AnswerChose += 1;

                db.SubmitChanges();
                
            }

          
            
        }

        public int GetQuestionIDByName(int pollID, string questionName)
        {
            int questionID = (from question in db.Questions
                                where question.Name == questionName && question.PollId == pollID
                             select question.Id).Single();

            return questionID;
        }

        public int GetAnswerChoseCount(int questionID, string answer)
        {
            var c = (from stat in db.Statistics
                     where stat.QuestionID == questionID && stat.AnswerName == answer
                     select stat.AnswerChose).SingleOrDefault();

            int count = (int)c;

            return count;
        }

        
        public string GetPollCompletedText(int pollID)
        {
            string text = (from poll in db.Polls where poll.Id == pollID select poll.EndDesc).SingleOrDefault();

            return text;
        }

        // 
        public int GetPollID(string pollName, string pollDescription, string pollCompletedText, string userName)
        {
            int pollID = (from poll in db.Polls
                          where
                              poll.Name == pollName && poll.Description == pollDescription && userName == poll.UserName && 
                              poll.EndDesc == pollCompletedText
                          select poll.Id).SingleOrDefault();

            return pollID;
            
        }

        // User must answer - T, else - F
        public bool IsAnswerRequired(int pollId, string questionName)
        {
            string required =
                (from q in db.Questions where q.PollId == pollId && q.Name == questionName select q.Required).
                    SingleOrDefault();

            if (required == "T")
            {
                return true;
            }

            return false;
      
        }

        // iterate through all questions and get max sequence number
        public int GetMaxSequenceNumber(int pollID)
        {
            int max = 0;

            var sequence = (from q in db.Questions where q.PollId == pollID select q.Sequence).ToList();

            if (sequence.Count == 0)
            {
                return max;
            }
            else
            {
                max = sequence.Max().Value;

                return max;
                
            }
            
            
        }

        public void UpdatePollPreferences(int pollID, string name, string description, string endDescription, int limit, string url, bool showResults, bool multipleAnswers, bool publicPoll)
        {
            Poll p = (from poll in db.Polls where poll.Id == pollID select poll).SingleOrDefault();

            p.Name = name;

            p.Description = description;

            p.EndDesc = endDescription;

            p.ResponseLimit = limit;

            p.Url = url;

            p.ShowResults = showResults;

            p.MultipleAnswers = multipleAnswers;

            p.PublicPoll = publicPoll;

            db.SubmitChanges();
        }

        // delete poll by its Id
        public void DeletePoll(int pollId)
        {
            Poll p = (from poll in db.Polls where poll.Id == pollId select poll).SingleOrDefault();

            if (p != null)
            {
                db.Polls.DeleteOnSubmit(p);

                db.SubmitChanges();


                
            }
            
        }

        // check if is such poll name in database
        public bool IsPollName(string pollName, string userName)
        {
            var name = (from poll in db.Polls where poll.Name == pollName && poll.UserName == userName select poll.Name).SingleOrDefault();

            if (name == null)
            {
                return false;
            }

            if (name.ToString() != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // get question answers from Statistics database
        public List<string> GetQuestionAnswers(int questionId)
        {
            List<string> Answers =
                (from answers in db.Statistics where answers.QuestionID == questionId select answers.AnswerName).ToList();

            return Answers;
            
        }

        public int QuestionAnswerChoseCount(int questionId, string answerName)
        {
            int questionAnswerChoseCount = (from answer in db.Statistics
                                            where answer.AnswerName == answerName && answer.QuestionID == questionId
                                            select answer.AnswerChose).Single();

            return questionAnswerChoseCount;
        }

        public int GetPollCount()
        {
            return db.Polls.Count();
        }

        public int GetQuestionCount()
        {
            return db.Questions.Count();
        }

        public int GetAnswerCount()
        {
            return db.Statistics.Count();
        }

        public void AddResponse(int pollId)
        {
            Poll poll = (from p in db.Polls where p.Id == pollId select p).Single();

            poll.ResponseCount += 1;

            db.SubmitChanges();
            
        }

        public int GetResponseLimit(int pollId)
        {
            Poll poll = (from p in db.Polls where p.Id == pollId select p).Single();

            return poll.ResponseLimit;
        }

        public int GetResponseCount(int pollId)
        {
            Poll poll = (from p in db.Polls where p.Id == pollId select p).Single();

            return poll.ResponseCount;
        }

        public bool HasPollAnswers(int pollId)
        {
            var stat = (from s in db.Statistics where s.PollID == pollId select s).FirstOrDefault();

            if (stat == null)
            {
                return false;
            }

            return true;      
        }

        public bool HasPollQuestions(int pollId)
        {
            var question = (from q in db.Questions where q.PollId == pollId select q).FirstOrDefault();

            if (question == null)
            {
                return false;
            }

            return true;
        }

        public bool ShowPollResults(int pollId)
        {
            var result = (from p in db.Polls where p.Id == pollId select p.ShowResults).Single();

            return result;
        }

        public void InsertResponderIP(string ip, int pollId)
        {
            RespondersIP resIP = new RespondersIP();

            resIP.PollId = pollId;

            resIP.IP = ip;

            db.RespondersIPs.InsertOnSubmit(resIP);

            db.SubmitChanges();
        }

        public bool IsIpAddressRegistered(string ip, int pollId)
        {
            var obj = (from i in db.RespondersIPs where i.PollId == pollId select i).SingleOrDefault();

            if (obj == null)
            {
                return false;
            }

            return true;
        }

        public bool IsMultipleAnswerChecked(int pollId)
        {
            bool check =
                (from obj in db.Polls where obj.Id == pollId select obj.MultipleAnswers).Single();

            return check;

        }

        public bool IsPollPublic(int pollId)
        {
            bool value = (from obj in db.Polls where obj.Id == pollId select obj.PublicPoll).Single();

            return value;
        }


    }

   

    




}

