using System.Collections.Generic;
using System.Globalization;

namespace DentalResearchApp.Code.Impl
{
    public class Countries
    {
        public static List<string> CountryList()
        {
            //Creating Dictionary
            var cultureList = new List<string>();

            //getting the specific CultureInfo from CultureInfo class
            CultureInfo[] getCultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo getCulture in getCultureInfo)
            {
                //creating the object of RegionInfo class
                RegionInfo getRegionInfo = new RegionInfo(getCulture.Name);
                //adding each country Name into the Dictionary
                if (!(cultureList.Contains(getRegionInfo.EnglishName)))
                {
                    cultureList.Add(getRegionInfo.EnglishName);
                }
            }
            //returning country list
            return cultureList;
        }
    }
}