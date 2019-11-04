using System;
using System.Configuration;

namespace TestBuilder
{
    public class FeatureConfig : IFeatureConfig
    {
        public bool ShouldShowDeletedPosts => bool.Parse(ConfigurationManager.AppSettings["ShouldShowDeletedPosts"]);
    }
}