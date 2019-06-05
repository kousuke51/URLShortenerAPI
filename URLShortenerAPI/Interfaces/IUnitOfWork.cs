using System;

namespace URLShortenerAPI.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUrlInfoRepository UrlInfo { get; }
        int Complete();
    }
}
