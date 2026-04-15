using Core.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Jwt
{
    public interface ITokenService
    {
        string GetToken(string userId, string userName, IList<string> roles);
    }
}
