# UnlocodeImporter

![GitHub tag (latest SemVer)](https://img.shields.io/github/v/tag/SaundersCreativeStrategies/Unlocade?sort=semver)
![Build](https://img.shields.io/github/actions/workflow/status/SaundersCreativeStrategies/Unlocade/build.yml?branch=main)
![License](https://img.shields.io/github/license/SaundersCreativeStrategies/Unlocade)

UnlocodeImporter is a structured command-line tool for inspecting, querying, and preparing Microsoft Access (MDB/ACCDB) databases.

It provides metadata inspection, schema analysis, query execution, structured diagnostics, and extensible rendering capabilities.

---

## ✨ Features

- List MDB tables
- Inspect table schema (columns, keys, indexes, relationships)
- Execute SELECT queries
- MySQL-style ASCII table output
- JSON output support
- Configurable column truncation
- Structured diagnostics with runtime tracking
- Clean architecture with DTO-based boundaries
- Microsoft ACE OLE DB provider validation

---

## 📦 Requirements

- .NET (target framework used by project)
- Microsoft ACE OLE DB Provider
- Windows (OLE DB dependency)

Download provider:
https://www.microsoft.com/en-us/download/details.aspx?id=54920

---

## 🚀 Installation

### Option 1 – Build from Source

```bash
git clone https://github.com/SaundersCreativeStrategies/Unlocade.git

cd Unlocade

dotnet build
```

### Option 1 – Build from Source

```bash
dotnet run -- mdb table list --file data.mdb
```

---

## Usage

### List Tables

```bash
UnlocodeImporter mdb table list --file data.mdb
```

### Show Table Schema

```bash
UnlocodeImporter mdb table schema --file data.mdb --table tableName
```

### Execute Query

#### Structured:

```bash
UnlocodeImporter mdb table query --file data.mdb --table tableName --top 10
```

#### Raw SQL:

```bash
UnlocodeImporter mdb table query --file data.mdb --sql "SELECT TOP 5 * FROM tableName"
```

####  With WHERE Clause

```bash
UnlocodeImporter mdb table query --file data.mdb --table UNLOCODE --where "Country = 'PH'"
```

### Global Options

- --file <MDB_FILE> (required)
- --verbose
- --json
- --max-width <WIDTH>
- --truncate-mode strict|friendly

### Versioning

See CHANGELOG.md for release history.
