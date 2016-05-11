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

        public String getSelectedAnswer()
        {
            List<Answer> selectedAnswers = new List<Answer>();

            if (QuestionType == Quiz_Engine.Properties.Resources.trueFalse || QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                foreach (Answer a in Answers)
                {
                    if (a.Selected)
                    {
                        selectedAnswers.Add(a);
                    }

                }
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                if (Answers[0].Selected)
                    selectedAnswers.Add(Answers[0]);
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {

                foreach (Answer a in Answers)
                {
                    if (a.Selected)
                        selectedAnswers.Add(a);
                }
            }
            String returnStrng = "";
            foreach (Answer a in selectedAnswers)
            {
                if (QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
                {
                    returnStrng += a.TypedAnswer + ", ";
                }
                else
                {
                    returnStrng += a.AnswerText + ", ";
                }
            }

            if (returnStrng.Length > 2)
                returnStrng = returnStrng.Remove(returnStrng.Length - 2);
            return returnStrng;
        }

        public String getCorrectAnswer()
        {
            List<Answer> selectedAnswers = new List<Answer>();

            if (QuestionType == Quiz_Engine.Properties.Resources.trueFalse || QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                foreach (Answer a in Answers)
                {
                    if (a.Correct)
                    {
                        selectedAnswers.Add(a);
                    }

                }
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.fillInTheAnswer)
            {
                selectedAnswers.Add(Answers[0]);
                /*
                if (Answers[0].TypedAnswer.Equals(Answers[0].AnswerText, StringComparison.InvariantCultureIgnoreCase))
                {
                    answeredCorrectly = true;
                }
                selectedAnswer = questions[displayedQuestionIndex].Answers[0].TypedAnswer;
                correctAnswer = questions[displayedQuestionIndex].Answers[0].AnswerText;
                 * */
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                foreach (Answer a in Answers)
                {
                    if (a.Correct)
                        selectedAnswers.Add(a);
                }
            }
            String returnStrng = "";
            foreach (Answer a in selectedAnswers)
            {
                returnStrng += a.AnswerText+", ";
            }
            if (returnStrng.Length > 2)
                returnStrng = returnStrng.Remove(returnStrng.Length - 2);

            return returnStrng;
        }
        
        public bool isAnsweredCorrectly()
        {
            if (QuestionType == Quiz_Engine.Properties.Resources.multipleChoice)
            {
                foreach (Answer a in Answers)
                {
                    if (a.Selected && a.Correct)
                    {
                        return true;
                    }
                }
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.multipleAnswer)
            {
                int correctAnswerCount = 0;
                foreach (Answer a in Answers)
                {
                    if ((a.Correct && a.Selected) || (!a.Correct && !a.Selected))
                        correctAnswerCount += 1;
                }

                if (correctAnswerCount == Answers.Count)
                    return true;
            }
            else if (QuestionType == Quiz_Engine.Properties.Resources.trueFalse)
            {
                foreach (Answer a in Answers)
                {
                    if (a.Selected && a.Correct)
                    {
                        return true;
                    }
                }
            }
            else
            {
                // Fill in The Answer question
                if (Answers[0].TypedAnswer.Equals(Answers[0].AnswerText, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
