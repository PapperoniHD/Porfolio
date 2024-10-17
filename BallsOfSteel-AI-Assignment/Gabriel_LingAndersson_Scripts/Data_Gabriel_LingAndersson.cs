using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Gabriel_LingAndersson
{
	public class Data_Gabriel_LingAndersson
	{
		public static Dictionary<Battlefield.Node, NodeData> Dictionary = new();

		public void AddEntry(Battlefield.Node node, NodeData data)
		{
			if (Dictionary.TryGetValue(node, out NodeData value))
			{
				value.EnemyFirePower = data.EnemyFirePower ?? value.EnemyFirePower;
				value.Influence = data.Influence ?? value.Influence;
				return;
			}
			Dictionary.Add(node, data);
		}

		public NodeData GetEntry(Battlefield.Node node)
		{
			NodeData val;
			if (Dictionary.TryGetValue(node, out val))
			{
				return val;
			}

			return null;
		}

		public Data_Gabriel_LingAndersson Influence(List<Game.Unit> friendlyTeam, List<Game.Unit> enemyTeam, IEnumerable<Graphs.INode> nodes)
		{
			if (friendlyTeam.Any() && enemyTeam.Any())
			{
				List<Game.Unit>[] teams = new[] { friendlyTeam, enemyTeam };
				foreach (var node in nodes)
				{
					if (node is Battlefield.Node currNode)
					{
						float currentScore = 0.0f;
						for (int i = 0; i < teams.Length; i++)
						{
							foreach (var unit in teams[i])
							{
								if (unit == null || currNode == null)
								{
									continue;
								}

								float dist = Vector3.Distance(unit.transform.position, currNode.WorldPosition);
								if (dist < Unit.FIRE_RANGE)
								{
									currentScore += (1.0f - (dist / Unit.FIRE_RANGE)) * (i == 0 ? 1.0f : -1.0f);
								}
							}
						}

						AddEntry(currNode,
							new NodeData(null, currentScore / (friendlyTeam.Count() + enemyTeam.Count())));
					}
				}
			}

			return this;
		}

		public Data_Gabriel_LingAndersson FirePower(List<Game.Unit> enemyTeam, IEnumerable<Graphs.INode> nodes)
		{
			if (enemyTeam.Any())
			{
				foreach (var node in nodes)
				{
					if (node is Battlefield.Node currNode)
					{
						float currentScore = 0;
						foreach (var unit in enemyTeam)
						{
							float dist = Vector3.Distance(unit.transform.position, currNode.WorldPosition);
							if (dist < Unit.FIRE_RANGE)
							{
								currentScore -= 1.0f;
							}
						}

						AddEntry(currNode, new NodeData(currentScore / enemyTeam.Count, null));
					}
				}
			}

			return this;
		}

		public bool GetNodeValue(Battlefield.Node node, out NodeData outVar)
		{
			if (Dictionary.ContainsKey(node))
			{
				if (Dictionary[node].EnemyFirePower is { } currFirePower &&
					Dictionary[node].Influence is { } currInfluence)
				{
					outVar = Dictionary[node];
					return true;
				}
			}

			outVar = null;
			return false;
		}
	}
	public class NodeData
	{
		public float? EnemyFirePower;
		public float? Influence;

		public NodeData(float? enemyFirePower = null, float? influence = null)
		{
			EnemyFirePower = enemyFirePower;
			Influence = influence;
		}
	}
}
