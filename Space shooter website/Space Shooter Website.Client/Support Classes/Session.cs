using Model.Entitys;

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
            CurrentRun.CurrentHp = hp;
            CurrentRun.CurrentScore = score;
            CurrentRun.CurrentLevel = level;
            CurrentRun.CurrentShieldLevel = shield;
            CurrentRun.CurrentBlasterCount = blasters;

            // --- PROGRESS CALCULATION ---
            if (isEndless)
            {
                Progress = "100%";
            }
            else if (maxEnemies > 0)
            {
                // Percentage of enemies killed vs total level enemies
                double progressPercent = ((double)killed / maxEnemies) * 100;
                // Add 'public int Progress {get; set;}' to RunInfo. and the rest
                // for now: field here - a way to move the data between pages
                Progress = progressPercent + "%";
            }

            // Notify the Layout to re-render the HUD
            OnGameStateChanged?.Invoke();
        }
    }
}