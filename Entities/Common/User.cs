using Core.Entities.Concrete;

namespace Entities.Common
{
    public class User : AppUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
