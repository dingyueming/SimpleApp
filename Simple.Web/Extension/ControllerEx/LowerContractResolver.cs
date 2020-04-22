using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Web.Extension.ControllerEx
{
    public class LowerContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
