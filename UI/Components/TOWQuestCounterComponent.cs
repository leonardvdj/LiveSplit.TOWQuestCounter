using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.UI.Components
{
    class TOWQuestCounterComponent : IComponent
    {
        public string ComponentName
        {
            get { return "TOW Quest Counter"; }
        }

        public IDictionary<string, Action> ContextMenuControls { get; protected set; }
        protected InfoTextComponent InternalComponent;

        private LiveSplitState _state;
        private int _count;
        private bool IsHooked = false;
        private int UpdateTime = 0;

        private Process Game;
        private int QuestOffset, FoundationOffset;

        private List<Quest> Quests;

        public TOWQuestCounterComponent(LiveSplitState state)
        {
            this.InternalComponent = new InfoTextComponent("Quest Count", "0");

            _state = state;
            _state.OnReset += state_OnReset;
        }

        public void Dispose()
        {
            _state.OnReset -= state_OnReset;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            UpdateTime++;
            if (UpdateTime > 30)
            {
                UpdateTime = 0;
                if (Game != null && Game.HasExited)
                {
                    Game = null;
                    IsHooked = false;
                }
                if (!IsHooked)
                {
                    List<Process> GameProcesses = Process.GetProcesses().ToList().FindAll(x => x.ProcessName.StartsWith("Indiana"));
                    if (GameProcesses.Count > 0)
                    {
                        Game = GameProcesses.First();
                        string version = "";
                        switch (Game.MainModule.ModuleMemorySize)
                        {
                            case 71692288:
                                version = "v1.0 (EGS)";
                                QuestOffset = 0x03D9C7F8;
                                FoundationOffset = 0x03FF7408;
                                break;
                            case 71729152:
                                version = "v1.1 (EGS)";
                                QuestOffset = 0x03DA3978;
                                FoundationOffset = 0x03FFE788;
                                break;
                            case 71880704:
                                version = "v1.2 (EGS)";
                                break;
                            case 74125312:
                                version = "v1.1 (MS)";
                                QuestOffset = 0x03FF0078;
                                FoundationOffset = 0x0424AD78;
                                break;
                        }
                        IsHooked = new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0).Deref<ulong>(Game, out var ignore);
                        if (IsHooked)
                        {
                            Debug.WriteLine($"TOW {version} found.");
                            Debug.WriteLine("Found Quest Offsets");

                            Quests = new List<Quest>()
                            {
                                //Emerald Vale
                                new Quest("frightened_engineer",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x14A90),
                                            new Comparison[] {
                                                new Comparison(3, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("kindred_spirits",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xBBF0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("fistful_digits",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x94F0),
                                            new Comparison[] {
                                                new Comparison(3, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("small_grave_matter",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xB3D0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("die_robot",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x9F10),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("long_tomorrow",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xA770),
                                            new Comparison[] {
                                                new Comparison(3, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("illustrated_journal",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x4130),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("comes_now_power",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x7310),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("stranger_strange_land",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xE3D0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Groundbreaker
                                new Quest("passage_anywhere",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x14D0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("balance_due",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11DD0),
                                            new Comparison[] {
                                                new Comparison(2, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("puppet_masters",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x2230),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("distress_signal",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xF5B0),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("warm_spaceship",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x10A90),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("solution_vial",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x9490),
                                            new Comparison[] {
                                                new Comparison(6, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("doom_to_roseway",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x65F0),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("silent_voices",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xC310),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("who_goes_there",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x101D0),
                                            new Comparison[] {
                                                new Comparison(3, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("worst_contact",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xBF30),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("space_crime_continuum",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xF350),
                                            new Comparison[] {
                                                new Comparison(7, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("ice_palace",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xF350),
                                            new Comparison[] {
                                                new Comparison(10, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("chimerists_experiment",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xB4D0),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("salvager_in_sky",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11F10),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("sapphire_wine",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x13550),
                                            new Comparison[] {
                                                new Comparison(3, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("bite_the_sun",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x8370),
                                            new Comparison[] {
                                                new Comparison(6, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Roseway
                                new Quest("by_his_bootstraps",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x6A50),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("vulcans_hammer",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x6B50),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("amateur_alchemist",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x69D0),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("journey_into_smoke",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x6970),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Byzantium
                                new Quest("demolished_woman",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x13D10),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("long_distance",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x490),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("signal_point",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11330),
                                            new Comparison[] {
                                                new Comparison(20, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("foundation",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(FoundationOffset, 0xDD8, 0x1A0, 0x1E8, 0x290, 0x710),
                                            new Comparison[] {
                                                new Comparison(31, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("secret_not_forgotten",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xF970),
                                            new Comparison[] {
                                                new Comparison(7, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("at_central",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x117B0),
                                            new Comparison[] {
                                                new Comparison(40, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("back_from_retirement",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x5170),
                                            new Comparison[] {
                                                new Comparison(40, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("low_crusade",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xBF10),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("space_suits_wont_travel",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x142F0),
                                            new Comparison[] {
                                                new Comparison(70, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("cupid_laboratory",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x4710),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("all_halcyon_day",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x10310),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("lying_earth",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x12D10),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Scylla
                                new Quest("friendships_due",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xAF70),
                                            new Comparison[] {
                                                new Comparison(6, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("weapons_from_void",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x136D0),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("empty_man",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xB7B0),
                                            new Comparison[] {
                                                new Comparison(10, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //The Unreliable
                                new Quest("cleaning_machine",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x41F0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Stellar Bay
                                new Quest("radio_free_monarch",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11310),
                                            new Comparison[] {
                                                new Comparison(25, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("family_matter",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x104F0),
                                            new Comparison[] {
                                                new Comparison(5, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("herricks_handiwork",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xC5D0),
                                            new Comparison[] {
                                                new Comparison(6, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("passion_pills",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x14010),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("grimm_tomorrow",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x6F70),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("secret_people",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xED30),
                                            new Comparison[] {
                                                new Comparison(8, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("stainless_steel_rat",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x14750),
                                            new Comparison[] {
                                                new Comparison(8, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("bolt_with_his_name",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x112B0),
                                            new Comparison[] {
                                                new Comparison(25, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("errors_unseen",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x112F0),
                                            new Comparison[] {
                                                new Comparison(25, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("flowers_for_sebastian",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11CF0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("picketts_biggest_game",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xD7F0),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("star_crossed_troopers",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x3F0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("mandibles_of_doom",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x10D70),
                                            new Comparison[] {
                                                new Comparison(12, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("canids_cradle",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x9690),
                                            new Comparison[] {
                                                new Comparison(50, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Amber Heights
                                new Quest("little_memento",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xA190),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("the_commuter",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x11290),
                                            new Comparison[] {
                                                new Comparison(25, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("pay_for_printer",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x17B0),
                                            new Comparison[] {
                                                new Comparison(40, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("odd_jobs",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x76F0),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("sucker_bait",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x41D0),
                                            new Comparison[] {
                                                new Comparison(50, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Fallbrook
                                new Quest("slaughterhouse_clive",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xC9B0),
                                            new Comparison[] {
                                                new Comparison(40, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("spratkings",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0x6E70),
                                            new Comparison[] {
                                                new Comparison(4, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //Phineas' Lab
                                new Quest("city_and_stars",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xFDF0),
                                            new Comparison[] {
                                                new Comparison(1, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                new Quest("brave_new_world",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xE8B0),
                                            new Comparison[] {
                                                new Comparison(2, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                                //C&P Boarst Factory
                                new Quest("cysty_dance",
                                    new MultiPointer[] {
                                        new MultiPointer(new DeepPointer(QuestOffset, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0, 0xC9F0),
                                            new Comparison[] {
                                                new Comparison(20, Comparison.GREATER_THAN_OR_EQUAL)
                                            })
                                    }),
                            };
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Quests.Count; i++)
                    {
                        for (int x = 0; x < Quests[i].Pointers.Length; x++)
                        {
                            int completionState = 0;
                            Quests[i].Pointers[x].Pointer.Deref<int>(Game, out completionState);
                            for (int z = 0; z < Quests[i].Pointers[x].Comparisons.Length; z++)
                            {
                                Quests[i].Pointers[x].Comparisons[z].Completed = true;
                                switch (Quests[i].Pointers[x].Comparisons[z].Comparator)
                                {
                                    case Comparison.EQUALS:
                                        if (!Quests[i].Pointers[x].Comparisons[z].CompletionStates.Contains(completionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                    case Comparison.NOT_EQUALS:
                                        if (Quests[i].Pointers[x].Comparisons[z].CompletionStates.Contains(completionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                    case Comparison.GREATER_THAN:
                                        if (!(completionState > Quests[i].Pointers[x].Comparisons[z].CompletionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                    case Comparison.GREATER_THAN_OR_EQUAL:
                                        if (!(completionState >= Quests[i].Pointers[x].Comparisons[z].CompletionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                    case Comparison.LESS_THAN:
                                        if (!(completionState < Quests[i].Pointers[x].Comparisons[z].CompletionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                    case Comparison.LESS_THAN_OR_EQUAL:
                                        if (!(completionState <= Quests[i].Pointers[x].Comparisons[z].CompletionState))
                                            Quests[i].Pointers[x].Comparisons[z].Completed = false;
                                        break;
                                }
                            }
                            Quests[i].Pointers[x].Completed = Array.FindAll(Quests[i].Pointers[x].Comparisons, k => k.Completed == true).Length == Quests[i].Pointers[x].Comparisons.Length;
                        }
                        Quests[i].Completed = Array.FindAll(Quests[i].Pointers, k => k.Completed == true).Length != 0;
                    }

                    _count = Quests.FindAll(x => x.Completed == true).Count;
                }
            }

            if (invalidator != null && this.InternalComponent.InformationValue != _count.ToString())
            {
                this.InternalComponent.InformationValue = _count.ToString();
                invalidator.Invalidate(0f, 0f, width, height);
            }
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region region)
        {
            this.PrepareDraw(state);
            this.InternalComponent.DrawVertical(g, state, width, region);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region region)
        {
            this.PrepareDraw(state);
            this.InternalComponent.DrawHorizontal(g, state, height, region);
        }

        void PrepareDraw(LiveSplitState state)
        {
            this.InternalComponent.NameLabel.ForeColor = state.LayoutSettings.TextColor;
            this.InternalComponent.ValueLabel.ForeColor = state.LayoutSettings.TextColor;
            this.InternalComponent.NameLabel.HasShadow
                = this.InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;
        }

        void state_OnReset(object sender, TimerPhase t)
        {
            _count = 0;
        }

        public XmlNode GetSettings(XmlDocument document) { return document.CreateElement("Settings"); }
        public Control GetSettingsControl(LayoutMode mode) { return null; }
        public void SetSettings(XmlNode settings) { }
        public void RenameComparison(string oldName, string newName) { }
        public float MinimumWidth { get { return this.InternalComponent.MinimumWidth; } }
        public float MinimumHeight { get { return this.InternalComponent.MinimumHeight; } }
        public float VerticalHeight { get { return this.InternalComponent.VerticalHeight; } }
        public float HorizontalWidth { get { return this.InternalComponent.HorizontalWidth; } }
        public float PaddingLeft { get { return this.InternalComponent.PaddingLeft; } }
        public float PaddingRight { get { return this.InternalComponent.PaddingRight; } }
        public float PaddingTop { get { return this.InternalComponent.PaddingTop; } }
        public float PaddingBottom { get { return this.InternalComponent.PaddingBottom; } }
    }
}
