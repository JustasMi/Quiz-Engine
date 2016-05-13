using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class QuizPreference
    {
        int quiz_size = 20;
        List<Topic> topics = new List<Topic>();
        bool easy = false;
        bool intermediate = false;
        bool hard = false;

        bool bookwork = false;
        bool background = false;
        bool application = false;

        bool feedback = false;
        bool summary = false;

        public QuizPreference()
        {

        }

        public int QuizSize
        {
            get
            {
                return this.quiz_size;
            }
            set
            {
                this.quiz_size = value;
            }
        }

        public bool Easy
        {
            get
            {
                return this.easy;
            }
            set
            {
                this.easy = value;
            }
        }

        public bool Intermediate
        {
            get
            {
                return this.intermediate;
            }
            set
            {
                this.intermediate = value;
            }
        }

        public bool Hard
        {
            get
            {
                return this.hard;
            }
            set
            {
                this.hard = value;
            }
        }

        public bool Bookwork
        {
            get
            {
                return this.bookwork;
            }
            set
            {
                this.bookwork = value;
            }
        }

        public bool Application
        {
            get
            {
                return this.application;
            }
            set
            {
                this.application = value;
            }
        }

        public bool Background
        {
            get
            {
                return this.background;
            }
            set
            {
                this.background = value;
            }
        }

        public List<Topic> Topics
        {
            get
            {
                return this.topics;
            }
            set
            {
                this.topics = value;
            }
        }

        public bool Summary
        {
            get
            {
                return this.summary;
            }
            set
            {
                this.summary = value;
            }
        }

        public bool Feedback
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

    }
}
