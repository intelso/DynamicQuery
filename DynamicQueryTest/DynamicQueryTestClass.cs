using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicQueryTest.Translation;

namespace DynamicQueryTest
{
    class DynamicQueryTestClass: DynamicQueryBase
    {
        [QueryField(false, 2013, "GetYears")]
        public int Year { get; set; }
        [QueryField(false, null)]
        public string ClientName { get; set; }
        [QueryField(false, "EU", "GetCountries")]
        public string Country { get; set; }
        [QueryField(false, "05.10.2012")]
        public DateTime DateFrom { get; set; }
        [QueryField(false, 200)]
        public int Participants { get; set; }
        

        [QueryMethod(true,"ID","Description")]
        public Func<DataTable> GetYears { get; set; }
        [QueryMethod(true, "Description", "Description")]
        public Func<DataTable> GetCountries { get; set; }


        internal override string GetLocalizedString(string key)
        {
            if (key=="Year")
            {
                return Strings.strYear;
            }
            if (key == "ClientName")
            {
                return Strings.strClientName;
            }
            if (key == "Country")
            {
                return Strings.strCountry;
            }
            if (key == "DateFrom")
            {
                return Strings.strDateFrom;
            }
            if (key == "Participants")
            {
                return Strings.strParticipants;
            }
            return base.GetLocalizedString(key);
        }
    }
}
