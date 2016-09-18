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
            
            Storage regex = new Storage("([a-zA-Z0-9_]+)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\r\n([0-9]+|NaN)\\s*",
                new List<RegexDB.RegexDataExtractor.Column>(){
                    new RegexDB.RegexDataExtractor.Column("name"),
                    new RegexDB.RegexDataExtractor.Column("GA"),
                    new RegexDB.RegexDataExtractor.Column("ILS"),
                    new RegexDB.RegexDataExtractor.Column("GAI"),
                    new RegexDB.RegexDataExtractor.Column("MEM")
                });
            StreamReader file = new StreamReader(@"C:\Users\master\Downloads\CFLP GA\CFLP GA\bin\Debug\0_short_results.txt");
            string lines = file.ReadToEnd();
            regex.ExtractFromString(lines);
            file = new StreamReader(@"C:\Users\master\Downloads\CFLP GA\CFLP GA\bin\Debug\1_short_results.txt");
            lines = file.ReadToEnd();
            regex.ExtractFromString(lines);
            regex.show(listView1);
            file.Dispose();
        }
    }
}
