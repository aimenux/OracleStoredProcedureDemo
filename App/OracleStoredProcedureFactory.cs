using Oracle.ManagedDataAccess.Client;

namespace App;

public static class OracleStoredProcedureFactory
{
    public static OracleStoredProcedure CreateOracleStoredProcedure(string procedureName, params OracleParameter[] oracleParameters)
    {
        var parameters = new OracleDynamicParameters();
        parameters.AddParameters(oracleParameters);
        return new OracleStoredProcedure(procedureName, parameters);
    }
}