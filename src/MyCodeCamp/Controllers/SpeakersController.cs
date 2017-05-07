using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCodeCamp.Data;
using MyCodeCamp.Data.Entities;
using MyCodeCamp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyCodeCamp.Controllers
{
    [Authorize]
    [Route("api/camps/{moniker}/speakers")]
    public class SpeakersController : BaseController
    {
        private ILogger<SpeakersController> _logger;
        private IMapper _mapper;
        private ICampRepository _repo;

        public SpeakersController(ICampRepository repo,
            ILogger<SpeakersController> logger,
            IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var speakers = includeTalks ? _repo.GetSpeakersByMonikerWithTalks(moniker) : _repo.GetSpeakersByMoniker(moniker);

                return Ok(_mapper.Map<IEnumerable<SpeakerModel>>(speakers));
            }
            catch
            {
            }

            return BadRequest();
        }

        [HttpGet("{id}", Name = "SpeakerGet")]
        public ActionResult Get(string moniker, int id, bool includeTalks = false)
        {
            try
            {
                var speaker = includeTalks ? _repo.GetSpeakerWithTalks(id) : _repo.GetSpeaker(id);
                if (speaker == null) return NotFound();
                if (speaker.Camp.Moniker != moniker) return BadRequest("Speaker not in specified camp");

                return Ok(_mapper.Map<SpeakerModel>(speaker));
            }
            catch
            {
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult> Post(string moniker, [FromBody]SpeakerModel model)
        {
            try
            {
                var camp = _repo.GetCampByMoniker(moniker);
                if (camp == null) return NotFound($"Camp {moniker} was not found");

                var speaker = _mapper.Map<Speaker>(model);
                speaker.Camp = camp;

                _repo.Add(speaker);

                if (await _repo.SaveAllAsync())
                {
                    var url = Url.Link("SpeakerGet", new { moniker = camp.Moniker, id = speaker.Id });
                    return Created(url, _mapper.Map<SpeakerModel>(speaker));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while adding speaker: {ex}");
            }

            return BadRequest("Could not add new speaker");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string moniker, int id,
            [FromBody] SpeakerModel model)
        {
            try
            {
                var speaker = _repo.GetSpeaker(id);
                if (speaker == null) return NotFound($"Could not find a speaker with an ID of {id}");
                if (speaker.Camp.Moniker != moniker) return BadRequest("Speaker and Camp do not match");

                _mapper.Map(model, speaker);

                if (await _repo.SaveAllAsync())
                {
                    return Ok(_mapper.Map<SpeakerModel>(speaker));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while updating speaker: {ex}");
            }

            return BadRequest("Could not update speaker");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string moniker, int id)
        {
            try
            {
                var speaker = _repo.GetSpeaker(id);
                if (speaker == null) return NotFound($"Could not find a speaker with an ID of {id}");
                if (speaker.Camp.Moniker != moniker) return BadRequest("Speaker and Camp do not match");

                _repo.Delete(speaker);

                if (await _repo.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception thrown while deleting speaker: {ex}");
            }
            return BadRequest("Could not delete speaker");
        }
    }
}
