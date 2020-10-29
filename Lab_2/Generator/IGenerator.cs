using System;
using System.Runtime.InteropServices.ComTypes;

namespace Listsoft
{
    namespace Lab_Faker
    {
		namespace Generators
		{
			public interface IGenerator
			{
				object Next(Type type);
				bool CanGenerate(Type type);
			}
			public interface INonGenericGenerator : IGenerator
			{
			}
			public interface IGenericGenerator:IGenerator
			{
				void SetGeneratorsManager(GeneratorsManager generatorsManager);
			}
		}
	}
}