using System.Collections.Generic;
using System.Linq;


namespace SwaggerLab
{
    public static class HeroGenerator
    {
        public static string GetHeroName(string firstName, string lastName)
        {
            var f = firstName.First();
            var l = lastName.First();

            return $"{GetHeroFirst(f)} {GetHeroLast(l)}";
        }


        public static string GetHeroFirst(char letter)
        {
            return sHeroFirst[char.ToUpper(letter)];
        }


        public static string GetHeroLast(char letter)
        {
            return HeroLast[char.ToUpper(letter)];
        }


        private static readonly Dictionary<char, string> sHeroFirst = new Dictionary<char, string>()
        {
             {'A', "Captain" },
             {'B', "Night" },
             {'C', "The" },
             {'D', "Ancient" },
             {'E', "Spider" },
             {'F', "Invisible" },
             {'G', "Master" },
             {'H', "Mr" },
             {'I', "Silver" },
             {'J', "Dark" },
             {'K', "Professor" },
             {'L', "Radioactive" },
             {'M', "Incredible" },
             {'N', "Impossible" },
             {'O', "Iron" },
             {'P', "Rocket" },
             {'Q', "Human" },
             {'R', "Power" },
             {'S', "Green" },
             {'T', "Super" },
             {'U', "Wonder" },
             {'V', "Metal" },
             {'W', "Doctor" },
             {'X', "Masked" },
             {'Y', "Crimson" },
             {'Z', "Omega" }
        };


        private static readonly Dictionary<char, string> HeroLast = new Dictionary<char, string>()
        {
             {'A', "Lightning" },
             {'B', "Knight" },
             {'C', "Hulk" },
             {'D', "Centurion" },
             {'E', "Surfer" },
             {'F', "Girl" },
             {'G', "Warrior" },
             {'H', "Man" },
             {'I', "Ghost" },
             {'J', "Master" },
             {'K', "Hornet" },
             {'L', "Phantom" },
             {'M', "Crusader" },
             {'N', "Daredevil" },
             {'O', "Machine" },
             {'P', "America" },
             {'Q', "X" },
             {'R', "Doom" },
             {'S', "Fist" },
             {'T', "Shadow" },
             {'U', "Patriot" },
             {'V', "Claw" },
             {'W', "Some" },
             {'X', "Some" },
             {'Y', "Some" },
             {'Z', "Some" }
        };
    }
}
