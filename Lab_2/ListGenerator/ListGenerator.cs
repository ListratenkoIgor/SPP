using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Listsoft.Lab_Faker.Generators;

namespace Listsoft
{
    namespace Lab_Faker
    {
        namespace Generators
        {

            public class ListGenerator : IGenericGenerator
            {
                private GeneratorsManager generatorsManager;
                public ListGenerator()
                {
//                    generatorsManager = new GeneratorsManager();
                }
                public void SetGeneratorsManager(GeneratorsManager generatorsManager)
                {
                    this.generatorsManager = generatorsManager;
                }

                private object Generate(Type type)
                {
                    if (generatorsManager.CanGenerate(type))
                    {
                        return generatorsManager.Next(type);
                    }
                    return null;
                }

                public bool CanGenerate(Type type)
                {
                    if (type.GetInterface(typeof(IEnumerable<>).Name)!=null)
                        return type.GetGenericTypeDefinition() == typeof(List<>);
                    return false;
                }
                public object Next(Type type)
                {

                    var listType = type.GetGenericTypeDefinition();
                    var genericType = type.GetGenericArguments()[0];
                    var constructedListType = listType.MakeGenericType(genericType);

                    Random random = new Random();
                    int length = random.Next(2, 15);

                    var list = Activator.CreateInstance(constructedListType);

                    for (int i = 0; i < length; i++)
                    {
                        list.GetType().GetMethod("Add").Invoke(list, new[] { Generate(genericType) });
                    }

                    return list;
                }
            }
        }
    }
}