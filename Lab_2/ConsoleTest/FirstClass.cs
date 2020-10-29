using System;
using System.Collections.Generic;

namespace Listsoft
{
    namespace Lab_Faker
    {
        [DTO]
        public class FirstClass
        {
            public SecondClass secondClass;
            public int integerNumber;
            public string String;
            public int[] integerArray;
            public List<int> integerList;
            public DateTime dateTime { get; set; }

            private FirstClass(int x) {
                this.integerNumber = x;
            }
            public FirstClass()
            {
                integerNumber = 1;
                String = "1";
            }
        }
    }
}