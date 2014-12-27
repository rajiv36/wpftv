using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDTV.Entities
{
    /// <summary>
    /// every property required for the Temperature class would be mentioned here..
    /// all the properties bound in the ui should be present in this Temperature class hierarchically..
    /// </summary>
    public class Temperature
    {
    }

    /// <summary>
    /// Temperature Item Collection which represents the item array which can be directly bound to any collection accepting control
    /// </summary>
    public class TemperatureItemCollection : System.Collections.ObjectModel.Collection<TemperatureItem>
    {

    }

    /// <summary>
    /// All the properties required for a TemperatureItem is found here
    /// </summary>
    public class TemperatureItem
    {
    }
}
