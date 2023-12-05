using BusinessLogicLayer;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Service.Contract;
using BusinessLogicLayer.Service.Service;
using DataAccessLayer.Model;
using DataAccessLayer.Repository.Contract;
using DataAccessLayer.Repository.Repository;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Validation;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

builder.Services.AddSingleton(jwtOptions);

builder.Services.AddScoped<IValidator<NewUserDTO>, UserValidator>();
builder.Services.AddDbContext<CoderByteAssessmentDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<IRepositoryWrapper, WrapperRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Configuring the Authentication Service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
    {
        //convert the string signing key to byte array
        byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SigningKey);

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
        };
    });

// Configuring the Authorization Service
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/User", async ([FromBody] NewUserDTO newUserDTO, IValidator<NewUserDTO> validator, IUserService _userService,
    ITokenService _tokenService, JwtOptions jwtOptions) =>
{
    ValidationResult validationResult = await validator.ValidateAsync(newUserDTO);
    if (!validationResult.IsValid)
        return Results.ValidationProblem(validationResult.ToDictionary());

    CreateUserResponseDTO createUserResponseDTO = await _userService.AddNewUserAsync(newUserDTO);
    createUserResponseDTO.AccessToken = _tokenService.CreateAccessToken(jwtOptions, createUserResponseDTO.Id);

    return Results.Ok(new { createUserResponseDTO.Id, createUserResponseDTO.AccessToken });
});

app.MapGet("/Get", async (string id, IUserService userService) =>
{
    return await userService.GetUserByID(id);
}).RequireAuthorization();

app.Run();

public partial class Program { }