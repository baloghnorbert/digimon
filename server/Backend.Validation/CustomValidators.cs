using System;

namespace Backend.Validation
{
    public static class CustomValidators
    {
        public static bool ValidateUri(string uri)
        {
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }
}
