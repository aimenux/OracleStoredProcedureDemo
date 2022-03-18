using System.Data;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace App;

public interface IOracleParameters : SqlMapper.IDynamicParameters
{
}

public class OracleDynamicParameters : IOracleParameters
{
    private readonly DynamicParameters _dynamicParameters = new DynamicParameters();
    private readonly List<OracleParameter> _oracleParameters = new List<OracleParameter>();

    public void AddParameters(params OracleParameter[] oracleParameters)
    {
        if (oracleParameters is null) return;

        foreach (var oracleParameter in oracleParameters)
        {
            _oracleParameters.Add(oracleParameter);
        }
    }

    public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
    {
        ((SqlMapper.IDynamicParameters)_dynamicParameters).AddParameters(command, identity);

        if (command is OracleCommand oracleCommand)
        {
            oracleCommand.Parameters.AddRange(_oracleParameters.ToArray());
        }
    }
}