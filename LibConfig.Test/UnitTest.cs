using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace LibConfig.Test
{
    [TestClass]
    public class UnitTest
    {
        static string[] file = File.ReadAllLines("test.txt");

        public static void Main(string[] args)
        {
            Console.WriteLine("FILE\n====");
            Console.WriteLine(String.Join("\n", file));

            ConfigDocument document = new ConfigDocument(file);

            Console.WriteLine("\n\nDOCUMENT\n========");
            Console.WriteLine(String.Join("\n", document.ReadAllLines()));
            Console.ReadKey();
        }

        [TestMethod]
        public void ConfigDocumentAddSection()
        {
            ConfigDocument document = new ConfigDocument(file);

            Assert.IsFalse(document.ContainsSection("section3"));
            document.AddSection("section3");
            Assert.IsTrue(document.ContainsSection("section3"));
        }

        [TestMethod]
        public void ConfigDocumentContainsSection()
        {
            ConfigDocument document = new ConfigDocument(file);

            Assert.IsTrue(document.ContainsSection("section1"));
            Assert.IsTrue(document.ContainsSection("section2"));
        }

        [TestMethod]
        public void ConfigDocumentRemoveSection()
        {
            ConfigDocument document = new ConfigDocument(file);

            Assert.IsTrue(document.ContainsSection("section1"));
            Assert.IsTrue(document.ContainsSection("section2"));

            document.RemoveSection("section1");
            Assert.IsFalse(document.ContainsSection("section1"));
            Assert.IsTrue(document.ContainsSection("section2"));

            document = new ConfigDocument(file);

            document.RemoveSection("section2");
            Assert.IsTrue(document.ContainsSection("section1"));
            Assert.IsFalse(document.ContainsSection("section2"));
        }

        [TestMethod]
        public void ConfigDocumentGetSections()
        {
            ConfigDocument document = new ConfigDocument();
            List<ConfigSection> sections = document.Sections;

            for (int i = 0; i < sections.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.AreEqual("section1", sections[i].Name);
                        break;

                    case 1:
                        Assert.AreEqual("section2", sections[i].Name);
                        break;

                    default:
                        Assert.Fail("Too many sections!");
                        break;
                }
            }
        }

        [TestMethod]
        public void ConfigDocumentGetSetting()
        {
            ConfigDocument document = new ConfigDocument(file);
            Assert.AreEqual("value1", document.GetValue("section1", "key1"));
        }

        [TestMethod]
        public void ConfigDocumentSetSetting()
        {
            ConfigDocument document = new ConfigDocument(file);
            document.SetValue("key", "newValue");
            Assert.AreEqual("newValue", document.GetValue("key"));
            document.SetValue("section1", "key1", "newValue");
            Assert.AreEqual("newValue", document.GetValue("section1", "key1"));
        }

        [TestMethod]
        public void ConfigLine()
        {
            ConfigLine line = new ConfigLine();
            Assert.AreEqual("", line.Content);

            line = new ConfigLine("content");
            Assert.AreEqual("content", line.Content);
        }

        [TestMethod]
        public void ConfigSetting()
        {
            ConfigSetting setting = new ConfigSetting("name", "value");
            Assert.AreEqual("name = value", setting.Content);

            setting.Value = "test";
            Assert.AreEqual("name = test", setting.Content);
        }
    }
}
