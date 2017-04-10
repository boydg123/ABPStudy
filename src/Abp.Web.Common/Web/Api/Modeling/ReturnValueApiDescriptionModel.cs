using System;

namespace Abp.Web.Api.Modeling
{
    /// <summary>
    /// API返回值描述模型
    /// </summary>
    [Serializable]
    public class ReturnValueApiDescriptionModel
    {
        public Type Type { get; }
        public string TypeAsString { get; }

        public ReturnValueApiDescriptionModel(Type type)
        {
            Type = type;
            TypeAsString = type.FullName;
        }
    }
}