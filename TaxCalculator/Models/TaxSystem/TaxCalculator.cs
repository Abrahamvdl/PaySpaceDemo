using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxCalculator.Models.TaxSystem
{
    public interface iTaxCalculator
    {
        TaxType taxType { get; }

        double CalculateTax(double AnualIncome);
    }

    public class ProgressiveTaxCalculator : iTaxCalculator
    {
        private struct TaxBracket
        {
            public double TaxRate { get; private set; }
            public double TaxUpperLimit { get; private set; }

            public TaxBracket(double taxRate, double taxUpperLimit)
            {
                this.TaxRate = taxRate;
                this.TaxUpperLimit = taxUpperLimit;
            }
        }

        //As an upgrade this can be put in the appsettings file, and then loaded into a singleton.
        //It will allow for much improved flexability.
        private TaxBracket[] taxBrackets =
        {
            new TaxBracket(0.0d, 0),
            new TaxBracket(0.10d, 8350),
            new TaxBracket(0.15d, 33950),
            new TaxBracket(0.25d, 82250),
            new TaxBracket(0.28d, 171550),
            new TaxBracket(0.33d, 372950),
            new TaxBracket(0.35d, Double.PositiveInfinity),
        };     
        

        public TaxType taxType { get; private set; }

        public ProgressiveTaxCalculator()
        {
            taxType = TaxType.Progressive;
        }

        public double CalculateTax(double AnualIncome)
        {
            double taxValue = 0;
            double workingIncome = AnualIncome;

            for (int i = 1; i < taxBrackets.Length; i++)
            {
                if (workingIncome > 0 & workingIncome < taxBrackets[i].TaxUpperLimit)
                {
                    taxValue += workingIncome * taxBrackets[i].TaxRate;                    
                }else if (workingIncome >= taxBrackets[i].TaxUpperLimit)
                {
                    taxValue += (taxBrackets[i].TaxUpperLimit - taxBrackets[i - 1].TaxUpperLimit) * taxBrackets[i].TaxRate;
                }
                else if (workingIncome < 0)
                {
                    return taxValue;
                }
                workingIncome = AnualIncome - taxBrackets[i].TaxUpperLimit;
            }            

            return taxValue;
        }
    }

    public class FlatValueTaxCalculator : iTaxCalculator
    {
        const double taxVal =10000d;
        const double taxLimit = 200000d;
        const double taxLimitRate = 0.05d;

        public TaxType taxType { get; private set; }

        public FlatValueTaxCalculator()
        {
            taxType = TaxType.FlatValue;
        }

        public double CalculateTax(double AnualIncome)
        {
            if (AnualIncome <= 0) return 0;            
            else if (AnualIncome > 0 & AnualIncome < taxLimit) return AnualIncome * taxLimitRate;
            else return taxVal;
        }
    }

    public class FlatRateTaxCalculator : iTaxCalculator
    {
        const double flatTaxRate = 0.175d;

        public TaxType taxType { get; private set; }

        public FlatRateTaxCalculator()
        {
            taxType = TaxType.FlatRate;
        }

        public double CalculateTax(double AnualIncome)
        {
            if (AnualIncome > 0) return AnualIncome * flatTaxRate;            
            else return 0;            
        }
    }
}
