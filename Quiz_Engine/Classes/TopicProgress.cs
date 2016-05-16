using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class TopicProgress
    {
        int total;
        int correct;
        Dictionary<string, List<int>> properties;
        public TopicProgress(int correct, int total)
        {
            this.correct = correct;
            this.total = total;
        }

        public int Total
        {
            get
            {
                return this.total;
            }
            set
            {
                this.total = value;
            }
        }

        public int Correct
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

        public Dictionary<string, List<int>> Properties
        {
            get
            {
                return this.properties;
            }
            set
            {
                this.properties = value;
            }
        }
    }
}
