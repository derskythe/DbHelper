// ***********************************************************************
// Assembly         : Containers
// Author           : p.g.parpura
// Created          : 08-31-2020
//
// Last Modified By : p.g.parpura
// Last Modified On : 09-21-2020
// ***********************************************************************
// <copyright file="DbConfigOption.cs" company="skif@skif.ws">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.ComponentModel.DataAnnotations;

namespace Containers.Settings;


/// <summary>
/// Class DbConfigOption.
/// </summary>
public class DbConfigOption
{
    /// <summary>
    /// The name
    /// </summary>
    public const string NAME = "DbConfig";

    /// <summary>
    /// Gets or sets the name of the host.
    /// </summary>
    /// <value>The name of the host.</value>
    [Required]
    public string HostName { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    /// <value>The username.</value>
    [Required]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    /// <value>The password.</value>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the name of the service.
    /// </summary>
    /// <value>The name of the service.</value>
    [Required]
    public string ServiceName { get; set; }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return  $"Server={HostName};"            +
                $"Database={ServiceName};"       +
                $"User Id={Username};"           +
                $"Password={Password};"          +
                "Persist Security Info=True;"    +
                "Integrated Security=True;"      +
                "MultipleActiveResultSets=true;" +
                "Trusted_Connection=False;"      +
                "TrustServerCertificate=True;";
    }
}