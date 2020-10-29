using System;
using System.Collections.Generic;
using System.Text;

namespace Listsoft
{
    namespace Lab_Faker
    {
        namespace Generators
        {
            public class PrimitiveGenerator : INonGenericGenerator
            {
                private readonly Random random;
                private readonly List<Type> primitiveTypes;
                public PrimitiveGenerator()
                {
                    random = new Random();
                    primitiveTypes = new List<Type>() { typeof(int), typeof(byte), typeof(sbyte),
                typeof(short), typeof(ushort), typeof(uint), typeof(long),
                typeof(ulong), typeof(float), typeof(double), typeof(decimal),
                typeof(char), typeof(string), typeof(bool)};
                }

                public bool CanGenerate(Type type)
                {
                    if (primitiveTypes.Contains(type))
                    {
                        return true;
                    }

                    return false;
                }

                private long GenerateLong()
                {
                    byte[] buffer = new byte[8];

                    random.NextBytes(buffer);
                    return BitConverter.ToInt64(buffer, 0);
                }

                private string GenerateString()
                {
                    int count = random.Next(1, 20);
                    string result = "";

                    for (int i = 0; i < count; i++)
                    {
                        result += (char)random.Next('A', 'z');
                    }

                    return result;
                }

                public object Next(Type type)
                {
                    return (Type.GetTypeCode(type)) switch
                    {
                        TypeCode.Boolean => random.Next(0, 2) > 0,
                        TypeCode.Byte    => (byte)random.Next(),
                        TypeCode.SByte   => (sbyte)random.Next(),
                        TypeCode.Int16   => (short)random.Next(),
                        TypeCode.UInt16  => (ushort)random.Next(),
                        TypeCode.Int32   => (int)random.Next(),
                        TypeCode.UInt32  => (uint)random.Next(),
                        TypeCode.Int64   => (long)GenerateLong(),
                        TypeCode.UInt64  => (ulong)GenerateLong(),
                        TypeCode.Single  => (float)random.NextDouble(),
                        TypeCode.Double  => (double)random.NextDouble(),
                        TypeCode.Decimal => new decimal(random.NextDouble()),                        
                        TypeCode.Char    => (char)random.Next('A', 'z'),
                        TypeCode.String  => GenerateString(),
                        _                => null,
                    };
                }
            }
        }
    }
}
