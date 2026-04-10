namespace Domain.Aggregates.GymClasses;

public sealed class GymClass
{
    public GymClass()
    {
    }

    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public string Instructor { get; set; } = null!;

    private GymClass(string id, string name, DateTime startTime, string instructor)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        Instructor = instructor;
    }

    public static GymClass Create(string name, DateTime startTime, string instructor)
    {
        return new GymClass(Guid.NewGuid().ToString(), name, startTime, instructor);
    }

    public static GymClass Rehydrate(string id, string name, DateTime startTime, string instructor)
    {
        return new GymClass(id, name, startTime, instructor);
    }


}