using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace TaxCalculator.Models
{
    public class Tax
    {
        [Key]
        public int id { get; set; }

        public string PersonName { get; set; }
        public double AnnualIncome { get; set; }
        public string PostalCode { get; set; }
        public DateTime? RecordDate { get; set; }
        public double CalculatedTax { get; set; }

        public double CalculateTax()
        {
            this.CalculatedTax = TaxSystem.PostalCodeTaxFactory.GetTax(this.PostalCode, this.AnnualIncome);
            return CalculatedTax;
        }
    }
}
