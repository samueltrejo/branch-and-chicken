using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BranchAndChicken.Api.Commands;
using BranchAndChicken.Api.Models;
using BranchAndChicken.Api.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BranchAndChicken.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Trainer>> GetAllTrainers()
        {
            return new TrainerRepository().GetAll();
        }

        [HttpGet("{name}")]
        public ActionResult<Trainer> GetByName(string name)
        {
            return new TrainerRepository().Get(name);
        }

        [HttpDelete("{name}")]
        public IActionResult DeleteTrainer(string name)
        {
            new TrainerRepository().Remove(name);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTrainer(UpdateTrainerCommand updatedTrainerCommand, Guid id)
        {
            var repo = new TrainerRepository();
            var updatedTrainer = new Trainer
            {
                Name = updatedTrainerCommand.Name,
                YearsOfExperience = updatedTrainerCommand.YearsOfExperience,
                Specialty = updatedTrainerCommand.Specialty,
            };
            var trainer = repo.Update(updatedTrainer, id);

            return Ok(trainer);
        }

        [HttpPost]
        public IActionResult CreateTrainer(AddTrainerCommand newTrainerCommand)
        {
            var newTrainer = new Trainer()
            {
                Id = Guid.NewGuid(),
                Name = newTrainerCommand.Name,
                YearsOfExperience = newTrainerCommand.YearsOfExperience,
                Specialty = newTrainerCommand.Specialty,
            };

            var repo = new TrainerRepository();
            var craetedTrainer = repo.Add(newTrainer);

            return Created(uri: $"api/trainers/{craetedTrainer.Name}", craetedTrainer);
        }
    }
}