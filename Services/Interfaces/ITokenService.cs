using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshTokenValue();
        string Hash(string value);
    }
}
