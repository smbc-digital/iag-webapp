namespace StockportWebapp.ViewModels;

public class VideoViewModel
{
    public string VideoToken { get; set; }
    public string PhotoId { get; set; }
    public string Title { get; set; } = string.Empty;

    public VideoViewModel(string title, string videoToken, string photoId)
    {
        Title = title;
        VideoToken = videoToken;
        PhotoId = photoId;
    }
}