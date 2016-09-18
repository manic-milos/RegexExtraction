using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Table
    {
        RegexExtractor regex;
        public List<Column> columns = new List<Column>();
        public List<Row> rows = new List<Row>();
        public Table(string regex, List<Column> columns)
        {
            this.regex = new RegexExtractor(regex);
            this.columns = columns;
        }

        
        public void print(string lines)
        {


        }
        public Table ExtractFromString(string lines)
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
        public Table CloneColumns()
        {
            List<Column> newColumns = new List<Column>();
            foreach(Column column in columns)
            {
                newColumns.Add(new Column(column.name));
            }
            Table newStorage = new Table(this.regex.regexstring, newColumns);
            return newStorage;
        }
        public void AddRows(IEnumerable<Row> newRows)
        {
            foreach(Row row in newRows)
            {
                AddRow(row);
            }
        }
        public void AddRow(Row row)
        {
            rows.Add(row);
            for (int i = 0; i < columns.Count; i++)
            {
                row.items[i].column = columns[i];
            }
        }
        public Table Where(Func<Row,bool> clause)
        {
            Table queryResult = this.CloneColumns();
            var result = this.rows.Where(clause);
            queryResult.AddRows(result);
            return queryResult;
        }
        //public Table Select(List<Column> selectedColumns)
        //{
        //    Table queryResult = new Table(this.regex.regexstring,
        //        selectedColumns);
        //    foreach(Row row in rows)
        //    {

        //    }
        //}
    }
}
