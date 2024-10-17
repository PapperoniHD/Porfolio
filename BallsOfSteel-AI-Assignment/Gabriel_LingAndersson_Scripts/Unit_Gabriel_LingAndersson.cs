using AI;
using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gabriel_LingAndersson
{
    public class Unit_Gabriel_LingAndersson : Unit
    {
        #region Properties

        public new Team_Gabriel_LingAndersson Team => base.Team as Team_Gabriel_LingAndersson;



		#endregion

		protected override Unit SelectTarget(List<Unit> enemiesInRange)
		{
			Unit lowestHealthUnit = null;
			float lowestHealth = float.MaxValue;
			foreach (var enemy in enemiesInRange)
			{
				if (enemy.Health < lowestHealth)
				{
					lowestHealth = enemy.Health;
					lowestHealthUnit = enemy;
				}
			}
			return lowestHealthUnit;
		}

		protected override GraphUtils.Path GetPathToTarget()
		{
			return Team.GetShortestPath(CurrentNode, TargetNode);
		}

	}

}