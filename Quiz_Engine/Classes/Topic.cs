﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz_Engine.Classes
{
    public class Topic
    {
        private String name;
        private int id;

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

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }


        public Topic(int id, String name)
        {
            this.id = id;
            this.name = name;
        }

        /*
        public String getName()
        {
            return this.name;
        }

        public int getId()
        {
            return this.id;
        }
        */
        public override string ToString()
        {
            return this.name;
        }
         

    }
}
