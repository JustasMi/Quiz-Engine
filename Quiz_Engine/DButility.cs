using MySql.Data.MySqlClient;
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
            System.Diagnostics.Debug.WriteLine(question + " is being added.");
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

        public void addAnswers(int questionID, List<String> answers)
        {
            String sqlCommand = "INSERT INTO answers (answer,questionId) VALUES ";
            foreach (String answer in answers)
            {
                sqlCommand += "('"+answer+"', "+questionID+"), ";
            }
            sqlCommand = sqlCommand.Remove(sqlCommand.Length - 2) + ";";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();

            //System.Diagnostics.Debug.WriteLine(sqlCommand);
        }

        public void addUser(String name)
        {
            String sqlCommand = "INSERT INTO users (name) VALUES ('"+name+"');";

            MySqlCommand command = new MySqlCommand(sqlCommand, conn);
            command.ExecuteNonQuery();
        }
    }
}
