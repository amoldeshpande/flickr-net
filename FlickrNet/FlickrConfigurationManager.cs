using System;
using System.Xml;

#if !(MONOTOUCH || WindowsCE || SILVERLIGHT || DOTNETSTANDARD)
using System.Configuration;
#endif
#if DOTNETSTANDARD
#endif
namespace FlickrNet
{
    /// <summary>
    /// Summary description for FlickrConfigurationManager.
    /// </summary>
    internal class FlickrConfigurationManager
#if !DOTNETSTANDARD
        : IConfigurationSectionHandler
#endif
    {
        private static string configSection = "flickrNet";
        private static FlickrConfigurationSettings settings;

        public static FlickrConfigurationSettings Settings
        {
            get
            {
                if (settings == null)
                {
#if !DOTNETSTANDARD
                    settings = (FlickrConfigurationSettings)ConfigurationManager.GetSection(configSection);
#else
                    settings = FlickrConfigurationSettings.FromJsonFile("appsettings.json",configSection); 
#endif // !DOTNETSTANDARD
                }

                return settings;
            }
        }

#if !DOTNETSTANDARD
        public object Create(object parent, object configContext, XmlNode section)
        {
            configSection = section.Name;
            return new FlickrConfigurationSettings(section);
        }
#endif
    }
}
