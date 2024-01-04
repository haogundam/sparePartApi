using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SparePart
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires {  get; set; }
    }
}
