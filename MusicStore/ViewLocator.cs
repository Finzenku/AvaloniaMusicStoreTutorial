using HanumanInstitute.MvvmDialogs.Avalonia;

namespace MusicStore
{
    public class ViewLocator : ViewLocatorBase
    {
        /// <inheritdoc/>
        protected override  string GetViewName(object viewModel)
        {
            string text = viewModel.GetType().FullName!.Replace(".ViewModels.", ".Views.");
            if (text.EndsWith("ViewModel"))
            {
                text = text.Remove(text.Length - "ViewModel".Length) + "View";
            }

            return text;
        }
    }
}