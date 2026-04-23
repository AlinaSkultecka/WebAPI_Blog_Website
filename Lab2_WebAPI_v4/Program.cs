using Azure.Storage.Blobs;
using Lab2_WebAPI_v4;
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

// -------------------- CONTROLLERS --------------------
builder.Services.AddControllers();


// -------------------- DATABASE --------------------

// First try to get connection string from configuration.
// In Azure, this can come from App Service -> Key Vault reference.
// Locally, this comes from appsettings.json.
var connString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connString))
{
    throw new Exception("DefaultConnection is missing from configuration.");
}

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


// -------------------- AZURE BLOB STORAGE --------------------

builder.Services.AddSingleton(x =>
{
    var configuration = x.GetRequiredService<IConfiguration>();
    var blobConnectionString = configuration.GetConnectionString("AzureBlobStorage");

    if (string.IsNullOrWhiteSpace(blobConnectionString))
        throw new Exception("AzureBlobStorage connection string is missing.");

    return new BlobServiceClient(blobConnectionString);
});

builder.Services.AddScoped<BlobLoggingService>();

// -------------------- JWT AUTHENTICATION --------------------

var jwtSettings = builder.Configuration.GetSection("Jwt");
var secret = jwtSettings["Key"];
var issuer = jwtSettings["Issuer"];
var audience = jwtSettings["Audience"];

if (string.IsNullOrWhiteSpace(secret))
{
    throw new Exception("JWT secret key is missing.");
}

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
            ValidIssuer = issuer,
            ValidAudience = audience,
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
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
});


var app = builder.Build();


// -------------------- MIDDLEWARE --------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/ping", () => "API is running");

app.Run();
