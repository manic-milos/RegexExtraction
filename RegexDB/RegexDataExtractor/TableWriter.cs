using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    abstract class TableWriter
    {
        public abstract string header
        {
            get;
        }
        public abstract string footer
        {
            get;
        }
        public abstract string pattern
        {
            get;
        }
        public virtual string get(Table table)
        {
            string result = header;
            int lines = int.MaxValue;
            foreach (Row row in table.rows)
            {
                if (lines-- < 0)
                    break;
                string rowpattern = pattern;
                for (int i = 0; i < table.columns.Count; i++)
                {
                    rowpattern = rowpattern.Replace("/" + table.columns[i].name + "/", row.items[i].value);
                }
                result += rowpattern;
            }
            result += footer;
            return result;

        }
    }
}
