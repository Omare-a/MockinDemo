using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockinDemo;
public class UserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public int CountActiveUsers()
    {
        var allUsers = _repo.GetUsers();

        if(allUsers is null)
            throw new InvalidOperationException("Repository returned null user list.");
        
        var activeUsers = allUsers.Count(c => c.IsActive);

        return activeUsers;
    }

}
