using BranchAndChicken.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BranchAndChicken.Api.Repository
{
    public class TrainerRepository
    {
        string _connectionString = "Server=localhost;Database=BranchAndChicken;Trusted_Connection=True";

        Trainer GetTrainerFromDataReader(SqlDataReader dataReader)
        {
            // explicit castt
            var id = (int)dataReader["id"];
            // implicit cast
            var name = dataReader["name"] as string;
            // convert to
            var yearsOfExperience = Convert.ToInt32(dataReader["yearsofexperience"]);
            // try parse
            Enum.TryParse<Specialty>(dataReader["specialty"].ToString(), out var specialty);

            var trainer = new Trainer
            {
                Id = id,
                Name = name,
                YearsOfExperience = yearsOfExperience,
                Specialty = specialty
            };

            return trainer;
        }

        public bool Remove(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "delete from Trainer where [name] = @name";
                cmd.Parameters.AddWithValue("name", name);
                return cmd.ExecuteNonQuery() == 1;
            }
        }

        public Trainer Get(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Trainer where Trainer.Name = @name";
                cmd.Parameters.AddWithValue("name", name);
                var dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    return GetTrainerFromDataReader(dataReader);
                }
            }
            return null;
        }

        public List<Trainer> GetAll()
        {
            var trainers = new List<Trainer>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select * from Trainer";
                var dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    trainers.Add(GetTrainerFromDataReader(dataReader));
                }
            }
            return trainers;
        }

        public Trainer Update(Trainer updatedTrainer, int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"update [Trainer]
                                    set[Name] = @name
                                    ,[YearsOfExperience] = @yearsOfExperience
                                    ,[Specialty] = @specialty
                                    output inserted.*
                                    where id = @id";
                cmd.Parameters.AddWithValue("name", updatedTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", updatedTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("specialty", updatedTrainer.Specialty);
                cmd.Parameters.AddWithValue("id", id);
                var dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    return GetTrainerFromDataReader(dataReader);
                }

                return null;
            }
        }

        public Trainer Add(Trainer newTrainer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"insert into [Trainer]
                                    ([Name]
                                    ,[YearsOfExperience]
                                    ,[Specialty])
                                    output inserted*
                                    values
                                    (@name
                                    , @yearsOfExperience
                                    , @specialty)";
                cmd.Parameters.AddWithValue("name", newTrainer.Name);
                cmd.Parameters.AddWithValue("yearsOfExperience", newTrainer.YearsOfExperience);
                cmd.Parameters.AddWithValue("specialty", newTrainer.Specialty);
                var dataReader = cmd.ExecuteReader();

                if (dataReader.Read())
                {
                    return GetTrainerFromDataReader(dataReader);
                }

                return null;
            }
        }
    }
}
