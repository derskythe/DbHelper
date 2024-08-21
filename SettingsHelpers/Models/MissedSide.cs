using System;
using System.Runtime.Serialization;

namespace SettingsHelper.Models;


/// <summary>
/// Enum MissedSide
/// </summary>
[Serializable]
public enum MissedSide
{
    /// <summary>
    /// The source
    /// </summary>
    [EnumMember]
    Source,
    /// <summary>
    /// The target
    /// </summary>
    [EnumMember]
    Target,
    /// <summary>
    /// The both
    /// </summary>
    [EnumMember]
    Both
}
