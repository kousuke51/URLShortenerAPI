using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URLShortenerAPI.Data;
using URLShortenerAPI.Models;

namespace URLShortenerAPI.Utility
{
    public static class UrlHelper
    {
        private const int SHORTENED_URL_LENGTH = 8;
        public static void GenerateShortUrl(UrlInfo model, string urlPrefix, UrlInfoContext context, int daysUntilExpire, ILogger logger)
        {
            var hash = new Hash();
            var result = hash.CalculateMD5Hash(model.LongUrl);
            var hashArray = SplitList(result, SHORTENED_URL_LENGTH).ToList();

            //Check if hash is already in the table
            //  If not
            //      Insert new record
            //  Else, check if the longIrl matches what is in the DB already
            //      If yes, return the short url
            //      Else, we have a collision and start over with the next entry in the list

            var options = new DbContextOptions<UrlInfoContext>();

            try
            {
                using (UnitOfWork uow = new UnitOfWork(context))
                {
                    foreach (var item in hashArray)
                    {
                        var urlCheck = uow.UrlInfo.Find(x => x.Suffix == item
                                                        && x.ExpirationDate > DateTime.Now)
                                                        .FirstOrDefault(); //Check if shortUrl exists and has not expired.
                        if (urlCheck == null)
                        {
                            model.Suffix = item;
                            model.ShortUrl = urlPrefix + item;
                            model.UpdatedDate = DateTime.Now;
                            model.CreatedDate = DateTime.Now;
                            model.ExpirationDate = DateTime.Now.AddDays(daysUntilExpire);
                            uow.UrlInfo.Add(model);
                            uow.Complete();
                            return;
                        }
                        else
                        {
                            //return only if the LongUrl matches the current LongUrl else we have a hash collision, 
                            //so we loop again to get the next hash value.
                            if (model.LongUrl == urlCheck.LongUrl)
                            {
                                model.Suffix = urlCheck.Suffix;
                                model.ShortUrl = urlCheck.ShortUrl;
                                model.UpdatedDate = urlCheck.UpdatedDate;
                                model.CreatedDate = urlCheck.CreatedDate;
                                model.ExpirationDate = urlCheck.ExpirationDate;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Handle Exception here
                logger.LogError("An error occured while Generating the short Url", ex);
            }
        }

        public static IEnumerable<UrlInfo> GetUrlListing(UrlInfoContext context, ILogger logger)
        {
            IEnumerable<UrlInfo> resultList = new List<UrlInfo>();
            try
            {
                using (UnitOfWork uow = new UnitOfWork(context))
                {
                    resultList = uow.UrlInfo.GetAll();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("An error occured while getting the list of URLs", ex);
            }
            return resultList;
        }

        private static IEnumerable<string> SplitList(string str, int itemLength)
        {
            return Enumerable.Range(0, str.Length / itemLength)
                .Select(i => str.Substring(i * itemLength, itemLength));
        }
    }
}
