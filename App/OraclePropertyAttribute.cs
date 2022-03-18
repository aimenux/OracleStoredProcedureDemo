namespace App;

[AttributeUsage(AttributeTargets.Property)]
public class OraclePropertyAttribute : Attribute
{
    public string Name { get; }

    public OraclePropertyAttribute(string name)
    {
        Name = name;
    }
}