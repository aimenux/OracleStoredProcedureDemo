namespace App;

public class Employee
{
    [OracleProperty("ID")]
    public long Id { get; set; }

    [OracleProperty("NAME")]
    public string Name { get; set; }

    [OracleProperty("SALARY")]
    public decimal Salary { get; set; }

    [OracleProperty("ADDRESS")]
    public string Address { get; set; }
}