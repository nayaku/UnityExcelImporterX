# UnityExcelImporterX - Unity Excel Data Import Tool

**[中文文档](README.cn.md)**

[![openupm](https://img.shields.io/npm/v/net.nayaku.unity-excel-importer-x?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/net.nayaku.unity-excel-importer-x/) [![openupm](https://img.shields.io/badge/dynamic/json?color=brightgreen&label=downloads&query=%24.downloads&suffix=%2Fmonth&url=https%3A%2F%2Fpackage.openupm.com%2Fdownloads%2Fpoint%2Flast-month%2Fnet.nayaku.unity-excel-importer-x)](https://openupm.com/packages/net.nayaku.unity-excel-importer-x/)

<hr/>

Automatically converts Excel files (.xls, .xlsx) into Unity ScriptableObject assets.

This Project is based on [unity-excel-importer](https://github.com/mikito/unity-excel-importer.git) and includes some features that are not available in the original project.

## Core Features

- **Zero-code Generation**: No need to manually write entity class scripts, automatically generate complete code
- **Real-time Sync**: Excel modifications automatically update Unity assets
- **Smart Comments**: Support for comment rows/columns and data boundaries
- **Rich Types**: Support basic types, enums, arrays, dictionaries, datetime and custom types
- **Multi-sheet Support**: Import all worksheets in an Excel file at once
- **Simple Functionality**: No configuration required, ready for use upon direct import

## Requirements
- Unity Version: 2021.3.45f1 or later
- Excel File Formats: .xls, .xlsx

## Installation

<details>
<summary>💡 Install via .unitypackage file (Recommended)</summary>

1. Visit [GitHub Releases page](https://github.com/nayaku/UnityExcelImporterX/releases)
2. Download the latest `.unitypackage` file
3. Double-click the file or import via **Assets → Import Package → Custom Package** in Unity

</details>

<details>
<summary>💡 Install via OpenUPM</summary>
This package is available on the <a href="https://openupm.com/packages/net.nayaku.unity-excel-importer-x/">OpenUPM</a> repository.
Please ensure your project has the `NPOI` and `Newtonsoft.Json` dependencies installed before installing.

```
openupm add net.nayaku.unity-excel-importer-x
```

</details>

<details>
<summary>💡 Install as GIT dependency via Package Manager</summary>

Please ensure your project has the `NPOI` and `Newtonsoft.Json` dependencies installed before installing.

1. Open Package Manager window (Window | Package Manager)
2. Click `+` button on the upper-left of a window, and select "Add package from git URL..."
3. Enter the following URL and click `Add` button

```
https://github.com/nayaku/UnityExcelImporterX.git?path=Assets/UnityExcelImporterX
```

</details>

## Quick Start

### Step 1: Create Excel File

Create an Excel file with the following structure:

| Row              | Content                    | Example                          |
| ---------------- | -------------------------- | -------------------------------- |
| **Row 1**  | Column names (field names) | `id`, `name`, `price`      |
| **Row 2**  | C# data types              | `int`, `string`, `float`   |
| **Row 3**  | Comments/Descriptions      | `ID`, `Item Name`, `Price` |
| **Row 4+** | Actual data                | `1`, `item1`, `99.5`       |

**Example Table Structure:**
![image-20250915154749933](./README.cn.assets/image-20250915154749933.png)

Place the Excel file in the Unity project's `Assets` directory or one of its subdirectories.

### Step 2: Auto-generate Code

1. **Select the Excel file in Unity**
2. **Right-click → Create → ExcelAssetScript** (or use **Assets → Create → ExcelAssetScript** from top menu)
3. The system automatically generates the entity and container class scripts (for example, `MstItems.cs`).

![image-20250910174623347](./README.assets/image-20250910174623347.png)

**Generated Code Example:**

```csharp
// Entity class - corresponds to each row of the table
[Serializable]
public class MstItemsEntity
{
    /// <summary>
    /// ID
    /// </summary>
    public int id;           // Auto-matches Excel column 1
    /// <summary>
    /// Item Name
    /// </summary>
    public string name;      // Auto-matches Excel column 2
    /// <summary>
    /// Price
    /// </summary>
    public float price;      // Auto-matches Excel column 3
}

// Container class - stores all table data
[ExcelAsset]
public class MstItems : ScriptableObject
{
    public List<MstItemsEntity> Entities;  // All row data
}
```

> [!WARNING]  
> **Important**: When the Excel table structure changes (e.g., adding/removing columns), repeat this step to generate the latest code.

### Step 3: Auto-import Data

- **Save the Excel file** (Ctrl+S)
- **Return to Unity**, the system will automatically detect changes and import data
- **A `.asset` file with the same name as the Excel file will be generated in the same directory**

If it is not generated automatically, manually reimport the Excel file to trigger generation: right-click the Excel file → **Reimport**.
![image-20250910174734537](./README.assets/image-20250910174734537.png)

### Done

Now you can view and edit imported data directly in Unity:

![image-20250915155540723](./README.cn.assets/image-20250915155540723.png)

## Advanced Features

### Indexing

Append `, key` to a field name to mark it as a primary key, for example `id, key`. Multiple primary keys are supported.

![Index example](./README.cn.assets/image-20260824110911760.png)

The generated code includes a `Dictionary` for quickly looking up data by its primary key:

```csharp
[Serializable]
public class KeyExampleEntity
{
    public int id;
    /// <summary>
    /// name of item
    /// </summary>
    public string name;
    public float hp;
}

[ExcelAsset]
public class KeyExample : ScriptableObject, ISerializationCallbackReceiver
{
    public List<KeyExampleEntity> item = new();
    public Dictionary<(int id, string name), KeyExampleEntity> itemDict;

    public void OnBeforeSerialize()
    {
        // Implement any logic needed before serialization
    }

    public void OnAfterDeserialize()
    {
        // Implement any logic needed after deserialization
        itemDict = new();
        foreach (KeyExampleEntity item in item)
        {
            var key = (
                item.id,
                item.name
            );
            if (itemDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate key found in itemDict (script: KeyExample): {key}. Each key must be unique.");
                continue; // Skip adding this item to the dictionary
            }
            itemDict[key] = item;
        }
    }
}
```

![Generated indexed data](./README.cn.assets/image-20260824111231781.png)

> If a duplicate primary key is found, an error is logged and that entry is skipped.
### Comment System

#### Row Comment

Enter `#` in the first cell of a row to skip the entire row.

#### Column Comment

Enter `#` in the first cell of a column to skip the entire column.

**Excel Table:**
![image-20250912202544622](./README.cn.assets/image-20250912202544622.png)

**Generated Code and Data:**

```csharp
[Serializable]
public class SummaryExampleEntity
{
    public int id;      // Only imports columns A, B, column C is ignored
    /// <summary>
    /// name of item
    /// </summary>
    public string name;
}

[ExcelAsset]
public class SummaryExample : ScriptableObject
{
    public List<SummaryExampleEntity> item;
}
```

![image-20250912202801969](./README.cn.assets/image-20250912202801969.png)

#### Sheet Comment

Prefix a worksheet name with `#` to ignore the entire worksheet.

### Data Boundaries

- **Column Boundary**: When an empty cell appears in row 1, all columns to the right will be ignored
- **Row Boundary**: When an empty cell appears in column 1, all rows below will be ignored

### Enum

First, create a C# enum:

```csharp
// Create ColorEnum.cs
public enum ColorEnum
{
    RED,
    GREEN,
    BLUE
}
```

Enter the enum values directly in Excel. The tool automatically matches them to the enum type:

![Enum values in Excel](./README.cn.assets/image-20250912203335293.png)

Generated code and data:

```csharp
[Serializable]
public class EnumExampleEntity
{
    public int id;
    /// <summary>
    /// Name
    /// </summary>
    public string name;
    /// <summary>
    /// Color
    /// </summary>
    public ColorEnum color; // Automatically matches the enum type
}
```

![Generated enum data](./README.cn.assets/image-20250912203534913.png)
### Complex Types

**Supports arrays, date/time values, dictionaries, and custom types.**

Square brackets may be omitted when using array types.

Create a custom type named `CustomType`:

```csharp
[Serializable]
public class CustomType
{
    public int x;
    public string s;
}
```

![Complex types in Excel](./README.cn.assets/image-20250915170746647.png)

![Generated complex-type data](./README.cn.assets/image-20250915170904074.png)
### Custom Asset Path

You can change the ScriptableObject generation position by specifying AssetPath as the ExcelAssetAttribute as shown below:

```csharp
[ExcelAsset(AssetPath = "Assets/Resources/MasterData")]
public class MstItems : ScriptableObject
{
    public List<MstItemsEntity> Entities;
}
```

### Debug Logging

When true is specified for LogOnImport of ExcelAssetAttribute, a log is output when the import process runs.

```csharp
[ExcelAsset(LogOnImport = true)]  // Output detailed logs during import
public class MstItems : ScriptableObject
{
    public List<MstItemsEntity> Entities;
}
```

### Custom File Association

You can change the association to a specific Excel file by specifying ExcelName of ExcelAssetAttribute:

```csharp
// Excel file name is "ItemData.xlsx"
// ScriptableObject class name is "MstItems"

[ExcelAsset(ExcelName = "ItemData")]  // Specify associated Excel file name
public class MstItems : ScriptableObject
{
    public List<MstItemsEntity> Entities;
}
```

### Modifying the Code Generation Template

The code generation template is located at [`Assets/UnityExcelImporterX/Editor/Templates/ExcelAssetScriptTemplete.cs.txt`](Assets/UnityExcelImporterX/Editor/Templates/ExcelAssetScriptTemplete.cs.txt).

You can customize the generated code style to match your project conventions.

## FAQ

<details>
<summary>Q: Excel changes not auto-updating?</summary>

**Solutions**:

1. Ensure Excel file is saved
2. Right-click the Excel file in Unity → **Reimport**
3. Check console for error messages

</details>

<details>
<summary>Q: Fields do not match after changing the headers?</summary>

After adding or removing columns, or changing field names or types, run **Create → ExcelAssetScript** again and wait for Unity to finish compiling before importing.

</details>

<details>
<summary>Q: Where is the generated `.asset` file?</summary>

By default, the asset is stored in the same directory as the Excel file. If `AssetPath` is set, look in the specified directory.

</details>
## License

This library is under the [MIT License](LICENSE.txt).

---

**If this tool helps you, please give it a ⭐Star!**
