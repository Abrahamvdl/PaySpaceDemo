using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models.TaxSystem
{
    public static class TaxTypeFactory
    {
        public static iTaxCalculator GetTaxCalculator(TaxType taxType)
        {
            switch (taxType)
            {
                case TaxType.Progressive: return new ProgressiveTaxCalculator();
                case TaxType.FlatValue: return new FlatValueTaxCalculator();
                case TaxType.FlatRate: return new FlatRateTaxCalculator();
                default: return new ProgressiveTaxCalculator();
            }
        }
    }

    public static class PostalCodeTaxFactory
    {
        const TaxType defaultTaxType = TaxType.Progressive; //map default type as Progressive Tax

        private static Dictionary<string, TaxType> PostalCodeTaxTypeMap = new Dictionary<string, TaxType>()
        {
            {"7441", TaxType.Progressive },
            {"A100", TaxType.FlatValue },
            {"7000", TaxType.FlatRate },
            {"1000", TaxType.Progressive }
        };

        public static double GetTax(string PostalCode, double AnualIncome)
        {
            TaxType taxType;
            if (!PostalCodeTaxTypeMap.TryGetValue(PostalCode, out taxType)) taxType = defaultTaxType; //if we do not have the postal code in our map then map the tax type to the default value.

            return TaxTypeFactory.GetTaxCalculator(taxType).CalculateTax(AnualIncome);
        }
    }
}
