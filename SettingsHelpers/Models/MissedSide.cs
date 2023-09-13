// ***********************************************************************
// Assembly         : SettingsHelper
// Author           : Skif
// Created          : 06-28-2021
//
// Last Modified By : Skif
// Last Modified On : 06-28-2021
// ***********************************************************************
// <copyright file="MissedSide.cs" company="SettingsHelper">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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