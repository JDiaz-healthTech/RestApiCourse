using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Movies.Api.Mapping;
using Movies.Application.Models;
using Movies.Application.Repositories;
using Movies.Contracts.Requests;
using System.Reflection;

namespace Movies.Api.Controllers;

[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;

    public MoviesController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;

    }

    [HttpPost(ApiEndPoints.Movies.Create)]
    public async Task<IActionResult> Create([FromBody] CreateMovieRequest request)
    {
        var movie = request.MapToMovie();
        await _movieRepository.CreateAsync(movie);
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        //return Created($"/{ApiEndPoints.Movies.Create}{movie.Id}", movie);
    }

    [HttpGet(ApiEndPoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var movie = await _movieRepository.GetByIdAsync(id);
        if (movie is null)

        {
            return NotFound();

        }

        var response = movie.MapToResponse();

        return Ok(response);

    }

    [HttpGet(ApiEndPoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var movies = await _movieRepository.GetAllAsync();
        var movieResponse = movies.MapToResponse();
        return Ok(movieResponse);
    }


    [HttpPut(ApiEndPoints.Movies.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateMovieRequest request)
    {
        var movie = request.MapToMovie(id);
        var updated = await _movieRepository.UpdateAsync(movie);
        if (!updated)
        {
            return NotFound();
        }
        var response = movie.MapToResponse();
        return Ok(response);
    }

    [HttpDelete(ApiEndPoints.Movies.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _movieRepository.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        var response = deleted.ToString();
        return Ok(response);
    }

    [HttpPatch(ApiEndPoints.Movies.Patch)]
    public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] MoviePartialUpdateRequest patch)
    {
        if ((patch is null)||(id == Guid.Empty))
        {
            return BadRequest("Revisa parámetros");
        }


        var movieToPatch = await _movieRepository.GetByIdAsync(id);
        if (movieToPatch == null)
        {
            return NotFound();
        }
        //tittle con dos t's es del DTO de movie.Contracts, el de una t es el de Movie.Aplication.Models
        if(patch.Tittle is not null)
        {
            movieToPatch.Title = patch.Tittle;
        }
        if(patch.YearOfRelease is not null)
        {
            movieToPatch.YearOfRelease = patch.YearOfRelease.Value;
        }
        if (patch.Genres is not null)
        {
            HashSet<string> newGenres = new HashSet<string>(patch.Genres.Select(g => g.Trim()).Where(g => g.Length > 0), StringComparer.OrdinalIgnoreCase);
            HashSet<string> oldGenres = new HashSet<string>(movieToPatch.Genres, StringComparer.OrdinalIgnoreCase);
            //Agrego los que faltan
            foreach (var g in newGenres.Except(oldGenres))
            {
                movieToPatch.Genres.Add(g);
            }
            //Quito los que ya no estan
            foreach (var g in oldGenres.Except(newGenres).ToList())
            {
                movieToPatch.Genres.Remove(g);
            }
        }
            return Ok("Updated Correctly");

        

    }

}




