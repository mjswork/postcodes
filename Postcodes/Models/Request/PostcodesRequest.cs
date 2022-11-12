using System.ComponentModel.DataAnnotations;

namespace Postcodes.Models.Request
{
    public class PostcodesRequest
    {
        [Required]
        [PostcodesRequired]
        public List<string> Postcodes { get; set; } = new List<string>();
    }
}
