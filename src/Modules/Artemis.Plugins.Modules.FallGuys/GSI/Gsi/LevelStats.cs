using ConsoleTest.FgGsi.StateMachines;
using System;
using System.Collections.Generic;

namespace FallGuys.Gsi
{
    public class LevelStats
    {
        public string Name { get; set; }
        public LevelType Type { get; set; }
        public int Season { get; set; }
        public bool IsFinal { get; set; }

        public LevelStats(string levelName, LevelType type, bool isFinal, int season)
        {
            Name = levelName;
            Type = type;
            Season = season;
            IsFinal = isFinal;
        }

        public static LevelStats GetStatsByLevelName(string levelName)
        {
            if (All.TryGetValue(levelName, out LevelStats stats))
                return stats;
            else
                return new LevelStats("Unknown", LevelType.Unknown, false, 0);
        }

        public static Dictionary<string, LevelStats> All = new(StringComparer.OrdinalIgnoreCase)
        {
            { "round_biggestfan",                 new LevelStats("Big Fans", LevelType.Race, false, 2) },
            { "round_1v1_button_basher",          new LevelStats("Button Bashers", LevelType.Hunt, false, 4) },
            { "round_door_dash",                  new LevelStats("Door Dash", LevelType.Race, false, 1) },
            { "round_gauntlet_02",                new LevelStats("Dizzy Heights", LevelType.Race, false, 1) },
            { "round_iceclimb",                   new LevelStats("Freezy Peak", LevelType.Race, false, 3) },
            { "round_dodge_fall",                 new LevelStats("Fruit Chute", LevelType.Race, false, 1) },
            { "round_chompchomp",                 new LevelStats("Gate Crash", LevelType.Race, false, 1) },
            { "round_gauntlet_01",                new LevelStats("Hit Parade", LevelType.Race, false, 1) },
            { "round_hoops_blockade_solo",        new LevelStats("Hoopsie Legends", LevelType.Hunt, false, 2) },
            { "round_gauntlet_04",                new LevelStats("Knight Fever", LevelType.Race, false, 2) },
            { "round_tunnel_race",                new LevelStats("Roll On", LevelType.Race, false, 4) },
            { "round_see_saw",                    new LevelStats("See Saw", LevelType.Race, false, 1) },
            { "round_shortcircuit",               new LevelStats("Short Circuit", LevelType.Race, false, 4) },
            { "round_skeefall",                   new LevelStats("Ski Fall", LevelType.Hunt, false, 3) },
            { "round_gauntlet_06",                new LevelStats("Skyline Stumble", LevelType.Race, false, 4) },
            { "round_lava",                       new LevelStats("Slime Climb", LevelType.Race, false, 1) },
            { "round_slimeclimb_2",               new LevelStats("Slimescraper", LevelType.Race, false, 4) },
            { "round_tip_toe",                    new LevelStats("Tip Toe", LevelType.Race, false, 1) },
            { "round_gauntlet_05",                new LevelStats("Tundra Run", LevelType.Race, false, 3) },
            { "round_gauntlet_03",                new LevelStats("Whirlygig", LevelType.Race, false, 1) },
            { "round_wall_guys",                  new LevelStats("Wall Guys", LevelType.Race, false, 2) },

            { "round_fruitpunch_s4_show",         new LevelStats("Big Shots", LevelType.Survival, false, 4) },
            { "round_block_party",                new LevelStats("Block Party", LevelType.Survival, false, 1) },
            { "round_hoverboardsurvival_s4_show", new LevelStats("Hoverboard Heroes", LevelType.Survival, false, 4) },
            { "round_jump_club",                  new LevelStats("Jump Club", LevelType.Survival, false, 1) },
            { "round_match_fall",                 new LevelStats("Perfect Match", LevelType.Survival, false, 1) },
            { "round_tunnel",                     new LevelStats("Roll Out", LevelType.Survival, false, 1) },
            { "round_snowballsurvival",           new LevelStats("Snowball Survival", LevelType.Survival, false, 3) },
            { "round_tail_tag",                   new LevelStats("Tail Tag", LevelType.Survival, false, 1) },

            { "round_basketfall_s4_show",         new LevelStats("Basketfall", LevelType.Team, false, 4) },
            { "round_egg_grab",                   new LevelStats("Egg Scramble", LevelType.Team, false, 1) },
            { "round_egg_grab_02",                new LevelStats("Egg Siege", LevelType.Team, false, 2) },
            { "round_fall_ball_60_players",       new LevelStats("Fall Ball", LevelType.Team, false, 1) },
            { "round_ballhogs",                   new LevelStats("Hoarders", LevelType.Team, false, 1) },
            { "round_hoops",                      new LevelStats("Hoopsie Daisy", LevelType.Team, false, 1) },
            { "round_jinxed",                     new LevelStats("Jinxed", LevelType.Team, false, 1) },
            { "round_chicken_chase",              new LevelStats("Pegwin Pursuit", LevelType.Team, false, 3) },
            { "round_territory_control_s4_show",  new LevelStats("Power Trip", LevelType.Team, false, 4) },
            { "round_rocknroll",                  new LevelStats("Rock'N'Roll", LevelType.Team, false, 1) },
            { "round_snowy_scrap",                new LevelStats("Snowy Scrap", LevelType.Team, false, 3) },
            { "round_conveyor_arena",             new LevelStats("Team Tail Tag", LevelType.Team, false, 1) },

            { "round_fall_mountain_hub_complete", new LevelStats("Fall Mountain", LevelType.Race, true, 1) },
            { "round_floor_fall",                 new LevelStats("Hex-A-Gone", LevelType.Survival, true, 1) },
            { "round_jump_showdown",              new LevelStats("Jump Showdown", LevelType.Survival, true, 1) },
            { "round_tunnel_final",               new LevelStats("Roll Off", LevelType.Survival, true, 3) },
            { "round_royal_rumble",               new LevelStats("Royal Fumble", LevelType.Hunt, true, 1) },
            { "round_thin_ice",                   new LevelStats("Thin Ice", LevelType.Survival, true, 3) },

            // Compatibility
            { "round_fruitpunch",                 new LevelStats("Big Shots", LevelType.Survival, false, 4) },
            { "round_hoverboardsurvival",         new LevelStats("Hoverboard Heroes", LevelType.Survival, false, 4) },
            { "round_basketfall",                 new LevelStats("Basketfall", LevelType.Team, false, 4) },
            { "round_territory",                  new LevelStats("Power Trip", LevelType.Team, false, 4) },
            { "round_1v1_button",                 new LevelStats("Button Bashers", LevelType.Hunt, false, 4) },
            { "round_slimeclimb",                 new LevelStats("Slimescraper", LevelType.Race, false, 4) },
        };
    }
}
