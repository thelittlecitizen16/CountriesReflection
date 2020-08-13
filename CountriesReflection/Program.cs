using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CountriesReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            var assembly = Assembly.LoadFile(@"C:\Users\thelittlecitizen16\source\repos\CountriesReflection\CountriesReflection\Countries.dll");

            var allCountries = assembly.DefinedTypes.ToList().Where(d => d.ImplementedInterfaces.Any(i=>i.Name == "ICountry")).ToList();


            foreach (var countries in allCountries)
            {
                Type type = assembly.GetType(countries.FullName);
                var instance = Activator.CreateInstance(type);

                string className = countries.Name;
                Console.WriteLine($"the class name: {className}");

                if (className == "MamasEmpire")
                {
                    var countryAtrribute = countries.CustomAttributes.Select(x=>x.ConstructorArguments.First()).ToList().First();
                    Console.WriteLine($"the continent is : {countryAtrribute}");

                }


                foreach (var item in countries.DeclaredMethods)
                {
                    MethodInfo methode = type.GetMethod(item.Name);
                  
                    try
                    {
                        var response = methode.Invoke(instance, null);
                        Console.WriteLine($"the { item.Name} is {response}");
                      
                    }
                    catch(NullReferenceException e)
                    {
                        Console.WriteLine($"the method {item.Name } not return");
                    }
                    catch (TargetParameterCountException e)
                    {
                        Console.WriteLine($"the method {item.Name } not return");
                    }


                    if (item.IsPrivate == true)
                    {
                        if (item.GetCustomAttributes().Where(x => x.GetType().Name == "TopSecretAttribute").Count() > 0)
                        {
                            Console.WriteLine($"WOW the country {className} have function {item.Name} that have TopSecretAttribute");
                        }
                    }
                }

            }
        }
    }
}
