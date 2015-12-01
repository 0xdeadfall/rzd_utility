﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

using RussianRailwaysUtility.Utils;
using RussianRailwaysUtility.Entities;

namespace RussianRailwaysUtility {
    class Program {

        private const int TIMER_DELAY = 60;// минут
        static void Main(string[] args) {
            try {
                Timer timer = new Timer(TIMER_DELAY * 1000 * 60);
                timer.AutoReset = true;
                timer.Elapsed += OnTimerTick;

                string choice = "";
                do {
                    Console.WriteLine("\n=======================================");
                    Console.WriteLine("СТАТУС: таймер " + ((timer.Enabled) ? "запущен" : "остановлен"));
                    Console.WriteLine("=======================================");
                    Console.WriteLine("exit - для выхода");
                    Console.WriteLine("start/stop - для старта/остановки таймера");
                    Console.WriteLine();
                    choice = Console.ReadLine();

                    if (choice.Equals("start", StringComparison.OrdinalIgnoreCase)) {
                        timer.Start();
                        Console.WriteLine("Таймер запущен!");
                    } else if (choice.Equals("stop", StringComparison.OrdinalIgnoreCase)) {
                        timer.Stop();
                        Console.WriteLine("Таймер остановлен!");
                    }

                } while (!choice.Equals("exit", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[ЕГГОР] Возникло исключение! Описание: " + ex.Message);
                Console.WriteLine("StackTrace:\n" + ex.StackTrace);
                Console.ForegroundColor = oldColor;
            }
            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey(false);
        }

        private static void OnTimerTick(object src, ElapsedEventArgs args) {
            try {
                ResponseObject robj = RzdUtility.AskSite();
                Console.WriteLine(PrintResults(robj));
            }
            catch (Exception ex) {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[ЕГГОР] Возникло исключение! Описание: " + ex.Message);
                Console.WriteLine("StackTrace:\n" + ex.StackTrace);
                Console.ForegroundColor = oldColor;
            }
        }

        private static string PrintResults(ResponseObject obj) {

            if (obj != null && obj.ResultCode.Equals("OK", StringComparison.OrdinalIgnoreCase)) {
                RequestInfo info = obj.RequestInfo[0];
                if (info != null) {
                    IList<Train> trains = info.Trains;
                    if (trains != null && trains.Count > 0) {
                        StringBuilder sb = new StringBuilder();
                        foreach (var train in trains) {
                            sb.AppendFormat(
@"Дата отбытия: {0} {1}
Дата прибытия: {2} {3}
Время в пути: {4}
Поезд #{5}
Пункт отправления: {6}
Пункт назначения: {7}
Перевозчик: {8}
",
                                train.DepDate, train.DepTime,
                                train.ArrivalDate, train.ArrivalTime,
                                train.TimeInWay,
                                train.TripNumber,
                                train.StationFrom, train.StationTo,
                                train.Carrier);
                            sb.AppendLine("Места:");
                            foreach (var seat in train.FoundSeats) {
                                sb.AppendFormat(@"-- {0}, {1} штук по {2} руб.", seat.Type, seat.FreeSeatsCount, seat.Price).AppendLine();
                            }
                            sb.AppendLine();
                        }
                        return sb.ToString();
                    } else {
                        return "Поездов не найдено.";
                    }
                } else {
                    throw new Exception("Объект RequestInfo не найден!");
                }
            } else {
                throw new Exception("Ошибка запроса!");
            }
            throw new Exception("Общая ошибка!");
        }
    }
}
