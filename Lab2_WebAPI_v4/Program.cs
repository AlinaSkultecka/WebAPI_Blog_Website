using Lab2_WebAPI_v4.Data;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


var connString = "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Lab2_v4_Db; Integrated Security = True; Connect Timeout = 30; Encrypt = True; Trust Server Certificate=False; Application Intent = ReadWrite; Multi Subnet Failover=False; Command Timeout = 30\r\n";

//Skapar upp EF som en service som kan injectas till repot
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString)
);

//Sätter upp repot så att det kan injectas i controllern
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();


var app = builder.Build();

app.MapControllers();

app.Run();