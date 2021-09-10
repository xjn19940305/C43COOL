using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Infrastructure;
using C43COOL.Models.Modules.Management;
using C43COOL.Models.Paging;
using C43COOL.Models.User.Management;
using C43COOL.Service.Global;
using C43COOL.Service.Interface.Management;
using C43COOL.Service.Permission.Strategy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Service.Impl.Management
{
    public class UserManagementService : IUserManagementService
    {
        private readonly C43DbContext dbContext;
        private readonly IMapper mapper;
        private readonly JwtService jwtService;
        private readonly IConfiguration configuration;
        private readonly IUserContext userContext;
        private readonly AuthContextFactory authContextFactory;
        private string Salt = string.Empty;
        public UserManagementService(
            C43DbContext dbContext,
            IMapper mapper,
            JwtService jwtService,
            IConfiguration configuration,
            IUserContext userContext,
            AuthContextFactory authContextFactory
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.jwtService = jwtService;
            this.configuration = configuration;
            this.userContext = userContext;
            this.authContextFactory = authContextFactory;
            Salt = configuration.GetSection("Salt").Value;
        }

        public async Task<PagedViewModel> Query(UserListQueryModel request)
        {
            var model = new PagedViewModel();
            var data = mapper.ProjectTo<UserInfoModel>(dbContext.User.AsNoTracking());
            model.TotalElements = await data.CountAsync();
            model.Data = await data.OrderByDescending(x => x.DateCreated)
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();
            return model;
        }

        public async Task Create(UserCreateModel model)
        {
            if (await dbContext.User.AnyAsync(f => f.PhoneNumber == model.PhoneNumber))
                throw new Exception("用户手机号已存在!");
            var entity = mapper.Map<User>(model);
            entity.Enabled = true;
            entity.Password = "123456".GetMd5WithSalt(Salt);
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }
        public async Task Modify(UserModifyModel model)
        {
            var data = await dbContext.User.FirstOrDefaultAsync(x => x.Id == model.Id);
            var entity = mapper.Map(model, data);
            data.UpdateAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync();
        }
        public async Task Delete(string UserId)
        {
            var user = await dbContext.User.FirstOrDefaultAsync(x => x.Id == UserId);
            if (user != null)
            {
                dbContext.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task Enabled(UserEnabledModel model)
        {
            var UserList = await dbContext.User.Where(x => model.UserIds.Contains(x.Id)).ToListAsync();
            UserList.ForEach(f => f.Enabled = model.Enabled);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserInfoModel> Get(string UserId)
        {
            return mapper.Map<UserInfoModel>(await dbContext.User
               .Where(x => x.Id == UserId)
               .FirstOrDefaultAsync());
        }

        public async Task<UserInfoModel> GetUserInfo()
        {
            return mapper.Map<UserInfoModel>(await dbContext.User
                    .Where(x => x.Id == userContext.Id)
                    .FirstOrDefaultAsync());
        }

        public async Task<string> SignInWithPassword(LoginSignInWithPasswordModel model)
        {
            var User = await dbContext.User.FirstOrDefaultAsync(x => x.PhoneNumber == model.Account);
            if (User == null)
                throw new Exception("账号不存在!");
            var pwd = model.Password.GetMd5WithSalt(Salt);
            if (User.Password != pwd)
                throw new Exception("账号或密码错误!");
            if (!User.Enabled)
                throw new Exception("该用户已被锁定!");
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, User.Id),
                new Claim(IdentityModel.JwtClaimTypes.NickName,User?.NickName ?? string.Empty),
                new Claim(IdentityModel.JwtClaimTypes.Name,User?.Name ?? string.Empty),
                new Claim("avatar",User?.Avatar ?? string.Empty),
                new Claim("OpenId",User?.OpenId ?? string.Empty)
                //new Claim("",)
            });
            return jwtService.GetToken(claims);
        }

        public async Task<List<ModuleViewModel>> GetModules()
        {
            var strategy = authContextFactory.GetAuthStrategyContext(userContext.Name);
            return strategy.Modules;
        }

        public async Task BindRole(UserBindModel model)
        {
            var userRole = await dbContext.Relevances.Where(x => x.Key == Define.USERROLE && x.FirstId == model.UserId).ToListAsync();
            foreach (var RoleId in model.RoleIds)
            {
                var dt = userRole.FirstOrDefault(x => x.SencondId == RoleId);
                if (dt != null)
                    userRole.Remove(dt);
                else
                {
                    await dbContext.AddAsync(new Relevance
                    {
                        Key = Define.USERROLE,
                        FirstId = model.UserId,
                        SencondId = RoleId
                    });
                }
            }
            dbContext.RemoveRange(userRole);
            await dbContext.SaveChangesAsync();
        }

        public async Task<string[]> GetBindRoleList(string UserId)
        {
            return await dbContext.Relevances.Where(x => x.Key == Define.USERROLE && x.FirstId == UserId).Select(f => f.SencondId).ToArrayAsync();
        }
    }
}
