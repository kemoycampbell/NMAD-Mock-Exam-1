using System.Threading.Tasks;
using Flexify.Models;

namespace Flexify.Repositories
{
    public interface IAuthenticationRepository
    {
        public Task<Auth> Authenticate(string key);
    }
}