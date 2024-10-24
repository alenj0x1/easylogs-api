namespace easylogsAPI.Shared;

public static class Parser
{
    public static string StatusCodeToCat(int statusCode)
    {
        try
        {
            return $"https://http.cat/{statusCode}";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}