using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BenFatto.Models;

namespace BenFatto.Tests {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestSelectEmpty() {

            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectLista("", "", "");

            bool x = Lista.Count > 0;
            Assert.IsFalse(x);
        }
        [TestMethod]
        public void TestSelectFilterIp() {

            string FilterIp = "221.";
            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectLista(FilterIp, "", "");

            bool x = Lista.Count > 0;
            Assert.IsTrue(x);
        }
        [TestMethod]
        public void TestSelectFilterUser() {

            string FilterUser = "-";
            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectLista("", FilterUser, "");

            bool x = Lista.Count > 0;
            Assert.IsTrue(x);
        }
        [TestMethod]
        public void TestSelectFilterHora() {

            string FilterHora = "09";
            DataLayer dl = new DataLayer();
            List<Table> Lista = dl.SelectLista("", "", FilterHora);

            bool x = Lista.Count > 0;
            Assert.IsTrue(x);
        }



    }
}
