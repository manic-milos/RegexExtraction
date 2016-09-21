using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class InstanceLatexWriter: TableWriter
    {
        string m_pattern =
                @"$/name/$& $/n/$ & $/m/$ & $/k/$\\ "+Environment.NewLine;
        string m_header = @"\begin{longtabu} to 1\textwidth{|  X[2] | X  X  X  }"
            + @"\caption{Instance malih dimenzija.\label{instancetable}}\\ "
            + @"Instanca & n & m &k \\\hline\endfirsthead "
            + @" \endfoot "
            + @"Instanca & n & m &k \\\hline\endhead\hline\endlastfoot "
            ;
        string m_footer = @"\end{longtabu}";
        public override string header
        {
            get
            {
                return m_header;
            }
        }
        public override string footer
        {
            get
            {
                return m_footer;
            }
        }
        public override string pattern
        {
            get
            {
                return m_pattern;
            }
        }
    }
}
