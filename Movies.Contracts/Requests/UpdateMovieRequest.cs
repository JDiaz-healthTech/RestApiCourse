using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Requests
{
    public class UpdateMovieRequest
    {
        public required string Title { get; set; }

        public required int YearOfRelease { get; set; }

        public required IEnumerable<string> Genres { get; set; } = Enumerable.Empty<string>();
    }

}

