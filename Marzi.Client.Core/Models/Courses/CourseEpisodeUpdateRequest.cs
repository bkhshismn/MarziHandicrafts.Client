namespace Marzi.Client.Core.Models.Courses;

public sealed class CourseEpisodeUpdateRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsFree { get; set; }
}
