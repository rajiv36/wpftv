using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
namespace NDTV.Entities
{
    public class FallOfWickets
    {
        private FallOfWickets fallOfWicket;

        /// <summary>
        /// Constructor
        /// </summary>
        public FallOfWickets()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element">response to be parsed</param>
        public FallOfWickets(XElement element)
        {
            ListOfWickets = new SortedList<int, FallOfWickets>();
            Parse(element);
        }

        /// <summary>
        /// parse the response and fill the appropriate model
        /// </summary>
        /// <param name="element">XElement</param>
        private void Parse(XElement element)
        {
            string wicket = string.Empty;
            if (null != element.Attribute("First") || false == string.IsNullOrEmpty(element.Attribute("First").Value))
            {
                wicket = element.Attribute("First").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 1);
            }
            if (null != element.Attribute("Second") || false == string.IsNullOrEmpty(element.Attribute("Second").Value))
            {
                wicket = element.Attribute("Second").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 2);
            }
            if (null != element.Attribute("Third") || false == string.IsNullOrEmpty(element.Attribute("Third").Value))
            {
                wicket = element.Attribute("Third").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 3);
            }
            if (null != element.Attribute("Fourth") || false == string.IsNullOrEmpty(element.Attribute("Fourth").Value))
            {
                wicket = element.Attribute("Fourth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 4);
            }
            if (null != element.Attribute("Fifth") || false == string.IsNullOrEmpty(element.Attribute("Fifth").Value))
            {
                wicket = element.Attribute("Fifth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 5);
            }
            if (null != element.Attribute("Sixth") || false == string.IsNullOrEmpty(element.Attribute("Sixth").Value))
            {
                wicket = element.Attribute("Sixth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 6);
            }
            if (null != element.Attribute("Seventh") || false == string.IsNullOrEmpty(element.Attribute("Seventh").Value))
            {
                wicket = element.Attribute("Seventh").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 7);
            }
            if (null != element.Attribute("Eighth") || false == string.IsNullOrEmpty(element.Attribute("Eighth").Value))
            {
                wicket = element.Attribute("Eighth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 8);
            }
            if (null != element.Attribute("Ninth") || false == string.IsNullOrEmpty(element.Attribute("Ninth").Value))
            {
                wicket = element.Attribute("Ninth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 9);
            }
            if (null != element.Attribute("Tenth") || false == string.IsNullOrEmpty(element.Attribute("Tenth").Value))
            {
                wicket = element.Attribute("Tenth").Value.ToString(CultureInfo.InvariantCulture);
                SplitWicketData(wicket, 10);
            }
        }

        /// <summary>
        /// Split fall of wicket data
        /// </summary>
        /// <param name="wicket">string</param>
        /// <param name="order">order at which wicket has fallen</param>
        private void SplitWicketData(string wicket, int order)
        {
            string[] wicketInfo = null;
            if (false == string.IsNullOrWhiteSpace(wicket) && wicket.Contains("|"))
            {
                wicketInfo = wicket.Split('|');
                fallOfWicket = new FallOfWickets();
                fallOfWicket.Order = order;
                if (null != wicketInfo[1])
                {
                    fallOfWicket.BatsmanName = wicketInfo[1].ToString(CultureInfo.InvariantCulture);
                }
                if (null != wicketInfo[2])
                {
                    fallOfWicket.Run = wicketInfo[2].ToString(CultureInfo.InvariantCulture);
                }
                if (null != wicketInfo[3])
                {
                    fallOfWicket.Over = wicketInfo[3].ToString(CultureInfo.InvariantCulture);
                }
                ListOfWickets.Add(order, fallOfWicket);
            }
            fallOfWicket = null;
        }

        /// <summary>
        /// List of wickets
        /// </summary>
        public SortedList<int, FallOfWickets> ListOfWickets
        {
            get;
            private set;
        }

        /// <summary>
        /// Order of fall of wicket
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// Runs at which the wicket fell
        /// </summary>
        public string Run
        {
            get;
            set;
        }

        /// <summary>
        /// Overs statistics at which wicket fell
        /// </summary>
        public string Over
        {
            get;
            set;
        }

        /// <summary>
        /// Name of the batsman
        /// </summary>
        public string BatsmanName
        {
            get;
            set;
        }
                
        /// <summary>
        /// Formatting the input to display like fall of wicket
        /// </summary>
        /// <returns>string format - fall of wickets</returns>
        public override string ToString()
        {
            StringBuilder stringFormat = new StringBuilder();
            if (ListOfWickets.Count > 0)
            {
                foreach (KeyValuePair<int, FallOfWickets> item in ListOfWickets)
                {
                    stringFormat.Append(item.Key.ToString(CultureInfo.InvariantCulture));
                    stringFormat.Append(Constants.Constant.Hyphen);
                    if (false == string.IsNullOrWhiteSpace(item.Value.Run))
                    {
                        stringFormat.Append(item.Value.Run);
                    }
                    if (false == string.IsNullOrWhiteSpace(item.Value.BatsmanName))
                    {
                        stringFormat.Append(Constants.Constant.Space + Constants.Constant.OpeningBrace + item.Value.BatsmanName + Constants.Constant.CommaWithSpace);
                    }
                    
                    if (false == string.IsNullOrWhiteSpace(item.Value.Over))
                    {
                        stringFormat.Append(item.Value.Over + Constants.Constant.Space + "ov)," + Constants.Constant.Space);
                    }
                }
                stringFormat.Remove(stringFormat.Length - 2, 2);
                return stringFormat.ToString();
            }
            return string.Empty;
        }
    }
}
