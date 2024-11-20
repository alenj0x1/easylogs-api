﻿using easylogsAPI.Domain.Entities;

namespace easylogsAPI.Domain.Interfaces.Repositories;

public interface ITokenRepository
{
    Task<Tokenaccess> CreateTokenAccess(Tokenaccess tokenAccess);
    Task<Tokenrefresh> CreateTokenRefresh(Tokenrefresh tokenRefresh);
    IQueryable<Tokenaccess> GetTokenAccesses();
    Tokenaccess? GetTokenAccess(string value);
    Tokenrefresh? GetTokenRefresh(string value);
    Tokenrefresh? GetTokenRefresh(Guid userId);
    Task<bool> DeleteTokenRefresh(Tokenrefresh tokenrefresh);
}