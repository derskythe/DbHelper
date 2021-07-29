// ***********************************************************************
// Assembly         : SettingsHelper
// Author           : Skif
// Created          : 06-28-2021
//
// Last Modified By : Skif
// Last Modified On : 06-28-2021
// ***********************************************************************
// <copyright file="DifferenceType.cs" company="SettingsHelper">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Runtime.Serialization;

namespace SettingsHelper.Models
{
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
}
