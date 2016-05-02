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
        private string question_type;
        private string difficulty;
        private string nature;
        private string feedback;

        public Question(int id, string question, Topic topic, String question_type, String difficulty, String nature, String feedback)
        {
            this.id = id;
            this.question = question;
            this.topic = topic;
            this.question_type = question_type;
            this.difficulty = difficulty;
            this.nature = nature;
            this.feedback = feedback;
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

        public string QuestionType
        {
            get
            {
                return this.question_type;
            }
            set
            {
                this.question_type = value;
            }
        }

        public string Difficulty
        {
            get
            {
                return this.difficulty;
            }
            set
            {
                this.difficulty = value;
            }
        }

        public string Nature
        {
            get
            {
                return this.nature;
            }
            set
            {
                this.nature = value;
            }
        }

        public string Feedback
        {
            get
            {
                return this.feedback;
            }
            set
            {
                this.feedback = value;
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
