using System;
using System.Collections.Generic;
using System.Text;

namespace Listsoft
{
    namespace Lab_Faker
    {
		public interface IFaker
		{
			T Create<T>();
		}
	}
}