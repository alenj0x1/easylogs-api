namespace easylogsAPI.Shared.Consts;

public static class ResponseConsts
{
    // User
    public const string UserFirstCreated = "User first created correctly";
    public const string UserCreated = "User created correctly";
    public const string UserUpdated = "User updated correctly";
    public const string UserDeleted = "User deleted correctly";
    public const string UserNotFound = "User not found";
    public const string UserOrPasswordIncorrect = "User or password incorrect";
    
    // Configuration
    public const string ConfigurationMissingJwtIssuer = "Missing JWT issuer";
    public const string ConfigurationMissingJwtAudience = "Missing JWT audience";
    public const string ConfigurationMissingJwtSecretKey = "Missing JWT secret key";
    public const string ConfigurationMissingPostgresConnectionString = "Missing PostgreSQL connection string";
    public const string ConfigurationMissingFirstUserUsername = "Missing First User username";
    public const string ConfigurationMissingFirstUserPassword = "Missing First User password";
    public const string ConfigurationMissingFirstUserEmail = "Missing First User email";
    
    // Authorization
    public const string TokenNotFound = "token_not_found";
    public const string TokenExpired = "token_expired";
}