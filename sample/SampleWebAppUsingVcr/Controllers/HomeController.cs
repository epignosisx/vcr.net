﻿using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SampleWebAppUsingVcr.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("https://httpbin.org/get");
            ViewData["Response"] = response;
            return View();
        }
    }
}
