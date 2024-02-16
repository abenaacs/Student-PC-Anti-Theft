using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeCampus.Models;
using SafeCampus.Services;

namespace SafeCampus.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TrackController : ControllerBase
{
    private readonly TrackService _trackService;

    public TrackController(TrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet,Authorize(Roles ="Admin,Guard")]
    public async Task<ActionResult<List<Track>>> Get()
    {
        var tracks = await _trackService.GetAllTracks();
        return Ok(tracks);
    }

    [HttpGet("{id:length(24)}", Name = "GetTrack"), Authorize("Guard")]
    public async Task<ActionResult<Track>> Get(string id)
    {
        var track = await _trackService.GetTrackById(id);

        if (track == null)
        {
            return NotFound();
        }

        return Ok(track);
    }

    [HttpPost, Authorize("Guard")]
    public async Task<ActionResult<Track>> Create(Track track)
    {
        await _trackService.CreateTrack(track);
        return CreatedAtRoute("GetTrack", new { id = track.Id }, track);
    }

    [HttpPut("{id:length(24)}"), Authorize(Roles = "Guard")]
    public async Task<IActionResult> Update(string id, Track track)
    {
        var existingTrack = await _trackService.GetTrackById(id);

        if (existingTrack == null)
        {
            return NotFound();
        }

        track.CreatedAt = existingTrack.CreatedAt;
        track.UpdatedAt = DateTime.UtcNow;

        await _trackService.UpdateTrack(id, track);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}"),Authorize(Roles = "Guard")]
    public async Task<IActionResult> Delete(string id)
    {
        var existingTrack = await _trackService.GetTrackById(id);

        if (existingTrack == null)
        {
            return NotFound();
        }

        await _trackService.DeleteTrack(id);

        return NoContent();
    }
}