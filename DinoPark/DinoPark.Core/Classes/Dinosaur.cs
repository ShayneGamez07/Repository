using DinoPark.Core.Enums;
using DinoPark.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DinoPark.Core.Classes
{
    public abstract class Dinosaur
    {
        private int _weight;
        private int _health = 5;
    
        protected Dinosaur(string name, string species, int weight)
        {
            Name = name;
            Species = species;
            Weight = weight;
            IsSedated = false;
        }

        public string Name { get; set; }

        public string Species { get; }

        public int Weight
        {
            get => _weight;
            set
            {
                if (value < 100)
                    throw new ArgumentException("A dinosaur must weigh at least 100kg");
                _weight = value;
            }
        }

        public int Health
        {
            get => _health;
            set
            {
                if (value < 0) _health = 0;
                else if (value > 5) _health = 5;
                else _health = value;
            }
        }

        public bool IsAlive => Health > 0;

        public bool IsSedated { get; set; }

        public override string ToString()
        {
            if (!IsAlive)
                return $"{Name} ({Species}) 💀";

            StringBuilder hearts = new();
            for (int i = 0; i < Health; i++) hearts.Append("🤎");
            for (int i = Health; i < 5; i++) hearts.Append("🤍");

            string sedated = IsSedated ? " 💤" : "";

            return $"{Name} ({Species}) - {Weight}kg {hearts}{sedated}";
        }
    }
    public class TRex : Dinosaur, ICarnivore, IDangerous
    {
        public TRex(string name, int weight)
            : base(name, "Tyrannosaurus Rex", weight) { }

        public int DangerLevel => 3;

        public string GetWarningMessage()
            => "⚠⚠⚠ EXTREEM GEVAARLIJK – Blijf uit de buurt!";
    }
    public class Triceratops : Dinosaur, IHerbivore
    {
        public Triceratops(string name, int weight)
            : base(name, "Triceratops", weight) { }
    }
    public class Deinocheirus : Dinosaur, IOmnivore, IDangerous
    {
        public Deinocheirus(string name, int weight)
            : base(name, "Deinocheirus", weight) { }

        public int DangerLevel => 1;

        public string GetWarningMessage()
            => "⚠ Gevaarlijk – kan mensen aanvallen!";
    }
    public class DinoParc
    {
        private static readonly Random _random = new();

        public string ParcName { get; }
        public List<Dinosaur> Dinosaurs { get; }

        public DinoParc(string parcName, bool includeDummyData)
        {
            ParcName = parcName;
            Dinosaurs = new List<Dinosaur>();

            if (includeDummyData)
            {
                Dinosaurs.Add(new TRex("Rex", 6000));
                Dinosaurs.Add(new Triceratops("Tricy", 4500));
                Dinosaurs.Add(new Deinocheirus("Dino", 3000));
            }
        }

        public void MoveIn(Dinosaur dino)
        {
            Dinosaurs.Add(dino);
        }

        public bool MoveOut(Dinosaur dino)
        {
            return Dinosaurs.Remove(dino);
        }

        public bool IsReadyToTransport(Dinosaur dino)
        {
            if (dino.Health < 2)
                throw new Exception("Dinosaurus is te zwak voor transport.");

            if (dino.Weight > 8000)
                throw new Exception("Dinosaurus is te zwaar voor transport.");

            if (dino is IDangerous && !dino.IsSedated)
                throw new Exception("Gevaarlijke dinosaurus moet verdoofd zijn.");

            return true;
        }

        public void Transport(Dinosaur dino, DinoParc destination)
        {
            IsReadyToTransport(dino);

            MoveOut(dino);

            dino.Health -= 1;

            bool dies = _random.Next(1, 11) == 1;
            if (dies)
            {
                dino.Health = 0;
                MoveIn(dino);
                throw new Exception("Dinosaurus stierf tijdens transport.");
            }

            destination.MoveIn(dino);
        }

        public void Feed(Enum food, int amount)
        {
            foreach (var dino in Dinosaurs)
            {
                if (!dino.IsAlive) continue;

                bool correctFood = false;

                if (food is Meat meat)
                {
                    if (dino is ICarnivore) correctFood = true;

                    if (dino is IOmnivore && meat <= Meat.Fish)
                        correctFood = true;
                }

                if (food is Plant && (dino is IHerbivore || dino is IOmnivore))
                    correctFood = true;

                if (correctFood)
                {
                    if (food is Meat)
                        dino.Weight += (int)(amount * 0.8);
                    else
                        dino.Weight += (int)(amount * 0.5);

                    dino.Health += 1;
                }
                else
                {
                    dino.Weight -= amount * 2;
                    dino.Health -= 1;
                }
            }
        }
    }
}
