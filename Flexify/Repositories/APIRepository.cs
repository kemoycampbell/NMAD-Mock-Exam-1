using System.Threading.Tasks;
using Flexify.Models;
using Microsoft.EntityFrameworkCore;

namespace Flexify.Repositories
{
    public class APIRepository: IAuthenticationRepository
    {
        private readonly DbSet<Auth> _auths;

        public APIRepository(DatabaseContext database)
        {
            _auths = database.Auths;
        }
        public async Task<Auth> Authenticate(string key)
        {
            return await _auths.FirstOrDefaultAsync(auth => auth.ApiKey == key);
        }
    }
}