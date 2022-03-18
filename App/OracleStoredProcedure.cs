namespace App;

public interface IOracleStoredProcedure
{
    public string ProcedureName { get; }

    public IOracleParameters OracleParameters { get; }
}

public class OracleStoredProcedure : IOracleStoredProcedure
{
    public OracleStoredProcedure(string procedureName, IOracleParameters oracleParameters)
    {
        ProcedureName = procedureName;
        OracleParameters = oracleParameters;
    }

    public string ProcedureName { get; }

    public IOracleParameters OracleParameters { get; }
}