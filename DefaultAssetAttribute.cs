using System;

namespace SyluxDev.ElementsEngine
{
    [AttributeUsage(AttributeTargets.Field)]
    public class DefaultAssetAttribute : Attribute
    {
        /// <summary>
        /// The asset should be in Resources folder
        /// </summary>
        /// <param name="assetName">The name of the asset/prefab at the end of path E.g. Resources/assetName</param>
        public DefaultAssetAttribute(string assetName) => this.AssetName = assetName;
        public readonly string AssetName;
    }
}