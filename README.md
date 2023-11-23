# FiltroGarden

# Proyecto Jardineria.

## Creacion Entities y DbContext con DbFirts.
## Codigo que se debe usar para la creacion de entidades, configuraciones y Context del proyecto, DbFirts.

```
dotnet ef dbcontext scaffold "server=localhost;user=root;password=123456;database=jardineria;" Pomelo.EntityFrameworkCore.MySqql -s GardenApi -p Persistence --context GardenContext --context-dir Data --output-dir Entities
```

Despues de estos, nos encontraremos las Entidades y configuraciones en la carpeta de Persistencia en el archivo de DbContext, debemos mover las entidades y configuraciones a su respectiva carpeta al igual que "server=localhost;user=root;password=123456;database=jardineria;" en el archio Programs.cs


# Interfaces.
```
Procedemos a la creacion de interfaces, IUnitOfWork, IGenericRepository

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICliente :IGenericRepository<Cliente>
    {

    }
}
```


## IUnitOfWork
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ICliente Clientes {get; }
        IDetallePedido DetallePedidos {get; }
        IEmpleado Empleados {get; }
        IGamaProducto GamaProductos {get;}
        IOficina Oficinas {get;}
        IPago Pagos {get;}
        IPedido Pedidos {get;}
        IProducto Productos {get;}
        IRefreshToken RefreshTokens {get;}
        IRol Rols {get; }
        IUser Users {get; }


        Task<int> SaveAsync();
    }
}
```


## IGenericRepository	

```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class 
    {
        Task<T> GetByIdAsync(string stringId);
        Task<T> GetByIdAsync(int intId);
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        void Update(T entity);
        
    }
}
```


# Repositorio Generico, Repositorios.
## GenericRepository
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly GardenContext _context;
        public GenericRepository(GardenContext context)
        {
            _context = context;
        }
        public virtual void  Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().AddRange(entity);
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<(int totalRegistros, IEnumerable<T> registros)> GetAllAsync(
            int pageIndex,
            int pageSize, 
            string search)
        {
            var totalRegistros = await _context.Set<T>().CountAsync();
                var registros = await _context
                .Set<T>()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (totalRegistros, registros);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIdAsync(string id )
        {
            return await _context.Set<T>().FindAsync(id);
            
        }

        public virtual void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>()
                .Update(entity);

        }
    }
}
```

## Repositorios
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.Repositories
{
    public class ClienteRepository :GenericRepository<Cliente>, ICliente
    {

        private readonly GardenContext _context;
        public ClienteRepository(GardenContext context) : base(context)
        {
            _context = context;
            
        }

    
    }
}
```

# Endpoints
