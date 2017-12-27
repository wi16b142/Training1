using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training1.Classes
{
    public class Entry
    {
        private string button;
        private string state;

        public Entry(string button, string state)
        {
            this.button = button;
            this.state = state;
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string Button
        {
            get { return button; }
            set { button = value; }
        }
    }
}
