# Neonalig Extensions

Extension methods for Unity types to enhance functionality.

## Features

- **ColorUtils**: Generate unique colors from hashes, color formatting
- **ComponentExtensions**: Helper methods for Component operations
- **EnumerableExtensions**: LINQ-style extensions for collections

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

## Installation

```json
{
  "dependencies": {
    "com.neonalig.extensions": "1.0.0",
    "com.neonalig.hashing": "1.0.0"
  }
}
```
