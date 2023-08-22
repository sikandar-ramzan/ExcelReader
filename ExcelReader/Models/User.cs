using System.ComponentModel.DataAnnotations;

namespace ExcelReader.Models
{
    public class User
    {

        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string Team { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string UploadFileIds { get; set; } = string.Empty;
    }
}
