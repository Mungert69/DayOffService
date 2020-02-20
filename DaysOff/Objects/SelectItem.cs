using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class SelectItem
    {

        private string value;
        private string label;
        private int id;

        public string Value { get => value; set => this.value = value; }
        public string Label { get => label; set => label = value; }
        public int Id { get => id; set => id = value; }
    }
}
