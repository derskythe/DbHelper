namespace Shared;


/// <summary>
/// Class ComboboxItem.
/// </summary>
public sealed record ComboboxItem
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>The value.</value>
    public string Value { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this instance is a table.
    /// </summary>
    /// <value><c>true</c> if this instance is table; otherwise, <c>false</c>.</value>
    public ObjectType ObjectType { get; set; }

    /// <summary>
    /// Gets or sets the name of the clear.
    /// </summary>
    /// <value>The name of the clear.</value>
    public string ClearName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the additional data.
    /// </summary>
    /// <value>The additional data.</value>
    public string AdditionalData { get; set; } = string.Empty;

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return Value;
    }
}
