using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace BranchAndChicken.Api.Repository
{
    public class TrainerRepository
    {
        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True";

        public bool Remove(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "delete from Trainer where [name] = @name";
                return db.Execute(sql, new { name }) >= 1;
            }
        }

        public Trainer Get(string name)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = "select * from Trainer where Trainer.Name = @trainerName";
                var trainer = db.QueryFirst<Trainer>(sql, new { trainerName = name});
                return trainer;
            }
        }

        public List<Trainer> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var trainers = db.Query<Trainer>("select * from Trainer");
                return trainers.ToList();
            }
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"update [Trainer]
                                    set[Name] = @name
                                    ,[YearsOfExperience] = @yearsOfExperience
                                    ,[Specialty] = @specialty
                                    output inserted.*
                                    where id = @id";

                updatedTrainer.Id = id;
                var trainer = db.QueryFirst<Trainer>(sql, updatedTrainer);
                return trainer;
            }
        }

        public Trainer Add(Trainer newTrainer)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var sql = @"insert into [Trainer]
                                    ([Name]
                                    ,[YearsOfExperience]
                                    ,[Specialty])
                                    output inserted.*
                                    values
                                    (@name
                                    , @yearsOfExperience
                                    , @specialty)";

                var trainer = db.QueryFirst<Trainer>(sql, newTrainer);
                return trainer;
            }
        }
    }
}
