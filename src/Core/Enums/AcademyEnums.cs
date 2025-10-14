namespace Core.Enums;

public enum Segment
{
    Primary = 1,
    Junior = 2,
    Ssc,
    Hsc,
    Admission,
    Job,
    Others
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

public enum QuizType
{
    Mcq = 1,
    TrueFalse,
    ShortAnswer,
    Descriptive
}

public enum QuestionType
{
    Mcq = 1,
    TrueFalse,
    ShortAnswer,
    FillInTheBlanks,
    Matching,
    Descriptive
}

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    VeryHard
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