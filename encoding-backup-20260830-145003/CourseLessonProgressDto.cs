namespace Marzi.Client.Core.Models.Courses;

public sealed class CourseLessonProgressDto
{
    public Guid LessonId { get; set; }

    public int WatchedSeconds { get; set; }

    public bool Completed { get; set; }
}