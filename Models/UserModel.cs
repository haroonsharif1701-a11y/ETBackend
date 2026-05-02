using ExpenseTracker.Data;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Models
{
    public class LoginRequestDTO
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public int IdUser { get; set; }
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
    }

    public class RegisterRequestDTO
    {
        public string LoginName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string RegId { get; set; }
        public string OS { get; set; }
    }

    public class RegisterResponseDTO
    {
        public int IdUser { get; set; }
        public string Message { get; set; }
    }

    public class UserModel
    {
        private readonly Db _db;

        public UserModel(Db db)
        {
            _db = db;
        }

        public LoginResponseDTO Login(string loginName, string password)
        {
            using (var cmd = _db.GetCommand("loginverify"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // HASH password (MUST match SQL)
                byte[] hashedPassword;
                using (var sha = SHA256.Create())
                {
                    hashedPassword = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                }

                cmd.Parameters.Add("@loginname", SqlDbType.VarChar).Value = loginName;
                cmd.Parameters.Add("@password", SqlDbType.VarBinary).Value = hashedPassword;

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    reader.Read();

                    return new LoginResponseDTO
                    {
                        IdUser = Convert.ToInt32(reader["id_user"]),
                        LoginName = reader["loginname"].ToString(),
                        FullName = reader["fullname"].ToString()
                    };
                }
            }
        }

        public RegisterResponseDTO Register(RegisterRequestDTO request)
        {
            using (var cmd = _db.GetCommand("user_addupd"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                var idParam = new SqlParameter("@id_user", SqlDbType.Int)
                {
                    Direction = ParameterDirection.InputOutput,
                    Value = 0
                };

                cmd.Parameters.Add(idParam);
                cmd.Parameters.Add("@loginname", SqlDbType.VarChar).Value = request.LoginName;
                cmd.Parameters.Add("@fullname", SqlDbType.VarChar).Value = request.FullName;
                cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = request.Password;
                cmd.Parameters.Add("@is_active", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@regid", SqlDbType.VarChar).Value = (object)request.RegId ?? DBNull.Value;
                cmd.Parameters.Add("@os", SqlDbType.VarChar).Value = (object)request.OS ?? DBNull.Value;
                cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = DBNull.Value;

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return new RegisterResponseDTO { Message = "Failed" };

                    reader.Read();

                    return new RegisterResponseDTO
                    {
                        IdUser = Convert.ToInt32(reader["id_user"]),
                        Message = reader["message"].ToString()
                    };
                }
            }
        }
    }

}
