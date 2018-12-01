using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CreditCards.Core.Model
{
    /// <summary>
    /// The number is [member number: 6 digits] '-' [scheme identifier: 1 capital letter]
    /// </summary>
    public class FrequentFlyerValidator
    {
        private readonly char[] SchemeIdList = { 'A', 'Q', 'Y' };

        private const int NumberLength = 8;

        private const int MemberLength = 6;


        public bool IsValid(string number)
        {
            if (number is null)
            {
                throw new ArgumentNullException(nameof(number));
            }

            if (number.Length != NumberLength)
            {
                return false;
            }

            var parts = number.Split('-');

            if (parts[0].Length != MemberLength)
            {
                return false;
            }

            if (! int.TryParse(parts[0], NumberStyles.None, null, out int _))
            {
                return false;
            }

            return SchemeIdList.Contains(number.Last());
        }
    }
}
