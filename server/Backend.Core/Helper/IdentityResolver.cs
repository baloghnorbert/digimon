namespace Backend.Core.Helper
{
    public static class IdentityResolver
    {
        public static string CodeIdentity(this string value)
        {
            string[] fragments = value.Split("-");
            //                                                          0                       1                         2                          3                            4                          5
            string fakeIdentity = $"{fragments[5]}-{fragments[3]}-{fragments[2]}-{fragments[4]}-{fragments[1]}-{fragments[0]}";

            return fakeIdentity;
        }
        public static string DecodeIdentity(this string value)
        {
            string[] fragments = value.Split("-");
            //                                                     0                         1                             2                              3                          4                          5
            string realIdentity = $"{fragments[5]}-{fragments[4]}-{fragments[2]}-{fragments[1]}-{fragments[3]}-{fragments[0]}";

            return realIdentity;
        }
    }
}
