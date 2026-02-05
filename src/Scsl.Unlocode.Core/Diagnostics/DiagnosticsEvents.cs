namespace Scsl.Unlocode.Core.Diagnostics;

public static class DiagnosticsEvents
{
    // ─────────────────────────
    // MDB / Access (1000–1099)
    // ─────────────────────────

    public static readonly DiagnosticsEventId MdbOpen = new(1000, "MDB_OPEN");
    public static readonly DiagnosticsEventId MdbOpenCompleted = new(1001, "MDB_OPEN_COMPLETED");
    public static readonly DiagnosticsEventId MdbListTablesStart = new(1010, "MDB_LIST_TABLES_START");
    public static readonly DiagnosticsEventId MdbListTablesCompleted = new(1011, "MDB_LIST_TABLES_COMPLETED");
    public static readonly DiagnosticsEventId MdbTablesFound = new(1012, "MDB_TABLES_FOUND");
    public static readonly DiagnosticsEventId MdbReadSchemaStart = new(1020, "MDB_READ_SCHEMA_START");
    public static readonly DiagnosticsEventId MdbReadSchemaCompleted = new(1021, "MDB_READ_SCHEMA_COMPLETED");
    public static readonly DiagnosticsEventId MdbSchemaColumnsFound = new(1022, "MDB_SCHEMA_COLUMNS_FOUND");

    // ─────────────────────────
    // MDB Schema Details
    // ─────────────────────────

    public static readonly DiagnosticsEventId MdbReadIndexesStart = new(1023, "MDB_READ_INDEXES_START");
    public static readonly DiagnosticsEventId MdbReadIndexesCompleted = new(1024, "MDB_READ_INDEXES_COMPLETED");
    public static readonly DiagnosticsEventId MdbReadForeignKeysStart = new(1025, "MDB_READ_FOREIGN_KEYS_START");
    public static readonly DiagnosticsEventId MdbReadForeignKeysCompleted = new(1026, "MDB_READ_FOREIGN_KEYS_COMPLETED");

    // ─────────────────────────
    // OLE DB Lifecycle (1100–1199)
    // ─────────────────────────

    public static readonly DiagnosticsEventId OleDbCreateStart = new(1100, "OLEDB_CREATE_START");
    public static readonly DiagnosticsEventId OleDbProviderSelected = new(1101, "OLEDB_PROVIDER_SELECTED");
    public static readonly DiagnosticsEventId OleDbDataSourceSet = new(1102, "OLEDB_DATASOURCE_SET");
    public static readonly DiagnosticsEventId OleDbConnectionCreated = new(1103, "OLEDB_CONNECTION_CREATED");
    public static readonly DiagnosticsEventId OleDbCreateCompleted = new(1104, "OLEDB_CREATE_COMPLETED");
    public static readonly DiagnosticsEventId OleDbCreateFailed = new(1199, "OLEDB_CREATE_FAILED");

    // ─────────────────────────
    // Import Pipeline (2000–2999)
    // ─────────────────────────

    public static readonly DiagnosticsEventId ImportStart = new(2000, "IMPORT_START");
    public static readonly DiagnosticsEventId ImportBatch = new(2001, "IMPORT_BATCH");
    public static readonly DiagnosticsEventId ImportComplete = new(2002, "IMPORT_COMPLETE");

    // ─────────────────────────
    // CLI / Presentation (3000–3099)
    // ─────────────────────────

    public static readonly DiagnosticsEventId CliRenderStart = new(3000, "CLI_RENDER_START");
    public static readonly DiagnosticsEventId CliRenderCompleted = new(3001, "CLI_RENDER_COMPLETED");
    public static readonly DiagnosticsEventId CliJsonOutput = new(3002, "CLI_JSON_OUTPUT");
    public static readonly DiagnosticsEventId CliTruncationEnabled = new(3003, "CLI_TRUNCATION_ENABLED");

    // ─────────────────────────
    // Errors (9000+)
    // ─────────────────────────

    public static readonly DiagnosticsEventId Error = new(9000, "ERROR");
}