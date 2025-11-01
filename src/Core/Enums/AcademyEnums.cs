namespace Core.Enums;

public enum Segment
{
    Primary = 1,
    Junior = 2,
    Ssc,
    Hsc,
    Admission,
    Job,
    InternationalExam
}

public enum Group
{
    Science = 1,
    Arts,
    Commerce,
    Others
}

public enum ContentType
{
    ReadingText = 1,
    Video,
    Pdf,
    Url,
    Others
}

// public enum QuizType
// {
//     Mcq = 1,
//     Cq,
//     ShortAnswer,
//     FillInTheBlanks
// }

public enum QuestionType
{
    Mcq = 1,
    Cq,
    ShortAnswer,
    FillInTheBlanks,
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    VeryHard
}

public enum QuizType
{
    Mock = 1, // mock test by self
    Quiz, // Quiz form admin provided
    Archive, // Quiz from question bank (archive)
}

public enum AssignmentType
{
    FileUpload = 1,
    TextSubmission,
    LinkSubmission
}

public enum InstituteType
{
    School = 1,
    College,
    University,
    Medical,
    Engineering
}