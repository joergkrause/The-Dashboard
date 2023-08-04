namespace TheDashboard.SharedEntities;

public record JobStartEvent(int ConsumerId) : Command;

public record JobStopEvent(string Data) : Command;
