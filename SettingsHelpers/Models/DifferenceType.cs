using System;
using System.Runtime.Serialization;

namespace SettingsHelper.Models;


/// <summary>
/// Enum DifferenceType
/// </summary>
[Serializable]
public enum DifferenceType
{
    /// <summary>
    /// The missing
    /// </summary>
    [EnumMember]
    Missing,
    /// <summary>
    /// The changes
    /// </summary>
    [EnumMember]
    ValueChanged,
    /// <summary>
    /// The null
    /// </summary>
    [EnumMember]
    Null,
    /// <summary>The forced change</summary>
    [EnumMember]
    ForcedChange
}
