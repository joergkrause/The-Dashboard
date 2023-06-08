namespace Workshop.DatabaseLayer;

public interface IAuditableEntityBase
{
}

public interface IAuditableEntityBaseProperties : IAuditableEntityBase
{
  DateTime CreatedAt { get; set; }

  DateTime ModifiedAt { get; set; }

  string CreatedBy { get; set; }

  string ModifiedBy { get; set; }

}