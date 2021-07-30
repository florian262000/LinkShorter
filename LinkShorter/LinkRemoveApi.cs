namespace LinkShorter
{
    public class LinkApiRemove
    {
        private string _shortPath;

        public string ShortPath
        {
            get => _shortPath;
            set => _shortPath ??= value;
        }
        
    }
}