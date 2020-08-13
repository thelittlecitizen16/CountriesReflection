using System;
using System.Linq;
using System.Reflection;
using System.IO;

namespace CountriesReflection
{
    class Program
    {
        public static void GetMamasEmpireContinent(string className, TypeInfo country)
        {
            if (className == "MamasEmpire")
            {
                var countryAtrribute = country.CustomAttributes
                    .Select(x => x.ConstructorArguments.First()).ToList().First();

                Console.WriteLine($"the continent of {className} is : {countryAtrribute}");

            }
        }

        public static void GetPrivateFunctionWithTopSecretAttribute(string className, MethodInfo item)
        {
            if (item.IsPrivate == true)
            {
                if (item.GetCustomAttributes().Where(x => x.GetType().Name == "TopSecretAttribute").Count() > 0)
                {
                    Console.WriteLine($"WOW the country {className} have function {item.Name} that have TopSecretAttribute");
                }
            }
        }

        public static void TryInvokeMethode(Type type, MethodInfo item, object instance)
        {
            MethodInfo methode = type.GetMethod(item.Name);

            try
            {
                var response = methode.Invoke(instance, null);

                Console.WriteLine($"the { item.Name} is {response}");
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine($"the method {item.Name } not return");
            }
            catch (TargetParameterCountException e)
            {
                Console.WriteLine($"the method {item.Name } not return");
            }
            catch (Exception e)
            {
                Console.WriteLine($"the method {item.Name } not return");
            }

        }

        static void Main(string[] args)
        {
            string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var assembly = Assembly.LoadFile(Path.Combine(location, "Countries.dll"));

            var allCountries = assembly.DefinedTypes.ToList()
                .Where(d => d.ImplementedInterfaces.Any(i=>i.Name == "ICountry")).ToList();


            foreach (var country in allCountries)
            {
                Type type = assembly.GetType(country.FullName);
                var instance = Activator.CreateInstance(type);

                string className = country.Name;
                Console.WriteLine($"the class name: {className}");

                Console.ForegroundColor = ConsoleColor.Green;
                GetMamasEmpireContinent(className, country);
                Console.ForegroundColor = ConsoleColor.White;

                foreach (var item in country.DeclaredMethods)
                {
                    TryInvokeMethode(type, item, instance);

                    Console.ForegroundColor = ConsoleColor.Blue;
                    GetPrivateFunctionWithTopSecretAttribute(className, item);
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }
        }
    }
}
