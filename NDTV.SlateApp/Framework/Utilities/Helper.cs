using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using NDTV.Entities;

namespace NDTV.Utilities
{
    /// <summary>
    /// Similar to Utility class where more generic helper modules fall in
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// A class used to remove all the html tags and trim until the first paragraph or line break..
        /// </summary>
        /// <param name="html">html string that should be formatted</param>
        /// <returns>string without html tags</returns>
        public static string RemoveHtmlTags(string html)
        {
            if (Regex.IsMatch(html, @"<br\s*/?>")) // regular expresion to check for "BREAK html tag"
            {
                Match match = Regex.Match(html, @"\s*(?<data>\S.*?)<br\s*/?>");//captures DATA until the first BREAK
                html = match.Value;
            }
            else
                if (Regex.IsMatch(html, @"<\s*/p\s*>")) //regular expression to chcek for "Paragraph html tag"
                {
                    Match match = Regex.Match(html, @"\s*(?<data>\S.*?)<\s*/p\s*>");//captures DATA until the first PARAGRAPH
                    html = match.Value;
                }
            return Regex.Replace(html, "</?.[^<]*>", matchDataReturn);//replace any html tag with string.empty
        }

        /// <summary>
        /// Callback template which nullifies the html tag
        /// </summary>
        /// <param name="mtch">the match value</param>
        /// <returns>returns empty string</returns>
        private static string matchDataReturn(Match mtch)
        {
            return string.Empty;
        }

        /// <summary>
        /// A converter which formats the data on the new control(METHOD not used as of now)
        /// If any error is present then the first html node element is read in the catch block which gets converetd to sectional xaml.
        /// </summary>
        /// <param name="html">html data</param>
        /// <returns>xaml section data</returns>
        public static string HtmlToXamlConverter(string html)
        {
            XmlTextReader saxParser = new XmlTextReader(string.Format(CultureInfo.CurrentCulture,"<html>{0}</html>", html), XmlNodeType.Element, null);
            saxParser.XmlResolver = null;
            StringBuilder xamlBuilder = new System.Text.StringBuilder();
            try
            {
                while (saxParser.Read())
                {
                    retrieveXamlforHtml(saxParser, xamlBuilder);
                }
            }
            catch (Exception e)
            {
                // Regex rgx = new Regex(@"<.>\s*(?<required>[^<]+)\s*</.>"); use this incase if we have to filter the html data until the first html node..
                xamlBuilder = new StringBuilder();
                XmlDocument domParser = new XmlDocument();
                domParser.XmlResolver = null;
                domParser.LoadXml(string.Format(CultureInfo.CurrentCulture,"<html>{0}</html>", html));
                Helper.RetrieveData(HtmlTag.Html, false, xamlBuilder);
                xamlBuilder.Append(domParser.InnerText);
                Helper.RetrieveData(HtmlTag.Html, true, xamlBuilder);
                throw e;
            }
            return xamlBuilder.ToString();
        }

