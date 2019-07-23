using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuceneNetCoreTest.Models;
using Microsoft.Extensions.Caching.Memory;
using LuceneNetCoreTest.Services;
using InvertedSoftware.DataBlock;

namespace LuceneNetCoreTest.Controllers
{
	public class HomeController : Controller
	{
		LuceneManager luceneManager;
		private IMemoryCache _cache;
		IBackgroundTaskQueue queue;

		public HomeController(LuceneManager luceneManager, IMemoryCache memoryCache, IBackgroundTaskQueue queue)
		{
			this.luceneManager = luceneManager;
			_cache = memoryCache;
			this.queue = queue;
		}
		public async Task<IActionResult> Index([FromQuery] string searchText)
		{
			// If bringing the data from the cache. Rebuild Lucene's index files on each cache refresh
			List<Resort> resortList = null;

			if (!_cache.TryGetValue("resortList", out resortList))
			{
				if (!luceneManager.IsThereAvailableIndex) // This is the first time we build an index so wait for it to be completed before searching.
					BuildSearch(); // Rebuild the Lucene indexes from the new data.
				else
					queue.QueueBackgroundWorkItem(async token =>
					{
						BuildSearch();
					}); // Do the indexing in the background while searching the old index.
			}

			List<Resort> searchResults = new List<Resort>();
			if (!string.IsNullOrWhiteSpace(searchText))
				searchResults = await Task.Factory.StartNew<List<Resort>>(()=> luceneManager.GetsearchResults(searchText));

			return View(searchResults);

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

		private List<Resort> GetAllResorts()
		{
			return CRUDHelper.GetObjectList<Resort>(() => new Resort(), "GetAllResorts", "{Your String Connection}");
		}

		private void BuildSearch()
		{
			// Set cache options. In this example to 10 seconds
			var cacheEntryOptions = new MemoryCacheEntryOptions()
			{
				AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(10)
			};
			_cache.Set("resortList", GetAllResorts(), cacheEntryOptions);
			luceneManager.InitLucene();
		}
	}
}
