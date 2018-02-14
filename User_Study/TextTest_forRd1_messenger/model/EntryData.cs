using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using WobbrockLib;
using WobbrockLib.Extensions;

namespace TextTest
{
    /// <summary>
    /// Data for a single character entry for a text entry study.
    /// </summary>
    public class EntryData : IXmlLoggable
    {
        #region Fields

        private char _char;
        private long _ticks;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public EntryData()
        {
            // do nothing
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aChar"></param>
        /// <param name="ticks"></param>
        public EntryData(char aChar, long ticks)
        {
            _char = aChar;
            _ticks = ticks;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public char Char
        {
            get { return _char; }
            set { _char = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort Code
        {
            get { return (ushort) _char; }
        }

        /// <summary>
        /// 
        /// </summary>
        public long Ticks
        {
            get { return _ticks; }
            set { _ticks = value; }
        }

        #endregion

        #region IXmlLoggable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteXmlHeader(XmlTextWriter writer)
        {
            // <entry char="g" value="103" time="63604023814.284" />
            writer.WriteStartElement("Entry");
            writer.WriteAttributeString("char", XmlConvert.ToString(this.Char));
            writer.WriteAttributeString("value", XmlConvert.ToString(this.Code));
            writer.WriteAttributeString("ticks", XmlConvert.ToString(this.Ticks));
            writer.WriteAttributeString("seconds", XmlConvert.ToString(TimeEx.Ticks2Sec(this.Ticks, 2)));
            writer.WriteEndElement(); // </Entry>
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteXmlFooter(XmlTextWriter writer)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool ReadFromXml(XmlTextReader reader)
        {
            reader.Read(); // <Entry>
            if (reader.Name != "Entry")
                throw new XmlException("XML format error: Expected the <Entry> element.");

            _char = XmlConvert.ToChar(reader.GetAttribute("char"));
            _ticks = XmlConvert.ToInt64(reader.GetAttribute("ticks"));

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <returns></returns>
        public bool WriteResultsToTxt(StreamWriter writer)
        {
            return true; // no calculations at this level
        }

        #endregion
    }
}
