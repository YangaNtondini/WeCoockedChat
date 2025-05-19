using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace WeCoockedChat.Services.VideoSearch
{
	public sealed class YouTubeSearch : IVideoSearch
	{
		private readonly HttpClient _http;
		private readonly string _apiKey;

		public YouTubeSearch(HttpClient http, IConfiguration cfg)
		{
			_http = http ?? throw new ArgumentNullException(nameof(http));
			_apiKey = cfg["YouTube:ApiKey"]
					  ?? throw new InvalidOperationException("Missing YouTube:ApiKey in configuration.");
		}

		private sealed record ApiResponse(List<Item> Items);

		private sealed record Item(Id Id, Snippet Snippet);

		private sealed record Id(string VideoId);

		private sealed record Snippet(
			string Title,
			string Description,
			ThumbnailGroup Thumbnails,
			DateTimeOffset? PublishedAt);

		private sealed record ThumbnailGroup(Thumbnail Default);

		private sealed record Thumbnail(string Url);

		public async Task<IList<VideoResult>> SearchAsync(string query, CancellationToken ct = default)
		{
			const int maxResults = 5;
			var url =
				$"https://www.googleapis.com/youtube/v3/search" +
				$"?part=snippet&type=video&safeSearch=strict" +
				$"&maxResults={maxResults}" +
				$"&q={Uri.EscapeDataString(query)}" +
				$"&key={_apiKey}";

			var root = await _http.GetFromJsonAsync<ApiResponse>(url, ct);

			var list = new List<VideoResult>(maxResults);

			if (root?.Items is { } items)
			{
				foreach (var item in items)
				{
					if (item.Id?.VideoId is null) continue;

					list.Add(new VideoResult
					{
						VideoId = item.Id.VideoId,
						Title = item.Snippet?.Title,
						Thumbnail = item.Snippet?.Thumbnails?.Default?.Url,
					});
				}
			}

			return list;
		}
	}
}
