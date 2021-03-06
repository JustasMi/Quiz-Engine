﻿using MySql.Data.MySqlClient;
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

        public int addUser(String name, int passwordHashed)
        {
            String sqlCommand = "INSERT INTO users (name, password) VALUES ('"+name+"', "+passwordHashed+");";

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
                if (q.QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
                {
                    foreach (Answer a in q.Answers)
                    {
                        if (a.Selected && a.Correct)
                        {
                            correct_answers += 1;
                        }
                    }
                }
                else if (q.QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
                {
                    int correctAnswerCount = 0;
                    foreach (Answer a in q.Answers)
                    {
                        if ((a.Correct && a.Selected) || (!a.Correct && !a.Selected))
                            correctAnswerCount += 1;
                    }

                    if (correctAnswerCount == q.Answers.Count)
                    {
                        correct_answers += 1;
                    }
                }
                else if (q.QuestionType == Quiz_Engine.Properties.Resources.trueFalse)
                {
                    foreach (Answer a in q.Answers)
                    {
                        if (a.Selected && a.Correct)
                        {
                            correct_answers += 1;
                        }
                    }
                }
                else
                {
                    // Fill in The Answer question
                    if (q.Answers[0].TypedAnswer.Equals(q.Answers[0].AnswerText, StringComparison.InvariantCultureIgnoreCase))
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
                
                if (q.QuestionType == Quiz_Engine.Properties.Resources.multipleChoice || q.QuestionType == Quiz_Engine.Properties.Resources.trueFalse)
                {
                    if (q.Answers.Find(answer => answer.Selected) != null && q.Answers.Find(answer => answer.Selected).Correct)
                        correctness = 1;
                }
                else if (q.QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
                {
                    int answerCount = 0;
                    foreach (Answer a in q.Answers)
                    {
                        if ((a.Correct && a.Selected) || (!a.Correct && !a.Selected))
                        {
                            answerCount += 1;
                        }
                    }

                    if (answerCount == q.Answers.Count)
                        correctness = 1;
                }
                else if (q.QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
                {
                    if (q.Answers[0].AnswerText.Equals(q.Answers[0].TypedAnswer, StringComparison.InvariantCultureIgnoreCase))
                        correctness = 1;
                }

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
            //String sqlCommand = "SELECT a.id, a.question, a.topic, b.name FROM questions a, topics b WHERE a.id IN (SELECT question FROM quiz_history_questions WHERE quiz="+quizID+") AND a.topic = b.id;";
            String sqlCommand = "SELECT a.id, a.question, a.topic, a.question_type, a.difficulty, a.nature, a.feedback, b.name FROM questions a, topics b WHERE a.id IN (SELECT question FROM quiz_history_questions WHERE quiz="+quizID+") AND a.topic = b.id;";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                // id 0, question 1, topic 2, question type 3, difficulty 4, nature 5, feedback 6, topic name 7  
                //questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), new Topic(Int32.Parse(rdr[2].ToString()), rdr[3].ToString())));
                questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), new Topic(Int32.Parse(rdr[2].ToString()), rdr[7].ToString()), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString(), rdr[6].ToString()));
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

        public User verifyUser(string userId, int hashedPassword)
        {
            String sqlCommand = "SELECT name, id FROM users WHERE name='"+userId+"' AND password="+hashedPassword+";";
            System.Diagnostics.Debug.WriteLine(sqlCommand);
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            //rdr.Read();
            if (rdr.Read())
            {
                return new User(userId, Int32.Parse(rdr[1].ToString()));
            }
            //int password = Int32.Parse(rdr[0].ToString());
            rdr.Close();

            return null;
            /*
            if (password == hashedPassword)
                return true;
            return false;
             */
        }

        public List<Question> getQuestionsFromPreferences(QuizPreference preferences)
        {
            List<Question> questions = new List<Question>();
            bool difficultyPreference = false;
            bool naturePreference = false;
            String sqlCommand = "SELECT q.id, q.question, q.question_type, q.difficulty, q.nature, q.feedback, q.topic, t.name FROM questions q, topics t WHERE ";
            
            if (preferences.Easy)
            {
                sqlCommand += "(q.difficulty = 'Easy' ";
                difficultyPreference = true;
            }

            if (preferences.Intermediate)
            {
                if (difficultyPreference)
                {
                    sqlCommand += "OR q.difficulty = 'Intermediate' ";
                }
                else
                {
                    sqlCommand += "(q.difficulty = 'Intermediate' ";
                    difficultyPreference = true;
                }
            }

            if (preferences.Hard)
            {
                if (difficultyPreference)
                {
                    sqlCommand += "OR q.difficulty = 'Hard') ";
                }
                else
                {
                    sqlCommand += "(q.difficulty = 'Hard') ";
                    difficultyPreference = true;
                }
            }
            else
            {
                if (difficultyPreference)
                {
                    sqlCommand +=") ";
                }
            }

            if (preferences.Application || preferences.Background || preferences.Bookwork)
            {
                if (difficultyPreference)
                {
                    sqlCommand += "AND ";
                }

                if (preferences.Application)
                {
                    sqlCommand += "(q.nature = 'Application' ";
                    naturePreference = true;
                }

                if (preferences.Background)
                {
                    if (naturePreference)
                    {
                        sqlCommand += "OR q.nature = 'Background' ";
                    }
                    else
                    {
                        sqlCommand += "(q.nature = 'Background' ";
                        naturePreference = true;
                    }
                }

                if (preferences.Bookwork)
                {
                    if (naturePreference)
                    {
                        sqlCommand += "OR q.nature = 'Bookwork') ";
                    }
                    else
                    {
                        sqlCommand += "(q.nature = 'Bookwork') ";
                        naturePreference = true;
                    }
                }
                else if (naturePreference)
                {
                    sqlCommand += ") ";
                }
            }

            if (preferences.Topics.Count > 0)
            {
                sqlCommand += "AND (";
                foreach (Topic t in preferences.Topics)
                {
                    sqlCommand += "q.topic=" + t.Id + " OR ";
                }
                sqlCommand = sqlCommand.Remove(sqlCommand.Length - 3);
                sqlCommand += ") ";
            }

            sqlCommand += "AND (q.topic = t.id) ORDER BY RAND() LIMIT " + preferences.QuizSize + ";";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), new Topic(Int32.Parse(rdr[6].ToString()), rdr[7].ToString()), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString()));
            }
            rdr.Close();
            System.Diagnostics.Debug.WriteLine(sqlCommand);
            return questions;
        }

        public bool checkPreviousAnswer(int questionId, int quizId)
        {
            String sqlCommand = "SELECT correctlyAnswered FROM mydb.quiz_history_questions WHERE quiz = "+quizId+" AND question = "+questionId+" ORDER BY id DESC LIMIT 1;";
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            bool correctness = (bool) rdr[0];
            rdr.Close();
            //SELECT correctlyAnswered FROM mydb.quiz_history_questions WHERE quiz = quizID AND question = questionID;
            return correctness;
        }

        public TopicProgress getProgress(Topic topic, User user)
        {
            Dictionary<string, List<int>> properties = new Dictionary<string, List<int>>();
            // QUERY returns progress quiz by quiz SELECT a.quiz, SUM(a.correctlyAnswered=1) AS correct, COUNT(a.correctlyAnswered) AS total, b.date FROM quiz_history_questions a, quiz_history b WHERE a.quiz = b.id AND a.topic="+topic.Id+" GROUP BY quiz
            //OLDER WITHOUT SPECIFYING USER "SELECT SUM(correctlyAnswered=1) AS correct, COUNT(correctlyAnswered) AS total FROM quiz_history_questions WHERE topic="+topic.Id+";";
            String sqlCommand = "SELECT IFNULL(SUM(a.correctlyAnswered=1),0) AS correct, COUNT(a.correctlyAnswered) AS total FROM quiz_history_questions a, quiz_history b WHERE a.topic="+topic.Id+" AND a.quiz=b.id AND b.user="+user.Id+";";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();
            rdr.Read();
            // READ DATA
            TopicProgress progress = new TopicProgress(Int32.Parse(rdr[0].ToString()), Int32.Parse(rdr[1].ToString()));
            rdr.Close();

            sqlCommand = "SELECT IFNULL(SUM(CASE WHEN b.difficulty='Easy' THEN 1 ELSE 0 END),0) AS easy_total, IFNULL(SUM(CASE WHEN b.difficulty='Easy' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS easy_correct, IFNULL(SUM(CASE WHEN b.difficulty='Intermediate' THEN 1 ELSE 0 END),0) AS intermediate_total, IFNULL(SUM(CASE WHEN b.difficulty='Intermediate' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS intermediate_correct, IFNULL(SUM(CASE WHEN b.difficulty='Hard' THEN 1 ELSE 0 END),0) AS hard_total, IFNULL(SUM(CASE WHEN b.difficulty='Hard' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS hard_correct, IFNULL(SUM(CASE WHEN b.nature='Background' THEN 1 ELSE 0 END),0) AS background_total, IFNULL(SUM(CASE WHEN b.nature='Background' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS background_correct, IFNULL(SUM(CASE WHEN b.nature='Application' THEN 1 ELSE 0 END),0) AS application_total, IFNULL(SUM(CASE WHEN b.nature='Application' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS application_correct, IFNULL(SUM(CASE WHEN b.nature='Bookwork' THEN 1 ELSE 0 END),0) AS bookwork_total, IFNULL(SUM(CASE WHEN b.nature='Bookwork' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END),0) AS bookwork_correct, COUNT(DISTINCT a.quiz) FROM quiz_history_questions a, questions b, quiz_history c WHERE a.question=b.id AND b.topic=" + topic.Id + " AND c.id=a.quiz AND c.user=" + user.Id + ";";
                //"SELECT SUM(CASE WHEN b.difficulty='Easy' THEN 1 ELSE 0 END) AS easy_total, SUM(CASE WHEN b.difficulty='Easy' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS easy_correct, SUM(CASE WHEN b.difficulty='Intermediate' THEN 1 ELSE 0 END) AS intermediate_total, SUM(CASE WHEN b.difficulty='Intermediate' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS intermediate_correct, SUM(CASE WHEN b.difficulty='Hard' THEN 1 ELSE 0 END) AS hard_total, SUM(CASE WHEN b.difficulty='Hard' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS hard_correct, SUM(CASE WHEN b.nature='Background' THEN 1 ELSE 0 END) AS background_total, SUM(CASE WHEN b.nature='Background' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS background_correct, SUM(CASE WHEN b.nature='Application' THEN 1 ELSE 0 END) AS application_total, SUM(CASE WHEN b.nature='Application' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS application_correct, SUM(CASE WHEN b.nature='Bookwork' THEN 1 ELSE 0 END) AS bookwork_total, SUM(CASE WHEN b.nature='Bookwork' AND a.correctlyAnswered=1 THEN 1 ELSE 0 END) AS bookwork_correct FROM quiz_history_questions a, questions b, quiz_history c WHERE a.question=b.id AND b.topic="+topic.Id+" AND c.id=a.quiz AND c.user="+user.Id+";";
            command = new MySqlCommand(sqlCommand, conn);
            rdr = command.ExecuteReader();
            rdr.Read();
            properties.Add("Easy", new List<int> { Int32.Parse(rdr[0].ToString()), Int32.Parse(rdr[1].ToString()) });
            properties.Add("Intermediate", new List<int> { Int32.Parse(rdr[2].ToString()), Int32.Parse(rdr[3].ToString()) });
            properties.Add("Hard", new List<int> { Int32.Parse(rdr[4].ToString()), Int32.Parse(rdr[5].ToString()) });

            properties.Add("Background", new List<int> { Int32.Parse(rdr[6].ToString()), Int32.Parse(rdr[7].ToString()) });
            properties.Add("Application", new List<int> { Int32.Parse(rdr[8].ToString()), Int32.Parse(rdr[9].ToString()) });
            properties.Add("Bookwork", new List<int> { Int32.Parse(rdr[10].ToString()), Int32.Parse(rdr[11].ToString()) });

            properties.Add("Tests", new List<int> { Int32.Parse(rdr[12].ToString()) });
            progress.Properties = properties;
            return progress;
        }

        public List<Question> getPastFailedQuestions(User currentUser, List<Topic> list, int limit)
        {
            List<Question> questions = new List<Question>();
            string topicCondition = "";
            if (list.Count > 1)
            {
                topicCondition = "(";
                foreach (Topic t in list)
                {
                    topicCondition += " a.topic = "+t.Id+" OR";
                }
                topicCondition = topicCondition.Remove(topicCondition.Length-3);
                topicCondition += ")";
            }
            else
            {
                topicCondition = "a.topic = "+list[0].Id;
            }

            String sqlCommand = "SELECT c.id, c.question, c.question_type, c.difficulty, c.nature, c.feedback, c.topic, d.name FROM quiz_history_questions a, quiz_history b, questions c, topics d WHERE b.id = a.quiz AND c.id = a.question AND d.id=a.topic AND b.user = "+currentUser.Id+" AND "+topicCondition+" AND a.correctlyAnswered = 0 GROUP BY a.question LIMIT "+limit+";";
            System.Diagnostics.Debug.WriteLine(sqlCommand);
            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            MySqlDataReader rdr = command.ExecuteReader();

            while (rdr.Read())
            {
                questions.Add(new Question(Int32.Parse(rdr[0].ToString()), rdr[1].ToString(), new Topic(Int32.Parse(rdr[6].ToString()), rdr[7].ToString()), rdr[2].ToString(), rdr[3].ToString(), rdr[4].ToString(), rdr[5].ToString()));
            }
            rdr.Close();

            return questions;
        }
    }
}
