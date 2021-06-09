using System;

namespace LinkShorter
{
    public class LinkData
    {
        public Guid Id { get; set; }
        public string TargetUrl { get; set; }
        public string ShortPath { get; set; }
        public int ClickCounter { get; set; }
        public string TimeStamp { get; set; }
        public Guid CreatorId { get; set; }
    }
}