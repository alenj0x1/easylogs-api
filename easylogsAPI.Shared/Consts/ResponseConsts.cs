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
    public const string MissingConfigurationJwtIssuer = "Missing JWT issuer";
    public const string MissingConfigurationJwtAudience = "Missing JWT audience";
    public const string MissingConfigurationJwtSecretKey = "Missing JWT secret key";
    public const string MissingConfigurationPostgresConnectionString = "Missing PostgreSQL connection string";
    public const string MissingConfigurationFirstUserUsername = "Missing First User username";
    public const string MissingConfigurationFirstUserPassword = "Missing First User password";
    public const string MissingConfigurationFirstUserEmail = "Missing First User email";
    
    // Authorization
    public const string TokenNotFound = "Token not found";
    public const string TokenExpired = "Token expired";
}