// ***********************************************************************
// Assembly         : DbHelper
// Author           : Skif
// Created          : 06-29-2021
//
// Last Modified By : Skif
// Last Modified On : 10-15-2021
// ***********************************************************************
// <copyright file="ComboboxItem.cs" company="DbHelper">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Shared
{
    /// <summary>
    /// Class ComboboxItem.
    /// </summary>
    public class ComboboxItem
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is table.
        /// </summary>
        /// <value><c>true</c> if this instance is table; otherwise, <c>false</c>.</value>
        public ObjectType ObjectType { get; set; }
        /// <summary>
        /// Gets or sets the name of the clear.
        /// </summary>
        /// <value>The name of the clear.</value>
        public string ClearName { get; set; }
        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        /// <value>The additional data.</value>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Value;
        }
    }
}