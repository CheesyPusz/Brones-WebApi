using System;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    public interface IBronesWebAPIClient_PatientVerwijderen
    {
        Task<bool> RemovePatientAsync(Patient_PatientVerwijderen patient);
    }

    public class Patient_PatientVerwijderen
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [TestClass]
    class TestPatiëntVerwijderen
    {
        [TestMethod]
        public async Task Patient_word_verwijderd()
        {
            // Arrange
            var mockBronesWebAPIClient = new Mock<IBronesWebAPIClient_PatientVerwijderen>();
            mockBronesWebAPIClient.Setup(client => client.RemovePatientAsync(It.IsAny<Patient_PatientVerwijderen>()))
                                  .ReturnsAsync(true);
            var patient = new Patient_PatientVerwijderen { Name = "John Botbreuk", Age = 30 };
            // Act
            var result = await mockBronesWebAPIClient.Object.RemovePatientAsync(patient);
            // Assert
            Assert.IsTrue(result);
        }
    }
}
