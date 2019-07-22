using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Lucene.Net.Analysis.Standard;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Microsoft.Extensions.Caching.Memory;
using LuceneNetCoreTest.Models;
using Lucene.Net.QueryParsers.Classic;
using System.Threading;

namespace LuceneNetCoreTest
{
	/// <summary>
	/// This class needs to be added as a singleton in order to operate correctly.
	/// Add services.AddSingleton<LuceneManager>(); in ConfigureServices(IServiceCollection services)
	/// </summary>
	public class LuceneManager
	{
		LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
		private readonly IHostingEnvironment _env;
		private IMemoryCache _cache;
		private string LuceneDir => Path.Combine(_env.ContentRootPath, "Lucene_Index");
		private string[] LuceneSubDir = new string[] { "Index1", "Index2" };
		volatile int LuceneSubDirIndex = 0;
		private readonly object indexBuildLock = new object();

		public LuceneManager(IHostingEnvironment env, IMemoryCache memoryCache)
		{
			_env = env;
			_cache = memoryCache;
		}

		/// <summary>
		/// Build or Rebuild the Lucene index files
		/// </summary>
		public void InitLucene()
		{
			// Only allow one Index build at a time.
			lock (indexBuildLock)
			{
				// Build or rebuild indexes in the directory currently not being used for search
				// This is done in order not to slow the application's search while rebuilding indexes
				string indexBuildDirectory = null;
				if (LuceneSubDirIndex == 0)
					indexBuildDirectory = Path.Combine(LuceneDir, LuceneSubDir[1]);
				else
					indexBuildDirectory = Path.Combine(LuceneDir, LuceneSubDir[0]);

				var dir = FSDirectory.Open(indexBuildDirectory);
				//create an analyzer to process the text
				var analyzer = new StandardAnalyzer(AppLuceneVersion);
				//create an index writer
				var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
				IndexWriter writer = new IndexWriter(dir, indexConfig);
				writer.DeleteAll(); // This will rebuild the index files

				List<Resort> resortList = null;
				if (!_cache.TryGetValue("resortList", out resortList)) // get the resort database from the cache
					return;

				foreach (var resort in resortList)
				{
					var doc = new Document();
					doc.Add(new StringField("ResortID", resort.ResortID.ToString(), Field.Store.YES));
					doc.Add(new TextField("ResortName", resort.ResortName, Field.Store.YES));
					doc.Add(new TextField("ResortDescription", resort.ResortDescription, Field.Store.YES));
					writer.AddDocument(doc);
				}
				writer.Commit();
				writer.Flush(triggerMerge: true, applyAllDeletes: true);
				writer.Dispose();

				// Set the search directory to the new index
				if (LuceneSubDirIndex == 0)
					Interlocked.Increment(ref LuceneSubDirIndex);
				else
					Interlocked.Decrement(ref LuceneSubDirIndex);
			}
		}

		/// <summary>
		/// Get a list of resorts matching the query
		/// </summary>
		/// <param name="searchText"></param>
		/// <returns></returns>
		public List<Resort> GetsearchResults(string searchText)
		{
			List<Resort> searchResults = new List<Resort>();

			var fields = new[] { "ResortName", "ResortDescription" };
			var analyzer = new StandardAnalyzer(AppLuceneVersion);
			var queryParser = new MultiFieldQueryParser(AppLuceneVersion, fields, analyzer);
			var query = queryParser.Parse(searchText);
			var dir = FSDirectory.Open(Path.Combine(LuceneDir, LuceneSubDir[LuceneSubDirIndex]));
			var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
			IndexWriter writer = new IndexWriter(dir, indexConfig);
			var searcher = new IndexSearcher(writer.GetReader(applyAllDeletes: true));
			var hits = searcher.Search(query, 20 /* top 20 */).ScoreDocs;

			foreach (var hit in hits)
			{
				var foundDoc = searcher.Doc(hit.Doc);
				searchResults.Add(new Resort()
				{
					ResortID = Convert.ToInt32(foundDoc.Get("ResortID")),
					ResortName = foundDoc.Get("ResortName"),
					ResortDescription = foundDoc.Get("ResortDescription")
				});
				
			}
			writer.Dispose();

			return searchResults;
		}
	}
}
