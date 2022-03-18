using System.Data;
using System.Reflection;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace App;

public interface IOracleProvider
{
    Task<ICollection<T>> RunStoredProcedureAsync<T>(IOracleStoredProcedure procedure) where T : new();
}

public class OracleProvider : IOracleProvider
{
    private readonly OracleConnection _connection;

    public OracleProvider(OracleConnection connection)
    {
        _connection = connection;
    }

    public async Task<ICollection<T>> RunStoredProcedureAsync<T>(IOracleStoredProcedure procedure) where T : new()
    {
        var oracleResults = await _connection.QueryAsync(procedure.ProcedureName, procedure.OracleParameters, commandType: CommandType.StoredProcedure);
        var results = MapTo<T>(oracleResults);
        return results;
    }

    private static ICollection<T> MapTo<T>(IEnumerable<object> oracleResults) where T : new()
    {
        var results = new List<T>();

        foreach (IDictionary<string, object> oracleTypeResult in oracleResults)
        {
            var result = new T();
            var currentTypeProperties = result.GetType().GetProperties();
            foreach (var oraclePropertyName in oracleTypeResult.Keys)
            {
                var currentTypePropertyInfo = currentTypeProperties.Single(x => HasOraclePropertyName(x, oraclePropertyName));
                var oraclePropertyValue = ConvertToType(oracleTypeResult[oraclePropertyName], currentTypePropertyInfo.PropertyType);
                currentTypePropertyInfo.GetSetMethod()?.Invoke(result, new[] { oraclePropertyValue });
            }
            results.Add(result);
        }

        return results;
    }

    private static bool HasOraclePropertyName(MemberInfo propertyInfo, string oraclePropertyName)
    {
        var currentOraclePropertyName = propertyInfo.GetCustomAttribute<OraclePropertyAttribute>()?.Name;
        return string.Equals(currentOraclePropertyName, oraclePropertyName, StringComparison.OrdinalIgnoreCase);
    }

    private static object ConvertToType(object value, Type type)
    {
        return type == typeof(decimal) ? Convert.ToDecimal(value) : value;
    }
}