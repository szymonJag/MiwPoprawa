using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoprawaMiW.Models
{
    public class Column<T>
    {
        public int IdOfColumn { get; set; }
        public Dictionary<int, T> Rows { get; set; }

        public Column()
        {

        }
        public Column(int idOfColumn, Dictionary<int, T> rows)
        {
            IdOfColumn = idOfColumn;
            Rows = rows;
        }
    }
}
