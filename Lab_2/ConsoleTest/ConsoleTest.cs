using System;
using Newtonsoft.Json;
namespace Listsoft
{
    namespace Lab_Faker
    {
        class ConsoleTest
        {
            /*
            public static void DisplayTypeInfo(Type t)
            {
                Console.WriteLine("\r\n{0}", t);
                Console.WriteLine("\tIs this an array? {0}",
                t.IsArray);
                Console.WriteLine("\tIs this a generic type definition? {0}",
                    t.IsGenericTypeDefinition);
                Console.WriteLine("\tIs it a generic type? {0}",
                    t.IsGenericType);
                Type[] typeArguments = t.GetGenericArguments();
                Console.WriteLine("\tList type arguments ({0}):", typeArguments.Length);
                foreach (Type tParam in typeArguments)
                {
                    Console.WriteLine("\t\t{0}", tParam);
                }
            }
            */
            public class JsonSerializer
            {
                public static string Serialize(object obj)
                {
                    return JsonConvert.SerializeObject(obj, Formatting.Indented);
                }
            }
            static void Main(string[] args)
            {
                var faker = new Faker();
                FirstClass firstClass = faker.Create<FirstClass>();
                SecondClass secondClass = faker.Create<SecondClass>();
                Console.WriteLine(JsonSerializer.Serialize(firstClass));
                Console.WriteLine(JsonSerializer.Serialize(secondClass));
            }
        }
    } 
}