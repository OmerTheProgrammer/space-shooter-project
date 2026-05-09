using Client_Manager___API;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Model.Data_Transfer_Objects;
using Model.Entitys;
using System.Threading.Tasks;

namespace Space_Shooter_Website.Client.Support_Classes
{
    public class Session
    {
        public User? CurrentUser { get; set; }
        public RunInfo CurrentRun { get; set; } = new RunInfo();
        public int SelectedLevel { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPlayer { get; set; }
        public bool IsLoggedIn => CurrentUser != null;
        public bool IsContuiningRun = false;

        public string Progress = "100%";
        public bool IsEndless { get; set; } = false;

        public bool IsSettingsVisible { get; set; } = false;

        public async void ToggleSettings(IJSRuntime JSRuntime)
        {
            IsSettingsVisible = !IsSettingsVisible;
            JSRuntime.InvokeVoidAsync("UpdatePaused", IsSettingsVisible);
            // This triggers StateHasChanged in the Layout because it's subscribed
            try
            {
                UpdateScreenFunc?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        public async void SaveSettings(IJSRuntime JSRuntime, ApiService apiService)
        {
            Player CurrentPlayer = CurrentUser as Player;
            try
            {
                await JSRuntime.InvokeVoidAsync("UpdateUserSoundPrefrence", CurrentPlayer.IsSoundOn);
                await JSRuntime.InvokeVoidAsync("UpdateUserMusicPrefrence", CurrentPlayer.IsMusicOn);
                await apiService.UpdatePlayer(
                    PlayerDTO.FromEntity(CurrentPlayer, dto =>
                    {
                        dto.IsMusicOn = CurrentPlayer.IsMusicOn;
                        dto.IsSoundOn = CurrentPlayer.IsSoundOn;
                    })
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Updateing Sound Currently Failed: " + ex.Message);
                JSRuntime.InvokeVoidAsync("ShowAlert", "Updateing Sound Currently Failed: " + ex.Message);
            }
            UpdateScreenFunc?.Invoke();
        }

        public async Task Logout(ApiService api, IJSRuntime JS, NavigationManager NavManager)
        {
            string message = "Are you sure you want to Logout?";
            // Confirmation logic (Simple browser confirm for now)
            bool confirmed = await JS.InvokeAsync<bool>("confirm", message);
            if (confirmed)
            {

                if (IsPlayer && CurrentRun.RunStopDate != new DateTime(1753, 1, 1, 12, 0, 0))
                {
                    TimeSpan runDuration = DateTime.Now - CurrentRun.RunStopDate;
                    int MinLen = (int)runDuration.TotalMinutes;
                    bool wantToSaveShortRun = await JS.InvokeAsync<bool>("confirm",
                        $"This run was saved in website last, {MinLen} minutes long, you're In Lvl {CurrentRun.CurrentLevel}.");
                    if (wantToSaveShortRun)
                    {
                        await SendRunToServer(api, JS);
                    }
                }
                //no matter if saved and left, replace the run
                CurrentRun = new RunInfo();
                IsContuiningRun = false;

                CurrentUser = null;
                IsAdmin = false;
                IsPlayer = false;
                NavManager.NavigateTo("Log In", forceLoad: true);
                UpdateScreenFunc?.Invoke();
                UpdateScreenFunc = null;
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
            if (CurrentRun.RunStopDate != new DateTime(1753, 1, 1, 12, 0, 0) && CurrentUser is Player currentP)//the run was saved once at least
            {
                try
                {
                    // This is where we actually hit the DB
                    var runResult = await api.InsertRunInfo(CurrentRun);

                    if (runResult.error == null)
                    {
                        await JS.InvokeVoidAsync("ShowAlert", "Saved Run to Command Center!");

                        // Update Player Lifetime score
                        currentP.TotalScore += CurrentRun.CurrentScore;

                        // Only update MaxLevel if the current run reached a higher sector
                        currentP.MaxLevel = Math.Max(currentP.MaxLevel, CurrentRun.CurrentLevel);

                        // 3. Push the updated Player profile to the DB
                        var playerResult = await api.UpdatePlayer(
                            PlayerDTO.FromEntity(currentP, dto => {
                                dto.TotalScore = currentP.TotalScore;
                                dto.MaxLevel = currentP.MaxLevel;
                            })
                        );

                        if (playerResult.error == null)
                        {
                            Console.WriteLine("ShowAlert", "Mission Debrief Complete: Pilot Stats Updated!");
                        }
                    }
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
                Console.WriteLine("No current run or player isn't logged in.");
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
                //saved once so the run exists.
                IsContuiningRun = true;
            }
        }

        public async Task HandlePotentialOverwrite(ApiService api, IJSRuntime JS)
        {
            if (IsContuiningRun && (CurrentRun != null && CurrentRun != new RunInfo()))
            {
                // Calculate time since the last recorded stop
                TimeSpan runDuration = DateTime.Now - CurrentRun.RunStopDate;

                if (runDuration.TotalSeconds < 135)
                {
                    bool wantToSave = await JS.InvokeAsync<bool>("confirm",
                        "This run was updated locally very recently. Save current progress to database before continuing?");

                    if (wantToSave)
                    {
                        await TrySaveRun(api,JS);
                    }
                }
                else
                {
                    // Auto-save if it's been a while
                    await TrySaveRun(api, JS);
                }
            }
        }

        private async Task TrySaveRun(ApiService api, IJSRuntime JS)
        {
            try
            {
                await SendRunToServer(api, JS);
            }
            catch (Exception e)
            {
                await JS.InvokeVoidAsync("ShowAlert", $"Failed Saving: {e.Message}");
            }
        }
    }
}