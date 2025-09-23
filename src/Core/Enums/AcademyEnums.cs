namespace Core.Enums;

public enum Segment
{
    PRIMARY = 1,
    SSC,
    HSC,
    ADMISSION,
    JOB,
    OTHERS
}

public enum Group
{
    SCIENCE = 1,
    ARTS,
    COMMERCE,
    OTHERS
}

public enum ContentType
{
    READING_TEXT = 1,
    VIDEO,
    PDF,
    URL,
    OTHERS
}

public enum QuizType
{
    MCQ = 1,
    TRUE_FALSE,
    SHORT_ANSWER,
    DESCRIPTIVE
}

public enum QuestionType
{
    MCQ,
    TrueFalse,
    ShortAnswer,
    Essay,
    FillInTheBlank
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
    FILE_UPLOAD = 1,
    TEXT_SUBMISSION,
    LINK_SUBMISSION
}

public enum InstituteType
{
    School = 1,
    College,
    University,
    Medical,
    Engineering
}