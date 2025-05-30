using CinemaMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace CinemaMVC.Repositories
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {

        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
