using ExpenseTracker.Helper;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class AuthService
    {
        private readonly UserModel _userModel;
        private readonly JwtHelper _jwt;

        public AuthService(UserModel userModel, JwtHelper jwt)
        {
            _userModel = userModel;
            _jwt = jwt;
        }

        public LoginResponseDTO Login(LoginRequestDTO request)
        {
            var user = _userModel.Login(request.LoginName, request.Password);

            if (user == null)
                return null;

            user.Token = _jwt.GenerateToken(user);

            return user;
        }
        public RegisterResponseDTO Register(RegisterRequestDTO request)
        {
            // Basic validation (keep business logic here)
            if (string.IsNullOrWhiteSpace(request.LoginName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return new RegisterResponseDTO
                {
                    Message = "LoginName and Password are required"
                };
            }

            return _userModel.Register(request);
        }
    }
}
