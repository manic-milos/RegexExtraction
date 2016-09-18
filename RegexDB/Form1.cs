using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RegexDB.RegexDataExtractor;


namespace RegexDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Table regex = new Table("([a-zA-Z0-9_]+)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\\s*",
                new List<RegexDB.RegexDataExtractor.Column>(){
                    new RegexDB.RegexDataExtractor.Column("name"),
                    new RegexDB.RegexDataExtractor.Column("GA"),
                    new RegexDB.RegexDataExtractor.Column("ILS"),
                    new RegexDB.RegexDataExtractor.Column("GAI"),
                    new RegexDB.RegexDataExtractor.Column("MEM")
                });
            for (int i = 0; i < 21; i++)
            {
                StreamReader file = new StreamReader(@"C:\Users\master\Downloads\CFLP GA\CFLP GA\bin\Debug\"+i+"_short_results.txt");
                string lines = file.ReadToEnd();
                regex.ExtractFromString(lines);
                file.Dispose();
            }
            //regex.show(listView1);
            Table agregated = regex.CloneColumns();
            for (int i = 0; i < 64; i++)
            {
                Table queryResult = regex.Where(new Func<Row, bool>(row => row.items[0].value == regex.columns[0].items[i].value));
                Row newrow = new Row();
                newrow.AddItem(new Item(){value=regex.columns[0].items[i].value});
                for (int j = 1; j < 5; j++)
                {
                    var avg1 = queryResult.rows.Average(
                        new Func<Row, decimal?>(row => double.IsNaN(row.items[j].getDoubleValue()) ? null : (decimal?)Convert.ToDecimal(row.items[j].getDoubleValue())));
                    newrow.AddItem(new Item() { value = avg1.ToString() });
                    
                }
                agregated.AddRow(newrow);
                
            }
            agregated.show(listView1); 
            var averagega = regex.Average(0, 1, "GA");
            var averageils = regex.Average(0, 2, "ILS");
            var averagegai = regex.Average(0, 3, "GAI");
            var averagemem = regex.Average(0, 4, "MEM");
            var averages = averagega.Join(averageils, 0, 0).Join(averagegai,0,0).Join(averagemem,0,0);
            averages.show(listView2);
        }
    }
}
