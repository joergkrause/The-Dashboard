using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDashboard.DatabaseLayer.Domain;

public abstract class NamedEntity<T> : EntityBase<T> where T : IEquatable<T>
{
  public string Name { get; set; } = default!;

  public string Description { get; set; } = default!;

}
