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

    public static Guid ToGuid(string value)
    {
        try
        {
            return Guid.Parse(value);
        }
        catch (Exception e)
        {
            return Guid.Empty;
        }
    }
}