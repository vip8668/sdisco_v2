using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
