using KadoshDomain.Enums;
using KadoshDomain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KadoshTests.ValueObjects
{
    [TestClass]
    public class DocumentTests
    {
        [TestMethod]
        [DataTestMethod]
        [DataRow("")]
        [DataRow("12345")]
        [DataRow("73866467040")]
        public void ShouldReturnErrorWhenCPFIsInvalid(string invalidCPF)
        {
            var document = new Document(number: invalidCPF, type: EDocumentType.CPF);
            Assert.IsFalse(document.IsValid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("738.664.670-44")]
        [DataRow("85565047013")]
        [DataRow("311.721.250-74")]
        [DataRow("25340751015")]
        public void ShouldReturnSucessWhenCPFIsValid(string validCPF)
        {
            var document = new Document(number: validCPF, type: EDocumentType.CPF);
            Assert.IsTrue(document.IsValid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("")]
        [DataRow("12345")]
        [DataRow("73866467040")]
        public void ShouldReturnErrorWhenCNPJIsInvalid(string invalidCNPJ)
        {
            var document = new Document(number: invalidCNPJ, type: EDocumentType.CNPJ);
            Assert.IsFalse(document.IsValid);
        }

        [TestMethod]
        [DataTestMethod]
        [DataRow("16.906.111/0001-00")]
        [DataRow("85244147000180")]
        [DataRow("41.261.476/0001-63")]
        [DataRow("62122460000140")]
        public void ShouldReturnSucessWhenCNPJIsValid(string validCNPJ)
        {
            var document = new Document(number: validCNPJ, type: EDocumentType.CNPJ);
            Assert.IsTrue(document.IsValid);
        }
    }
}