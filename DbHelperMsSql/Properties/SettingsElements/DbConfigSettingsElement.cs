using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SettingsHelper;

namespace DbHelperMsSql.Properties.SettingsElements;


/// <summary>
/// Class Settings.
/// Implements the <see cref="SettingsElementBase" />
/// </summary>
/// <seealso cref="SettingsElementBase" />
public sealed class DbConfigSettingsElement : SettingsElementBase
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
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
    public override string ToString()
    {
        return $"Server={HostName};"            +
               $"Database={ServiceName};"       +
               $"User Id={Username};"           +
               $"Password={Password};"          +
               "Persist Security Info=True;"    +
               "Integrated Security=True;"      +
               "MultipleActiveResultSets=true;" +
               "Trusted_Connection=False;";
    }
}
