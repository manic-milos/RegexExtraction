using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class Item:IEquatable<Item>
    {
        public enum TYPE
        {
            STRING,
            INT,
            FLOAT
        }
        private TYPE m_type = TYPE.STRING;
        public TYPE type
        {
            get
            {
                return m_type;
            }
            set
            {
                m_type = value;
            }
        }
        public string value
        {
            get;
            set;
        }
        //TODO nasledjeni kako treba
        public Row row = null;
        public Column column = null;
        public int getInt32Value()
        {
            return int.Parse(value.Trim());
        }
        public long getInt64Value()
        {
            return long.Parse(value.Trim());
        }
        public double getDoubleValue()
        {
            return double.Parse(value.Trim());
        }
        public decimal getDecimalValue()
        {
            return decimal.Parse(value.Trim());
        }


        public bool Equals(Item other)
        {
            if (this.value == other.value)
                return true;
            return false;
        }
        public class comparer:EqualityComparer<Item>
        {

            public override bool Equals(Item x, Item y)
            {
                return x.Equals(y);
            }

            public override int GetHashCode(Item obj)
            {
                return obj.value.GetHashCode();
            }
        }
        
    }
}
