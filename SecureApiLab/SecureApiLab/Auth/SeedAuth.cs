using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureApiLab
{
    public class SeedAuth
    {
        private readonly IRoleStore<IdentityRole> mRoleStore;

        private readonly IUserStore<ApiUser> mUserStore;
        private readonly IPasswordHasher<ApiUser> mHasher;


        public SeedAuth(IRoleStore<IdentityRole> roleStore, IUserStore<ApiUser> userStore, IPasswordHasher<ApiUser> hasher)
        {
            mRoleStore = roleStore;
            mUserStore = userStore;
            mHasher = hasher;
        }


        public async Task SeedAsync()
        {
            var cts = new CancellationTokenSource();

            var role = new IdentityRole("Admin");
            await mRoleStore.CreateAsync(role, cts.Token);

            var user = new ApiUser { Id = "mike", UserName = "mike" };
            user.PasswordHash = mHasher.HashPassword(user, "123");
            
            await mUserStore.CreateAsync(user, cts.Token);
        }
    }
}
