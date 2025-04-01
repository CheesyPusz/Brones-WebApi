using System;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    public interface IBronesWebAPIClient_PatientToevoegen
    {
        Task<bool> AddPatientAsync(Patient_PatientToevoegen patient);
    }

    public class Patient_PatientToevoegen
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    [TestClass]
    public class TestPatiëntToevoegen
    {
        [TestMethod]
        public async Task Patient_word_toegevoegd()
        {
            // Arrange
            var mockBronesWebAPIClient = new Mock<IBronesWebAPIClient_PatientToevoegen>();
            mockBronesWebAPIClient.Setup(client => client.AddPatientAsync(It.IsAny<Patient_PatientToevoegen>()))
                                  .ReturnsAsync(true);

            var patient = new Patient_PatientToevoegen { Name = "John Botbreuk", Age = 30 };

            // Act
            var result = await mockBronesWebAPIClient.Object.AddPatientAsync(patient);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
