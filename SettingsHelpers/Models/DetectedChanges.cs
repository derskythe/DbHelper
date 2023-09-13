// ***********************************************************************
// Assembly         : SettingsHelper
// Author           : Skif
// Created          : 06-28-2021
//
// Last Modified By : Skif
// Last Modified On : 06-28-2021
// ***********************************************************************
// <copyright file="DetectedChanges.cs" company="SettingsHelper">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using Newtonsoft.Json.Linq;

namespace SettingsHelper.Models;


/// <summary>
/// Class DetectedChanges.
/// </summary>
public class DetectedChanges
{
    /// <summary>
    /// Gets or sets the path.
    /// </summary>
    /// <value>The path.</value>
    public string Path { get; set; }
    /// <summary>
    /// Gets or sets the side.
    /// </summary>
    /// <value>The side.</value>
    public MissedSide Side { get; set; }
    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>The type.</value>
    public DifferenceType Type { get; set; }
    /// <summary>
    /// Gets or sets the source value.
    /// </summary>
    /// <value>The source value.</value>
    public string SourceValue { get; set; }
    /// <summary>
    /// Gets or sets the target value.
    /// </summary>
    /// <value>The target value.</value>
    public string TargetValue { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether this instance has values.
    /// </summary>
    /// <value><c>true</c> if this instance has values; otherwise, <c>false</c>.</value>
    public bool HasValues { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetectedChanges"/> class.
    /// </summary>
    /// <param name="side">The side.</param>
    /// <param name="type">The type.</param>
    public DetectedChanges(MissedSide side, DifferenceType type)
    {
        Side = side;
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetectedChanges"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="path">The path.</param>
    /// <param name="sourceValue">The source value.</param>
    /// <param name="targetValue">The target value.</param>
    public DetectedChanges(DifferenceType type, string path, string sourceValue, string targetValue)
    {
        Side = MissedSide.Target;
        Type = type;
        Path = path;
        HasValues = true;
        SourceValue = sourceValue;
        TargetValue = targetValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetectedChanges"/> class.
    /// </summary>
    /// <param name="side">The side.</param>
    /// <param name="token">The token.</param>
    public DetectedChanges(MissedSide side, JToken token)
    {
        Side = side;
        Type = DifferenceType.Missing;

        Path = token.Path;

        if (side == MissedSide.Target)
        {
            SourceValue = string.Empty;
            var (hasValue, value) = token.HasValueTuple();
            TargetValue = value;
            HasValues = hasValue;
        }
        else
        {
            var (_, value) = token.HasValueTuple();
            SourceValue = value;
            TargetValue = string.Empty;
            HasValues = token.HasValues;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetectedChanges"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="path">The path.</param>
    /// <param name="target">The target.</param>
    /// <param name="hasValues">if set to <c>true</c> [has values].</param>
    public DetectedChanges(string source, string path, string target, bool hasValues)
    {
        Side = MissedSide.Both;
        Type = DifferenceType.ValueChanged;
        Path = path;
        SourceValue = source;
        TargetValue = target;
        HasValues = hasValues;
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return $"Path: {Path}, Side: {Side}, Type: {Type}, SourceValue: {SourceValue}, TargetValue: {TargetValue}, HasValues: {HasValues}";
    }
}