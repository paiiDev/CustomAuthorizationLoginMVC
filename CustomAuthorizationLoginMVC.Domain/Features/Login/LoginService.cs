using CustomAuthorizationLoginMVC.Database.DataAccess;
using CustomAuthorizationLoginMVC.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAuthorizationLoginMVC.Domain.Features.Login
{
    public class LoginService
    {
        private readonly AppDbContext _db;
        public LoginService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var user = _db.TblUsers.FirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);
                if (user is null)
                {
                    throw new Exception("Invalid username or password.");
                }

                TblLogin login = new TblLogin
                {
                    UserId = Convert.ToInt32(user.UserId),
                    SessionId = Guid.NewGuid().ToString(),
                    SessionExpired = DateTime.Now.AddHours(1)
                };

                _db.TblLogin.Add(login);
                await _db.SaveChangesAsync();

                return new LoginResponseDto
                {
                    UserId = user.UserId.ToString(),
                    SessionId = login.SessionId,
                    SessionExpired = login.SessionExpired
                };
            } catch (Exception ex)
            {
                throw new Exception("An error occurred during login: " + ex.Message);
            }
           
        }
    }
}
