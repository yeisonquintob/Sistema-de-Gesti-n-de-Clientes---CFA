using System;
using System.Threading.Tasks;
using CFA.Entities.Models;
using CFA.DataAccess.Repositories.Interfaces;

namespace CFA.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Cliente> ClienteRepository { get; }
        IGenericRepository<TipoDocumento> TipoDocumentoRepository { get; }
        IGenericRepository<Genero> GeneroRepository { get; }
        IGenericRepository<DireccionCliente> DireccionClienteRepository { get; }
        IGenericRepository<TelefonoCliente> TelefonoClienteRepository { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}