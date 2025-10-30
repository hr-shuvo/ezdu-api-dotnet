using Core.App.Services.Interfaces;
using Core.Entities;
using Core.Enums;

namespace Infrastructure.Data.Seeds;

public class DefaultAppEntities
{
    private readonly IUnitOfWork _unitOfWork;

    public DefaultAppEntities(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SeedAllAsync()
    {
        var classRepo = _unitOfWork.Repository<Class>();

        if (!await classRepo.ExistsAsync())
        {
            for (var i = 12; i > 5; i--)
            {
                if (i is 11 or 9) continue;

                var classEntity = new Class()
                {
                    Name = i switch
                    {
                        10 => "SSC",
                        12 => "HSC",
                        _ => "Class " + i
                    },
                    Segment = i > 10 ? Segment.Hsc :
                        i > 8 ? Segment.Ssc :
                        i > 5 ? Segment.Junior : Segment.Primary
                };

                await classRepo.AddAsync(classEntity);
                await classRepo.SaveChangesAsync();

                for (var sub = 1; sub <= 12; sub++)
                {
                    var name = sub switch
                    {
                        1 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Higher Math"
                            : "Mathematics",
                        2 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "General Math"
                            : "Science",
                        3 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Bangla 1st Paper"
                            : "Bangla",
                        4 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Bangla 2nd Paper"
                            : "English",
                        5 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "English 1st Paper"
                            : "ICT",
                        6 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "English 2nd Paper"
                            : "Religion",
                        7 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Physics"
                            : "Bangladesh and Global Studies",
                        8 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Chemistry"
                            : "Home Science",
                        9 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Biology"
                            : "Agriculture Studies",
                        10 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "ICT"
                            : "Physical Education",
                        11 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Economics"
                            : "Arts and Crafts",
                        12 => classEntity.Segment is Segment.Hsc or Segment.Ssc
                            ? "Accounting"
                            : "Moral Education",
                        _ => ""
                    };

                    var subjectEntity = new Subject()
                    {
                        Name = name,
                        SubTitle = name.ToLower(),
                        Code = name.ToLower().Replace(" ", "") + classEntity.Id,
                        Segment = classEntity.Segment,
                        ClassId = classEntity.Id,
                    };

                    await _unitOfWork.Repository<Subject>().AddAsync(subjectEntity);
                    await _unitOfWork.CompleteAsync();

                    await AddLessonsForSubject(subjectEntity, classEntity.Segment);
                }
            }
        }
    }


    private async Task AddLessonsForSubject(Subject subject, Segment segment)
    {
        List<string> lessons = segment switch
        {
            Segment.Hsc or Segment.Ssc => subject.Name switch
            {
                "Higher Math" => new List<string> { "Algebra", "Calculus", "Geometry", "Trigonometry" },
                "General Math" => new List<string> { "Number Theory", "Statistics", "Probability", "Vectors" },
                "Bangla 1st Paper" => new List<string> { "Poetry", "Prose", "Essay", "Grammar" },
                "Bangla 2nd Paper" => new List<string> { "Literature Analysis", "Letter Writing", "Composition" },
                "English 1st Paper" => new List<string> { "Grammar", "Vocabulary", "Reading Comprehension" },
                "English 2nd Paper" => new List<string> { "Writing Skills", "Listening", "Speaking" },
                "Physics" => new List<string> { "Mechanics", "Thermodynamics", "Optics", "Electricity" },
                "Chemistry" => new List<string> { "Organic Chemistry", "Inorganic Chemistry", "Physical Chemistry" },
                "Biology" => new List<string> { "Botany", "Zoology", "Human Anatomy", "Genetics" },
                "ICT" => new List<string> { "Computer Basics", "Programming", "Database", "Internet" },
                "Economics" => new List<string> { "Microeconomics", "Macroeconomics", "Trade", "Finance" },
                "Accounting" => new List<string>
                    { "Journal Entries", "Ledger", "Trial Balance", "Financial Statements" },
                _ => new List<string> { "Lesson 1", "Lesson 2", "Lesson 3" }
            },
            _ => subject.Name switch
            {
                "Mathematics" => new List<string> { "Numbers", "Algebra", "Geometry", "Fractions" },
                "Science" => new List<string> { "Physics Basics", "Chemistry Basics", "Biology Basics" },
                "Bangla" => new List<string> { "Alphabet", "Grammar", "Poems", "Stories" },
                "English" => new List<string> { "Alphabet", "Grammar", "Reading", "Writing" },
                "ICT" => new List<string> { "Computer Basics", "Typing", "Internet", "MS Office" },
                "Religion" => new List<string> { "Islam", "Hinduism", "Christianity", "Buddhism" },
                "Bangladesh and Global Studies" => new List<string>
                    { "History", "Geography", "Civics", "Current Events" },
                "Home Science" => new List<string> { "Nutrition", "Family Management", "Health", "Hygiene" },
                "Agriculture Studies" => new List<string> { "Farming", "Crops", "Soil Science", "Pest Control" },
                "Physical Education" => new List<string>
                    { "Exercise", "Sports Rules", "Team Sports", "Health Education" },
                "Arts and Crafts" => new List<string> { "Drawing", "Painting", "Crafting", "Design" },
                "Moral Education" => new List<string> { "Values", "Ethics", "Social Responsibility", "Teamwork" },
                _ => new List<string> { "Lesson 1", "Lesson 2" }
            }
        };

        foreach (var lessonName in lessons)
        {
            var lesson = new Lesson()
            {
                Name = lessonName,
                Content = lessonName,
                SubTitle = lessonName.ToLower(),
                SubjectId = subject.Id,
            };

            await _unitOfWork.Repository<Lesson>().AddAsync(lesson);
            await _unitOfWork.CompleteAsync();

            await AddTopicsForLesson(lesson, subject);
        }
    }

    private async Task AddTopicsForLesson(Lesson lesson, Subject subject)
    {
        List<string> topics = lesson.Name switch
        {
            "Algebra" => ["Linear Equations", "Quadratic Equations", "Polynomials"],
            "Calculus" => ["Limits", "Derivatives", "Integrals"],
            "Geometry" => ["Triangles", "Circles", "Coordinate Geometry"],
            "Trigonometry" => ["Trigonometric Ratios", "Identities", "Equations"],

            "Mechanics" => ["Motion", "Force", "Work & Energy"],
            "Thermodynamics" => ["Heat", "Temperature", "Laws of Thermodynamics"],

            "Organic Chemistry" => ["Hydrocarbons", "Alcohols", "Carboxylic Acids"],

            "Numbers" => ["Counting", "Odd & Even", "Addition"],
            "Fractions" => ["Proper Fractions", "Improper Fractions", "Operations"],

            _ => ["Topic 1", "Topic 2", "Topic 3"]
        };

        var order = 1;
        foreach (var topicDesc in topics)
        {
            var topic = new Topic
            {
                Name = topicDesc,
                Description = topicDesc,
                SubjectId = subject.Id,
                LessonId = lesson.Id,
                Order = order++
            };

            await _unitOfWork.Repository<Topic>().AddAsync(topic);
            await _unitOfWork.CompleteAsync();

            await AddTopicContent(topic);
        }
    }


    private async Task AddTopicContent(Topic topic)
    {
        const int contentCount = 100;
        var order = 1;
        var rnd = new Random();
        var types = Enum.GetValues(typeof(ContentType)).Cast<ContentType>().ToList();

        const int batchSize = 10;
        var currentBatch = 0;

        for (var i = 0; i < contentCount; i++)
        {
            var type = types[rnd.Next(types.Count)];
            string contentText = null;
            string url = null;

            switch (type)
            {
                case ContentType.ReadingText:
                    contentText = $"Detailed explanation and exercises for {topic.Name} (item {i + 1}).";
                    break;
                case ContentType.Video:
                    url = $"https://www.youtube.com/watch?v=dummyvideo{topic.Id}_{i + 1}";
                    break;
                case ContentType.Pdf:
                    url = $"https://example.com/pdf/{topic.Id}/lesson{i + 1}.pdf";
                    break;
                case ContentType.Url:
                    url = $"https://www.bangladesh.gov.bd/education/{topic.Name.Replace(" ", "").ToLower()}";
                    break;
                case ContentType.Others:
                    contentText = $"Additional resource or activity for {topic.Name} (item {i + 1}).";
                    break;
            }

            var topicContent = new TopicContent
            {
                Name = contentText ?? topic.Name,
                TopicId = topic.Id,
                Order = order++,
                Type = type,
                Content = contentText,
                Url = url
            };

            await _unitOfWork.Repository<TopicContent>().AddAsync(topicContent);
            currentBatch++;

            // Commit every batchSize inserts
            if (currentBatch >= batchSize)
            {
                await _unitOfWork.CompleteAsync();
                currentBatch = 0;
            }
        }

        // Commit any remaining items
        if (currentBatch > 0)
        {
            await _unitOfWork.CompleteAsync();
        }
    }
}