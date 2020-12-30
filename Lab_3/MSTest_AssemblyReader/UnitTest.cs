using System;
using Listsoft.Lab3_AssemblyReader;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MSTest_AssemblyReader
{
    [TestClass]
    public static class ExtensionClass
    {
        public static int ExtensionMethod(this UnitTest unitTest, int a, int b)
        {
            return a + b;
        }
    }
    [TestClass]
    public class UnitTest
    {
        private int intValue;
        public string stringValue { get; set; }
        public void Method()
        {
            Console.WriteLine("Method");
        }

        [TestMethod]
        public void TestMethod()
        {
            AssemblyInfo assemblyInfo = AssemblyReader.GetAssemblyInfo(@".\MSTest_AssemblyReader.dll");
            Assert.AreEqual("MSTest_AssemblyReader", assemblyInfo.Name);
            Assert.AreEqual(1, assemblyInfo.Namespaces.Count);
            Assert.AreEqual("MSTest_AssemblyReader", assemblyInfo.Namespaces[0].Name);
            Assert.AreEqual(2, assemblyInfo.Namespaces[0].Classes.Count);
            Assert.AreEqual("MSTest_AssemblyReader.UnitTest", assemblyInfo.Namespaces[0].Classes[1].Name);
            Assert.AreEqual(3, assemblyInfo.Namespaces[0].Classes[1].Members.Count);
            Assert.AreEqual("Int32 intValue", assemblyInfo.Namespaces[0].Classes[1].Members[0].Values[0]);
            Assert.AreEqual("System.String stringValue", assemblyInfo.Namespaces[0].Classes[1].Members[1].Values[0]);
            Assert.AreEqual("Void Method()", assemblyInfo.Namespaces[0].Classes[1].Members[2].Values[2]);
            Assert.AreEqual("Int32 ExtensionMethod(MSTest_AssemblyReader.UnitTest, Int32, Int32)",
                assemblyInfo.Namespaces[0].Classes[1].Members[2].Values[10]);
        }
    }
}
