namespace StockportWebapp.ViewModels;

public class VideoViewModel
{
    [Required]
    public string VideoToken { get; set; }

    [Required]
    public string PhotoId { get; set; }
    public string Title { get; set; } = string.Empty;

    public VideoViewModel(string title, string videoToken, string photoId)
    {
        Title = title;
        VideoToken = videoToken;
        PhotoId = photoId;
    }

    public VideoViewModel(string videoToken, string photoId)
    {
        VideoToken = videoToken;
        PhotoId = photoId;
    }
}