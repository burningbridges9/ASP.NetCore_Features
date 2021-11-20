using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryServiceApi.Services
{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
    }
}
