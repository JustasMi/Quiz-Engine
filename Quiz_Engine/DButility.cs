using MySql.Data.MySqlClient;
using Quiz_Engine.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine
{
    class DButility
    {
        private String connectionString = Quiz_Engine.Properties.Resources.connectionString;
        MySqlConnection conn;

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

        public void addTopic(String topic)
        {
            String sqlCommand = "INSERT INTO topics (name) VALUES('"+topic+"');";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }

        public int addQuestion(String question, int topicID)
        {
            //System.Diagnostics.Debug.WriteLine(question + " is being added.");
            String sqlCommand = "INSERT INTO questions (question, topic) VALUES('" + question + "', "+topicID+");";
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

            String sqlCommand = "SELECT id, question FROM questions WHERE topic="+topic.Id+" ORDER BY RAND() LIMIT "+questionAmount+";";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), topic));
            }
            rdr.Close();

            return questions;
        }

        public List<Answer> getAnswers(int questionId)
        {
            List<Answer> answers = new List<Answer>();

            String sqlCommand = "SELECT answer, correct FROM answers WHERE questionId=" + questionId + " ORDER BY RAND();";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                //System.Diagnostics.Debug.WriteLine("Answer: " + rdr[0] + ", correctness: " + rdr[1] + ", equals to 1: " + (Boolean) rdr[1]);
                answers.Add(new Answer(rdr[0].ToString(), (Boolean) rdr[1]));
            }
            rdr.Close();

            return answers;
        }

        public void addQuizResult(User currentUser, List<Question> questions, DateTime date)
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
            String sqlCommand = "INSERT INTO quiz_history (user, date, total_questions, correct_answers) VALUES (" + currentUser.Id + ", DATE(NOW()), " + questions.Count + ", " + correct_answers + ");";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }
    }
}
