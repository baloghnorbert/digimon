using Backend.Core.Variables;

namespace Backend.Core.Helper
{
    public static class BaseUrlResolver
    {
        private static string CreateURL(string pathPart)
        {
#if DEBUG
            string url = $@"{ServerPathEnviorement.Base()}\{pathPart}"
                        .Replace(@"/", @"\")
                        .Replace(@"\\", @"\");
#else
            string url =  $"{ApplicationConstans.API_URL}/{profile.ProfileImage}";
# endif

            return url;
        }
    }
}
