using System.ComponentModel;

namespace TheDashboard.DatabaseLayer.Attributes;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EncryptAttribute : Attribute
{
}
