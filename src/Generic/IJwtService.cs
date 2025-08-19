namespace metabolon.Generic;

using metabolon.Models;
using metabolon.Services;

public interface IJwtService
{
    AuthToken GenerateToken(User u);
}