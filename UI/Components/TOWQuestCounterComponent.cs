using LiveSplit.ComponentUtil;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        private DeepPointer QuestOffset, FoundationOffset;
        private ulong QuestBase, FoundationBase;

        private List<Quest> Quests = new List<Quest>()
        {
            //Emerald Vale
            new Quest() { Offset =  0x14A90,    Name = "frightened_engineer",       CompletionState = 3 },
            new Quest() { Offset =  0xBBF0,     Name = "kindred_spirits",           CompletionState = 1 },
            new Quest() { Offset =  0x94F0,     Name = "fistful_digits",            CompletionState = 3 },
            new Quest() { Offset =  0xB3D0,     Name = "small_grave_matter",        CompletionState = 1 },
            new Quest() { Offset =  0x9F10,     Name = "die_robot",                 CompletionState = 1 },
            new Quest() { Offset =  0xA770,     Name = "long_tomorrow",             CompletionState = 3 },
            new Quest() { Offset =  0x4130,     Name = "illustrated_journal",       CompletionState = 1 },
            new Quest() { Offset =  0x7310,     Name = "comes_now_power",           CompletionState = 1 },
            new Quest() { Offset =  0xE3D0,     Name = "stranger_strange_land",     CompletionState = 1 },

            //Groundbeaker
            new Quest() { Offset =  0x14D0,     Name = "passage_anywhere",          CompletionState = 1 },
            new Quest() { Offset =  0x11DD0,    Name = "balance_due",               CompletionState = 2 },
            new Quest() { Offset =  0x2230,     Name = "puppet_masters",            CompletionState = 1 },
            new Quest() { Offset =  0xF5B0,     Name = "distress_signal",           CompletionState = 5 },
            new Quest() { Offset =  0x10A90,    Name = "warm_spaceship",            CompletionState = 1 },
            new Quest() { Offset =  0x9490,     Name = "solution_vial",             CompletionState = 6 },
            new Quest() { Offset =  0x65F0,     Name = "doom_to_roseway",           CompletionState = 4 },
            new Quest() { Offset =  0xC310,     Name = "silent_voices",             CompletionState = 5 },
            new Quest() { Offset =  0x101D0,    Name = "who_goes_there",            CompletionState = 3 },
            new Quest() { Offset =  0xBF30,     Name = "worst_contact",             CompletionState = 4 },
            new Quest() { Offset =  0xF350,     Name = "space_crime_continuum",     CompletionState = 7 },
            new Quest() { Offset =  0xF350,     Name = "ice_palace",                CompletionState = 10 },
            new Quest() { Offset =  0xB4D0,     Name = "chimerists_experiment",     CompletionState = 4 },
            new Quest() { Offset =  0x11F10,    Name = "salvager_in_sky",           CompletionState = 1 },
            new Quest() { Offset =  0x13550,    Name = "sapphire_wine",             CompletionState = 3 },
            new Quest() { Offset =  0x8370,     Name = "bite_the_sun",              CompletionState = 6 },

            //Roseway
            new Quest() { Offset =  0x6A50,     Name = "by_his_bootstraps",         CompletionState = 5 },
            new Quest() { Offset =  0x6B50,     Name = "vulcans_hammer",            CompletionState = 4 },
            new Quest() { Offset =  0x69D0,     Name = "amateur_alchemist",         CompletionState = 5 },
            new Quest() { Offset =  0x6970,     Name = "journey_into_smoke",        CompletionState = 4 },

            //Byzantium
            new Quest() { Offset =  0x13D10,    Name = "demolished_woman",          CompletionState = 1 },
            new Quest() { Offset =  0x490,      Name = "long_distance",             CompletionState = 1 },
            new Quest() { Offset =  0x11330,    Name = "signal_point",              CompletionState = 20 },
            new Quest() { Offset =  0x710,      Name = "foundation",                CompletionState = 31 },
            new Quest() { Offset =  0xF970,     Name = "secret_not_forgotten",      CompletionState = 7 },
            new Quest() { Offset =  0x117B0,    Name = "at_central",                CompletionState = 40 },
            new Quest() { Offset =  0x5170,     Name = "back_from_retirement",      CompletionState = 40 },
            new Quest() { Offset =  0xBF10,     Name = "low_crusade",               CompletionState = 5 },
            new Quest() { Offset =  0x142F0,    Name = "space_suits_wont_travel",   CompletionState = 70 },
            new Quest() { Offset =  0x4710,     Name = "cupid_laboratory",          CompletionState = 1 },
            new Quest() { Offset =  0x10310,    Name = "all_halcyon_day",           CompletionState = 1 },
            new Quest() { Offset =  0x12D10,    Name = "lying_earth",               CompletionState = 1 },

            //Scylla
            new Quest() { Offset =  0xAF70,     Name = "friendships_due",           CompletionState = 6 },
            new Quest() { Offset =  0x136D0,    Name = "weapons_from_void",         CompletionState = 5 },
            new Quest() { Offset =  0xB7B0,     Name = "empty_man",                 CompletionState = 10 },

            //The Unreliable
            new Quest() { Offset =  0x41F0,     Name = "cleaning_machine",          CompletionState = 1 },

            //Stellar Bay
            new Quest() { Offset =  0x11310,    Name = "radio_free_monarch",        CompletionState = 25 },
            new Quest() { Offset =  0x104F0,    Name = "family_matter",             CompletionState = 5 },
            new Quest() { Offset =  0xC5D0,     Name = "herricks_handiwork",        CompletionState = 6 },
            new Quest() { Offset =  0x14010,    Name = "passion_pills",             CompletionState = 1 },
            new Quest() { Offset =  0x6F70,     Name = "grimm_tomorrow",            CompletionState = 1 },
            new Quest() { Offset =  0xED30,     Name = "secret_people",             CompletionState = 8 },
            new Quest() { Offset =  0x14750,    Name = "stainless_steel_rat",       CompletionState = 8 },
            new Quest() { Offset =  0x112B0,    Name = "bolt_with_his_name",        CompletionState = 25 },
            new Quest() { Offset =  0x112F0,    Name = "errors_unseen",             CompletionState = 25 },
            new Quest() { Offset =  0x11CF0,    Name = "flowers_for_sebastian",     CompletionState = 1 },
            new Quest() { Offset =  0xD7F0,     Name = "picketts_biggest_game",     CompletionState = 4 },
            new Quest() { Offset =  0x3F0,      Name = "star_crossed_troopers",     CompletionState = 1 },
            new Quest() { Offset =  0x10D70,    Name = "mandibles_of_doom",         CompletionState = 12 },
            new Quest() { Offset =  0x9690,     Name = "canids_cradle",             CompletionState = 50 },

            //Amber Heights
            new Quest() { Offset =  0xA190,     Name = "little_memento",            CompletionState = 1 },
            new Quest() { Offset =  0x11290,    Name = "the_commuter",              CompletionState = 25 },
            new Quest() { Offset =  0x17B0,     Name = "pay_for_printer",           CompletionState = 40 },
            new Quest() { Offset =  0x76F0,     Name = "odd_jobs",                  CompletionState = 4 },
            new Quest() { Offset =  0x41D0,     Name = "sucker_bait",               CompletionState = 50 },

            //Fallbrook
            new Quest() { Offset =  0xC9B0,     Name = "slaughterhouse_clive",      CompletionState = 40 },
            new Quest() { Offset =  0x6E70,     Name = "spratkings",                CompletionState = 4 },

            //Phineas' Lab
            new Quest() { Offset =  0xFDF0,     Name = "city_and_stars",            CompletionState = 1 },
            new Quest() { Offset =  0xE8B0,     Name = "brave_new_world",           CompletionState = 2 },
            
            //C&P Boarst Factory
            new Quest() { Offset =  0xC9F0,     Name = "cysty_dance",               CompletionState = 20 }
        };

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
                                QuestOffset = new DeepPointer(0x03D9C7F8, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0);
                                FoundationOffset = new DeepPointer(0x03FF7408, 0xDD8, 0x1A0, 0x1E8, 0x290);
                                break;
                            case 71729152:
                                version = "v1.1 (EGS)";
                                QuestOffset = new DeepPointer(0x03DA3978, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0);
                                FoundationOffset = new DeepPointer(0x03FFE788, 0xDD8, 0x1A0, 0x1E8, 0x290);
                                break;
                            case 74125312:
                                version = "v1.1 (MS)";
                                QuestOffset = new DeepPointer(0x03FF0078, 0x20, 0x0, 0x8, 0x18, 0x8, 0x0);
                                FoundationOffset = new DeepPointer(0x0424AD78, 0xDD8, 0x1A0, 0x1E8, 0x290);
                                break;
                        }
                        IsHooked = QuestOffset.Deref<ulong>(Game, out QuestBase);
                        FoundationOffset.Deref<ulong>(Game, out FoundationBase);
                        if (IsHooked)
                        {
                            Debug.WriteLine($"TOW {version} found.");
                            Debug.WriteLine("Found Quest Offsets");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Quests.Count; i++)
                    {
                        if (Quests[i].Name == "foundation")
                        {
                            int QuestState;
                            Game.ReadValue<int>((IntPtr)(FoundationBase + Quests[i].Offset), out QuestState);
                            bool Completed = QuestState >= Quests[i].CompletionState;
                            Quests[i].Completed = Completed;
                        }
                        else
                        {
                            int QuestState;
                            Game.ReadValue<int>((IntPtr)(QuestBase + Quests[i].Offset), out QuestState);
                            bool Completed = QuestState >= Quests[i].CompletionState;
                            Quests[i].Completed = Completed;
                        }
                        //Debug.WriteLine($"{Quests[i].Name} = {Quests[i].Completed}");
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
