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

                var d = countries.DeclaredMethods.First().DeclaringType;
                foreach (var item in countries.DeclaredMethods)
                {
                    Console.WriteLine(item.Name);
                    MethodInfo methode = type.GetMethod(item.Name);
                     
                    try
                    {
                        var response = methode.Invoke(instance, null);
                        Console.WriteLine($"the { item.Name} is {response}");

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("the method not return");
                    }


                }

               

                

                MethodInfo methodePopulation = type.GetMethod("get_Population");
                var responsePopulation = methodePopulation.Invoke(instance, null);

                MethodInfo methodeCalculateSize = type.GetMethod("CalculateSize");
                var responseCalculateSize = methodeCalculateSize.Invoke(instance, null);

             //   Console.WriteLine($"the class name: {className} and the name: {responseName} and the population: {responsePopulation} and the size: {responseCalculateSize}");


            }
            
            Console.WriteLine("Hello World!");
        }
    }
}
