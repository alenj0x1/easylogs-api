using System.Text;

namespace easylogsAPI.Shared;

public static class Generate
{
    private const string Characters = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNORPQRSTUVXYWZ0123456789!@#$%^&*()_+{}[]<>|";
    private const string Alphabet = "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNORPQRSTUVXYWZ";

    public static long Milliseconds(DateTime dateTime)
    {
        try
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static string RandomString(int length = 50, bool onlyAlphabet = false)
    {
        try
        {
            var chars = new StringBuilder();

            switch (onlyAlphabet)
            {
                case false:
                {
                    for (var i = 0; i < length; i++)
                    {
                        chars.Append(Characters[new Random().Next(0, Characters.Length)]);
                    }

                    break;
                }
                case true:
                {
                    for (var i = 0; i < length; i++)
                    {
                        chars.Append(Alphabet[new Random().Next(0, Alphabet.Length)]);
                    }

                    break;
                }
            }

            return chars.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}