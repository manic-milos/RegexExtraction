using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Item
    {
        public string value { 
            get; 
            set; 
        } 
        public Row row=null;
        public Column column=null;
    }
}
