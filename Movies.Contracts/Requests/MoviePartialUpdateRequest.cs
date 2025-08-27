using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Requests
{
    public sealed class MoviePartialUpdateRequest
    {
        public string? Tittle { get; init; }
        public int? YearOfRelease { get; init; }
        public List<string>? Genres { get; init; }
    }
}
