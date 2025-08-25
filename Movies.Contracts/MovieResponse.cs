﻿namespace Movies.Contracts
{
    public class MovieResponse
    {
        public required Guid Id { get; init; }

        public required string Title { get; set; }

        public required int YearOfRelease { get; set; }

        public required IEnumerable<string> Genres { get; set; } = Enumerable.Empty<string>();
    }
}