        /// <summary>
        /// Converts the node into a specific XAML Tag
        /// </summary>
        /// <param name="xmlText">SAX XML Parser</param>
        /// <param name="xamlBuilder">String builder used to construct the Section representation</param>
        private static void retrieveXamlforHtml(XmlTextReader xmlText, StringBuilder xamlBuilder)
        {
            switch (xmlText.NodeType)
            {
                case XmlNodeType.Element:
                    Helper.RetrieveData(Helper.ConvertHtmlNodeToTag(xmlText.Name), false, xamlBuilder);
                    break;
                case XmlNodeType.Text:
                    xamlBuilder.Append(xmlText.Value.ToString());
                    break;
                case XmlNodeType.EndElement:
                    Helper.RetrieveData(Helper.ConvertHtmlNodeToTag(xmlText.Name), true, xamlBuilder);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// The method which build the XAML Stream..
        /// </summary>
        /// <param name="tag">HTML Tag</param>
        /// <param name="isEndElement">indicates if the node obtained is the start or end element</param>
        /// <param name="xamlBuilder">Output Stream</param>
        /// <returns>stringbuilder which builds the xaml section</returns>
        private static StringBuilder RetrieveData(HtmlTag tag, bool isEndElement, StringBuilder xamlBuilder)
        {
            switch (tag)
            {
                case HtmlTag.Html:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Section xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Paragraph>" : @"</Paragraph></Section>");
                    break;
                case HtmlTag.P:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph>" : @"</Paragraph>");
                    break;
                case HtmlTag.B:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Span FontWeight=""bold"">" : @"</Span>");
                    break;
                case HtmlTag.Br:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph><LineBreak/></Paragraph>" : @"<Paragraph><LineBreak/></Paragraph>");
                    break;
                case HtmlTag.I:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Run FontStyle=""italic"">" : @"</Run>");
                    break;
                case HtmlTag.Li:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<ListItem><Paragraph>" : @"</Paragraph></ListItem>");
                    break;
                case HtmlTag.Ol:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph><List MarkerStyle=""Decimal"">" : @"</List></Paragraph>");
                    break;
                case HtmlTag.Pre:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph>" : @"</Paragraph>");
                    break;
                case HtmlTag.Span:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph>" : @"</Paragraph>");
                    break;
                case HtmlTag.U:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Run TextDecorations=""Underline"">" : @"</Run>");
                    break;
                case HtmlTag.Ul:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph><List MarkerStyle=""Disc"">" : @"</List></Paragraph>");
                    break;
                default:
                    xamlBuilder.AppendLine((false == isEndElement) ? @"<Paragraph>" : @"</Paragraph>");
                    break;
            }
            return xamlBuilder;
        }

        /// <summary>
        /// converter which converts the HTML Node into a specific Enum value
        /// </summary>
        /// <param name="htmlNode">htnl node type</param>
        /// <returns>enum equivalent to html tag</returns>
        private static HtmlTag ConvertHtmlNodeToTag(string htmlNode)
        {
            switch (htmlNode)
            {
                case "html": return HtmlTag.Html;
                case "p": return HtmlTag.P;
                case "i": return HtmlTag.I;
                case "b": return HtmlTag.B;
                case "u": return HtmlTag.U;
                case "br": return HtmlTag.Br;
                case "pre": return HtmlTag.Pre;
                case "ul": return HtmlTag.Ul;
                case "li": return HtmlTag.Li;
                case "ol": return HtmlTag.Ol;
                case "span": return HtmlTag.Span;
                default: return HtmlTag.Other;
            }
        }

        /// <summary>
        /// Format the innings number for commentary URL
        /// Suppose if the inningsName is FirstInnings, return "1"
        /// </summary>
        /// <param name="inningsName">inningsName</param>
        /// <returns>formatted innings number</returns>
        public static string FormatInningsNumber(string inningsName)
        {
            if (null != inningsName && false == string.IsNullOrEmpty(inningsName))
            {
                switch (inningsName)
                {
                    case Constants.Constant.FirstInnings:
                        return "1";

                    case Constants.Constant.SecondInnings:
                        return "2";

                    case Constants.Constant.ThirdInnings:
                        return "3";

                    case Constants.Constant.FourthInnings:
                        return "4";

                    default:
                        return "1";
                }
            }
            else
            {
                return "1";
            }
        }

        /// <summary>
        /// HTML tags considered for the conversion
        /// </summary>
        private enum HtmlTag
        {
            Html = 0,
            P,
            I,
            B,
            U,
            Br,
            Pre,
            Ul,
            Li,
            Ol,
            Span,
            Other  //An enumeration to represent any other HTMLTags
        }
       
    }

    /// <summary>
    /// The fragmentation framework which plays an important role during dynamic loading of extra items in the 
    /// Items control 
    /// </summary>
    /// <typeparam name="T">any data type which is a part of the LIST on which the fragmentational operations has to be performed</typeparam>
    public static class DataFragmentation<T>
    {
        /// <summary>
        /// A flag which says the format in which we have to display the data..
        /// </summary>
        public enum FragmentedDataflow
        {
            RightToLeft = 0,
            UpToDown
        }

        /// <summary>
        /// The algorithm implemented which Fragments the Source in such a wa
        /// </summary>
        /// <param name="noOfContainers">Number of Containers (upto 2 containers handled)</param>
        /// <param name="source">Source</param>
        /// <param name="containers">Containers which get refreshed</param>
        /// <param name="flow">Enumeration which says how to add item data</param>
        /// <param name="noOfItems">Number of items to be retrieved</param>
        private static void Fragmenter(int noOfContainers, object source, object containers, FragmentedDataflow flow, int noOfItems)
        {
            List<T> sourceList = ((IEnumerable<T>)source).ToList();
            List<IEnumerable<T>> containerList = ((IEnumerable<IEnumerable<T>>)containers).ToList();
            int numberOfItemsToRetrieve = noOfItems;

            int totalAlreadyInTheContainer = 0;
            List<int> itemsToFallInContainer = new List<int>(noOfContainers);

            foreach (object box in containerList)
            {
                totalAlreadyInTheContainer += ((IEnumerable<T>)box).ToList().Count;
                itemsToFallInContainer.Add(0);
            }

            if (totalAlreadyInTheContainer == sourceList.Count)
            {
                return;//We have already finished with all the items from the source..
            }

            List<T> sourceContext = ((IEnumerable<T>)sourceList).Skip(totalAlreadyInTheContainer)
                                                                      .Take(numberOfItemsToRetrieve).ToList();


            int actualItemsPresent = sourceContext.Count;
            if ((actualItemsPresent % noOfContainers) == 0)
            {
                for (int containerIndex = 0; containerIndex < noOfContainers; containerIndex++)
                {
                    itemsToFallInContainer[containerIndex] = actualItemsPresent / noOfContainers;
                }
            }
            else
            {
                //only 2 containers handled for now..
                if (((ObservableCollection<T>)containerList[0]).Count <= ((ObservableCollection<T>)containerList[1]).Count)
                {
                    itemsToFallInContainer[0] = (actualItemsPresent / noOfContainers) + 1;
                    if (noOfContainers > 1) itemsToFallInContainer[1] = (actualItemsPresent / noOfContainers);
                }
                else
                {
                    itemsToFallInContainer[1] = (actualItemsPresent / noOfContainers) + 1;
                    if (noOfContainers > 1) itemsToFallInContainer[0] = (actualItemsPresent / noOfContainers);
                }
            }
            int startIndex = 0;
            for (int containerIndex = 0; containerIndex < noOfContainers; containerIndex++)
            {
                if (flow == FragmentedDataflow.UpToDown)
                {
                    for (int dataMovementUpDownIndex = startIndex; dataMovementUpDownIndex < startIndex + itemsToFallInContainer[containerIndex]; dataMovementUpDownIndex++)
                    {
                        ((ObservableCollection<T>)containerList[containerIndex]).Add(sourceContext[dataMovementUpDownIndex]);
                    }
                    startIndex += itemsToFallInContainer[containerIndex];
                }
                else
                {
                    for (int dataRightToLeft = 0; dataRightToLeft < itemsToFallInContainer[containerIndex]; dataRightToLeft++)
                    {
                        int currentIndex = startIndex + dataRightToLeft * noOfContainers;
                        if (currentIndex < sourceContext.Count)
                        {
                            ((ObservableCollection<T>)containerList[containerIndex]).Add(sourceContext[(startIndex) + (dataRightToLeft * noOfContainers)]);
                        }
                    }
                    startIndex = containerIndex + 1;
                }
            }
        }

        private delegate void FragmentationDelegate(int noOfContainers, object source, object containers, FragmentedDataflow flow, int noOfItems);

        /// <summary>
        /// The algorithm implemented which Fragments the Source in such a wa
        /// </summary>
        /// <param name="noOfContainers">Number of Containers (upto 2 containers handled)</param>
        /// <param name="source">Source</param>
        /// <param name="containers">Containers which get refreshed</param>
        /// <param name="flow">Enumeration which says how to add item data</param>
        /// <param name="noOfItems">Number of items to be retrieved</param>
        public static void Fragment(int noOfContainers, object source, object containers, FragmentedDataflow flow, int noOfItems)
        {
            FragmentationDelegate dispatcherMethod;
            dispatcherMethod = Fragmenter;
            object[] parameters = new object[]{noOfContainers,source,containers,flow,noOfItems};
            System.Windows.Application.Current.Dispatcher.BeginInvoke(dispatcherMethod, parameters);         
        }

    }

   
}
