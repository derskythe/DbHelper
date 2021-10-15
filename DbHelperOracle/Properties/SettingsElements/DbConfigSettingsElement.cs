// ***********************************************************************
// Assembly         : DbHelper
// Author           : Skif
// Created          : 06-28-2021
//
// Last Modified By : Skif
// Last Modified On : 06-28-2021
// ***********************************************************************
// <copyright file="Settings.cs" company="DbHelper">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SettingsHelper;

namespace DbHelperOracle.Properties.SettingsElements
{
    /// <summary>
    /// Class Settings.
    /// Implements the <see cref="SettingsElementBase" />
    /// </summary>
    /// <seealso cref="SettingsElementBase" />
    public class DbConfigSettingsElement : SettingsElementBase
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>The name of the host.</value>
        [Required]
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        [DataMember]
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [DataMember]
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        [DataMember]
        [Required]
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        [DataMember]
        [Required]
        public int Port { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={HostName})(PORT={Port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={ServiceName})));User Id={Username};Password={Password};Connection Timeout=15;";
        }
    }
}