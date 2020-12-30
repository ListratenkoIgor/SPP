using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestsGeneratorTester
{
    [TestClass]
    public class UnitTest
    {
        private TestsGenerator.TestsGenerator _generator = new TestsGenerator.TestsGenerator();

        private const string EmptyClassPath = @".\SourseDir\EmptyClass.cs";
        private const string ClassWithoutPublicMethodsPath = @".\SourseDir\ClassWithoutPublicMethods.cs";        
        private const string TwoClassInOneFilePath = @".\SourseDir\TwoClassInOneFile.cs";
        private const string OuterClassPath = @".\SourseDir\Class1.cs";

        [TestMethod]
        public void EmptyClassTest()
        { 
            var sourceCode = File.ReadAllText(EmptyClassPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void ClassWithoutPublicMethodsTest()
        {
            var sourceCode = File.ReadAllText(ClassWithoutPublicMethodsPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(0, tests.Count);
        }

        [TestMethod]
        public void TwoClassInOneFileTest()
        {
            var sourceCode = File.ReadAllText(TwoClassInOneFilePath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(2, tests.Count);
        }

        [TestMethod]
        public void OuterClassTest()
        {
            var sourceCode = File.ReadAllText(OuterClassPath);
            var tests = _generator.Generate(sourceCode);
            Assert.AreEqual(1, tests.Count);
            Assert.AreEqual("Class1UnitTest.cs", tests[0].FileName);
        }
    }
}
//System.Environment.CurrentDirectory;