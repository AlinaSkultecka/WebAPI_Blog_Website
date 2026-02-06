using Lab2_WebAPI_v4.Data.DTOs;
using Lab2_WebAPI_v4.Data.Entities;
using Lab2_WebAPI_v4.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Lab2_WebAPI_v4.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Här skall vi ha CRUD funktionalitet mot databasen. Innebär att vi skall ha
        //endpoint som stöder GET, POST, PUT, DELETE
        private readonly IUserRepo _repo;

        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }

        //Endpoints i en controller skall vara tunna och inte innehålla
        //mycket kod. Bara ta emot en request och skicka den vidare till repot
        //och sedan skicka tillbaka en response
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_repo.GetAllUsers());
        }


        [HttpPost]
        public IActionResult AddUser(User user)
        {

            _repo.AddUser(user);

            return Created();

        }

        [HttpPut]
        public IActionResult UpdateUser(User user)
        {

            _repo.UpdateUser(user);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {

            if (id <= 0)
                return BadRequest();

            _repo.DeleteUser(id);
            return Ok();

        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto user)
        {
            var dbUser = _repo.GetAllUsers()
                .SingleOrDefault(u =>
                    u.UserName == user.UserName &&
                    u.Password == user.Password);

            if (dbUser == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, dbUser.UserName),
                new Claim("UserID", dbUser.UserID.ToString())
            };

            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("mykey1234567&%%485734579453%&//1255362"));

            var signinCredentials =
                new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5239",
                audience: "http://localhost:5239",
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: signinCredentials
            );

            var tokenString =
                new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }
    }
}
