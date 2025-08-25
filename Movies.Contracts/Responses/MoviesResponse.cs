using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Contracts.Responses
{
    public class MoviesResponse
    {
        //public required Guid Id { get; init; }

        //public required string Title { get; set; }

        //public required int YearOfRelease { get; set; }

        public required IEnumerable<MovieResponse> Items { get; init; } = Enumerable.Empty<MovieResponse>();
    }
}
