using URLShortenerAPI.Data;
using URLShortenerAPI.Interfaces;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Repository
{
    public class UrlInfoRepository : Repository<UrlInfo>, IUrlInfoRepository
    {
        public UrlInfoRepository(UrlInfoContext context)
            : base(context)
        {
        }

        public UrlInfoContext UrlInfoContext
        {
            get { return Context as UrlInfoContext; }
        }
    }
}
