using easylogsAPI.Domain.Entities;
using easylogsAPI.Models.Helpers;

namespace easylogsAPI.Application.Interfaces.Helpers;

public interface IToken
{
    CreateAccessTokenData? CreateAccessToken(Userapp userapp, DateTime expiration);
}