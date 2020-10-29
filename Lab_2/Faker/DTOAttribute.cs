using System;
using System.Collections.Generic;
using System.Text;


namespace Listsoft
{
	namespace Lab_Faker
	{

		[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
		public class DTOAttribute : Attribute
		{
		}
	}
}