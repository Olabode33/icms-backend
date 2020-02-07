using System.ComponentModel.DataAnnotations;

namespace ICMSDemo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
