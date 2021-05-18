using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gsb4;

namespace TestUnitaire
{
    [TestClass]
    public class UnitTest1
    { 
        GestionDate date = new GestionDate();
        [TestMethod]
       
        public void TestMoisPrecedent()
        {
            Assert.AreEqual("202102", date.moisPrecedent(), "Erreur");
            Assert.AreNotEqual("202105", date.moisPrecedent(), "Erreur");

        }
        public void TestDateDuJour()
        {
            Assert.AreEqual("23/03/2021", date.dateJour(), "Erreur");

        }
    }
}
