using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Models;

namespace TaxCalculator.Controllers
{
    public class HomeController : Controller
    {
        private Data.ApplicationDbContext _context;
        public HomeController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        //Index page showning all the records.
        public IActionResult Index()
        {
            //Initial testing
            //var taxEntity = new Tax {PersonName = "Apie", AnnualIncome = 20000d, PostalCode = "7000", RecordDate = DateTime.Now };
            //taxEntity.CalculateTax();            
            //sqlData.Add(taxEntity);
            
            SQLTaxData sqlData = new SQLTaxData(_context);

            var model = new HomePageViewModel();
            model.Taxes = sqlData.GetAll();
            model.tax = new TaxAddViewModel();

            return View(model);
        }

        //Add a record via the add form on the home page
        [HttpPost]
        public IActionResult AddRecord(TaxAddViewModel input)
        {
            SQLTaxData sqlData = new SQLTaxData(_context);
            if (ModelState.IsValid)
            {
                Tax tax = new Tax();
                tax.PersonName = input.PersonName;
                tax.AnnualIncome = input.AnnualIncome;
                tax.PostalCode = input.PostalCode;
                tax.RecordDate = DateTime.Now;
                tax.CalculateTax();

                sqlData.Add(tax);
            }
            return new RedirectResult("/");
        }

        //Delete a record
        public IActionResult DeleteRecord(int id)
        {
            SQLTaxData sqlData = new SQLTaxData(_context);
            sqlData.Delete(id);
            return new RedirectResult("/");
        }

        //About Page
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        //Contact Page
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //Privacy Page
        public IActionResult Privacy()
        {
            return View();
        }

        //Error Page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


    //Our Database Interface
    public class SQLTaxData
    {
        private Data.ApplicationDbContext _context { get; set; }

        public SQLTaxData(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Tax tax)
        {
            _context.Add(tax);
            _context.SaveChanges();
        }

        public Tax Get(int ID)
        {
            return _context.Taxes.FirstOrDefault(e => e.id == ID);
        }

        public IEnumerable<Tax> GetAll()
        {
            return _context.Taxes.ToList<Tax>();
        }

        public bool Delete(int ID)
        {
            Tax tax = this.Get(ID);
            if (tax != null)
            {
                _context.Remove(tax);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(Tax tax)
        {
            if (tax != null)
            {
                _context.Remove(tax);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

    }

    #region Command Objects
    //Command objects used by Actions to map input values and output values to.

    public class HomePageViewModel
    {
        public IEnumerable<Tax> Taxes { get; set; }
        public TaxAddViewModel tax { get; set; }
    }

    public class TaxAddViewModel
    {
        public string PersonName { get; set; }
        public double AnnualIncome { get; set; }
        public string PostalCode { get; set; }
    }

    #endregion
}
