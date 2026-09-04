using PetsProjects.Models;

namespace PetsProjects;

public interface IJwtProvider
{
    string GenerateToken(UserModel user);
}