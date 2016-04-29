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


        public Answer(String answer)
        {
            this.answer = answer;
        }

        public Answer (String answer, Boolean correct)
        {
            this.answer = answer;
            this.correct = correct; 
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
