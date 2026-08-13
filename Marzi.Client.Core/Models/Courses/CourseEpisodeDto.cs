namespace Marzi.Client.Core.Models.Courses;

public sealed class CourseEpisodeDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string VideoUrl { get; set; } = string.Empty;

    public int Order { get; set; }

    public int DurationSeconds { get; set; }

    public bool IsFree { get; set; }

    public string Status { get; set; } = string.Empty;
}