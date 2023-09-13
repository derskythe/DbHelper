// ***********************************************************************
// Assembly         : Shared
// Author           : p.g.parpura
// Created          : 10-15-2021
//
// Last Modified By : p.g.parpura
// Last Modified On : 10-15-2021
// ***********************************************************************
// <copyright file="ObjectType.cs" company="Shared">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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