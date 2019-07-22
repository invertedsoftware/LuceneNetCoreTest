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
		public IActionResult Index([FromQuery] string searchText)
		{

			// If bringing the data from the cache. Rebuild Lucene's index files on each cache refresh
			List<Resort> resortList = null;
			if (!_cache.TryGetValue("resortList", out resortList))
			{
				resortList = SeedDatabase();
				// Set cache options. In this example to 5 seconds
				var cacheEntryOptions = new MemoryCacheEntryOptions()
				{
					AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(5)
				};

				// Save the data in cache.
				_cache.Set("resortList", resortList, cacheEntryOptions);
				luceneManager.InitLucene(); // Rebuild the Lucene indexes from the new data
			}

			List<Resort> searchResults = new List<Resort>();
			if (!string.IsNullOrWhiteSpace(searchText))
				searchResults = luceneManager.GetsearchResults(searchText);

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

		private List<Resort> SeedDatabase()
		{
			return new List<Resort>()
			{
				new Resort() { ResortID = 1, ResortName = "Bellagio Hotel and Casino", ResortDescription = "Inspired by the villages of Europe, AAA Five Diamond Bellagio overlooks a Mediterranean-blue lake with fountains performing a magnificent ballet. Escape into an otherworldly plane of dance, music and acrobatics with “O” by Cirque du Soleil — aquatic theater in our Parisian-style opera house." },
				new Resort() { ResortID = 2, ResortName = "New York-New York Hotel & Casino", ResortDescription = "New York-New York Resort in Las Vegas is your home for all things entertainment and gaming. From live music on the Brooklyn Bridge to rowdy evenings around dueling pianos, you’re sure to find something for the whole family! And with 84,000 square of feet of casino space, there are more than 67 table games and hundreds of slot machines to entertain the novice to the pro. Why not be a part of it all at New York-New York?" },
				new Resort() { ResortID = 3, ResortName = "The Venetian", ResortDescription = "Part of a complex that includes The Palazzo and Sands Expo Convention Center, this lavish, Italian-themed casino resort on the Las Vegas Strip is 3 miles from McCarran International Airport. Ritzy suites with plush sitting areas feature flat-screens, Wi-Fi and minibars, plus soaking tubs. Upgraded suites add dining areas, fireplaces and/or whirlpool tubs. Valet parking is free. There are 20 restaurants, some run by acclaimed chefs, and bars and lounges, as well as a nightclub, a wax museum and a theater. Other amenities include a buzzy casino, a shopping mall, and artificial canals with gondola rides, along with 10 outdoor pools and a spa." },
				new Resort() { ResortID = 4, ResortName = "The Cosmopolitan of Las Vegas", ResortDescription = "Across the Strip from Planet Hollywood, this chic casino hotel has views of the adjacent Bellagio fountains and is 2 miles from McCarran International Airport. Sophisticated rooms feature floor-to-ceiling windows, flat-screen TVs and Wi-Fi; some have balconies.Suites add dining areas and kitchenettes; some offer private pools, terraces and/or butler service. There are multiple bars and restaurants, including a lobby bar set inside a 3-story crystal chandelier.Other amenities include a bustling casino and an outdoor performance venue, plus 3 outdoor pools, and a spa with a hammam.There's also a gym, a wedding chapel and a seasonal ice rink." },
				new Resort() { ResortID = 5, ResortName = "Paris Las Vegas", ResortDescription = "Across the Strip from The Bellagio, this French-themed casino hotel with a half-size Eiffel Tower is a 9-minute walk from a Las Vegas Monorail station. The warm rooms and have marble bathrooms, flat-screen TVs and free Wi-Fi. Suites add living areas, minibars and whirlpool tubs. Room service is available. In addition to an elegant casino, amenities include a 2-acre rooftop pool in a French-style garden, and an indoor Parisian-style street with live entertainment and shopping areas. There's also a lounge with dueling pianos, plus nightclubs and several restaurants, including a destination steakhouse. A resort fee applies." }};
		}
	}
}
