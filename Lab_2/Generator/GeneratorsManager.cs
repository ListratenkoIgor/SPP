using System;
using System.Collections.Generic;
using System.IO;
using Listsoft.Lab_Faker.Plugins;

namespace Listsoft
{
    namespace Lab_Faker
    {
        namespace Generators
        {
            public class GeneratorsManager : IGenerator
            {
                private readonly string PLUGINS_PATH = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

                private List<IGenerator> generators;

                public GeneratorsManager()
                {
                    var nonGenericLoader = new PluginsLoader<INonGenericGenerator>(PLUGINS_PATH);
                    var genericLoader = new PluginsLoader<IGenericGenerator>(PLUGINS_PATH);
                    List<INonGenericGenerator>  nonGenericGenerators = nonGenericLoader.Load();
                    nonGenericGenerators.Add(new PrimitiveGenerator());
                    List<IGenericGenerator> genericGenerators = genericLoader.Load();
                    List<IGenerator> tempGenerators = new List<IGenerator>();
                    tempGenerators.AddRange(nonGenericGenerators);
                    tempGenerators.AddRange(genericGenerators);
                    GeneratorsManager generatorsManager = new GeneratorsManager(tempGenerators);
                    foreach (var generic in genericGenerators) {
                        generic.SetGeneratorsManager(generatorsManager);
                    }
                    generators = new List<IGenerator>();
                    generators.AddRange(nonGenericGenerators);
                    generators.AddRange(genericGenerators);

                }
                public GeneratorsManager(List<IGenerator> generators)
                {
                    this.generators = generators;
                }

                public bool CanGenerate(Type type)
                {
                    foreach (var generator in generators)
                    {
                        if (generator.CanGenerate(type))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                public object Next(Type type)
                {
                    foreach (var generator in generators)
                    {
                        if (generator.CanGenerate(type))
                        {
                            return generator.Next(type);
                        }
                    }

                    return null;
                }
            }
        }
    }
}