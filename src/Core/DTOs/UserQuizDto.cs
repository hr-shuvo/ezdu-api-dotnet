using Core.Enums;

namespace Core.DTOs;

public class UserQuizDto
{
    public QuizType QuizType { get; set; }
    /// <summary>
    /// Need for QuizType.Quiz or QuizType.Archive
    /// </summary>
    public long QuizId { get; set; }

    // public int Xp { get; set; }
    // public int Type { get; set; }
    public int MarkPercentage { get; set; }

    // public ICollection<QuizSubmission> Submissions { get; set; }
}

// public abstract class QuizSubmission
// {
//     /// <summary>
//     /// Question ID
//     /// </summary>
//     public long QId { get; set; }
//     /// <summary>
//     /// Selected Option ID
//     /// </summary>
//     public long OpId { get; set; }
//
//     /// <summary>
//     /// true if the answer is correct
//     /// </summary>
//     public bool Flag { get; set; }
// }