using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Player : User
    {
        private int maxLevel = 1;
        private int totalScore = 0;
        private bool isSoundOn = true;
        private bool isMusicOn = true;

        public int MaxLevel { get => maxLevel; set => maxLevel = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }
        public bool IsSoundOn { get => isSoundOn; set => isSoundOn = value; }
        public bool IsMusicOn { get => isMusicOn; set => isMusicOn = value; }

        public override string ToString()
        {
            return $"{base.ToString()}" +
                $"Max level : {this.MaxLevel},Total Score:{this.TotalScore}, " +
                $"Sound is {this.IsSoundOn}ly on, " +
                $"Music is {this.IsMusicOn}ly on";
        }
    }
}
