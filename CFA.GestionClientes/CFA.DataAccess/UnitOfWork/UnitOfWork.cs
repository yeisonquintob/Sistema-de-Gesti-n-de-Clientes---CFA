
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CFA.DataAccess.Context;
using CFA.DataAccess.Repositories.Interfaces;
using CFA.DataAccess.Repositories.Implementation;
using CFA.Entities.Models;
namespace CFA.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CFADbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed;

        private IGenericRepository<Cliente>? _clienteRepository;
        private IGenericRepository<TipoDocumento>? _tipoDocumentoRepository;
        private IGenericRepository<Genero>? _generoRepository;
        private IGenericRepository<DireccionCliente>? _direccionClienteRepository;
        private IGenericRepository<TelefonoCliente>? _telefonoClienteRepository;

        public UnitOfWork(CFADbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Cliente> ClienteRepository => 
            _clienteRepository ??= new GenericRepository<Cliente>(_context);

        public IGenericRepository<TipoDocumento> TipoDocumentoRepository =>
            _tipoDocumentoRepository ??= new GenericRepository<TipoDocumento>(_context);

        public IGenericRepository<Genero> GeneroRepository =>
            _generoRepository ??= new GenericRepository<Genero>(_context);

        public IGenericRepository<DireccionCliente> DireccionClienteRepository =>
            _direccionClienteRepository ??= new GenericRepository<DireccionCliente>(_context);

        public IGenericRepository<TelefonoCliente> TelefonoClienteRepository =>
            _telefonoClienteRepository ??= new GenericRepository<TelefonoCliente>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                await _transaction!.CommitAsync();
            }
            finally
            {
                await _transaction!.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                await _transaction!.RollbackAsync();
            }
            finally
            {
                await _transaction!.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _transaction?.Dispose();
            }
            _disposed = true;
        }
    }
}