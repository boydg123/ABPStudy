using System.Collections.Generic;
using System.Linq;

namespace Abp.Application.Features
{
    /// <summary>
    /// Used to store <see cref="Feature"/>s.
    /// 用于存储<see cref="Feature"/>
    /// </summary>
    public class FeatureDictionary : Dictionary<string, Feature>
    {
        /// <summary>
        /// Adds all child features of current features recursively.
        /// 递归添加当前功能的所有子功能
        /// </summary>
        public void AddAllFeatures()
        {
            foreach (var feature in Values.ToList())
            {
                AddFeatureRecursively(feature);
            }
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="feature">功能</param>
        private void AddFeatureRecursively(Feature feature)
        {
            //Prevent multiple adding of same named feature.
            Feature existingFeature;
            if (TryGetValue(feature.Name, out existingFeature))
            {
                if (existingFeature != feature)
                {
                    throw new AbpInitializationException("Duplicate feature name detected for " + feature.Name);
                }
            }
            else
            {
                this[feature.Name] = feature;
            }

            //Add child features (recursive call)
            foreach (var childFeature in feature.Children)
            {
                AddFeatureRecursively(childFeature);
            }
        }
    }
}