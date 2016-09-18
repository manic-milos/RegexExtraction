using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Storage
    {
        RegexExtractor regex;
        public List<Column> columns = new List<Column>();
        public List<Row> rows = new List<Row>();
        public Storage(string regex, List<Column> columns)
        {
            this.regex = new RegexExtractor(regex);
            this.columns = columns;
        }
        public void print(string lines)
        {


        }
        public Storage ExtractFromString(string lines)
        {
            int position = 0;
            while (position < lines.Length)//TODO sugavo
            {
                string[] data = regex.extractFromString(lines, out position);
                Row newRow = new Row();
                for (int i = 0; i < data.Length; i++)
                {
                    Item item = new Item() { value = data[i] };
                    newRow.AddItem(item);
                    columns[i].AddItem(item);
                }
                rows.Add(newRow);
                lines = lines.Substring(position);

            }
            return this;
        }
        public void show(System.Windows.Forms.ListView listview)
        {
            while (listview.Columns.Count > 0)
            {
                listview.Columns.RemoveAt(0);
            }
            foreach (Column column in columns)
            {
                listview.Columns.Add(column.name);
            }
            foreach(Row row in rows)
            {
                System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(row.ToStringArray());
                listview.Items.Add(listViewItem);
            }
        }
    }
}
