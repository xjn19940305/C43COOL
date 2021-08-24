using AutoMapper;
using C43COOL.Domain;
using C43COOL.Domain.Base;
using C43COOL.Infrastructure;
using C43COOL.Models.Paging;
using C43COOL.Models.User.Management;
using C43COOL.Service.Global;
using C43COOL.Service.Interface.Management;
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
        private string Salt = string.Empty;
        public UserManagementService(
            C43DbContext dbContext,
            IMapper mapper,
            JwtService jwtService,
            IConfiguration configuration,
            IUserContext userContext
            )
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.jwtService = jwtService;
            this.configuration = configuration;
            this.userContext = userContext;
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
            if (!await dbContext.User.AnyAsync(x => x.PhoneNumber == model.Account))
                throw new Exception("账号不存在!");
            var pwd = model.Password.GetMd5WithSalt(Salt);
            var entity = await dbContext.User.FirstOrDefaultAsync(x => x.PhoneNumber == model.Account && x.Password == pwd);
            if (entity == null)
                throw new Exception("账号或密码错误!");
            if (!entity.Enabled)
                throw new Exception("该用户已被锁定!");
            var claims = new List<Claim>();
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, entity.Id),
                new Claim(IdentityModel.JwtClaimTypes.NickName,entity.Name),
                new Claim("avatar",!string.IsNullOrWhiteSpace(entity.Avatar)?entity.Avatar: string.Empty),
                new Claim("OpenId",!string.IsNullOrWhiteSpace(entity.OpenId)?entity.OpenId: string.Empty),
            });
            return jwtService.GetToken(claims);
        }
    }
}
