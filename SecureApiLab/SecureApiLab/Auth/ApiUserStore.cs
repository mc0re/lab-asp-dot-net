using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SecureApiLab
{
    public class ApiUserStore<T> : IUserStore<T>, IUserPasswordStore<T>, IUserClaimStore<T> where T : IdentityUser
    {
        private List<T> mList = new List<T>();


        void IDisposable.Dispose()
        {
            // Do nothing
        }


        Task<IdentityResult> IUserStore<T>.CreateAsync(T user, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = user.UserName.ToUpper();
            mList.Add(user);
            return Task.FromResult(IdentityResult.Success);
        }


        Task<IdentityResult> IUserStore<T>.DeleteAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "no", Description = "Cannot delete a user" }));
        }


        Task<T> IUserStore<T>.FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(mList.First(u => u.Id == userId));
        }


        Task<T> IUserStore<T>.FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(mList.FirstOrDefault(u => u.NormalizedUserName == normalizedUserName));
        }


        Task<string> IUserStore<T>.GetNormalizedUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }


        Task<string> IUserStore<T>.GetUserIdAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }


        Task<string> IUserStore<T>.GetUserNameAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }


        Task IUserStore<T>.SetNormalizedUserNameAsync(T user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }


        Task IUserStore<T>.SetUserNameAsync(T user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }


        Task<IdentityResult> IUserStore<T>.UpdateAsync(T user, CancellationToken cancellationToken)
        {
            var idx = mList.FindIndex(u => u.Id == user.Id);
            mList[idx] = user;

            return Task.FromResult(IdentityResult.Success);
        }


        #region IUserPasswordStore implementation

        Task IUserPasswordStore<T>.SetPasswordHashAsync(T user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }


        Task<string> IUserPasswordStore<T>.GetPasswordHashAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }


        Task<bool> IUserPasswordStore<T>.HasPasswordAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        #endregion


        #region IUserClaimStore implementation

        private IDictionary<T, IList<Claim>> mClaims = new Dictionary<T, IList<Claim>>();


        Task<IList<Claim>> IUserClaimStore<T>.GetClaimsAsync(T user, CancellationToken cancellationToken)
        { 
            if (mClaims.TryGetValue(user, out var claims))
                return Task.FromResult(claims);

            return Task.FromResult((IList<Claim>) new Claim[] { });
        }


        Task IUserClaimStore<T>.AddClaimsAsync(T user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (mClaims.TryGetValue(user, out var existing))
            {
                foreach (var cl in claims)
                    existing.Add(cl);
            }
            else
            {
                var clList = new List<Claim>(claims);
                mClaims.Add(user, clList);
            }

            return Task.CompletedTask;
        }


        Task IUserClaimStore<T>.ReplaceClaimAsync(T user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            if (mClaims.TryGetValue(user, out var existing))
            {
                if (existing.Contains(claim))
                    existing.Remove(claim);

                existing.Add(newClaim);
            }
            else
            {
                var clList = new List<Claim>();
                clList.Add(newClaim);
                mClaims.Add(user, clList);
            }

            return Task.CompletedTask;
        }


        Task IUserClaimStore<T>.RemoveClaimsAsync(T user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            if (mClaims.TryGetValue(user, out var existing))
            {
                foreach (var cl in claims)
                    existing.Remove(cl);
            }

            return Task.CompletedTask;
        }


        Task<IList<T>> IUserClaimStore<T>.GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            var userList = new List<T>();

            foreach (var kv in mClaims)
                foreach (var cl in kv.Value)
                {
                    if (cl == claim)
                        userList.Add(kv.Key);
                }

            return Task.FromResult((IList<T>) userList);
        }

        #endregion
    }
}