using System;
using System.ComponentModel.DataAnnotations;

namespace URLShortenerAPI.Models
{
    public class UrlInfo
    {
        [Key]
        [Required]
        public int UrlInfoId { get; set; }

        //[Required]
        [Display(Name = "Short Url")]
        public string ShortUrl { get; set; }

        [Required]
        [Display(Name = "Long Url")]
        public string LongUrl { get; set; }

        public string Suffix { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}
