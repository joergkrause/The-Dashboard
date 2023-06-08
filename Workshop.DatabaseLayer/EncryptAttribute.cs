using System.ComponentModel;

namespace Workshop.DatabaseLayer;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EncryptAttribute : Attribute
{
}
