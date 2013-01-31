namespace GUI.Model
{
    public class SmartRow
    {
        #region Fields
		
        public string Attribute { get; set; }
        public string Current { get; set; }
        public string Threshold { get; set; }
        public string RawData { get; set; }
        public string RealData { get; set; }
 
	    #endregion

        #region Constructor

        public SmartRow(string attribute, string current, string threshold, string rawData, string realData)
        {
            Attribute = attribute;
            Current = current;
            Threshold = threshold;
            RawData = rawData;
            RealData = realData;
        }
        
        #endregion
    }
}
