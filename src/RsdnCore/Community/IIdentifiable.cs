namespace Rsdn.Community
{
	/// <summary>
	/// Represents an object with the identity property.
	/// </summary>
	public interface IIdentifiable
	{
		/// <summary>
		/// Gets the identity value.
		/// </summary>
		int Id { get; }
	}
}
