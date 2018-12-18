using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureApiLab
{
    internal class ApiRoleStore<T> : IRoleStore<T> where T : IdentityRole
    {
        private List<T> mList = new List<T>();


        public void Dispose()
        {
            // Do nothing
        }


        public Task<IdentityResult> CreateAsync(T role, CancellationToken cancellationToken)
        {
            mList.Add(role);
            return Task.FromResult(IdentityResult.Success);
        }


        public Task<IdentityResult> DeleteAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "no", Description = "Cannot delete a role" }));
        }


        public Task<T> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(mList.First(r => r.Id == roleId));
        }


        public Task<T> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(mList.First(r => r.NormalizedName == normalizedRoleName));
        }


        public Task<string> GetNormalizedRoleNameAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }


        public Task<string> GetRoleIdAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }


        public Task<string> GetRoleNameAsync(T role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }


        public Task SetNormalizedRoleNameAsync(T role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }


        public Task SetRoleNameAsync(T role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }


        public Task<IdentityResult> UpdateAsync(T role, CancellationToken cancellationToken)
        {
            var idx = mList.FindIndex(r => r.Id == role.Id);
            mList[idx] = role;

            return Task.FromResult(IdentityResult.Success);
        }
    }
}