using Client_Manager___API;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using System.Threading.Tasks;
using ViewModel;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class Session
    {
        public User? CurrentUser { get; set; }
        public int SelectedLevel { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
        public bool IsContuiningRun = false;

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

        public async Task ToggleMusic(IJSRuntime JSRuntime, ApiService apiService)
        {
            Player CurrentPlayer = CurrentUser as Player;
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsMusicOn = !CurrentPlayer.IsMusicOn;
                try
                {
                    JSRuntime.InvokeVoidAsync("UpdateUserMusicPrefrence", CurrentPlayer.IsMusicOn);
                    await apiService.UpdatePlayer(
                        PlayerDTO.FromEntity(CurrentPlayer, dto =>
                        {
                            dto.IsMusicOn = CurrentPlayer.IsMusicOn;
                        })
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Updateing Music Currently Failed: " + ex.Message);
                    JSRuntime.InvokeVoidAsync("ShowAlert", "Updateing Music Currently Failed: " + ex.Message);
                }
            }

            // This triggers StateHasChanged in the Layout because it's subscribed
            UpdateScreenFunc?.Invoke();
        }

        public async void ToggleSound(IJSRuntime JSRuntime, ApiService apiService)
        {
            Player CurrentPlayer = CurrentUser as Player;
            if (CurrentPlayer != null)
            {
                CurrentPlayer.IsSoundOn = !CurrentPlayer.IsSoundOn;
                try
                {
                    JSRuntime.InvokeVoidAsync("UpdateUserSoundPrefrence", CurrentPlayer.IsSoundOn);
                    await apiService.UpdatePlayer(
                        PlayerDTO.FromEntity(CurrentPlayer, dto =>
                        {
                            dto.IsSoundOn = CurrentPlayer.IsSoundOn;
                        })
                    );
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Updateing Sound Currently Failed: " + ex.Message);
                    JSRuntime.InvokeVoidAsync("ShowAlert", "Updateing Sound Currently Failed: " + ex.Message);
                }
                // This triggers StateHasChanged in the Layout because it's subscribed
                UpdateScreenFunc?.Invoke();
            }
        }

        public RunInfo CurrentRun { get; set; } = new RunInfo();

        public async Task Logout(ApiService api, IJSRuntime JS, NavigationManager NavManager)
        {
            string message = "Are you sure you want to Logout?";

            if (!IsContuiningRun && IsPlayer && CurrentRun.RunStopDate != new DateTime(1753, 1, 1, 12, 0, 0))
            {
                message += $"\nthis will save the current Run, you're In Lvl {CurrentRun.CurrentLevel}?";
            }
            // Confirmation logic (Simple browser confirm for now)
            bool confirmed = await JS.InvokeAsync<bool>("confirm", message);
            if (confirmed)
            {
                if (IsPlayer)
                {
                    await SendRunToServer(api, JS);
                    CurrentRun = new RunInfo();
                }
                CurrentUser = null;
                IsAdmin = false;
                IsPlayer = false;
                NavManager.NavigateTo("Log In", forceLoad: true);
                UpdateScreenFunc?.Invoke();
            }
        }

        //HUD
        public event Action? UpdateScreenFunc;
        public void UpdateGameStats(int hp, int score, int level, int shield, int blasters, int killed, int maxEnemies, bool isEndless)
        {
            if (CurrentRun != null)
            {
                CurrentRun.CurrentHp = hp;
                CurrentRun.CurrentScore = score;
                CurrentRun.CurrentLevel = level;
                CurrentRun.CurrentShieldLevel = shield;
                CurrentRun.CurrentBlasterCount = blasters;
                CurrentRun.CurrentLevel = level;
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
                double progressPercent = (1 - (((double)killed / maxEnemies))) * 100;
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


        public async Task SendRunToServer(ApiService api, IJSRuntime JS)
        {
            // We only sync if there is a run and a logged-in user
            if (CurrentRun.RunStopDate != new DateTime(1753, 1, 1, 12, 0, 0) && CurrentUser != null)//the run was saved once at least
            {
                try
                {
                    // This is where we actually hit the DB
                    var result = await api.InsertRunInfo(CurrentRun);
                    await JS.InvokeVoidAsync("ShowAlert", "Saved Run to Command Center!");
                    Console.WriteLine("Synced Run to Command Center. Run ID: " + CurrentRun.Idx);
                }
                catch (Exception ex)
                {
                    // 2. Try to tell the user, but don't crash if they already left
                    try
                    {
                        await JS.InvokeVoidAsync("ShowAlert", "Sync Failed: " + ex.Message);
                    }
                    catch (JSDisconnectedException)
                    {
                        // Ignore: The user closed the tab, they don't need the alert anyway
                    }
                    // Log error but don't crash the game
                    Console.WriteLine("Sync Failed: " + ex.Message);
                }
            }
            else
            {
                await JS.InvokeVoidAsync("ShowAlert", "Sync Failed: no current run or player isn't logged in.");
                Console.WriteLine("Sync Failed: no current run or player isn't logged in.");
            }
        }

        public void UpdateRunLocal(bool gameover)
        {
            if (CurrentRun != null)
            {
                CurrentRun.RunStopDate = DateTime.Now;
                CurrentRun.IsRunOver = gameover; // 'gameover' comes from JS (t = died, f = in level or end of level)
                CurrentRun.Player = CurrentUser as Player;

                // This is strictly local. No API calls here.
                Console.WriteLine($"Local stats updated. Mission status: {(gameover ? "FAILED" : "SUCCESS")}");
            }
        }
    }
}