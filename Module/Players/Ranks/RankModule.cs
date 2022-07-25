using Nexus.Module.Players.Db;

namespace Nexus.Module.Players.Ranks
 {
     public class RankModule : SqlModule<RankModule, Rank, uint>
     {
         protected override string GetQuery()
         {
             return "SELECT * FROM `player_ranks`;";
         }
 
         public override void OnPlayerConnected(DbPlayer dbPlayer)
         {
             dbPlayer.Rank = this[dbPlayer.RankId] ?? this[0];             
         }
     }
 }