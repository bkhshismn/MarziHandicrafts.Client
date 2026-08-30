namespace Marzi.Client.Core.Models.Courses;

public enum VideoProcessingStatus
{
    Pending = 0,
    Processing = 1,
    Ready = 2,
    Failed = 3
}

public sealed class CourseEpisodeDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public int Order { get; set; }

    public int DurationSeconds { get; set; }

    public bool IsFree { get; set; }

    public VideoProcessingStatus Status { get; set; }
}