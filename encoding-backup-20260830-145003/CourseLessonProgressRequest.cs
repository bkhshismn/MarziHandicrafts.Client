namespace Marzi.Client.Core.Requests;

public sealed class CourseLessonProgressRequest
{
    public int WatchedSeconds { get; set; }

    public bool Completed { get; set; }
}