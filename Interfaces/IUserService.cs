using Automate.Models;

namespace Automate.Interfaces
{
    public interface IUserService
    {
        User? Authenticate(string? username, string? password);
    }
}
