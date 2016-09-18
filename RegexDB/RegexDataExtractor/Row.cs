using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Row
    {
        List<Item> items = new List<Item>();
        public Item AddItem(Item item)
        {
            item.row = this;
            //TODO ? provera koji je po redu;
            items.Add(item);
            return item;
        }
        public string[] ToStringArray()
        {
            string[] result = new string[items.Count];
            for(int i=0;i<result.Length;i++)
            {
                result[i] = items[i].value;
            }
            return result;
        }
    }
}
