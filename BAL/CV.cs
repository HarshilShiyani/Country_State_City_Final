using Microsoft.AspNetCore.Http;
namespace Country_State_City_Final.BAL
{
    public static class CV
    {
        private static IHttpContextAccessor _contextAccessor;
        static CV()
        {
            _contextAccessor = new HttpContextAccessor();
        }
        public static string? username()
        {
            string username = "";
            if (_contextAccessor.HttpContext.Session.GetString("username") != null)
            {
                username = _contextAccessor.HttpContext.Session.GetString("username").ToString();
            }
            Console.WriteLine(username);

            return username;
        }
    }
}
