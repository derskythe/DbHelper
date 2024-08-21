using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using SettingsHelper;

namespace DbHelperPostgre.Properties.SettingsElements;

/// <summary>
/// Class Settings.
/// Implements the <see cref="SettingsElementBase" />
/// </summary>
/// <seealso cref="SettingsElementBase" />
public sealed class DbConfigSettingsElement : SettingsElementBase {
  /// <summary>
  /// Gets or sets the name of the host.
  /// </summary>
  /// <value>The name of the host.</value>
  [DataMember]
  [Required]
  public string HostName { get; set; }

  /// <summary>
  /// Gets or sets the username.
  /// </summary>
  /// <value>The username.</value>
  [DataMember]
  [Required]
  public string Username {
    get; set;
  }

  /// <summary>
  /// Gets or sets the password.
  /// </summary>
  /// <value>The password.</value>
  [DataMember]
  [Required]
  public string Password {
    get; set;
  }

  /// <summary>
  /// Gets or sets the name of the service.
  /// </summary>
  /// <value>The name of the service.</value>
  [DataMember]
  [Required]
  public string Database {
    get; set;
  }

  /// <summary>
  /// Gets or sets the port.
  /// </summary>
  /// <value>The port.</value>
  [DataMember]
  [Required]
  [Range(1, 65535, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
  public int Port {
    get; set;
  }

  /// <summary>
  /// Returns a <see cref="System.String" /> that represents this instance.
  /// </summary>
  /// <returns>A <see cref="System.String" /> that represents this
  /// instance.</returns>
  public override string ToString() {
    var connectionString =
        $"Host={HostName};Port={Port};Database={Database};User Id={Username};Password={Password};";
    return connectionString;
  }
}
