using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DealDouble.Entities
{
    public class Auction : BaseEntity
    {
        public virtual Category Category { get; set; }
        public int CategoryID { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(1, 1000000, ErrorMessage ="Actual ammount must be 1 to 1000000")]
        public decimal ActualAmount { get; set; }

        public DateTime? StartingTime { get; set; }
        public Nullable<DateTime> EndingTime { get; set; }

        public virtual List<AuctionPicture> AuctionPictures { get; set; }

        public virtual List<Bid> Bids { get; set; }
    }
}
