namespace Domain.Aggregates.TraingSessions;

public class TrainingSession
{
    public Guid Id { get; private set; }
    public Guid TrainerMemberId { get; private set; }
    public string SessionName { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public int Capacity { get; private set; } = 0;
    public string Location { get; private set; } = null!;

    private TrainingSession()
    {

    }

    public TrainingSession(Guid id, Guid trainerMemberId, string sessionName, DateTimeOffset createdAt, DateTimeOffset startDate, DateTimeOffset endDate, int capacity, string location) 
    {
        Id = id;
        TrainerMemberId = trainerMemberId;
        SessionName = sessionName;
        CreatedAt = createdAt;
        StartDate = startDate;
        EndDate = endDate;
        Capacity = capacity;
        Location = location;
    }

    public static TrainingSession Create(Guid trainerMemberId, string sessionName, DateTimeOffset startDate, DateTimeOffset endDate, int capacity, string location)
    {
        if (trainerMemberId == Guid.Empty)
            throw new ArgumentException("TrainerMemberId cannot be empty.");

        if (string.IsNullOrWhiteSpace(sessionName))
            throw new ArgumentException("Session name cannot be empty.");

        if (startDate > endDate)
            throw new ArgumentException("The training session cannot start after it ends.");

        if (capacity < 0)
            throw new ArgumentException("Capacity cannot be negative.");

        return new TrainingSession
            (
                Guid.NewGuid(),
                trainerMemberId,
                sessionName,
                DateTimeOffset.UtcNow,
                startDate,
                endDate,
                capacity,
                location
            );
    }

    public static TrainingSession Rehydrate(Guid id, Guid trainerMemberId, string sessionName, DateTimeOffset createdAt, DateTimeOffset startDate, DateTimeOffset endDate, int capacity, string location)
    {
        return new TrainingSession(id, trainerMemberId, sessionName, createdAt, startDate, endDate, capacity, location)
        { 
        
        };
    }

    // I was helped by chatGPT in explaining how to create a "partial" update.

    public void UpdateTrainingSession(Guid? trainerMemberId, string? sessionName, DateTimeOffset? startDate, DateTimeOffset? endDate, int? capacity, string? location)
    {
        if (trainerMemberId.HasValue)
            TrainerMemberId = trainerMemberId.Value;

        if (!string.IsNullOrWhiteSpace(sessionName))
            SessionName = sessionName;

        if(startDate.HasValue)
            StartDate = startDate.Value;

        if(endDate.HasValue)
            EndDate = endDate.Value;

        if(capacity.HasValue)
        {
            if (capacity < 0)
                throw new ArgumentException("Capacity cannot be negative.");

            Capacity = capacity.Value;
        }

        if(!string.IsNullOrWhiteSpace(location))
            Location = location;

        if(startDate.HasValue && endDate.HasValue && StartDate > EndDate)
            throw new ArgumentException("The training session cannot start after it ends.");
    }

    public void DecreaseCapacity()
    {
        if (Capacity <= 0)
            throw new ArgumentException("The training session is fully booked.");

        Capacity--;
    }

    public void IncreaseCapacity()
    {
        Capacity++;
    }

}
