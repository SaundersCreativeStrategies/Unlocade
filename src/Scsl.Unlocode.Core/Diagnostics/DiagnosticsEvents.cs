namespace Scsl.Unlocode.Core.Diagnostics;

public static class DiagnosticsEvents
{
    // Mdb
    public static readonly DiagnosticsEventId MdbOpen = new(1000, "MDB_OPEN");
    public static readonly DiagnosticsEventId MdbListTables = new(1001, "MDB_LIST_TABLES");
    public static readonly DiagnosticsEventId MdbReadSchema = new(1002, "MDB_READ_SCHEMA");
    public static readonly DiagnosticsEventId MdbTablesFound = new(1003, "MDB_TABLES_FOUND");

    // OLE DB Connection Lifecycle (1100 range)
    public static readonly DiagnosticsEventId OleDbCreateStart = new(1100, "OLEDB_CREATE_START");
    public static readonly DiagnosticsEventId OleDbProviderSelected = new(1101, "OLEDB_PROVIDER_SELECTED");
    public static readonly DiagnosticsEventId OleDbDataSourceSet = new(1102, "OLEDB_DATASOURCE_SET");
    public static readonly DiagnosticsEventId OleDbConnectionCreated = new(1103, "OLEDB_CONNECTION_CREATED");

    public static readonly DiagnosticsEventId OleDbCreateFailed = new(1199, "OLEDB_CREATE_FAILED");

    // Import
    public static readonly DiagnosticsEventId ImportStart = new(2000, "IMPORT_START");
    public static readonly DiagnosticsEventId ImportBatch = new(2001, "IMPORT_BATCH");
    public static readonly DiagnosticsEventId ImportComplete = new(2002, "IMPORT_COMPLETE");

    // error
    public static readonly DiagnosticsEventId Error = new(9000, "ERROR");
}