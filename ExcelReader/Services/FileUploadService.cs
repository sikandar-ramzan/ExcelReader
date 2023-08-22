using ExcelDataReader;
using ExcelReader.Data;
using ExcelReader.Data.Enum;
using ExcelReader.Models;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.Mvc;

namespace ExcelReader.Services
{
    public interface IFileUploadService
    {
        Task UploadExcelFile(IFormFile file);
        Task<List<ITRequest>> GetExcelFileData();
        DateTime ConvertToDate(string dateString);

    }

    public class FileUploadService : IFileUploadService
    {
        private readonly DataContext _dbContext;

        public FileUploadService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UploadExcelFile(IFormFile file)
        {
            var itRequests = new List<ITRequest>();



            var sourceFileId = Guid.NewGuid().ToString();
            string fileName = file.FileName;

            using (var stream = file.OpenReadStream())
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        itRequests.Add(new ITRequest
                        {
                            RequestId = Guid.NewGuid(),
                            Author = reader.GetValue(1).ToString(),
                            Type = reader.GetValue(2).ToString(),
                            Subject = reader.GetValue(3).ToString(),
                            Body = reader.GetValue(4).ToString(),
                            SourceFileId = sourceFileId,
                            RequestSubmissionDate = ConvertToDate(reader.GetValue(5).ToString()),
                            RequestCompletionDate = ConvertToDate(reader.GetValue(6).ToString()),
                            Status = reader.GetValue(7).ToString(),
                        });
                    }
                }
            }


            var userFileData = new UserFile()
            {
                FileId = sourceFileId,
                Filename = fileName,
                Owner = "Sikandar",
                UploadDate = DateTime.Now,

            };

            _dbContext.ITRequests.AddRange(itRequests);
            _dbContext.UserFiles.Add(userFileData);
            await _dbContext.SaveChangesAsync();


        }

        public DateTime ConvertToDate(string dateString)
        {
            // Define the expected date format(s)
            string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy" }; // Add more formats as needed

            // Attempt to parse the date string
            if (DateTime.TryParseExact(dateString, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result; // Date string successfully parsed
            }
            else
            {
                return new DateTime(); // Date string could not be parsed
            }
        }

        public async Task<List<ITRequest>> GetExcelFileData()
        {
            return await _dbContext.ITRequests.ToListAsync();
        }
    }
}



/*using var memoryStream = new MemoryStream();
await file.CopyToAsync(memoryStream);

using var package = new ExcelPackage(memoryStream);
ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the data is in the first worksheet

int rowCount = worksheet.Dimension.Rows;

List<ITRequest> requests = new List<ITRequest>();

for (int row = 2; row <= rowCount; row++) // Start from 2 to skip the header row
{
    var request = new ITRequest
    {
        Author = worksheet.Cells[row, 1].Value.ToString(),
        Type = worksheet.Cells[row, 2].Value.ToString(),
        Subject = worksheet.Cells[row, 3].Value.ToString(),
        Body = worksheet.Cells[row, 4].Value.ToString(),
        RequestSubmissionDate = DateTime.Parse(worksheet.Cells[row, 5].Value.ToString()),
        RequestCompletionDate = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString()),
        Status = Enum.Parse<ITRequestStatusTypes>(worksheet.Cells[row, 7].Value.ToString())
    };
    requests.Add(request);
}

_dbContext.ITRequests.AddRange(requests);
await _dbContext.SaveChangesAsync();
*/