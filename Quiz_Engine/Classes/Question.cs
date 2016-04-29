using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class Question
    {
        private int id;
        private string question;
        private Topic topic;
        private List<Answer> answers;

        public Question(int id, string question, Topic topic)
        {
            this.id = id;
            this.question = question;
            this.topic = topic;
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public string QuestionText
        {
            get
            {
                return this.question;
            }
            set
            {
                this.question = value;
            }
        }

        public Topic Topic
        {
            get
            {
                return this.topic;
            }
        }

        public List<Answer> Answers
        {
            get
            {
                return this.answers;
            }
            set
            {
                this.answers = value;
            }
        }


    }
}
