using Ci.Sort;
using Ci.Sort.Enums;
using Ci.Sort.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ci.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var sortOrder = new SortOrder
            {
                Key = nameof(Engine.Name),
                Order = Order.Descending
            };

            foreach (var car in DataGenerator.GetCars())
            {
                Console.WriteLine($"Car => Engine => {car.Engine.Name}; {car.Engine.EnginePower}; Seat = > {car.CarSeat.SeatType}");
            }

            foreach (var engine in DataGenerator.GetCars().Select(c => c.Engine).Sort(sortOrder))
            {
                Console.WriteLine($"Engine => {engine.Name}; {engine.EnginePower}");
            }

            foreach (var car in DataGenerator.GetCars().Sort(sortOrder))
            {
                Console.WriteLine($"Car => Engine => {car.Engine.Name}; {car.Engine.EnginePower}; Seat = > {car.CarSeat.SeatType}");
            }

            foreach (var car in DataGenerator.GetCars().AsQueryable().Sort(sortOrder))
            {
                Console.WriteLine($"Car => Engine => {car.Engine.Name}; {car.Engine.EnginePower}; Seat = > {car.CarSeat.SeatType}");
            }


            Console.ReadLine();
        }
    }

    class Engine
    {
        public string Name { get; set; }
        public int EnginePower { get; set; }
    }

    class CarSeat
    {
        public SeatType SeatType { get; set; }
    }

    class Car
    {
        public Engine Engine { get; set; }
        public CarSeat CarSeat { get; set; }
    }

    enum SeatType
    {
        Skin,
        Rubber
    }

    static class DataGenerator
    {
        [DebuggerStepThrough]
        public static List<Car> GetCars()
            => new List<Car>
            {
                new Car
                {
                    CarSeat = new CarSeat
                    {
                        SeatType = SeatType.Rubber
                    },
                    Engine = new Engine
                    {
                        EnginePower = 10,
                        Name = "01 Engine"
                    }
                },

                new Car
                {
                    CarSeat = new CarSeat
                    {
                        SeatType = SeatType.Rubber
                    },
                    Engine = new Engine
                    {
                        EnginePower = 12,
                        Name = "02 Engine"
                    }
                },


                new Car
                {
                    CarSeat = new CarSeat
                    {
                        SeatType = SeatType.Skin
                    },
                    Engine = new Engine
                    {
                        EnginePower = 30,
                        Name = "03 Engine"
                    }
                },
            };
    }
}

