using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workshop.BuildingBlocks.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class NullGuidException : Exception
{
  public NullGuidException() { }
  public NullGuidException(string message) : base(message) { }
  public NullGuidException(string message, Exception inner) : base(message, inner) { }
  protected NullGuidException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
