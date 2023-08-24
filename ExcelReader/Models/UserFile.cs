﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExcelReader.Models
{
    public class UserFile
    {
        [Key]
        public Guid FileId { get; set; }
        public string Filename { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}
