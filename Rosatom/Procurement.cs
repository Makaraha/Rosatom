using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosatom
{
    public class Procurement
    {
        public class Short : IEquatable<Short>
        {
            public string Url { get; set; }
            public DateTime? AcceptingApplicationsDeadline { get; set; }
            public DateTime? SummarizingDate { get; set; }

            public bool Equals(Short other)
            {
                return Url == other.Url;
            }
        }
    }
}
