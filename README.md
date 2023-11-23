# FiltroGarden

# Proyecto Jardineria.

## Creacion Entities, Configuraciones y DbContext con DbFirts.
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


# Extensions.
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.UnitOfWork;
using AspNetCoreRateLimit;
using Domain.Interfaces;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void ConfigureCors(this  IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder =>
                        builder
                            .AllowAnyOrigin() //WithOrigins("https://domini.com")
                            .AllowAnyMethod() //WithMethods(*GET", "POST")
                            .AllowAnyHeader()
                );
            });

        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void ConfigureRatelimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options =>
            {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-IP";
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "10s",
                        Limit = 2
                    }
                };
            }
            );

            
        }
 
            
    }
}
```




## Profiles
```
namespace API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<DetallePedido, DetallePedidoDto>().ReverseMap();
            CreateMap<Empleado, EmpleadoDto>().ReverseMap();
            CreateMap<GamaProducto, GamaProductoDto>().ReverseMap();
            CreateMap<Oficina, OficinaDto>().ReverseMap();
            CreateMap<Pago, PagoDto>().ReverseMap();
            CreateMap<Pedido, PedidoDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();

        }

    }
}

```



# Endpoints

Cliente

GET
http://localhost:5229/api/Cliente

POST
http://localhost:5229/api/Cliente

GET
http://localhost:5229/api/Cliente/{id}

PUT
http://localhost:5229/api/Cliente/{id}

DELETE
http://localhost:5229/api/Cliente/{id}

DetallePedido

GET
http://localhost:5229/api/DetallePedido

POST
http://localhost:5229/api/DetallePedido

GET
http://localhost:5229/api/DetallePedido/{id}

PUT
http://localhost:5229/api/DetallePedido/{id}

DELETE
http://localhost:5229/api/DetallePedido/{id}

Empleado


GET
http://localhost:5229/api/Empleado

POST
http://localhost:5229/api/Empleado

GET
http://localhost:5229/api/Empleado/{id}

PUT
http://localhost:5229/api/Empleado/{id}

DELETE
http://localhost:5229/api/Empleado/{id}

GamaProducto


GET
http://localhost:5229/api/GamaProducto

POST
http://localhost:5229/api/GamaProducto

GET
http://localhost:5229/api/GamaProducto/{id}

PUT
http://localhost:5229/api/GamaProducto/{id}

DELETE
http://localhost:5229/api/GamaProducto/{id}

Oficina


GET
http://localhost:5229/api/Oficina

POST
http://localhost:5229/api/Oficina

GET
http://localhost:5229/api/Oficina/{id}

PUT
http://localhost:5229/api/Oficina/{id}

DELETE
http://localhost:5229/api/Oficina/{id}

Pago


GET
http://localhost:5229/api/Pago

POST
http://localhost:5229/api/Pago

GET
http://localhost:5229/api/Pago/{id}

PUT
http://localhost:5229/api/Pago/{id}

DELETE
http://localhost:5229/api/Pago/{id}

Pedido


GET
http://localhost:5229/api/Pedido

POST
http://localhost:5229/api/Pedido

GET
http://localhost:5229/api/Pedido/{id}

PUT
http://localhost:5229/api/Pedido/{id}

DELETE
http://localhost:5229/api/Pedido/{id}

Para esta consulta se filtran pedidos donde la fecha esperada es menor a la fecha de entrega entonces construye una cadena, devolviendo el resultado.

GET
http://localhost:5229/api/Pedido/Consulta1

Producto


GET
http://localhost:5229/api/Producto

POST
http://localhost:5229/api/Producto

GET
http://localhost:5229/api/Producto/{id}

PUT
http://localhost:5229/api/Producto/{id}

DELETE
http://localhost:5229/api/Producto/{id}

Para esta consulta usamos el metodo join para conectar dos tablas(producto y detallePedidos, buscando la suma de pedidos agrupados por productos, y entonces esta devuelve como entero.)
GET
http://localhost:5229/api/Producto/Consulta4










