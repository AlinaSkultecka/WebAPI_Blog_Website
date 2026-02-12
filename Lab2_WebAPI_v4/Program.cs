using Lab2_WebAPI_v4.Core.Services;
using Lab2_WebAPI_v4.Core.Services.Interfaces;
using Lab2_WebAPI_v4.Data;
using Lab2_WebAPI_v4.Data.Interfaces;
using Lab2_WebAPI_v4.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


// -------------------- DATABASE --------------------

var connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Lab2_v4_Db;Integrated Security=True;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connString)
);


// -------------------- REPOSITORIES --------------------

builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IPostRepo, PostRepo>();
builder.Services.AddScoped<ICommentRepo, CommentRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();


// -------------------- SERVICES --------------------

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddAutoMapper(typeof(Lab2_WebAPI_v4.Core.Mapping.MappingProfile).Assembly);



// -------------------- JWT AUTHENTICATION --------------------

var jwtSettings = builder.Configuration.GetSection("Jwt");

var secret = jwtSettings["Key"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();


// -------------------- SWAGGER --------------------

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Blog Community API",
        Version = "v1",
        Description = "Web API for blog community (Lab 2)"
    });

    // Enable JWT in Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {your token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
});


var app = builder.Build();


// -------------------- MIDDLEWARE --------------------

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
