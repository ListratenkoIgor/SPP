using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Listsoft
{
    namespace Lab3_AssemblyReader
    {
        public static class AssemblyReader
        {
            private static BindingFlags BindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            private static Assembly LoadAssembly(string assemblyFile) {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyFile);
                    return assembly;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            public static AssemblyInfo GetAssemblyInfo(string assemblyFile)
            {

                AssemblyInfo assemblyInfo = null;
                Assembly assembly = LoadAssembly(assemblyFile);
                if (assembly != null)
                {
                    assemblyInfo = new AssemblyInfo(assembly.GetName().Name);
                    assembly.GetTypes().Select(x => x.Namespace).Distinct().Where(Namespace => Namespace != null).ToList().ForEach(Namespace =>
                    {
                        NamespaceInfo namespaceInfo = new NamespaceInfo(Namespace);
                        assemblyInfo.Namespaces.Add(namespaceInfo);
                        assembly.GetTypes().Where(x => x.IsClass && x.Namespace == Namespace).ToList().ForEach(Class =>
                        {
                            ClassInfo classInfo = new ClassInfo(Class.ToString());
                            namespaceInfo.Classes.Add(classInfo);
                            Class.GetFields(BindingFlags).ToList().ForEach(Field =>
                                classInfo.Members.First(Member => Member.Name == "Fields").Values.Add(Field.ToString()));
                            Class.GetProperties(BindingFlags).ToList().ForEach(Property =>
                                classInfo.Members.First(Member => Member.Name == "Properties").Values.Add(Property.ToString()));
                            Class.GetMethods(BindingFlags).Where(m => !m.IsDefined(typeof(ExtensionAttribute))).ToList().ForEach(Method =>
                                classInfo.Members.First(Member => Member.Name == "Methods").Values.Add(Method.ToString()));
                        });
                        assembly.GetTypes().Where(t => t.IsClass && t.Namespace == Namespace).ToList().ForEach(Class =>
                        {
                            Class.GetMethods().Where(Method => Method.IsDefined(typeof(ExtensionAttribute), false)).ToList().ForEach(Method =>
                            {
                                ClassInfo extendedType = null;
                                assemblyInfo.Namespaces.ToList().ForEach(Namespace =>
                                    extendedType = Namespace.Classes.First(Class => Class.Name == Method.GetParameters()[0].ParameterType.ToString()));
                                extendedType?.Members.First(Method => Method.Name == "Methods").Values.Add(Method.ToString());
                            });
                        });
                    });
                }
                return assemblyInfo;
            }
        }
    }
}