using System;
using System.Collections.Generic;
using System.Text;

namespace MvvMHelpers.core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenerateItemPropertiesAttribute : Attribute
    {
        public string[] IgnoreProperties { get; set; }

        public GenerateItemPropertiesAttribute(string[] ignoreProperties)
        {
            IgnoreProperties = ignoreProperties;
        }
        public GenerateItemPropertiesAttribute()
        {
            IgnoreProperties = new string[0];
        }

    }
}
