using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using TZ_Nurzhan_MVC.Models;

namespace TZ_Nurzhan_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string connectionString = @"Data Source=DESKTOP-DKM570K\MYDBSERVER;Initial Catalog=TZ_Nurzhan;Integrated Security=True";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            var cities = GetData("SELECT * FROM Cities");
            return View(cities);
        }

        [HttpGet]
        public ActionResult GetWorkshops(int cityId)
        {
            var workshops = GetData($"SELECT * FROM Workshops WHERE CityID = {cityId}");
            return Json(workshops);
        }

        [HttpGet]
        public ActionResult GetEmployees(int workshopId)
        {
            var employees = GetData($"SELECT * FROM Employees WHERE WorkshopID = {workshopId}");
            return Json(employees);
        }

        [HttpGet]
        public ActionResult GetTeams()
        {
            var teams = GetData("SELECT * FROM Teams");
            return Json(teams);
        }

        [HttpGet]
        public ActionResult GetShifts()
        {
            var shifts = GetData("SELECT * FROM Shifts");
            return Json(shifts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<Dictionary<string, object>> GetData(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                var result = new List<Dictionary<string, object>>();
                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        dict[reader.GetName(i)] = reader.GetValue(i);
                    }
                    result.Add(dict);
                }
                return result;
            }
        }
    }
}
