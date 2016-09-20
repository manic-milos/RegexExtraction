using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexDB.RegexDataExtractor
{
    class LaTexWriter : TableWriter
    {
        Table table;
        string mm_header = @"\begin{table}\hskip-4.0cm" +
                @"\begin{tabular}[c]{ c | c | c | c  c  c  c  c  c  c  c  c |}" +
                 @"Instanca  & metoda & $sol$ & $t_{tot}[s]$ & $t_{best}[s]$ & $gen$ & $eval$ &$caching$&  $agap[\%]$ & $\sigma$&$cache[\%]$ \\\hline";
        string m_pattern =
                @"\multirow{5}{*}{$/name/$} & $CPLEX$ & /bestsol/ & \multicolumn{8}{ c }{-}\\\nopagebreak " +
                @"&$GA$ & /GAsol/ & /GAttot/ & /GAt/ & /GAgen/ & /GAeval/ & /GAcache/ &/GAaagap/ & /GAsigma/ & /GAcachepercent/\\\nopagebreak " +
                @"&$ILS$ & /ILSsol/ & /ILSttot/ & /ILSt/ & - & /ILSeval/ & /ILScache/ &/ILSaagap/ & /ILSsigma/ & /ILScachepercent/\\\nopagebreak " +
                @"&$GA+ILS$ & /GAAsol/ & /GAAttot/ & /GAAt/ & /GAAgen/ & /GAAeval/ & /GAAcache/ &/GAAaagap/ & /GAAsigma/ & /GAAcachepercent/\\\nopagebreak " +
                @"&$Mem$ & /MEMsol/ & /MEMttot/ & /MEMt/ & /MEMgen/ & /MEMeval/ & /MEMcache/ &/MEMaagap/ & /MEMsigma/ & /MEMcachepercent/\\\hline\pagebreak[0]"
                ;
        string mm_footer = @"\end{tabular}" +
                @"\end{table}";
        string m_header = @"\begin{longtabu} to 1.3\textwidth {| X[1] | X[1.1] | X[2] | X[0.8]  X [0.6] X[0.6]  X[1.5]  X[1.5]  X[0.8] X [0.5] X[0.5]}"
                            + @"\caption{Eksperimentalni rezultati.\label{long}}\\"

                             + @"Instanca  & metoda & $sol$ & $t_{tot}[s]$ & $t_{best}[s]$ & $gen$ & $eval$ &$caching$&  $agap[\%]$ & $\sigma$&$cache$ \\ \hline"
                             + @"\endfirsthead\hline\endfoot "
                             + @"Instanca  & metoda & $sol$ & $t_{tot}[s]$ & $t_{best}[s]$ & $gen$ & $eval$ &$caching$&  $agap[\%]$ & $\sigma$&$cache$ \\ \hline"
                             + @"\endhead\hline"
                             + @"\hline\endlastfoot"
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
        public LaTexWriter(Table table)
        {
            this.table = table;
        }

    }
}
