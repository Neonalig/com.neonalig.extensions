using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Neonalig.Extensions
{
    public static class ComponentExtensions
    {
        public static bool TryGetComponentInChildren<T>(this Component component, [NotNullWhen(true)] out T? result)
            => TryGetComponentInChildren(component, false, out result);

        public static bool TryGetComponentInChildren<T>(this Component component, bool includeInactive, [NotNullWhen(true)] out T? result)
        {
            result = component.GetComponentInChildren<T>(includeInactive);
            return result != null;
        }

        public static bool TryGetComponentInParent<T>(this Component component, [NotNullWhen(true)] out T? result)
            => TryGetComponentInParent(component, false, out result);

        public static bool TryGetComponentInParent<T>(this Component component, bool includeInactive, [NotNullWhen(true)] out T? result)
        {
            result = component.GetComponentInParent<T>(includeInactive);
            return result != null;
        }
    }
}
