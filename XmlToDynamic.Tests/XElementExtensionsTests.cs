using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlToDynamic;
using System.Xml.Linq;
using System.Dynamic;

namespace XmlToDynamic.Tests
{
    [TestClass]
    public class XElementExtensionsTests
    {
        [TestMethod]
        public void ToDynamic_Translates_Simple_Properties()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.docusign.net/API/3.0"">
    <testchild1>one</testchild1>
    <testchild2>two</testchild2>
</testcontainer>");
           
            // Act
            dynamic result = xml.ToDynamic();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.testchild1);
            Assert.AreEqual("one", result.testchild1.Value);
            Assert.IsNotNull(result.testchild2);
            Assert.AreEqual("two", result.testchild2.Value);
        }

        [TestMethod]
        public void ToDynamic_Translates_Nested_Elements()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer>
    <testchild1>
        <testsubchild>one</testsubchild>
    </testchild1>
    <testchild2>
        <testsubchild>two</testsubchild>
    </testchild2>
</testcontainer>");

            // Act
            dynamic result = xml.ToDynamic();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.testchild1);
            Assert.IsNotNull(result.testchild1.testsubchild);
            Assert.AreEqual("one", result.testchild1.testsubchild.Value);
            Assert.IsNotNull(result.testchild2);
            Assert.IsNotNull(result.testchild2.testsubchild);
            Assert.AreEqual("two", result.testchild2.testsubchild.Value);
        }

        [TestMethod]
        public void ToDynamic_Translates_Repeated_Properties_Into_Collections()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer>
    <child>A</child>
    <child>B</child>
</testcontainer>");

            // Act
            dynamic result = xml.ToDynamic();

            // Assert
            Assert.IsNotNull(result);
            
            Assert.IsNotNull(result.child);
            Assert.AreEqual(2, result.child.Count);
            Assert.AreEqual("A", result.child[0].Value);
            Assert.AreEqual("B", result.child[1].Value);
        }

        [TestMethod]
        public void ToDynamic_Translates_Repeated_Properties_Into_Collections_With_SubObjects()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer>
    <child><name>Jon</name><age>13</age></child>
    <child><name>Esther</name><age>18</age></child>
</testcontainer>");

            // Act
            dynamic result = xml.ToDynamic();

            // Assert
            Assert.IsNotNull(result);

            //pluralize test
            //Assert.IsNotNull(result.children);
            //Assert.AreEqual(2, result.children.Count);
            //Assert.AreEqual("Jon", result.children[0].name.Value);
            //Assert.AreEqual("13", result.children[0].age.Value);
            //Assert.AreEqual("Esther", result.children[1].name.Value);
            //Assert.AreEqual("18", result.children[1].age.Value);

            //no longer pluralizing
            Assert.IsNotNull(result.child);
            Assert.AreEqual(2, result.child.Count);
            Assert.AreEqual("Jon", result.child[0].name.Value);
            Assert.AreEqual("13", result.child[0].age.Value);
            Assert.AreEqual("Esther", result.child[1].name.Value);
            Assert.AreEqual("18", result.child[1].age.Value);
        }

        [TestMethod]
        public void ToDynamic_Translates_Attributes()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer>
    <testchild1 testattr=""testvalue"">one</testchild1>
</testcontainer>");

            // Act
            dynamic result = xml.ToDynamic();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.testchild1);
            Assert.AreEqual("testvalue", result.testchild1["testattr"]);
        }

        [TestMethod]
        public void ToDynamic_Returns_Dynamic_Object_Which_Can_Be_Modified_Later()
        {
            // Arrange
            var xml = XElement.Parse(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<testcontainer>
    <testchild1>one</testchild1>
</testcontainer>");

            // Act
            dynamic result = xml.ToDynamic();
            result.testchild2 = "two";

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.testchild2);
            Assert.AreEqual("two", result.testchild2);
        }
    }
}
