using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SettingsHelper.Models;

namespace SettingsHelper;


/// <summary>
/// Class JsonHelpers.
/// </summary>
internal static class JsonHelpers
{
    /// <summary>
    /// Replaces the nested.
    /// </summary>
    /// <param name="self">The self.</param>
    /// <param name="path">The path.</param>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">self</exception>
    /// <exception cref="ArgumentException">Path cannot be null or empty - path</exception>
    public static void ReplaceNested(this JObject self, string path, JToken value)
    {
        if (self is null)
        {
            throw new ArgumentNullException(nameof(self));
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }

        var pathParts = path.Split('.');
        JToken currentNode = self;

        for (int i = 0; i < pathParts.Length; i++)
        {
            var pathPart = pathParts[i];
            var isLast = i == pathParts.Length - 1;
            var partNode = currentNode?.SelectToken(pathPart);

            if (partNode is null)
            {
                var nodeToAdd = isLast ? value : new JObject();
                ((JObject)currentNode)?.Add(pathPart, nodeToAdd);
                currentNode = currentNode?.SelectToken(pathPart);
            }
            else
            {
                currentNode = partNode;

                if (isLast)
                    currentNode.Replace(value);
            }
        }
    }

    /// <summary>
    /// Compares the json.
    /// </summary>
    /// <param name="sourceToken">The source token.</param>
    /// <param name="targetToken">The target token.</param>
    /// <param name="forcedList"></param>
    /// <returns>System.ValueTuple&lt;System.Boolean, List&lt;DetectedChanges&gt;&gt;.</returns>
    public static (bool IsEqual, List<DetectedChanges> Differences) CompareJson(
        this JToken sourceToken,
        JToken targetToken,
        List<string> forcedList
    )
    {
        if (sourceToken == null && targetToken == null)
        {
            return (false, new List<DetectedChanges>(0));
        }

        if (sourceToken == null)
        {
            return (false, new List<DetectedChanges>
            {
                new DetectedChanges(
                    MissedSide.Source,
                    DifferenceType.Null
                )
            });
        }

        if (targetToken == null)
        {
            return (false, new List<DetectedChanges>
            {
                new DetectedChanges(
                    MissedSide.Target,
                    DifferenceType.Null
                )
            });
        }

        if (JToken.DeepEquals(sourceToken, targetToken))
        {
            return (true, new List<DetectedChanges>(0));
        }

        var differences = new List<DetectedChanges>();

        switch (sourceToken.Type)
        {
            case JTokenType.Object:
                {
                    var current = sourceToken as JObject;
                    var model = targetToken as JObject;

                    if (current == null && model == null)
                    {
                        return (false, new List<DetectedChanges>(0));
                    }

                    if (current == null)
                    {
                        return (false, new List<DetectedChanges>
                    {
                        new DetectedChanges(
                            MissedSide.Source,
                            DifferenceType.Null
                        )
                    });
                    }

                    if (model == null)
                    {
                        return (false, new List<DetectedChanges>
                    {
                        new DetectedChanges(
                            MissedSide.Target,
                            DifferenceType.Null
                        )
                    });
                    }

                    var addedKeys = current.Properties()
                                           .Select(c => c.Name)
                                           .Except(
                                               model.Properties()
                                                    .Select(c => c.Name)
                                           );

                    var removedKeys = model.Properties().Select(c => c.Name).Except(current.Properties().Select(c => c.Name));

                    //var differentValues = model.Properties().Select(c => c.Path).Except(current.Properties().Select(c => c.Path));
                    var unchangedKeys = current.Properties()
                                               .Where(c => JToken.DeepEquals(c.Value, targetToken[c.Name]))
                                               .Select(c => c.Name);

                    var calculated = addedKeys as string[] ?? addedKeys.ToArray();

                    foreach (var token in calculated)
                    {
                        if (token.IsEqual("_forced"))
                        {
                            continue;
                        }

                        differences.Add(new DetectedChanges(
                                            MissedSide.Target,
                                            sourceToken[token]
                                        )
                        );
                    }

                    foreach (var token in removedKeys)
                    {
                        differences.Add(
                            new DetectedChanges(
                                MissedSide.Source,
                                targetToken[token]
                            )
                        );
                    }

                    var potentiallyModifiedKeys = current.Properties()
                                                         .Select(c => c.Name)
                                                         .Except(calculated)
                                                         .Except(unchangedKeys);

                    foreach (var k in potentiallyModifiedKeys)
                    {
                        var foundDiff = CompareJson(current[k], model[k], forcedList);

                        if (!foundDiff.IsEqual)
                        {
                            differences.AddRange(foundDiff.Differences);
                        }
                    }
                }

                break;

            case JTokenType.Array:
                {
                    var current = sourceToken as JArray;
                    var model = targetToken as JArray;

                    if (current == null && model == null)
                    {
                        return (false, new List<DetectedChanges>(0));
                    }

                    if (current == null)
                    {
                        return (false, new List<DetectedChanges>
                    {
                        new DetectedChanges(MissedSide.Source, DifferenceType.Null)
                    });
                    }

                    if (model == null)
                    {
                        return (false, new List<DetectedChanges>
                    {
                        new DetectedChanges(MissedSide.Target, DifferenceType.Null),
                    });
                    }

                    var plus = new JArray(current.Except(model, new JTokenEqualityComparer()));
                    var minus = new JArray(model.Except(current, new JTokenEqualityComparer()));

                    if (plus.HasValues)
                    {
                        foreach (var token in plus)
                        {
                            differences.Add(new DetectedChanges(MissedSide.Target, token));
                        }
                    }

                    if (minus.HasValues)
                    {
                        foreach (var token in minus)
                        {
                            differences.Add(new DetectedChanges(MissedSide.Source, token));
                        }
                    }
                }

                break;

            default:
                var (sourceHasValue, sourceValue) = ((JValue)sourceToken).HasValueTuple();
                var (targetHasValue, targetValue) = ((JValue)targetToken).HasValueTuple();

                if (sourceHasValue && targetHasValue && sourceValue.IsEqual(targetValue))
                {
                    return (true, new List<DetectedChanges>(0));
                }

                if (forcedList.Contains(sourceToken.Path))
                {
                    return (false, new List<DetectedChanges>
                    {
                        new DetectedChanges(DifferenceType.ForcedChange, sourceToken.Path, sourceValue, targetValue)
                    });
                }
                else
                {
                    return (
                        false,
                        new List<DetectedChanges>
                        {
                            new DetectedChanges(sourceValue, sourceToken.Path, targetValue, sourceHasValue)
                        }
                    );
                }
        }

        return (false, differences);
    }

    public static (bool hasValue, string value) HasValueTuple(this JValue token)
    {
        if (token.Value != null)
        {
            return (true, token.Value.ToString());
        }

        return (false, string.Empty);
    }

    public static (bool hasValue, string value) HasValueTuple(this JToken token)
    {
        if (token.HasValues)
        {
            return (true, token.Value<string>());
        }

        return (false, string.Empty);
    }
}
