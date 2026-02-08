using Model.Entitys;
using Client_Manager___API;
using Microsoft.AspNetCore.Components;
using ViewModel;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class Session
    {
        public User? CurrentUser { get; set; }
        public int SelectedLevel { get; set; } = 1;
        public bool IsAdmin { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsLoggedIn => CurrentUser != null;

        public string Progress = "100%";
        public bool IsEndless { get; set; } = false;

        public bool IsSettingsVisible { get; set; } = false;
        public void ToggleSettings()
        {
            IsSettingsVisible = !IsSettingsVisible;
            // This triggers StateHasChanged in the Layout because it's subscribed
            OnGameStateChanged?.Invoke();
        }

        public RunInfo CurrentRun { get; set; } = new RunInfo();

        public void Logout()
        {
            CurrentUser = null;
            IsAdmin = false;
            IsPlayer = false;
        }

        //HUD
        public event Action? OnGameStateChanged;
        public void UpdateGameStats(int hp, int score, int level, int shield, int blasters, int killed, int maxEnemies, bool isEndless)
        {
            if(CurrentRun != null)
            {
                CurrentRun.CurrentHp = hp;
                CurrentRun.CurrentScore = score;
                CurrentRun.CurrentLevel = level;
                CurrentRun.CurrentShieldLevel = shield;
                CurrentRun.CurrentBlasterCount = blasters;
                IsEndless = isEndless;
            }

            // --- PROGRESS CALCULATION ---
            if (IsEndless)
            {
                Progress = "100%";
            }
            else if (maxEnemies > 0)
            {
                // Percentage of enemies killed vs total level enemies
                double progressPercent = (1-(((double)killed / maxEnemies))) * 100;
                // Add 'public int Progress {get; set;}' to RunInfo. and the rest
                // for now: field here - a way to move the data between pages
                Progress = progressPercent.ToString("F2") + "%";
            }

            // Notify the Layout to re-render the HUD
            OnGameStateChanged?.Invoke();
        }

        //public void ResetRun()
        //{
        //    CurrentRun = new RunInfo();
        //    Progress = "0%";
        //    IsEndless = false;
        //    OnGameStateChanged?.Invoke();
        //}

        public async void SaveRun(ApiService api, bool HadWon)
        {
            CurrentRun.Player = CurrentUser as Player;
            if (CurrentUser != null && CurrentRun.Player != null)
            {
                CurrentRun.RunStopDate = DateTime.Now;
                CurrentRun.IsRunOver = HadWon; //Set Wrong - temporary before DB changes
                try
                {
                    //await api.InsertRunInfo(
                    //    CurrentRun
                    //);
                    Console.WriteLine("Saving " + CurrentRun + " To DB.");
                }
                catch (Exception ex)
                {
                    throw new ExpandedException("Error saving run info: ", ex.Message);
                }
            }
        }
    }
}