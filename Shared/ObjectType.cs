using System;
using System.Runtime.Serialization;

namespace Shared;


/// <summary>
/// Enum ObjectType
/// </summary>
[Serializable]
public enum ObjectType
{
    /// <summary>
    /// The table
    /// </summary>
    [EnumMember]
    Table,

    /// <summary>
    /// The view
    /// </summary>
    [EnumMember]
    View,

    /// <summary>
    /// The procedure
    /// </summary>
    [EnumMember]
    Procedure,

    /// <summary>
    /// The package
    /// </summary>
    [EnumMember]
    Package
}
