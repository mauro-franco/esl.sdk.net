using System;
using NUnit.Framework;
using Silanis.ESL.SDK;
using Silanis.ESL.SDK.Builder;

namespace SDK.Tests
{
    public class DocumentPackageAttributesBuilderTest
    {
        public DocumentPackageAttributesBuilderTest()
        {
        }

        [Test]
        public void buildWithSpecifiedValues()
        {

            DocumentPackageAttributesBuilder documentPackageAttributesBuilder = new DocumentPackageAttributesBuilder()
                                        .withAttribute("First Name", "Adam")
                                        .withAttribute("Last Name", "Smith");
            DocumentPackageAttributes documentPackageAttributes = documentPackageAttributesBuilder.build();

            Assert.AreEqual("Adam", documentPackageAttributes.Contents["First Name"]);
            Assert.AreEqual("Smith", documentPackageAttributes.Contents["Last Name"]);

        }

    }
}

