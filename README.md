# Neonalig Extensions

High-performance extension methods for common Unity and .NET types.

## Features

* **ColorUtils** - Deterministic color generation & formatting
* **ComponentExtensions** - Common component helpers
* **EnumerableExtensions** - Collection utilities

## Usage
 
### Color Utilities 
 
```csharp 
using Neonalig.Extensions; 
using UnityEngine; 
 
// Get a unique color for any value 
Color playerColor = playerId.GetUniqueColor(0.8f, 0.9f); 
Color typeColor = typeof(MyClass).GetUniqueColor(0.7f, 1.0f); 
 
// Format colored text 
string coloredText = "Important".GetColoredString(Color.red); 
``` 
 
### Component Extensions 
 
```csharp 
using Neonalig.Extensions; 
 
// Quick component operations 
gameObject.GetOrAddComponent<Rigidbody>(); 
```

## Dependencies

This package is dependent on [com.neonalig.hashing](https://github.com/Neonalig/com.neonalig.hashing).

Ensure you have installed that package prior to this one, or your project will not be able to be compiled.

## Installation

### Option 1 - Package Manager (Recommended)

1. Open **Window ▸ Package Manager**
2. Click **➕**
3. Select **Install package from Git URL…**
4. Paste:

```
https://github.com/Neonalig/com.neonalig.extensions.git#v1.0.0
```

Supported suffixes:

* `#v1.0.0` – tag
* `#main` – branch
* `#<commit-hash>` – exact commit

> **Tip:** Using a tag or commit hash is recommended for reproducible builds.

---

### Option 2 - `manifest.json`

Add to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.neonalig.attributes": "https://github.com/Neonalig/com.neonalig.extensions.git#v1.0.0"
  }
}
```

---

### Option 3 - Scoped Dependency

If you are consuming this from a local package or a scoped registry, use the package name directly:

```json
{
  "dependencies": {
    "com.neonalig.extensions": "1.0.0"
  }
}
```

### Requirements

* Unity **2021.3 LTS** or newer