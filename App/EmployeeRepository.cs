using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace App;

public interface IEmployeeRepository
{
    Task<ICollection<Employee>> GetEmployeesAsync();

    Task<ICollection<Employee>> FindEmployeesAsync(int minSalary = 10000, int maxSalary = 15000);
}

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IOracleProvider _oracleProvider;

    public EmployeeRepository(IOracleProvider oracleProvider)
    {
        _oracleProvider = oracleProvider;
    }

    public async Task<ICollection<Employee>> GetEmployeesAsync()
    {
        const string name = "FOO.USP_GET_EMPLOYEES";
        var parameters = new[]
        {
            new OracleParameter("EMP_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
        };

        var procedure = OracleStoredProcedureFactory.CreateOracleStoredProcedure(name, parameters);
        var employees = await _oracleProvider.RunStoredProcedureAsync<Employee>(procedure);
        return employees;
    }

    public async Task<ICollection<Employee>> FindEmployeesAsync(int minSalary, int maxSalary)
    {
        const string name = "FOO.USP_FIND_EMPLOYEES";
        var parameters = new[]
        {
            new OracleParameter("MIN_SALARY", OracleDbType.Int32, minSalary, ParameterDirection.Input),
            new OracleParameter("MAX_SALARY", OracleDbType.Int64, maxSalary, ParameterDirection.Input),
            new OracleParameter("EMP_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
        };

        var procedure = OracleStoredProcedureFactory.CreateOracleStoredProcedure(name, parameters);
        var employees = await _oracleProvider.RunStoredProcedureAsync<Employee>(procedure);
        return employees;
    }
}