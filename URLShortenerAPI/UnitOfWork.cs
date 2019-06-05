using URLShortenerAPI.Data;
using URLShortenerAPI.Interfaces;
using URLShortenerAPI.Repository;

namespace URLShortenerAPI
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UrlInfoContext _context;

        public UnitOfWork(UrlInfoContext context)
        {
            _context = context;
            UrlInfo = new UrlInfoRepository(_context);
        }

        public IUrlInfoRepository UrlInfo { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
