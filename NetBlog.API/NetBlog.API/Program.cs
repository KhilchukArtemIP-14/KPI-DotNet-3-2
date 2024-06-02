using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetBlog.Application.Features.Authorization.Commands;
using NetBlog.Domain.RepositoryContracts;
using NetBlog.Persistance.Context;
using NetBlog.Persistance.Mappings;
using NetBlog.Persistance.Repository.Implementations;
using System.Reflection;
using System.Text;

namespace NetBlog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<NetBlogDbContext>(options =>
            {
                options.UseSqlServer(connection, b => b.MigrationsAssembly("NetBlog.Persistance"));
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo() { Title = "NetBlog Api", Version = "v1" });

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
                    });
            });
            builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
            }
            );


            /*builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<ICommentsService, CommentService>();
            builder.Services.AddScoped<IUserSummaryService, UserSummaryService>();

            builder.Services.AddScoped<IAuthorizationRequirement, CanModifyPostRequirement>();
            builder.Services.AddScoped<IAuthorizationRequirement, CanDeleteCommentRequirement>();
            builder.Services.AddScoped<IAuthorizationRequirement, CanUpdateUserSummaryRequirement>();

            builder.Services.AddScoped<IAuthorizationHandler, CanModifyPostHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, CanDeleteCommentHandler>();
            builder.Services.AddScoped<IAuthorizationHandler, CanUpdateUserSummaryHandler>();*/

            builder.Services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NetBlog")
                .AddEntityFrameworkStores<NetBlogDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    AuthenticationType = "Jwt",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                });

            /*builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CanModifyPostPolicy", policy =>
                {
                    policy.Requirements.Add(new CanModifyPostRequirement());
                });
                options.AddPolicy("CanDeleteCommentPolicy", policy =>
                {
                    policy.Requirements.Add(new CanDeleteCommentRequirement());
                });
                options.AddPolicy("CanUpdateUserSummary", policy =>
                {
                    policy.Requirements.Add(new CanUpdateUserSummaryRequirement());
                });
            });*/

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
            });
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
