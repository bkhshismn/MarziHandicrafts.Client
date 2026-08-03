

namespace Marzi.Client.Core.Models.Courses
{
    public sealed class CourseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool IsFree { get; set; }
        public bool IsPublished { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public List<CourseSectionDto> Sections { get; set; } = [];
    }

    public sealed class CourseSectionDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public List<CourseLessonDto> Lessons { get; set; } = [];
    }

    public sealed class CourseLessonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DurationSeconds { get; set; }
        public bool IsFreePreview { get; set; }
        public int OrderIndex { get; set; }

        public Guid? EpisodeId { get; set; }
        public string? VideoStatus { get; set; }
        public string? ThumbnailUrl { get; set; }
        public bool HasVideo { get; set; }
    }
}
