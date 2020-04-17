using System;
using System.Collections.Generic;
using System.Text;

namespace Simple.Infrastructure.ControllerExtension
{
    public class LowerContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
