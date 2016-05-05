using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class Answer
    {
        private String answer;
        private Boolean correct = false;
        private Boolean selected = false;
        private int id;
        private String typedAnswer = "";

        
        public Answer(String answer, Boolean correct)
        {
            this.answer = answer;
            this.correct = correct;
        }
        

        public Answer (String answer, Boolean correct, int id)
        {
            this.answer = answer;
            this.correct = correct;
            this.id = id;
        }

        public string AnswerText
        {
            get
            {
                return this.answer;
            }
            set
            {
                this.answer = value;
            }
        }

        public string TypedAnswer
        {
            get
            {
                return this.typedAnswer;
            }
            set
            {
                this.typedAnswer = value;
            }
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

        public Boolean Correct
        {
            get
            {
                return this.correct;
            }
            set
            {
                this.correct = value;
            }
        }

        public Boolean Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }
    }
}
