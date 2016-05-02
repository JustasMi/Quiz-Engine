using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class Quiz
    {
        private int id;
        private int totalQuestions;
        private int correctAnswers;
        private User user;
        private DateTime date;
        private String topics;

        public Quiz(int id, int totalQuestions, int correctAnswers, User user, DateTime date, String topics)
        {
            this.id = id;
            this.totalQuestions = totalQuestions;
            this.correctAnswers = correctAnswers;
            this.user = user;
            this.date = date;
            this.topics = topics;
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

        public int TotalQuestions
        {
            get
            {
                return this.totalQuestions;
            }
            set
            {
                this.totalQuestions = value;
            }
        }

        public int CorrectAnswers
        {
            get
            {
                return this.correctAnswers;
            }
            set
            {
                this.correctAnswers = value;
            }
        }

        public User User
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
            }
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        public String Topics
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

    }
}
