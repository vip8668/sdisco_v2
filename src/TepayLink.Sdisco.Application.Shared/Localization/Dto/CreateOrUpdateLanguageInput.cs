using System.ComponentModel.DataAnnotations;

namespace TepayLink.Sdisco.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}