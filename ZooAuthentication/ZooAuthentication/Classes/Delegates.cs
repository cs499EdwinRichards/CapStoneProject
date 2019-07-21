namespace ZooAuthentication.NamedEvents
{
  /// <summary>
  /// Contains a link to the method consuming this <see cref="NamedEvent"/>
  /// </summary>
  /// <param name="sender">The sender of the event</param>
  /// <param name="parameters">A collection of <see cref="NamedEventParameter"/>s</param>
  public delegate void NamedEventHandler(object sender, params NamedEventParameter[] parameters);

  /// <summary>
  /// Contains a link to the method consuming this <see cref="NamedEvent"/>
  /// </summary>
  /// <param name="sender">The sender of the event</param>
  /// <param name="parameters">A collection of <see cref="NamedEventParameter"/>s</param>
  /// <returns>An object from the event execution</returns>
  public delegate object NamedEventReturnHandler(object sender, params NamedEventParameter[] parameters);
}
