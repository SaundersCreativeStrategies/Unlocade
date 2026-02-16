# Changelog

All notable changes to this project will be documented in this file.

---

## [1.0.0] - 2026-02-16

### Added

#### CLI
- Structured command hierarchy:
    - `mdb table list`
    - `mdb table schema`
    - `mdb table query`
- Global `--file` option with validation
- `--verbose` diagnostics output
- JSON output support for list, schema, and query commands
- Table output customization:
    - `--max-width`
    - `--truncate-mode strict|friendly`

#### MDB Metadata
- Retrieve user-defined table list
- Retrieve table schema metadata including:
    - Column names
    - Friendly data type mapping
    - OLE DB type mapping
    - Primary key detection
    - Unique index detection
    - Foreign key detection
- Case-insensitive and partial table name resolution

#### Query Engine
- Execute SELECT queries against MDB tables
- Structured query options:
    - `--table`
    - `--select`
    - `--where`
    - `--top`
- Raw SQL execution via `--sql`
- Query results returned as `IReadOnlyList<MdbQueryRow>`
- `MdbQueryRequest` abstraction for query execution

#### Rendering
- Metadata-driven `ITableRenderer` abstraction
- `SpectreTableRenderer` implementation
- MySQL-style ASCII table rendering
- Dynamic column rendering for dictionary-backed query rows
- Friendly truncation logic with configurable suffix

#### Diagnostics
- Introduced structured diagnostics infrastructure
- `IDiagnosticsSink` abstraction
- `DiagnosticsScope` for timed operation tracking
- Event ID–based logging system
- Timestamped console diagnostics
- Centralized command exception handling
- Operation runtime telemetry

#### Validation & Provider Handling
- Access database file extension validation
- Microsoft ACE OLE DB provider detection
- Centralized validation via `GlobalSettings`
- Consistent command-level validation behavior

#### Architecture
- Clean separation between:
    - CLI layer
    - Infrastructure layer
    - Core metadata layer
- DTO-based boundaries (no `DataTable` leakage)
- Centralized `OleDbConnectionFactory`
- Interface-based metadata and query services

---

### Refactored

- Renamed and reorganized namespaces for CLI clarity
- Standardized diagnostics event IDs
- Centralized validation logic into `GlobalSettings` and `TableSettings`
- Updated renderer to support dictionary-based rows
- Improved exception flow consistency across commands

---

### Internal

- Introduced Nerdbank.GitVersioning
- Added structured example validation
- Improved help descriptions and CLI examples
- Cleaned up provider detection logic
- Improved diagnostics consistency and runtime reporting

---

## Notes

Version 1.0.0 marks the first stable release of **UnlocodeImporter**.
The CLI contract and core MDB inspection features are now considered stable.

Future releases will follow Semantic Versioning:
- Patch: bug fixes
- Minor: new backward-compatible features
- Major: breaking changes
