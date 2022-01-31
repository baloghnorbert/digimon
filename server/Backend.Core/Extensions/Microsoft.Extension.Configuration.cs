using System;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Maps the appSettings.json to a specific T object in DI
    /// use
    /// serviceCollection.AddSingleton<T>((_) => Configuration.GetObjectFromConfigSection<T>("_json section name"));
    /// </summary>
    public static class AppSettings
    {
        public static T GetObjectFromConfigSection<T>(this IConfiguration configurationRoot, string configSection) where T : new()
        {
            T result = new T();
            Type propertyType = null;

            foreach (var propInfo in typeof(T).GetProperties())
            {
                propertyType = propInfo.PropertyType;
                
                if (propInfo?.CanWrite ?? false)
                {
                    var value = Convert.ChangeType(configurationRoot.GetValue<string>($"{configSection}:{propInfo.Name}"), propInfo.PropertyType);
                    propInfo.SetValue(result, value, null);
                }
            }

            return result;
        }
    }
}
