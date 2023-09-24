using AuthorService.Application.Enums;

namespace AuthorService.Application.Models
{
    public class RequestedUser //Bu classı, controllerlardaki kullanıcılardan gelen jwtyi parse etmek için kullanıcaz. JWTnin claimleri, aşağıdaki propertylerle eşleşecektir.
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PreferredUsername { get; set; }
        public AuthRole Role { get; set; }
    }
}
