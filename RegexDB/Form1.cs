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

        private void button2_Click(object sender, EventArgs e)
        {
            string regexString = "([a-zA-Z0-9_]+)\\s+n=([0-9]+), m=([0-9]+), k=([0-9]+)\\s+"
                + "GA:"
                + "\\s+(\\[[01]+\\])\\s+"
                + "TestOnData:iterations to result=\\s+([0-9]+/?[0-9]*)\\s+"
                + "TestOnData:,time to best result=\\s+([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"
                + "TestOnData:,number of generations=\\s+([0-9]+)\\s+"
                + "TestOnData:,calls to eval=\\s+([0-9]+)\\s+"
                + "TestOnData:,cache hits=\\s+([0-9]+)\\s+"
                + "([0-9]+)\\s+"
                + "([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"

                + "ILS:"
                + "\\s+(\\[[01]+\\])\\s+"
                + "TestOnData:iterations to result=\\s+([0-9]+/?[0-9]*);*\\s+"
                + "TestOnData:,time to best result=\\s+([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"
                + "TestOnData:,calls to eval=\\s+([0-9]+)\\s+"
                + "TestOnData:,cache hits=\\s+([0-9]+)\\s+"
                + "([0-9]+)\\s+"
                + "([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"

                + "GAA:\\s+"
                + "GAAdvanced:[0-9]+\\s+"
                + "\\s+(\\[[01]+\\])\\s+"
                + "TestOnData:iterations to result=\\s+([0-9]+|ils)\\s+"
                + "TestOnData:,time to best result=\\s+([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"
                + "TestOnData:,number of generations=\\s+([0-9]+)\\s+"
                + "TestOnData:,calls to eval=\\s+([0-9]+)\\s+"
                + "TestOnData:,cache hits=\\s+([0-9]+)\\s+"
                + "([0-9]+)\\s+"
                + "([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"

                + "Mem:\\s+"
                + "Memetic:[0-9]+\\s+"
                + "\\s+(\\[[01]+\\])\\s+"
                + "TestOnData:iterations to result=\\s+([0-9]+/?[0-9]*)\\s+"
                + "TestOnData:,time to best result=\\s+([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"
                + "TestOnData:,number of generations=\\s+([0-9]+)\\s+"
                + "TestOnData:,calls to eval=\\s+([0-9]+)\\s+"
                + "TestOnData:,cache hits=\\s+([0-9]+)\\s+"
                + "([0-9]+)\\s+"
                + "([0-9]{2}:[0-9]{2}:[0-9]{2}\\.[0-9]*)\\s+"
                ;
            RegexExtractor reg = new RegexExtractor(regexString);
            Table regex = new Table(regexString, new List<Column>(){
                new Column("name"),
                new Column("n"),
                new Column("m"),
                new Column("k"),

                new Column("GA value"),
                new Column("GA iterationsToBest"),
                new Column("GA timeToBest"),
                new Column("GA generations"),
                new Column("GA eval calls"),
                new Column("GA cache hits"),
                new Column("GA result"),
                new Column("GA time"),

                new Column("ILS value"),
                new Column("ILS iterations to best"),
                new Column("ILS timeToBest"),
                new Column("ILS eval calls"),
                new Column("ILS cache hits"),
                new Column("ILS result"),
                new Column("ILS time"),

                new Column("GAA value"),
                new Column("GAA iterationsToBest"),
                new Column("GAA timeToBest"),
                new Column("GAA generations"),
                new Column("GAA eval calls"),
                new Column("GAA cache hits"),
                new Column("GAA result"),
                new Column("GAA time"),

                new Column("Mem value"),
                new Column("Mem iterationsToBest"),
                new Column("Mem timeToBest"),
                new Column("Mem generations"),
                new Column("Mem eval calls"),
                new Column("Mem cache hits"),
                new Column("Mem result"),
                new Column("Mem time")
            });
            for (int i = 0; i < 21; i++)
            {
                StreamReader file1 = new StreamReader(@"C:\Users\master\Downloads\CFLP GA\CFLP GA\bin\Debug\"+i+"_debug_log.txt");
                string lines1 = file1.ReadToEnd();
                //int charnum = 0;
                //reg.extractFromString(lines1, out charnum);
                regex.ExtractFromString(lines1);
                file1.Dispose();
            }

            //extract bestsol
            StreamReader fileBestsol = new StreamReader(@"C:\Users\master\Documents\Visual Studio 2013\Projects\RegexDB\bestsol.txt.txt");
            Table bestSol = new Table("([a-zA-Z0-9_]+)\\s+([0-9]+)\\s+", new List<Column>()
                {
                    new Column("name"),
                    new Column("bestsol")
                });
            bestSol.ExtractFromString(fileBestsol.ReadToEnd());
            fileBestsol.Dispose();

            //join best results with table
            regex = regex.Join(bestSol, 0, 0);

            //substract for error
            regex = regex.operation(new Func<Row, string>(
                row => Math.Abs(row.items[regex["GA result"]].getDoubleValue() - row.items[regex["bestsol"]].getDoubleValue()).ToString()), -1, "GAagap");
            regex = regex.operation(new Func<Row, string>(
                row => Math.Abs(row.items[regex["ILS result"]].getDoubleValue() - row.items[regex["bestsol"]].getDoubleValue()).ToString()), -1, "ILSagap");
            regex = regex.operation(new Func<Row, string>(
                row => Math.Abs(row.items[regex["GAA result"]].getDoubleValue() - row.items[regex["bestsol"]].getDoubleValue()).ToString()), -1, "GAAagap");
            regex = regex.operation(new Func<Row, string>(
                row => Math.Abs(row.items[regex["Mem result"]].getDoubleValue() - row.items[regex["bestsol"]].getDoubleValue()).ToString()), -1, "MEMagap");

            //make percentage
            regex = regex.operation(new Func<Row, string>(
                row => (row.items[regex["GAagap"]].getDoubleValue() / row.items[regex["bestsol"]].getDoubleValue()).ToString()
                ), regex["GAagap"]);
            regex = regex.operation(new Func<Row, string>(
                row => (row.items[regex["ILSagap"]].getDoubleValue() / row.items[regex["bestsol"]].getDoubleValue()).ToString()
                ), regex["ILSagap"]);
            regex = regex.operation(new Func<Row, string>(
                row => (row.items[regex["GAAagap"]].getDoubleValue() / row.items[regex["bestsol"]].getDoubleValue()).ToString()
                ), regex["GAAagap"]);
            regex = regex.operation(new Func<Row, string>(
                row => (row.items[regex["MEMagap"]].getDoubleValue() / row.items[regex["bestsol"]].getDoubleValue()).ToString()
                ), regex["MEMagap"]);


            //var sol1 = regex.Average(0, "bestsol", "bestbest");
            //aggregate
            var sol = regex.Average(0, "GA result", "GAsol").Join(regex.Average(0, "ILS result", "ILSsol"), 0, 0);
            sol = sol.Join(regex.Average(0, "GAA result", "GAAsol"), 0, 0).Join(regex.Average(0, "Mem result", "MEMsol"), 0, 0);

            var t = regex.Average(0, "GA timeToBest", "GAt").Join(regex.Average(0,"ILS timeToBest","ILSt"),0,0);
            t = t.Join(regex.Average(0, "GAA timeToBest", "GAAt"), 0, 0).Join(regex.Average(0,"Mem timeToBest","MEMt"),0,0);

            var ttot = regex.Average(0, "GA time", "GAttot").Join(regex.Average(0, "ILS time", "ILSttot"), 0, 0);
            ttot = ttot.Join(regex.Average(0, "GAA time", "GAAttot"), 0, 0).Join(regex.Average(0, "Mem time", "MEMttot"), 0, 0);

            var gen = regex.Average(0, "GA iterationsToBest", "GAgen").Join(regex.Average(0, "ILS iterations to best", "ILSiter"), 0, 0);
            gen = gen.Join(regex.Average(0, "GAA iterationsToBest", "GAAgen"), 0, 0).Join(regex.Average(0, "Mem iterationsToBest", "MEMgen"), 0, 0);

            var eval = regex.Average(0, "GA eval calls", "GAeval").Join(regex.Average(0, "ILS eval calls", "ILSeval"), 0, 0);
            eval = eval.Join(regex.Average(0, "GAA eval calls", "GAAeval"), 0, 0).Join(regex.Average(0, "Mem eval calls", "MEMeval"), 0, 0);

            var cache = regex.Average(0, "GA cache hits", "GAcache").Join(regex.Average(0, "ILS cache hits", "ILScache"), 0, 0);
            cache = cache.Join(regex.Average(0, "GAA cache hits", "GAAcache"), 0, 0).Join(regex.Average(0, "Mem cache hits", "MEMcache"), 0, 0);

            var agap = regex.Average(0, "GAagap", "GAaagap").Join(regex.Average(0, "ILSagap", "ILSaagap"), 0, 0);
            agap = agap.Join(regex.Average(0, "GAAagap", "GAAaagap"), 0, 0).Join(regex.Average(0, "MEMagap", "MEMaagap"), 0, 0);

            var results=sol.Join(t,0,0).Join(ttot,0,0).Join(gen,0,0).Join(eval,0,0).Join(cache,0,0).Join(agap,0,0);

            var sigma=regex.Select(new List<string>(){"name","GAagap","ILSagap","GAAagap","MEMagap"}).Join(agap, 0, 0);
            sigma = sigma.operation(new Func<Row, string>(
                row => Math.Pow(row.items[sigma["GAagap"]].getDoubleValue() - row.items[sigma["GAaagap"]].getDoubleValue(), 2).ToString()
                ),-1,"GAsigma");
            sigma = sigma.operation(new Func<Row, string>(
                row => Math.Pow(row.items[sigma["ILSagap"]].getDoubleValue() - row.items[sigma["ILSaagap"]].getDoubleValue(), 2).ToString()
                ), -1, "ILSsigma");
            sigma = sigma.operation(new Func<Row, string>(
                row => Math.Pow(row.items[sigma["GAAagap"]].getDoubleValue() - row.items[sigma["GAAaagap"]].getDoubleValue(), 2).ToString()
                ), -1, "GAAsigma");
            sigma = sigma.operation(new Func<Row, string>(
                row => Math.Pow(row.items[sigma["MEMagap"]].getDoubleValue() - row.items[sigma["MEMaagap"]].getDoubleValue(), 2).ToString()
                ), -1, "MEMsigma");
            var asigma = sigma.Average(0, "GAsigma", "GAsigma").Join(sigma.Average(0, "ILSsigma", "ILSsigma"), 0, 0);
            asigma=asigma.Join(sigma.Average(0,"GAAsigma","GAAsigma"),0,0).Join(sigma.Average(0,"MEMsigma","MEMsigma"),0,0);
            asigma = asigma.operation(new Func<Row, string>(
                row => (Math.Sqrt(row.items[asigma["GAsigma"]].getDoubleValue())*100).ToString()
                ),asigma["GAsigma"]);
            asigma = asigma.operation(new Func<Row, string>(
               row => (Math.Sqrt(row.items[asigma["ILSsigma"]].getDoubleValue())*100).ToString()
               ),asigma["GAsigma"]);
            asigma = asigma.operation(new Func<Row, string>(
               row => (Math.Sqrt(row.items[asigma["GAAsigma"]].getDoubleValue())*100).ToString()
               ), asigma["GAsigma"]);
            asigma = asigma.operation(new Func<Row, string>(
               row => (Math.Sqrt(row.items[asigma["MEMsigma"]].getDoubleValue())*100).ToString()
               ), asigma["GAsigma"]);
            results = results.Join(asigma, 0, 0);
            results = results.operation(new Func<Row, string>(
                row => (row.items[results["GAcache"]].getDoubleValue() / row.items[results["GAeval"]].getDoubleValue()) * 100 + ""
                ),-1,"GAcachepercent");
            results = results.operation(new Func<Row, string>(
                row => (row.items[results["ILScache"]].getDoubleValue() / row.items[results["ILSeval"]].getDoubleValue()) * 100 + ""
                ), -1, "ILScachepercent");
            results = results.operation(new Func<Row, string>(
                row => (row.items[results["GAAcache"]].getDoubleValue() / row.items[results["GAAeval"]].getDoubleValue()) * 100 + ""
                ), -1, "GAAcachepercent");
            results = results.operation(new Func<Row, string>(
                row => (row.items[results["MEMcache"]].getDoubleValue() / row.items[results["MEMeval"]].getDoubleValue()) * 100 + ""
                ), -1, "MEMcachepercent");
            results=results.Join(bestSol, 0, 0);
            regex.show(listView1);
            results.operation(new Func<Row, string>(row =>
                (row.items[results["GAaagap"]].getDoubleValue() * 100).ToString()), results["GAaagap"]);
            results.operation(new Func<Row, string>(row =>
                (row.items[results["ILSaagap"]].getDoubleValue() * 100).ToString()), results["ILSaagap"]);
            results.operation(new Func<Row, string>(row =>
                (row.items[results["GAAaagap"]].getDoubleValue() * 100).ToString()), results["GAAaagap"]);
            results.operation(new Func<Row, string>(row =>
                (row.items[results["MEMaagap"]].getDoubleValue() * 100).ToString()), results["MEMaagap"]);
            results.fixDoubleFormat("F2");
            results.show(listView2);
            SveLatexWriter latexWriter = new SveLatexWriter(results);
            string latexshow = results.show(latexWriter);
            richTextBox1.Text = latexshow;
            //var instancesTable = regex.Average(0, "n","n").Join(regex.Average(0,"m","m"),0,0).Join(regex.Average(0,"k","k"),0,0);
            //int ind = regex.getColumnIndexFromName("bestsol");
            //var large = instancesTable.Where(new Func<Row, bool>(row => row.items[0].value.Contains("capa") ||
            //    row.items[0].value.Contains("capb")));
            //var medium=instancesTable.Where(new Func<Row, bool>(row => row.items[0].value.Contains("pn")));
            //var small = instancesTable.Where(new Func<Row, bool>(row => row.items[0].value.Contains("pn") ? false : true));
            //small = small.Where(new Func<Row, bool>(row => row.items[0].value.Contains("capa") ? false : true));
            //small = small.Where(new Func<Row, bool>(row => row.items[0].value.Contains("capb") ? false : true));
            //richTextBox1.Text = large.show(new InstanceLatexWriter());
        }
    }
}
