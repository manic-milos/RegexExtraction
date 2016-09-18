using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Column
    {
        public List<Item> items=new List<Item>();
        public string name { get; set; }
        public Column(string name)
        {
            this.name = name;
        }
        public Item AddItem(Item item)
        {
            item.column = this;
            //TODO ? provera koji je po redu;
            items.Add(item);
            return item;
        }

    }
}
