using Avalonia.Interactivity;

namespace YngveHestem.GenericParameterCollection.Avalonia
{
    public class ParameterCollectionViewOnChangeEventArgs : RoutedEventArgs
    {
        /// <summary>
		/// This provides the full ParameterCollection that you also can get by calling the ParameterCollectionView.ParameterCollection.
		/// </summary>
		public ParameterCollection NewParameterCollection { get; }

		/// <summary>
		/// The key for the parameter that was updated.
		/// </summary>
		public string ParameterKey { get; }

		public ParameterCollectionViewOnChangeEventArgs(RoutedEvent routedEvent, ParameterCollection newParameterCollection, string parameterKey = null) : base(routedEvent)
		{
			NewParameterCollection = newParameterCollection;
			ParameterKey = parameterKey;
		}
    }
}
