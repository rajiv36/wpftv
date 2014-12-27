using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// every property required for the ProfitIndex class would be mentioned here..
    /// all the properties bound in the ui should be present in this ProfitIndex class hierarchically..
    /// </summary>
    public class ProfitIndex
    {
    }

    /// <summary>
    /// ProfitIndex Item Collection which represents the item array which can be directly bound to any collection accepting control
    /// </summary>
    public class ProfitIndexCollection : System.Collections.ObjectModel.Collection<ProfitIndexItem>
    {

    }

    /// <summary>
    /// All the properties required for a ProfitIndexItem is found here
    /// </summary>
    public class ProfitIndexItem
    {
        public int Id { get; set; }
        public string Graph { get; set; }
        public string High { get; set; }
        public string Exchange { get; set; }
        public string PercentageChange { get; set; }
        public string Low { get; set; }
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string Open { get; set; }
        public string Close { get; set; }
    }
}
