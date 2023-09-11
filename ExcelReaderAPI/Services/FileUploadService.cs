using System.Data;
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

        public void AddUserFiles(Guid fileId, string filename, SqlConnection connection)
        {
            var reqAuthor = "Sikandar R";
            var uploadDate = DateTime.Now;

            string output;

            string userInsertionQuery =
                "insert into ExcelReaderDb2.dbo.UserFiles values" +
                "(@fileId, @fileName, @author, @uploadDate)";

            /* using (SqlConnection conn = new SqlConnection(_databaseHelper.GetDBConnString()))
             {*/
            SqlCommand cmd = new SqlCommand(userInsertionQuery, connection);
            cmd.Parameters.AddWithValue("@fileId", fileId);
            cmd.Parameters.AddWithValue("@fileName", filename);
            cmd.Parameters.AddWithValue("@author", reqAuthor);
            cmd.Parameters.AddWithValue("@uploadDate", uploadDate);

            try
            {
                /*conn.Open();*/

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

            /*}*/

            /* return output;*/
        }

        public string UploadExcelFile(IFormFile file)
        {


            string fileName = file.FileName;
            List<ITRequest> ITRequestsFromFile;
            var sourceFileId = Guid.NewGuid();

            bool fileAreadyInDB;
            int rowsEffectCount = 0;

            using (var conn = new SqlConnection(_databaseHelper.GetDBConnString()))
            {
                conn.Open();
                var checkFileExistQuery = "select COUNT('FILE_ID') from dbo.UserFiles where filename = @filename";

                var checkFileExistCmd = new SqlCommand(checkFileExistQuery, conn);

                checkFileExistCmd.Parameters.AddWithValue("@filename", fileName);
                var filesInDb = (Int32)checkFileExistCmd.ExecuteScalar();
                fileAreadyInDB = filesInDb > 0;

                if (fileAreadyInDB) return "File Already Present in DB";

                AddUserFiles(sourceFileId, fileName, conn);

                ITRequestsFromFile = ExtractDataFromExcelFile(file, sourceFileId);

                // Create a SqlCommand for the stored procedure
                var cmd = new SqlCommand("Upload_IT_Request", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add parameters for the stored procedure
                cmd.Parameters.Add(new SqlParameter("@RequestId", SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new SqlParameter("@Author", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@Subject", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@Body", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@SourceFileId", SqlDbType.UniqueIdentifier, -1));
                cmd.Parameters.Add(new SqlParameter("@Request_Date", SqlDbType.DateTime2));
                cmd.Parameters.Add(new SqlParameter("@Completion_Date", SqlDbType.DateTime2));
                cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar, -1));

                try
                {
                    foreach (var itRequest in ITRequestsFromFile)
                    {
                        // Set parameter values for each ITRequest object
                        cmd.Parameters["@RequestId"].Value = itRequest.RequestId;
                        cmd.Parameters["@Author"].Value = itRequest.Author;
                        cmd.Parameters["@Type"].Value = itRequest.Type;
                        cmd.Parameters["@Subject"].Value = itRequest.Subject;
                        cmd.Parameters["@Body"].Value = itRequest.Body;
                        cmd.Parameters["@SourceFileId"].Value = itRequest.SourceFileId;
                        cmd.Parameters["@Request_Date"].Value = itRequest.RequestSubmissionDate;
                        cmd.Parameters["@Completion_Date"].Value = itRequest.RequestCompletionDate;
                        cmd.Parameters["@Status"].Value = itRequest.Status;


                        var response = cmd.ExecuteNonQuery();
                        rowsEffectCount += response;

                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during execution
                    Console.WriteLine("Error: " + ex.Message);
                }




            }


            return $"{rowsEffectCount} rows added";





        }


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

        public List<ITRequest> ExtractDataFromExcelFile(IFormFile file, Guid fileId)
        {

            var itRequests = new List<ITRequest>();

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
                            SourceFileId = fileId,
                            RequestSubmissionDate = ConvertToDate(reader.GetValue(5).ToString()),
                            RequestCompletionDate = ConvertToDate(reader.GetValue(6).ToString()),
                            Status = reader.GetValue(7).ToString(),
                        });
                    }
                }
            }

            return itRequests;

        }

        public List<ITRequestWithFile> GetAllItRequests()
        {
            var itRequests = new List<ITRequestWithFile>();

            using (SqlConnection connection = new SqlConnection(_databaseHelper.GetDBConnString()))
            {
                /*using (var command = new SqlCommand("GET_IT_REQUESTS", connection))*/
                using (var command = new SqlCommand("IT_REQUESTS_WITH_FILE", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var itRequest = new ITRequest
                            {
                                RequestId = (Guid)reader["RequestId"],
                                Author = reader["Author"].ToString(),
                                Type = reader["Type"].ToString(),
                                Subject = reader["Subject"].ToString(),
                                Body = reader["Body"].ToString(),
                                SourceFileId = (Guid)reader["SourceFileId"],
                                RequestSubmissionDate = (DateTime)reader["Request_Date"],
                                RequestCompletionDate = (DateTime)reader["Completion_Date"],
                                Status = reader["Status"].ToString()
                            };

                            var userFile = new UserFile
                            {
                                FileId = (Guid)reader["File_Id"],
                                Filename = reader["Filename"].ToString(),
                                Owner = reader["Owner"].ToString(),
                                UploadDate = (DateTime)reader["UploadDate"]
                            };

                            var itRequestWithFile = new ITRequestWithFile
                            {
                                ITRequest = itRequest,
                                UserFile = userFile
                            };


                            /*var itRequestWithFile = new ITRequestWithFile
                            {

                            }*/
                            itRequests.Add(itRequestWithFile);
                        }
                    }
                }
            }

            return itRequests;
        }

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


