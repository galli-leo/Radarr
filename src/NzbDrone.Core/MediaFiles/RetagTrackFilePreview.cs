﻿using System;
using System.Collections.Generic;

namespace NzbDrone.Core.MediaFiles
{
    public class RetagTrackFilePreview
    {
        public int AuthorId { get; set; }
        public int BookId { get; set; }
        public List<int> TrackNumbers { get; set; }
        public int TrackFileId { get; set; }
        public string Path { get; set; }
        public Dictionary<string, Tuple<string, string>> Changes { get; set; }
    }
}