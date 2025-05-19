namespace WeCoockedChat.Services.VideoSearch
{
	public interface IVideoSearch
	{
		Task<IList<VideoResult>> SearchAsync(string query, CancellationToken ct = default);
	}

	public class VideoResult
	{
		public string Provider { get; set; } = null!;
		public string VideoId { get; set; } = null!;
		public string Title { get; set; } = null!;
		public string Thumbnail { get; set; } = null!;
	}
}
