namespace TheDashboard.DatabaseLayer.Domain
{
  internal class BaseAuditableEntity
  {
    public object CreatedBy { get; internal set; }
    public object Created { get; internal set; }
  }
}