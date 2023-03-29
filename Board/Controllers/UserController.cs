using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Board.Views.User
{
    using Board.Data;
    using Board.Models;
    using Dropbox.Api.TeamLog;
    using Microsoft.EntityFrameworkCore;
    using static Dropbox.Api.Files.ListRevisionsMode;

    public class UserController : Controller
    {

        private readonly BoardContext _context;

        public UserController(BoardContext context)
        {
            _context = context;
        }

        //GET JOin
        public async Task<IActionResult> Join()
        {

            return View();
        }

        //Post JOIN
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(string password, string username, string email)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                //이메일 중복확인 
                var emailcheck = _context.User.FirstOrDefault(m => m.Email == email);

                if (emailcheck == null)
                {
                    //비밀번호 해쉬부분
                    byte[] salt = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                    string hashedPassword = HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), salt);
                    string passwordAndSalt = hashedPassword + "|" + Convert.ToBase64String(salt);

                    user.HashPassword = passwordAndSalt;
                    user.UserNickName = username;
                    user.Email = email;
                    user.UpdateDate = DateTime.Now;

                    try
                    {
                        _context.Add(user);
                        await _context.SaveChangesAsync();
                        //여기서 저장까지 되는데 return으로 넘어가지 않는다.
                        //ajax로도 계속 실패라고 뜨고

                         //그냥 return view일때는 성공이되는데 return redirect일때 안됨// ajax통신이라서 그런 듯. ajax result로 값이 넘어가니까 view에서 넘어가게 해야함
                    }
                    catch (DbUpdateException ex)
                    {
                        //Db저장에 실패했을때 메시지를 볼 수 있게 해준다. 아주 중요!
                        Console.WriteLine("Inner Exception: " + ex.InnerException?.Message);
                    }
                }
                else
                {
                    Console.WriteLine("email이 이미 존재합니다.");
                }
            }
            return View();
        }//post Join


        //해쉬를 받는 메소드
        private static string HashPasswordWithSalt(byte[] password, byte[] salt)
        {
            using (var sha = SHA256.Create())
            {
                byte[] saltedPassword = new byte[password.Length + salt.Length];
                Array.Copy(password, saltedPassword, password.Length);
                Array.Copy(salt, 0, saltedPassword, password.Length, salt.Length);
                byte[] hash = sha.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }//hash

        //Get Login
        public async Task<IActionResult> Login()
        {
            return View();
        }

        //Post Login
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid) { 
                
                Notice notice = new Notice();
                var user = _context.User.FirstOrDefault(x => x.Email == email);

                if (user != null) {
                    //password 비교하고 맞으면 게시판 페이지로 넘어간다.
                    //비밀번호 해쉬부분
                    string[] parts = user.HashPassword.Split('|');
                    string savedHashedPassword = parts[0];
                    string savedSalt = parts[1];

                    byte[] saltBytes = Convert.FromBase64String(savedSalt);
                    string hashedPassword = HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), saltBytes);
                    string passwordAndSalt = hashedPassword + "|" + savedSalt;

                    if (savedHashedPassword == hashedPassword)
                    {
                        return Json(new { success = true, data = new { id = user.UserId } });
                    }
                    else
                    {
                        return Json(new { success = false, errorMessage = "비밀번호가 일치하지 않습니다." });
                    }
                }
                
            }
            return View();
        }

        } //UserController : Controller


}//namespace
