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
        private DateTime runStopDate = new DateTime(1753, 1, 1, 12, 0, 0);
        private int currentHp = 5;
        private bool isRunOver = false;

        public Player Player { get => player; set => player = value; }
        public int CurrentScore { get => currentScore; set => currentScore = value; }
        public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
        public int CurrentShieldLevel {
            get
            {
                return currentShieldLevel;
            }
            set
            {
                if(value >= 0)
                {
                    currentShieldLevel = value;
                }
                else
                {
                    currentShieldLevel = 0;
                }
            }
        }
        public int CurrentBlasterCount {
            get
            {
                return currentBlasterCount;
            }
            set
            {
                if (value >= 1 && value <= 9)
                {
                    currentBlasterCount = value;
                }
                else
                {
                    currentBlasterCount = 1;
                }
            }
        }
        public DateTime RunStopDate { get => runStopDate; set => runStopDate = value; }
        public int CurrentHp
        {
            get
            {
                return currentHp;
            }
            set
            {
                if (value >= 0)
                {
                    currentHp = value;
                }
                else
                {
                    //if hp removing goes negative set to 0, but defult is 5
                    currentHp = 0;
                }
            }
        }
        public bool IsRunOver { get => isRunOver; set => isRunOver = value; }


        public override string ToString()
        {
            return $"{base.ToString()}, " +
                $" {this.Player}, Current Score: {this.CurrentScore}, " +
                $"Current Level: {this.CurrentLevel}, " +
                $"Current Shield Level: {this.CurrentShieldLevel}, " +
                $"Current Blaster Count: {this.CurrentBlasterCount}, " +
                $"Current health: {this.currentHp}, and is {this.IsRunOver}ly over. " +
                $"The Run Stopped at: {this.RunStopDate}";
        }
    }
}
