using System;

namespace Model.Entitys
{
    public class RunInfo : BaseEntity
    {
        private Player player;
        private int currentScore = 0;
        private int currentLevel = 1;
        private int currentShieldLevel = 0;
        private int currentBlasterCount = 1;
        private DateTime runStopDate;
        private int currentHp = 5;
        private bool isRunOver;

        public Player Player { get => player; set => player = value; }
        public int CurrentScore { get => currentScore; set => currentScore = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public int CurrentShieldLevel { get => currentShieldLevel; set => currentShieldLevel = value; }
        public int CurrentBlasterCount { get => currentBlasterCount; set => currentBlasterCount = value; }
        public DateTime RunStopDate { get => runStopDate; set => runStopDate = value; }
        public int CurrentHp { get => currentHp; set => currentHp = value; }
        public bool IsRunOver { get => isRunOver; set => isRunOver = value; }

        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $" {this.Player}, Current Score: {this.CurrentScore}, " +
                $"Current Level: {this.CurrentLevel}, " +
                $"Current Shield Level: {this.CurrentShieldLevel}, " +
                $"Current Blaster Count: {this.CurrentBlasterCount}, " +
                $"Current health: {this.currentHp}, and is {this.IsRunOver}ly over. " +
                $"The Run Stopped at: {this.RunStopDate}.\n";
        }
    }
}
