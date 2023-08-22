using ExcelReader.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelReader.Models
{
    public class ExcelFile
    {
        /*[Key*/
        public string FileId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string RequestSubmissionDate { get; set; } = string.Empty;
        public string RequestCompletionDate { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
