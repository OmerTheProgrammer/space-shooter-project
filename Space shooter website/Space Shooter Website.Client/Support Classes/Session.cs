using Client_Manager___API;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Entitys;
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

        public void ToggleSettings(IJSRuntime JSRuntime)
        {
            IsSettingsVisible = !IsSettingsVisible;
            JSRuntime.InvokeVoidAsync("UpdatePaused", IsSettingsVisible);
            // This triggers StateHasChanged in the Layout because it's subscribed
            UpdateScreenFunc?.Invoke();
        }

        public void ToggleMusic(IJSRuntime JSRuntime)
        {
            Player CurrentPlayer = CurrentUser as Player;
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsMusicOn = !CurrentPlayer.IsMusicOn;
                JSRuntime.InvokeVoidAsync("UpdateUserMusicPrefrence", CurrentPlayer.IsMusicOn);
            }
            // This triggers StateHasChanged in the Layout because it's subscribed
            UpdateScreenFunc?.Invoke();
        }

        public void ToggleSound(IJSRuntime JSRuntime)
        {
            Player CurrentPlayer = CurrentUser as Player;
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsSoundOn = !CurrentPlayer.IsSoundOn;
                JSRuntime.InvokeVoidAsync("UpdateUserSoundPrefrence", CurrentPlayer.IsSoundOn);
            }
            // This triggers StateHasChanged in the Layout because it's subscribed
            UpdateScreenFunc?.Invoke();
        }

        public RunInfo CurrentRun { get; set; } = new RunInfo();

        public void Logout()
        {
            CurrentUser = null;
            IsAdmin = false;
            IsPlayer = false;
            UpdateScreenFunc?.Invoke();
        }

        //HUD
        public event Action? UpdateScreenFunc;
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
            UpdateScreenFunc?.Invoke();
        }

        //public void ResetRun()
        //{
        //    CurrentRun = new RunInfo();
        //    Progress = "0%";
        //    IsEndless = false;
        //    UpdateScreenFunc?.Invoke();
        //}

        public async void SaveRun(ApiService api, bool HadWon, IJSRuntime JS)
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
                    //temp remamber to delete in call Game.SaveGameResult() and here at func title
                    await JS.InvokeVoidAsync("print", "Saving " + CurrentRun + " To DB.");
                    Console.WriteLine();

                }
                catch (Exception ex)
                {
                    throw new ExpandedException("Error saving run info: ", ex.Message);
                }
            }
        }
    }
}