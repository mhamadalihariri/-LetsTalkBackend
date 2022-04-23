using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Let_sTalk.Models;


namespace Let_sTalk.Data
{
    public interface IUserRepository
    {
        User create(User user);
        User getByEmail(string email);
        User getById(int id);
        User update(User user);
    }
}
