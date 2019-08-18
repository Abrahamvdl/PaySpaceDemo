using NUnit.Framework;

using TaxCalculator.Models.TaxSystem;

namespace Tests
{
    [TestFixture]
    public class TestProgressiveTax
    {
        ProgressiveTaxCalculator progressiveTax;

        [SetUp]
        public void Setup()
        {
            progressiveTax = new ProgressiveTaxCalculator();
        }

        [Test]
        public void TestInputValues()
        {
            //Test -1 = 0
            Assert.AreEqual(0d, progressiveTax.CalculateTax(-1));

            //Test 0 = 0
            Assert.AreEqual(0d, progressiveTax.CalculateTax(0));

            //Test 1 => 0.1 (10%)
            Assert.AreEqual(0.1d, progressiveTax.CalculateTax(1));

            //Test 8349 => 834.9 (10%)
            Assert.AreEqual(834.90d, System.Math.Round(progressiveTax.CalculateTax(8349), 2));

            //Test 8350 => 835.0 (10%)
            Assert.AreEqual(835.0d, progressiveTax.CalculateTax(8350));

            //Test 8351 => 835.15 (10% + 15%)
            Assert.AreEqual(835.15d, progressiveTax.CalculateTax(8351));

            //Test 33950 => 4675.0 (10% + 15%)
            Assert.AreEqual(4675.0d, progressiveTax.CalculateTax(33950));

            //Test 82250 => 16750.0 (10% + 15% + 25%)
            Assert.AreEqual(16750.0d, progressiveTax.CalculateTax(82250));

            //Test 171550 => 27822.5 (10% + 15% + 25% + 28%)
            Assert.AreEqual(41754.0d, progressiveTax.CalculateTax(171550));

            //Test 372950 => 108216.0 (10% + 15% + 25% + 28% + 33%)
            Assert.AreEqual(108216.0d, progressiveTax.CalculateTax(372950));

            //Test 1000000 => 327 683,5 (10% + 15% + 25% + 28% + 33% + 35%)
            Assert.AreEqual(327683.5d, progressiveTax.CalculateTax(1000000));

            //Test 10000000 => 3 477 683,5‬ (10% + 15% + 25% + 28% + 33% + 35%)
            Assert.AreEqual(3477683.5d, progressiveTax.CalculateTax(10000000));

            //It required quite a bit of debugging, but the tests helped alot :)
        }
    }


    [TestFixture]
    public class TestFlatRateTax
    {
        FlatRateTaxCalculator flatRateTax;

        [SetUp]
        public void Setup()
        {
            flatRateTax = new FlatRateTaxCalculator();
        }

        [Test]
        public void TestInputValues()
        {
            //Test -1 = 0
            Assert.AreEqual(0d, flatRateTax.CalculateTax(-1));

            //Test 0 = 0
            Assert.AreEqual(0d, flatRateTax.CalculateTax(0));

            //Test 1 => 0.175 (17.5%)
            Assert.AreEqual(0.175d, flatRateTax.CalculateTax(1));

            //Test 1000000 => 175 000 (17.5%)
            Assert.AreEqual(175000.0d, flatRateTax.CalculateTax(1000000));

            //Test 10000000 => 1 750 000 (17.5%)
            Assert.AreEqual(1750000.0d, flatRateTax.CalculateTax(10000000));

        }
    }

    [TestFixture]
    public class TestFlatValueTax
    {
        FlatValueTaxCalculator flatValueTax;

        [SetUp]
        public void Setup()
        {
            flatValueTax = new FlatValueTaxCalculator();
        }

        [Test]
        public void TestInputValues()
        {
            //Test -1 = 0
            Assert.AreEqual(0d, flatValueTax.CalculateTax(-1));

            //Test 0 = 0
            Assert.AreEqual(0d, flatValueTax.CalculateTax(0));

            //Test 1 => 0.05 (5.0%)
            Assert.AreEqual(0.05d, flatValueTax.CalculateTax(1));

            //Test 190000 => 9 500 (5.0%)
            Assert.AreEqual(9500.0d, flatValueTax.CalculateTax(190000d));

            //Test 210000 => 10000 (10000)
            Assert.AreEqual(10000.0d, flatValueTax.CalculateTax(210000));

            //Test 1 000 000 => 10000 (10000)
            Assert.AreEqual(10000.0d, flatValueTax.CalculateTax(1000000));

        }
    }

    [TestFixture]
    public class TestTypeFactory
    {       

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void TestInputValues()
        {
            Assert.IsInstanceOf(typeof(ProgressiveTaxCalculator), TaxTypeFactory.GetTaxCalculator(TaxType.Progressive));

            Assert.IsInstanceOf(typeof(FlatValueTaxCalculator), TaxTypeFactory.GetTaxCalculator(TaxType.FlatValue));

            Assert.IsInstanceOf(typeof(FlatRateTaxCalculator), TaxTypeFactory.GetTaxCalculator(TaxType.FlatRate));          
        }
    }

    [TestFixture]
    public class TestPostalCodeTaxFactory
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TestInputValues()
        {
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("", -1));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("xxx", -1));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("7441", -1));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("A100", -1));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("7000", -1));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("1000", -1));

            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("", 0));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("xxx", 0));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("7441", 0));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("A100", 0));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("7000", 0));
            Assert.AreEqual(0d, PostalCodeTaxFactory.GetTax("1000", 0));

            Assert.AreEqual(327683.5d, PostalCodeTaxFactory.GetTax("", 1000000));
            Assert.AreEqual(327683.5d, PostalCodeTaxFactory.GetTax("xxx", 1000000));
            Assert.AreEqual(327683.5d, PostalCodeTaxFactory.GetTax("7441", 1000000));
            Assert.AreEqual(10000d, PostalCodeTaxFactory.GetTax("A100", 1000000));
            Assert.AreEqual(175000.0d, PostalCodeTaxFactory.GetTax("7000", 1000000));
            Assert.AreEqual(327683.5d, PostalCodeTaxFactory.GetTax("1000", 1000000));
        }
    }


}