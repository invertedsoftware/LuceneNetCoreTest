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

namespace LuceneNetCoreTest
{
	public class LuceneManager
	{
		LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;
		private readonly IHostingEnvironment _env;
		private IMemoryCache _cache;
		private string LuceneDir => Path.Combine(_env.ContentRootPath, "Lucene_Index");

		public LuceneManager(IHostingEnvironment env, IMemoryCache memoryCache)
		{
			_env = env;
			_cache = memoryCache;
		}

		public void InitLucene()
		{
			// Build or rebuild indexes
			var dir = FSDirectory.Open(LuceneDir);
			//create an analyzer to process the text
			var analyzer = new StandardAnalyzer(AppLuceneVersion);
			//create an index writer
			var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
			IndexWriter writer = new IndexWriter(dir, indexConfig);
			writer.DeleteAll(); // This will rebuild the index files
			// get the resort database from the cache
			List<Resort> resortList = null;
			if (!_cache.TryGetValue("resortList", out resortList))
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
		}

		public List<string> GetsearchResults(string searchText)
		{
			List<string> searchResults = new List<string>();

			var fields = new[] { "ResortName", "ResortDescription" };
			var analyzer = new StandardAnalyzer(AppLuceneVersion);
			var queryParser = new MultiFieldQueryParser(AppLuceneVersion, fields, analyzer);
			var query = queryParser.Parse(searchText);
			var dir = FSDirectory.Open(LuceneDir);
			var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
			IndexWriter writer = new IndexWriter(dir, indexConfig);
			var searcher = new IndexSearcher(writer.GetReader(applyAllDeletes: true));
			var hits = searcher.Search(query, 20 /* top 20 */).ScoreDocs;

			foreach (var hit in hits)
			{
				var foundDoc = searcher.Doc(hit.Doc);
				searchResults.Add(foundDoc.Get("ResortID"));
			}
			writer.Dispose();

			return searchResults;
		}

	}
}
