using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BranchAndChicken.Api.Repository
{
    public class TrainerRepository
    {
        static List<Trainer> _trainers = new List<Trainer>
        {
            new Trainer
            {
                Id = Guid.NewGuid(),
                Name = "Nathan",
                Specialty = Specialty.TaeCluckDoe,
                YearsOfExperience = 0
            },
            new Trainer
            {
                Id = Guid.NewGuid(),
                Name = "Martin",
                Specialty = Specialty.Chudo,
                YearsOfExperience = 12
            },
            new Trainer
            {
                Id = Guid.NewGuid(),
                Name = "Adam",
                Specialty = Specialty.ChravMcgaw,
                YearsOfExperience = 3,
            }
        };

        public void Remove(string name)
        {
            _trainers.Remove(Get(name));
        }

        public Trainer Get(string name)
        {
            return _trainers.FirstOrDefault(trainer => trainer.Name == name);
        }


        public List<Trainer> GetAll()
        {
            return _trainers;
        }

        public Trainer Update(Trainer updatedTrainer, Guid id)
        {
            var user = _trainers.First(trainerCopy => trainerCopy.Id == id);
            user.Name = updatedTrainer.Name;
            user.YearsOfExperience = updatedTrainer.YearsOfExperience;
            user.Specialty = updatedTrainer.Specialty;

            return user;
        }

        public Trainer Add(Trainer newTrainer)
        {
            _trainers.Add(newTrainer);
            return newTrainer;
        }
    }
}
