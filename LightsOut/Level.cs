using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{
    public class Level
    {
        public string? Name { get; set; }

        public int? Columns { get; set; }

        public int? Rows { get; set; }

        public List<int>? On { get; }

        public Level(string name, int columns, int rows, List<int> on)
        {
            Name = name;
            Columns = columns;
            Rows = rows;
            On = on;
        }
    }
}
