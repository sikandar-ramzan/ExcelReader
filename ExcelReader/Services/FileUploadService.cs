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
        Task<bool> UploadExcelFile(IFormFile file);
        Task<List<ITRequest>> GetExcelFileData();
        DateTime ConvertToDate(string dateString);
        Task<List<ITRequestWithFile>> GetITRequestsWithFiles();


    }

    public class FileUploadService : IFileUploadService
    {
        private readonly DataContext _dbContext;

        public FileUploadService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*    public async Task UploadExcelFile(IFormFile file)
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


            }*/

        public async Task<bool> UploadExcelFile(IFormFile file)
        {
            var itRequests = new List<ITRequest>();

            string fileName = file.FileName;

            // Check if a file with the same filename already exists
            if (_dbContext.UserFiles.Any(f => f.Filename == fileName))
            {
                // A file with the same filename already exists, return false.
                return false;
            }

            var sourceFileId = Guid.NewGuid().ToString();

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

            try
            {
                await _dbContext.SaveChangesAsync();
                // Data saved successfully, return true.
                return true;
            }
            catch (DbUpdateException)
            {
                // Handle any exception that might occur during saving if needed.
                // Return false to indicate failure.
                return false;
            }
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

        public async Task<List<ITRequestWithFile>> GetITRequestsWithFiles()
        {
            var query = from itRequest in _dbContext.ITRequests
                        join userFile in _dbContext.UserFiles
                        on itRequest.SourceFileId equals userFile.FileId
                        select new ITRequestWithFile
                        {
                            ITRequest = itRequest,
                            UserFile = userFile
                        };

            return await query.ToListAsync();
        }
    }
}

