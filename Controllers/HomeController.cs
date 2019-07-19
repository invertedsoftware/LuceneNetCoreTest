using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuceneNetCoreTest.Models;
using Microsoft.Extensions.Caching.Memory;

namespace LuceneNetCoreTest.Controllers
{
	public class HomeController : Controller
	{
		LuceneManager luceneManager;
		private IMemoryCache _cache;

		public HomeController(LuceneManager luceneManager, IMemoryCache memoryCache)
		{
			this.luceneManager = luceneManager;
			_cache = memoryCache;
		}
		public IActionResult Index()
		{
			List<Resort> resortList = null;
			if (!_cache.TryGetValue("resortList", out resortList))
			{
				//TODO: Go to the database
				resortList = new List<Resort>() { new Resort() { ResortID = 1, ResortName = "LIttle Chapel", ResortDescription = "Located in las Vegas" },
					new Resort() { ResortID = 2, ResortName = "Big Chapel", ResortDescription = "Located in los Angeles" } };

				// Set cache options.
				var cacheEntryOptions = new MemoryCacheEntryOptions()
				{
					AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(5)
				};

				// Save data in cache.
				_cache.Set("resortList", resortList, cacheEntryOptions);
				luceneManager.InitLucene();
			}

			var searchResults = luceneManager.GetsearchResults("little");
			searchResults = luceneManager.GetsearchResults("big");
			searchResults = luceneManager.GetsearchResults("vegas and los angeles");
			ViewData["Results"] = searchResults;
			return View(resortList);
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
	}
}
