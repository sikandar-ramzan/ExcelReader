using ExcelDataReader;
using ExcelReader.Data;
using ExcelReader.Models;
using System.Globalization;
using Microsoft.EntityFrameworkCore;


namespace ExcelReader.Services
{
    public interface IFileUploadService
    {
        Task<bool> UploadExcelFile(IFormFile file);
        Task<List<ItRequest>> GetExcelFileData();
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


        public async Task<bool> UploadExcelFile(IFormFile file)
        {
            var itRequests = new List<ItRequest>();

            string fileName = file.FileName;

            // checking if a file with the same filename already exists
            if (_dbContext.UserFiles.Any(f => f.Filename == fileName))
            {
                return false;
            }

            var sourceFileId = Guid.NewGuid();

            using (var stream = file.OpenReadStream())
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        itRequests.Add(new ItRequest
                        {
                            RequestId = Guid.NewGuid(),
                            Author = reader.GetValue(1).ToString() ?? "",
                            Type = reader.GetValue(2).ToString() ?? "",
                            Subject = reader.GetValue(3).ToString() ?? "",
                            Body = reader.GetValue(4).ToString() ?? "",
                            SourceFileId = sourceFileId,
                            RequestSubmissionDate = ConvertToDate(reader.GetValue(5).ToString()),
                            RequestCompletionDate = ConvertToDate(reader.GetValue(6).ToString()),
                            Status = reader.GetValue(7).ToString() ?? "",
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

            _dbContext.UserFiles.Add(userFileData);
            await _dbContext.SaveChangesAsync();
            _dbContext.ItRequests.AddRange(itRequests);

            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {

                return false;
            }
        }

        public DateTime ConvertToDate(string? dateString)
        {
            string[] dateFormats = { "yyyy-MM-dd", "MM/dd/yyyy", "dd/MM/yyyy" };

            if (DateTime.TryParseExact(dateString, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                return new DateTime();
            }
        }

        public async Task<List<ItRequest>> GetExcelFileData()
        {
            return await _dbContext.ItRequests.ToListAsync();
        }

        public async Task<List<ITRequestWithFile>> GetITRequestsWithFiles()
        {
            var query = from itRequest in _dbContext.ItRequests
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

