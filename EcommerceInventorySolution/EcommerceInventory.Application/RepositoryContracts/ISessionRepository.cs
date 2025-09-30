using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Application.RepositoryContracts;
public interface ISessionRepository
{
    Task AddSessionAsync(Domain.Entities.Session session);
}
