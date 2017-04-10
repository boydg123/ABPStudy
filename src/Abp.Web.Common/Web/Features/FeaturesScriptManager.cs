using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Features;
using Abp.Dependency;
using Abp.Runtime.Session;

namespace Abp.Web.Features
{
    /// <summary>
    /// 功能脚本管理器
    /// </summary>
    public class FeaturesScriptManager : IFeaturesScriptManager, ITransientDependency
    {
        /// <summary>
        /// ABP Session
        /// </summary>
        public IAbpSession AbpSession { get; set; }

        /// <summary>
        /// 功能管理器
        /// </summary>
        private readonly IFeatureManager _featureManager;

        /// <summary>
        /// 功能检查器
        /// </summary>
        private readonly IFeatureChecker _featureChecker;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="featureManager">功能管理器</param>
        /// <param name="featureChecker">功能检查器</param>
        public FeaturesScriptManager(IFeatureManager featureManager, IFeatureChecker featureChecker)
        {
            _featureManager = featureManager;
            _featureChecker = featureChecker;

            AbpSession = NullAbpSession.Instance;
        }

        /// <summary>
        /// 获取包含所有功能信息的Javascript
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetScriptAsync()
        {
            var allFeatures = _featureManager.GetAll().ToList();
            var currentValues = new Dictionary<string, string>();

            if (AbpSession.TenantId.HasValue)
            {
                var currentTenantId = AbpSession.GetTenantId();
                foreach (var feature in allFeatures)
                {
                    currentValues[feature.Name] = await _featureChecker.GetValueAsync(currentTenantId, feature.Name);
                }
            }
            else
            {
                foreach (var feature in allFeatures)
                {
                    currentValues[feature.Name] = feature.DefaultValue;
                }
            }

            var script = new StringBuilder();

            script.AppendLine("(function() {");

            script.AppendLine();

            script.AppendLine("    abp.features = abp.features || {};");

            script.AppendLine();

            script.AppendLine("    abp.features.allFeatures = {");

            for (var i = 0; i < allFeatures.Count; i++)
            {
                var feature = allFeatures[i];
                script.AppendLine("        '" + feature.Name.Replace("'", @"\'") + "': {");
                script.AppendLine("             value: '" + currentValues[feature.Name].Replace(@"\", @"\\").Replace("'", @"\'") + "'");
                script.Append("        }");

                if (i < allFeatures.Count - 1)
                {
                    script.AppendLine(",");
                }
                else
                {
                    script.AppendLine();
                }
            }

            script.AppendLine("    };");

            script.AppendLine();
            script.Append("})();");

            return script.ToString();
        }
    }
}