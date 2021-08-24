﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C43COOL.Redis
{
    public class RedisCache
    {
        private readonly IConfiguration configuration;
        private IDatabase db;
        private string prefix;
        public RedisCache(
            IConfiguration configuration
            )
        {
            this.configuration = configuration;
            var conn = this.configuration.GetSection("Redis:Connection").Value;
            prefix = this.configuration.GetSection("Redis:Prefix").Value;
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(conn);
            db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, DateTime? dt = null) where T : class
        {
            var typeName = typeof(T).FullName;
            var keyName = $"{prefix}:{typeName}";
            var CacheValue = await db.HashGetAllAsync(keyName);
            List<HashEntry> list = new List<HashEntry>();
            list.AddRange(CacheValue);
            list.Add(new HashEntry(key, JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver(),
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
                DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTime
            })));
            await db.HashSetAsync(keyName, list.ToArray());
            if (dt != null)
                await db.KeyExpireAsync(keyName, dt);
        }
        public async Task<T> GetAsync<T>(string key) where T : class
        {
            try
            {
                var typeName = typeof(T).FullName;
                var dt = await db.HashGetAllAsync($"{prefix}:{typeName}");
                var data = dt.FirstOrDefault(x => x.Name == key);
                return JsonConvert.DeserializeObject<T>(data.Value);
            }
            catch
            {
                return default(T);
            }

        }
        public async Task SetStringAsync(string key, string value, int expireSecond)
        {
            await db.StringSetAsync($"{prefix}:{key}", value, TimeSpan.FromSeconds(expireSecond));
        }

        public async Task<string> GetStringAsync(string key)
        {
            return await db.StringGetAsync($"{prefix}:{key}");
        }

        public async Task DeleteAsync(string key)
        {
            await db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 分布式锁
        /// </summary>
        /// <param name="key">锁名称</param>
        /// <param name="value">锁凭证</param>
        /// <param name="ts">乐观锁可以给时间</param>
        /// <returns></returns>
        public async Task<bool> LockAsync(string key, string value, TimeSpan ts)
        {
            var keyName = $"{prefix}:{key}";
            return await db.LockTakeAsync(keyName, value, ts);
        }
        /// <summary>
        /// 分布式解锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> UnLockASync(string key, string value)
        {
            var keyName = $"{prefix}:{key}";
            return await db.LockReleaseAsync(keyName, value);
        }
    }
}
