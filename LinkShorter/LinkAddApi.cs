using System;
using System.Linq;

namespace LinkShorter
{
    public class LinkAddApiPost
    {
        private string _shortPath;
        public string TargetUrl { get; set; }

        public string ShortPath
        {
            get => _shortPath;
            set => _shortPath ??= value;
        }

        //public string accessToken { get; set; }

        public override string ToString()
        {
            return TargetUrl;
        }
        
    }
}