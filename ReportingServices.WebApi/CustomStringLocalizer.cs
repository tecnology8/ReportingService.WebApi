﻿using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingServices.WebApi
{
    public class CustomStringLocalizer : IStringLocalizer
    {
        Dictionary<string, Dictionary<string, string>> resources;

        const string HEADER1 = "Header1";
        const string HEADER2 = "Header2";
        const string HEADER3 = "Header3";
        public CustomStringLocalizer()
        {

            Dictionary<string, string> enDict = new Dictionary<string, string>
            {
                {HEADER1, "Header 1" },
                {HEADER2, "Header 2" },
                {HEADER3, "Header 3" }
            };

            Dictionary<string, string> ruDict = new Dictionary<string, string>
            {
                {HEADER1, "Заголовок 1" },
                {HEADER2, "Заголовок 2" },
                {HEADER3, "Заголовок 3" }
            };

            Dictionary<string, string> itDict = new Dictionary<string, string>
            {
                {HEADER1, "Intestazione 1" },
                {HEADER2, "Intestazione 2" },
                {HEADER3, "Intestazione 3" }
            };

            resources = new Dictionary<string, Dictionary<string, string>>
            {
                {"en", enDict },
                {"ru", ruDict },
                {"it", itDict }
            };
        }
        public LocalizedString this[string name] => throw new NotImplementedException();
        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}