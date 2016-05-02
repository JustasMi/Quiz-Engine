using MySql.Data.MySqlClient;
using Quiz_Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine
{
    public class DButility
    {
        private String connectionString = Quiz_Engine.Properties.Resources.connectionString;
        private MySqlConnection conn;

        public DButility()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = connectionString;
            conn.Open();
        }

        ~DButility()
        {
            conn.Close();
        }

        public void addTopic(String topic, int subjectId)
        {
            String sqlCommand = "INSERT INTO topics (name, subject) VALUES('"+topic+"', "+subjectId+");";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }
        // question text, topic ID, type, difficulty, nature, feedback
        public int addQuestion(String question, int topicID, String type, String difficulty, String nature, String feedback)
        {
            // Old SQL "INSERT INTO questions (question, topic) VALUES('" + question + "', "+topicID+");";
            String sqlCommand = "INSERT INTO questions (question, topic, question_type, difficulty, nature, feedback) VALUES ('"+question+"', "+topicID+", '"+type+"', '"+difficulty+"', '"+nature+"', '"+feedback+"');";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();

            sqlCommand = "SELECT LAST_INSERT_ID()";
            command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            rdr.Read();
            int questionID = Int32.Parse(rdr[0].ToString());
            rdr.Close();
            return questionID;

        }

        public void addAnswers(int questionID, List<Answer> answers)
        {
            String sqlCommand = "INSERT INTO answers (answer,questionId, correct) VALUES ";
            foreach (Answer answer in answers)
            {
                int correctAnswer = 0;
                if (answer.Correct)
                {
                    correctAnswer = 1;
                }

                sqlCommand += "('"+answer.AnswerText+"', "+questionID+", "+ correctAnswer+"), ";
            }
            sqlCommand = sqlCommand.Remove(sqlCommand.Length - 2) + ";";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }

        public int addUser(String name)
        {
            String sqlCommand = "INSERT INTO users (name) VALUES ('"+name+"');";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();

            sqlCommand = "SELECT LAST_INSERT_ID()";
            command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            rdr.Read();
            int userID = Int32.Parse(rdr[0].ToString());
            rdr.Close();

            return userID;
        }

        public void addSubject(String subject)
        {
            String sqlCommand = "INSERT INTO subjects (name) VALUES('" + subject + "');";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }

        public List<Topic> getTopics(int subjectID)
        {
            List<Topic> topics = new List<Topic>();

            String sqlCommand = "SELECT id, name FROM topics WHERE subject="+subjectID+";";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                //System.Diagnostics.Debug.WriteLine(rdr[0] + " -- " + rdr[1]);
                topics.Add(new Topic(Int32.Parse(rdr[0].ToString()), rdr[1].ToString()));
            }
            rdr.Close();
            
            return topics;
        }

        public List<Question> getQuestions(Topic topic, int questionAmount)
        {
            List<Question> questions = new List<Question>();

            String sqlCommand = "SELECT id, question, question_type, difficulty, nature, feedback FROM questions WHERE topic="+topic.Id+" ORDER BY RAND() LIMIT "+questionAmount+";";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), topic, rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString()));
            }
            rdr.Close();

            return questions;
        }

        public List<Answer> getAnswers(int questionId)
        {
            List<Answer> answers = new List<Answer>();

            String sqlCommand = "SELECT answer, correct, id FROM answers WHERE questionId=" + questionId + " ORDER BY RAND();";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                //System.Diagnostics.Debug.WriteLine("Answer: " + rdr[0] + ", correctness: " + rdr[1] + ", equals to 1: " + (Boolean) rdr[1]);
                answers.Add(new Answer(rdr[0].ToString(), (Boolean) rdr[1], Int32.Parse(rdr[2].ToString())));
            }
            rdr.Close();

            return answers;
        }

        public void addQuizResult(User currentUser, List<Question> questions, String topics)
        {
            int correct_answers = 0;
            foreach (Question q in questions)
            {
                foreach (Answer a in q.Answers)
                {
                    if (a.Selected && a.Correct)
                    {
                        correct_answers += 1;
                    }
                }
            }
            String sqlCommand = "INSERT INTO quiz_history (user, date, total_questions, correct_answers, topics) VALUES (" + currentUser.Id + ", NOW(), " + questions.Count + ", " + correct_answers + ", '"+topics+"');";
            //DATE(NOW())
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();

            // Get ID of the last entry
            sqlCommand = "SELECT LAST_INSERT_ID()";
            command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            rdr.Read();
            int quiz_history_ID = Int32.Parse(rdr[0].ToString());
            rdr.Close();

            // Save questions

            sqlCommand = "INSERT INTO quiz_history_questions (question, selectedAnswer, correctlyAnswered, topic, quiz) VALUES ";
            foreach (Question q in questions)
            {
                String selectedAnswer = q.Answers.Find(answer => answer.Selected) != null ? q.Answers.Find(answer => answer.Selected).Id.ToString() : "NULL";
                
                // Check if answer was correct, in that case value 1, else 0
                int correctness = 0;
                if (q.Answers.Find(answer => answer.Selected) != null && q.Answers.Find(answer => answer.Selected).Correct)
                    correctness = 1;
                sqlCommand += "(" + q.Id + ", " + selectedAnswer + ", "+correctness+", " + q.Topic.Id + ", " + quiz_history_ID + "), ";
            }
            sqlCommand = sqlCommand.Remove(sqlCommand.Length - 2) + ";";
            command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }

        public List<Quiz> getQuizHistory(User currentUser)
        {
            List<Quiz> quiz_history = new List<Quiz>();

            String sqlCommand = "SELECT id, date, total_questions, correct_answers, topics FROM quiz_history WHERE user=" + currentUser.Id + " ORDER BY date DESC;";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                //System.Diagnostics.Debug.WriteLine("Answer: " + rdr[0] + ", correctness: " + rdr[1] + ", equals to 1: " + (Boolean) rdr[1]);
                quiz_history.Add(new Quiz(Int32.Parse(rdr[0].ToString()), Int32.Parse(rdr[2].ToString()), Int32.Parse(rdr[3].ToString()), currentUser, (DateTime) rdr[1], rdr[4].ToString()));
            }
            rdr.Close();

            return quiz_history;
        }

        public List<Topic> getQuizTopics(int quizID)
        {
            List<Topic> topics = new List<Topic>();
            String sqlCommand = "SELECT id, name FROM topics WHERE id IN (SELECT DISTINCT topic FROM quiz_history_questions WHERE quiz="+quizID+");";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                //System.Diagnostics.Debug.WriteLine("Answer: " + rdr[0] + ", correctness: " + rdr[1] + ", equals to 1: " + (Boolean) rdr[1]);
                topics.Add(new Topic(Int32.Parse(rdr[0].ToString()), rdr[1].ToString()));
            }
            rdr.Close();

            return topics;
        }

        public List<Question> getQuizQuestions(int quizID)
        {
            List<Question> questions = new List<Question>();
            String sqlCommand = "SELECT a.id, a.question, a.topic, b.name FROM questions a, topics b WHERE a.id IN (SELECT question FROM quiz_history_questions WHERE quiz="+quizID+") AND a.topic = b.id;";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                //TODO: COMMENT THIS BACK IN!
                //questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), new Topic(Int32.Parse(rdr[2].ToString()), rdr[3].ToString())));
            }
            rdr.Close();

            return questions;
        }

        public void addTrueFalseAnswer(int questionId, Boolean answer)
        {
            String sqlCommand = "INSERT INTO answers (answer, questionId, correct) VALUES ('True', " + questionId + ", " + answer + "), ('False', " + questionId + ", " + !answer + ");";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }

        public void addFillInAnswer(int questionID, string answer)
        {
            String sqlCommand = "INSERT INTO answers (answer, questionId, correct) VALUES ('"+answer+"', " + questionID + ", " + true + ");";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }
    }
}
