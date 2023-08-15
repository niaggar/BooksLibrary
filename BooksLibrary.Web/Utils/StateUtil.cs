namespace BooksLibrary.Web.Utils
{
    public class StateUtil<T>
    {
        private T? _prop;
        public T Property
        {
            get => _prop ?? default;
            set
            {
                _prop = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
