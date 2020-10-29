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
            public class ArrayGenerator : IGenericGenerator
            {
                private GeneratorsManager generatorsManager;
                public ArrayGenerator()
                {                    
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
                    return type.IsArray;
                }
                public object Next(Type type)
                {
                    Type elementType = type.GetElementType();

                    Random random = new Random();
                    int length = random.Next(2, 15);

                    var array = Array.CreateInstance(elementType, length);
                    for (int i = 0; i < length; i++)
                    {
                        array.SetValue(Generate(elementType),i);
                    }
                    return array;
                }
            }
        }
    }
}