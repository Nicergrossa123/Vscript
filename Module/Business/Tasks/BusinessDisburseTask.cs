﻿using Nexus.Module.Players;
using Nexus.Module.Players.Db;
using Nexus.Module.Tasks;

namespace Nexus.Module.Business.Tasks
{
    public class BusinessDisburseTask : SqlTask
    {
        private readonly Business business;
        private readonly DbPlayer dbPlayer;
        private readonly int amount;

        public BusinessDisburseTask(Business business, DbPlayer dbPlayer, int amount)
        {
            this.business = business;
            this.dbPlayer = dbPlayer;
            this.amount = amount;
        }

        public override string GetQuery()
        {
            return $"UPDATE `business` SET money = money - '{amount}' WHERE id = '{business.Id}' AND money >= '{amount}';";
        }

        public override void OnFinished(int result)
        {
            if (result == 1)
            {
                business.Money -= amount;
                var businessResult =
                    ExecuteNonQuery($"UPDATE `player` SET money = money + '{amount}' WHERE id = '{dbPlayer.Id}';");
                if (businessResult == 1)
                {
                    dbPlayer.GiveMoney(amount);
                }
            }
        }
    }
}