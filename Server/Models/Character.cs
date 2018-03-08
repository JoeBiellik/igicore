﻿using System.Linq;
using CitizenFX.Core;
using System;
using System.Data.Entity;
using static IgiCore.Server.Server;
using IgiCore.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgiCore.Core.Extensions;
using Newtonsoft.Json;
using System.Data.Entity.Migrations;

namespace IgiCore.Server.Models
{
    public class Character : ICharacter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public bool Alive { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public Character()
        {
            Id = GuidGenerator.GenerateTimeBasedGuid();
            Alive = false; // New Characters fresh spawn
        }

        public static Character GetOrCreate(User user, Guid charId)
        {
            Character character = null;
            Debug.WriteLine("Starting Transaction");
            DbContextTransaction transaction = Db.Database.BeginTransaction();
            try
            {
                Debug.WriteLine("Checking for Char");
                if (user.Characters == null || !user.Characters.Any(c => c.Id == charId))
                {
                    Debug.WriteLine($"Character not found, creating new char for userid: {user.Id}  with id: {charId}");
                    character = new Character { UserId = user.Id };
                    Db.Characters.Add(character);
                    Db.SaveChanges();
                }
                else
                {
                    character = (Character)user.Characters.First(c => c.Id == charId);
                    Debug.WriteLine($"Character found for userId: {user.Id}  ID: {charId}");
                }
                Debug.WriteLine("Committing Transaction");
                transaction.Commit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception catch");
                transaction.Rollback();
                Debug.Write(ex.Message);
            }
            return character;
        }

        public static void Save(string json)
        {
            Debug.WriteLine("Saving Character Data");
            Debug.Write(json + "\n");
            Character newChar = JsonConvert.DeserializeObject<Character>(json);
            Db.Characters.AddOrUpdate(newChar);
            Db.SaveChanges();
        }
    }
}