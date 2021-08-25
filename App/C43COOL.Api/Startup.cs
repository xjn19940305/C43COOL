using AutoMapper;
using C43COOL.Domain;
using C43COOL.Models.JobModel;
using C43COOL.Models.Jwt;
using C43COOL.Redis;
using C43COOL.ScheduleTask;
using C43COOL.ScheduleTask.Jobs;
using C43COOL.Service.Global;
using C43COOL.Service.Impl.Management;
using C43COOL.Service.Interface.Management;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder =>
        {
            #if DEBUG   
                builder.AddConsole();
            #endif
        });
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(o => o.Filters.Add(typeof(CustomeExceptionFilter)))
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                   options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                   options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime;
               });
            services.AddHttpClient().AddHttpContextAccessor();
            services
               .AddHsts(o =>
               {
                   o.IncludeSubDomains = true;
               })
               .AddCors(o =>
               {
                   o.AddDefaultPolicy(p =>
                   {
                       p.SetIsOriginAllowed(domain => true);
                       p.AllowAnyMethod();
                       p.AllowAnyHeader();
                       p.AllowCredentials();
                   });
               })
               .Configure<ForwardedHeadersOptions>(options =>
               {
                   options.ForwardedHeaders =
                     ForwardedHeaders.All;
                   options.KnownNetworks.Clear();
                   options.KnownProxies.Clear();
               })
               .AddDataProtection();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("manager", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "C43COOL Management",
                    Description = "Manager Web Api",
                    TermsOfService = new Uri("https://example.com/terms")
                });
                c.SwaggerDoc("front", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "C43COOL",
                    Description = "Front Web Api",
                    TermsOfService = new Uri("https://example.com/terms")
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "\u5728\u4e0b\u6846\u4e2d\u8f93\u5165\u8bf7\u6c42\u5934\u4e2d\u9700\u8981\u6dfb\u52a0\u004a\u0077\u0074\u6388\u6743\u0054\u006f\u006b\u0065\u006e\uff1a\u0042\u0065\u0061\u0072\u0065\u0072\u0020\u0054\u006f\u006b\u0065\u006e",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                var xmlMSLPModelFile = $"{Assembly.Load("C43COOL.Models").GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlMSLPModelFile));
            });
            var config = Configuration.GetSection("DB_CONNECTION");
            services.AddDbContext<C43DbContext>(setup =>
            {
                setup.UseMySql(config.Value, ServerVersion.AutoDetect(config.Value), options =>
                {
                    options.MigrationsAssembly(System.Reflection.Assembly.Load("C43COOL.MySql").FullName)
                    .EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                }).UseLoggerFactory(MyLoggerFactory);
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    //获取验证失败的模型字段 
                    var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                   .FirstOrDefault();
                    //设置返回内容
                    var result = new ObjectResult(new ErrorResultModel
                    {
                        Code = "FieldValidation",
                        Message = errors,
                    });
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    return result;
                };
            });

            services.AddAutoMapper(
                Assembly.Load("C43COOL.Service"),
                Assembly.Load("C43COOL.ScheduleTask")
            );
            services.AddHealthChecks();
            services.AddAuthorization(config =>
            {
                //授权指定策略
                config.AddPolicy("o", policy =>
                {
                    policy.RequireClaim("role", "admin");
                });
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", options =>
            {
                //options.Events.OnChallenge = async (ctx) =>
                //{
                //    var ss = "";
                //};
                var jst = new JwtSecurityTokenHandler();
                jst.OutboundClaimTypeMap.Clear();
                jst.InboundClaimTypeMap.Clear();
                options.Audience = Configuration["JwtConfig:Audience"];
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(jst);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //是否调用对签名securityToken的SecurityKey进行验证。
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtConfig:SecurityKey"])),
                    //是否验证发行者
                    ValidateIssuer = true,
                    //将用于检查令牌的发行者是否与此发行者相同。
                    ValidIssuer = Configuration["JwtConfig:Issuer"],
                    //检查令牌的受众群体是否与此受众群体相同。
                    ValidateAudience = true,
                    //在令牌验证期间验证受众 。
                    ValidAudience = Configuration["JwtConfig:Audience"],
                    //验证生命周期                    
                    ValidateLifetime = true,
                    // If you want to allow a certain amount of clock drift, set that here
                    ClockSkew = TimeSpan.Zero
                };
                //定义在授权成功后是否应将持有者令牌存储在中 AuthenticationProperties 。
                options.SaveToken = true;
                options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                {
                    OnMessageReceived = (context) =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (string.IsNullOrEmpty(accessToken) == false)
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.Events.OnAuthenticationFailed = (ctx) =>
                {
                    if (ctx.Exception.GetType().Name == "SecurityTokenExpiredException")
                        ctx.Response.Headers["SecurityTokenExpired"] = "expired";
                    return Task.CompletedTask;
                };
            });
            //依赖注入
            services.TryAddScoped<JwtService>();
            services.TryAddSingleton<RedisCache>();
            services.TryAddScoped<IUserContext, OIDCUserContext>();
            services.TryAddScoped<IUserManagementService, UserManagementService>();
            services.TryAddScoped<IRoleManagementService, RoleManagementService>();
            services.TryAddScoped<IModulesManagementService, ModulesManagementService>();
            services.TryAddScoped<IDepartmentManagementService, DepartmentManagementService>();
            //添加Quartz服务
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            //通用注册服务
            services.AddSingleton<ScheduleService>();
            //注册全局调度任务
            services.AddSingleton<GlobalNotificationJob>();
            services.AddSingleton<DemoJob>();
            //services.AddSingleton<SignInBefore>();
            services.AddSingleton(new JobScheduleDTO { JobName = "全局调度器", JobType = typeof(GlobalNotificationJob), CronExpression = "0/59 0/1 * * * ? *" });
            //全局调度任务注册为宿主服务随程序启动关闭
            services.AddHostedService<CustomeScheduleHostService>();

            services.Configure<JwtConfig>(Configuration.GetSection("JwtConfig"));
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c => c.RouteTemplate = "swagger/{documentName}/swagger.json");
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/manager/swagger.json", "Manager");
                    c.SwaggerEndpoint("/swagger/front/swagger.json", "Front");
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
