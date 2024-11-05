namespace easylogsAPI.WebApi.Attributes;

public class PermissionAttribute(string values) : Attribute
{
    public string Values = values;
}