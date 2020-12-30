using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Listsoft.Lab_Faker.Generators;
namespace Listsoft
{
    namespace Lab_Faker
    {
        public class Faker : IFaker
        {
            private GeneratorsManager generatorsManager;
            private Stack<Type> DTOControlStack;

            public Faker()
            {
                generatorsManager = new GeneratorsManager();
                DTOControlStack = new Stack<Type>();
            }

            public bool IsDTO(Type type)
            {
                return type.GetCustomAttributes(typeof(DTOAttribute), false).Length == 1;
            }

            private object Generate(Type type)
            {
                if (generatorsManager.CanGenerate(type))
                {
                    return generatorsManager.Next(type);
                }

                if (IsDTO(type))
                {
                    if (!DTOControlStack.Contains(type))
                    {
                        MethodInfo createMethod = typeof(Faker).GetMethod("Create").MakeGenericMethod(type);
                        return createMethod.Invoke(this, null);
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }

            public T Create<T>()
            {
                Type type = typeof(T);

                if (!IsDTO(type))
                {
                    return default;
                }

                DTOControlStack.Push(type);

                ConstructorInfo constructor = GetConstructor(type);

                object[] parameters = GenerateParameters(constructor);
                object obj = constructor.Invoke(parameters);

                SetFieldsAndProperties(obj);

                DTOControlStack.Pop();

                return (T)obj;
            }

            private ConstructorInfo GetConstructor(Type type)
            {
                FieldInfo[] fields = type.GetFields();
                Type[] types = fields.Select(field => field.GetType()).ToArray();
                ConstructorInfo constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic, null, types, null);
                if (constructor == null)
                {
                    constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)[0];
                }
                return constructor;
            }

            private object[] GenerateParameters(ConstructorInfo constructor)
            {
                var parameters = new List<object>();
                constructor.GetParameters()
                    .ToList()
                    .ForEach(t => parameters.Add(Generate(t.ParameterType)));
                return parameters.ToArray();
            }

            private void SetFieldsAndProperties(object obj)
            {
                obj.GetType()
                    .GetFields()
                    .ToList()
                    .ForEach(
                        f => f.SetValue(obj, Generate(f.FieldType))
                    );
                obj.GetType()
                    .GetProperties()
                    .ToList()
                    .ForEach(
                        p => p.SetValue(obj, Generate(p.PropertyType))
                    );
            }
        }
    }
}