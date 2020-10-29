using System;
using System.Collections.Generic;
using System.Text;

namespace Listsoft
{
    namespace Lab_Faker
    {
        namespace Generators
        {
            public class DateTimeGenerator : INonGenericGenerator
            {
                public DateTimeGenerator()
                {                
                }

                public bool CanGenerate(Type type)
                {
                    return type == typeof(DateTime);
                }

                public object Next(Type type)
                {
                    Random random = new Random();
                    return new DateTime(random.Next(2001, DateTime.Now.Year), random.Next(1, 12), random.Next(1, 30), random.Next(0, 24), random.Next(0, 60), random.Next(0, 60));
                }
            }
        }
    }
}