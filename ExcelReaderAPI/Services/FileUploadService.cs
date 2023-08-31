using System.Data.SqlClient;
using System.Globalization;
using ExcelDataReader;
using ExcelReaderAPI.Models;

namespace ExcelReaderAPI.Services
{

    /*public interface IFileUploadService
    {
        Task<bool> UploadExcelFile(IFormFile file);
        Task<List<ITRequest>> GetExcelFileData();
        DateTime ConvertToDate(string dateString);
        Task<List<ITRequestWithFile>> GetITRequestsWithFiles();


    }*/

    public class FileUploadService
    {

        /*private readonly string _dbConnString;*/
        private readonly DatabaseHelper _databaseHelper;

        public FileUploadService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;

        }

        public string AddUserFiles()
        {

            var fileId = Guid.NewGuid();
            var filename = "Dummy File naem for now";
            var reqAuthor = "Abdul R";
            var uploadDate = DateTime.Now;

            string output;

            string userInsertionQuery =
                "insert into ExcelReaderDb2.dbo.UserFiles values" +
                "(@fileId, @fileName, @author, @uploadDate)";

            using (SqlConnection conn = new SqlConnection(_databaseHelper.GetDBConnString()))
            {
                SqlCommand cmd = new SqlCommand(userInsertionQuery, conn);
                cmd.Parameters.AddWithValue("@fileId", fileId);
                cmd.Parameters.AddWithValue("@fileName", filename);
                cmd.Parameters.AddWithValue("@author", reqAuthor);
                cmd.Parameters.AddWithValue("@uploadDate", uploadDate);

                try
                {
                    conn.Open();

                    // Execute the SQL command
                    output = $"{cmd.ExecuteNonQuery()} rows added!";

                    // The record has been inserted successfully
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during the execution
                    output = ex.Message;
                    Console.WriteLine("Error: " + ex.Message);
                }

            }

            return output;
        }

        /* public async Task<bool> UploadExcelFile(IFormFile file)
         {
             var itRequests = new List<ITRequest>();

             string fileName = file.FileName;

             // checking if a file with the same filename already exists
             *//*if (_dbContext.UserFiles.Any(f => f.Filename == fileName))
             {
                 return false;
             }*//*



             var sourceFileId = Guid.NewGuid();



             using (SqlConnection connection = new SqlConnection(connectionString))
             {
                 connection.Open();
                 // Do work here.  
             }

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

             *//* _dbContext.UserFiles.Add(userFileData);
              await _dbContext.SaveChangesAsync();
              _dbContext.ITRequests.AddRange(itRequests);*/

        /* try
         {
             await _dbContext.SaveChangesAsync();
             return true;
         }
         catch (DbUpdateException)
         {

             return false;
         }*//*
        return await { };
    }*/

        public DateTime ConvertToDate(string dateString)
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

        /* public async Task<List<ITRequest>> GetExcelFileData()
         {
             return { };*//*await _dbContext.ITRequests.ToListAsync();*//*
         }*/

        /* public async Task<List<ITRequestWithFile>> GetITRequestsWithFiles()
         {
             *//*var query = from itRequest in _dbContext.ITRequests
                         join userFile in _dbContext.UserFiles
                         on itRequest.SourceFileId equals userFile.FileId
                         select new ITRequestWithFile
                         {
                             ITRequest = itRequest,
                             UserFile = userFile
                         };*//*

             return await { }*//*query.ToListAsync();*//*
         }*/
    }
}


