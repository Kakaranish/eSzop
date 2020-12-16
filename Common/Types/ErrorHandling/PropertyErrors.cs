﻿using System.Collections.Generic;

namespace Common.Types.ErrorHandling
{
    public class PropertyErrors
    {
        public string Property { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}
