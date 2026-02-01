# 🆕 My Project

## ❓ What is My Project?
ETL and synchronization components for importing **UN/LOCODE** reference data from Microsoft Access (`.mdb`) into modern SQL databases.

## 🚀 Features

- UN/LOCODE import pipeline
- Resume-on-failure (checkpointing)
- Duplicate detection via natural keys
- Upsert logic (insert/update)
- SqlBulkCopy fallback for large datasets
- Designed for long-running and scheduled jobs

## Supported Platforms
- .NET 10 (LTS)
- SQL Server
- Windows (Access OLE DB source)

## Typical Usage

This package is intended to be used by:
- Console importers
- Background workers
- Data synchronization services

The reference implementation is provided in:
**`Unlocode.DataImporter`**

## License

MIT
