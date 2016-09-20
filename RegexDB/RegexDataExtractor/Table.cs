using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Table
    {
        public RegexExtractor regex;
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
            foreach (Row row in rows)
            {
                System.Windows.Forms.ListViewItem listViewItem = new System.Windows.Forms.ListViewItem(row.ToStringArray());
                listview.Items.Add(listViewItem);
            }
        }
        public Table CloneColumns()
        {
            List<Column> newColumns = new List<Column>();
            foreach (Column column in columns)
            {
                newColumns.Add(new Column(column.name));
            }
            Table newStorage = new Table(this.regex.regexstring, newColumns);
            return newStorage;
        }
        public void AddRows(IEnumerable<Row> newRows)
        {
            foreach (Row row in newRows)
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
                columns[i].AddItem(row.items[i]);
            }
        }
        public int AddColumn(string name)
        {
            columns.Add(new Column(name));
            return columns.Count - 1;
        }
        public Table Where(Func<Row, bool> clause)
        {
            Table queryResult = this.CloneColumns();
            var result = this.rows.Where(clause);
            queryResult.AddRows(result);
            return queryResult;
        }
        public Table Average(int? groupByIndex, string agregatedcolumnname, string resultingColName)
        {
            return Average(groupByIndex, getColumnIndexFromName(agregatedcolumnname), resultingColName);
        }
        public virtual Table Average(int? groupByIndex, int agregatedcolumnindex, string resultingColName)
        {
            int groupBy;
            if (groupByIndex.HasValue)
            {
                groupBy = groupByIndex.Value;

                Table average = new Table("",new List<Column>(){
                    new Column(columns[groupBy].name),
                    new Column(resultingColName)});
                var distinct = columns[groupBy].items.Distinct(new Item.comparer());
                int distcont=distinct.Count();
                foreach(Item item in distinct)
                {
                    Table queryResult = this.Where(
                        new Func<Row, bool>(
                            row => row.items[groupBy].value == item.value));
                    Row newrow = new Row();
                    newrow.AddItem(new Item() { value = item.value });
                    var avg1 = queryResult.rows.Average(
                        new Func<Row, decimal?>(
                            row => 
                                double.IsNaN(row.items[agregatedcolumnindex].getDoubleValue()) ? 
                                null : 
                                (decimal?)Convert.ToDecimal(row.items[agregatedcolumnindex].getDoubleValue())));
                    newrow.AddItem(new Item() { value = avg1.ToString() });
                    average.AddRow(newrow);
                }
                return average;

            }
            else
            {
                groupBy = -1;

                Table average = new Table("",new List<Column>(){
                    new Column(resultingColName)});
                Row newrow = new Row();
                var avg1 = this.rows.Average(
                        new Func<Row, decimal?>(
                            row =>
                                double.IsNaN(row.items[agregatedcolumnindex].getDoubleValue()) ?
                                null :
                                (decimal?)Convert.ToDecimal(row.items[agregatedcolumnindex].getDoubleValue())));
                newrow.AddItem(new Item() { value = avg1.ToString() });
                average.AddRow(newrow);
                return average;
            }

        }
        public Table changeAttrColumnwise(string columnName, Func<Item, Item> change, string newColumnName = null)
        {
            return changeAttrColumnwise(getColumnIndexFromName(columnName), change,newColumnName);
        }
        public Table changeAttrColumnwise(int columnIndex,Func<Item,Item> change,string newColumnName=null)
        {
            if (newColumnName != null)
            {
                int newindex=AddColumn(newColumnName);
                foreach (Item item in columns[columnIndex].items)
                {
                    Row row = item.row;
                    Item newItem = change(item);
                    columns[newindex].AddItem(newItem);
                    row.AddItem(newItem);
                }
            }
            else
            {
                foreach (Item item in columns[columnIndex].items)
                {
                    Row row = item.row;
                    Item newItem = change(item);
                    item.value = newItem.value;
                }
            }
            return this;
        }
        public Table operation(Func<Row,string> op,int existingColumnIndex=-1,string newColumnName=null)
        {
            if(existingColumnIndex>=0)
            {
                foreach(Row row in rows)
                {
                    string result=op(row);
                    row.items[existingColumnIndex].value = result;
                }
            }
            else if(newColumnName!=null)
            {
                int newColumnIndex=AddColumn(newColumnName);
                foreach (Row row in rows)
                {
                    string result = op(row);
                    Item item=row.AddItem(new Item() { value = result });
                    columns[newColumnIndex].AddItem(item);
                }
            }
            else
            {
                throw new Exception("No column for storing result given!");
            }
            return this;
        }
        public Table Join(Table other,int joinIndex1,int joinIndex2)
        {
            List<Column> resultingColumns = new List<Column>();
            resultingColumns.Add(new Column(columns[joinIndex1].name));
            for(int i=0;i<columns.Count;i++)
            {
                if(i!=joinIndex1)
                {
                    resultingColumns.Add(new Column(columns[i].name));
                }
            }
            for (int i = 0; i < other.columns.Count; i++)
            {
                if (i != joinIndex2)
                {
                    resultingColumns.Add(new Column(other.columns[i].name));
                }
            }
            Table result = new Table("", resultingColumns);
            foreach(Row row1 in rows)
            {
                foreach(Row row2 in other.rows)
                {
                    if(row1.items[joinIndex1].value==row2.items[joinIndex2].value)
                    {
                        Row newrow = new Row();
                        newrow.AddItem(new Item() { value = row1.items[joinIndex1].value });
                        for(int i=0;i<columns.Count;i++)
                        {
                            if (i != joinIndex1)
                            {
                                newrow.AddItem(new Item() { value=row1.items[i].value }); 
                            }
                        }
                        for (int i = 0; i < other.columns.Count; i++)
                        {
                            if (i != joinIndex2)
                            {
                                newrow.AddItem(new Item() { value = row2.items[i].value });
                            }
                        }
                        result.AddRow(newrow);
                    }
                }
            }
            return result;
        }
        public int getColumnIndexFromName(string s)
        {
            for(int i=0;i<columns.Count;i++)
            {
                if(columns[i].name==s)
                {
                    return i;
                }
            }
            throw new Exception("No column with that name!");
        }
        public int this[string key]
        {
            get
            {
                return getColumnIndexFromName(key);
            }
        }
        public Table Select(List<string> selectedColumns)
        {
            List<int> indexes = new List<int>();
            foreach(string name in selectedColumns)
            {
                indexes.Add(getColumnIndexFromName(name));
            }
            return Select(indexes);
        }
        public Table Select(List<int> selectedColumnIndexes)
        {
            Table queryResult = new Table("",
                new List<Column>());
            List<Column> selectedColumns = new List<Column>();
            foreach(int index in selectedColumnIndexes)
            {
                queryResult.AddColumn(columns[index].name);
            }
            foreach (Row row in rows)
            {
                Row newRow = new Row();
                foreach(int index in selectedColumnIndexes)
                {
                    newRow.AddItem(row.items[index]);
                }
                queryResult.AddRow(newRow);
            }
            return queryResult;
        }
        public string show(TableWriter writer)
        {
            return writer.get(this);
        }
        public void fixDoubleFormat(string format)
        {
            foreach(Row row in rows)
            {
                foreach(Item item in row.items)
                {
                    item.fixDoubleOutput(format);
                }
            }
        }
    }
}
