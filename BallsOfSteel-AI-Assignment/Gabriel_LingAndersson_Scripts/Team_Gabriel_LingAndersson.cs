using Game;
using Graphs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Gabriel_LingAndersson
{
    public class Team_Gabriel_LingAndersson : Team
    {
		#region Serialized Members

		[SerializeField] private Color m_myFancyColor;

		#endregion

		private Data_Gabriel_LingAndersson data = new();
		private Unit huntedUnit = null;

		private const float TooFarApartThreshold = 10f;
		private const int MaxDistance = 5;
		private float SquareMagnitude = 0;

		private readonly float[] fpSteps = new[] { -0.1f, -0.5f, -0.5f, -0.5f, -1.0f };

		private List<Battlefield.Node> closeNodes = new();

		private List<Battlefield.Node> sortNodes = new();
		public List<Unit_Gabriel_LingAndersson> myUnits;

		public Dictionary<Vector2Int, Battlefield.Node> nodeLookup = new Dictionary<Vector2Int, Battlefield.Node>();

		public override Color Color => m_myFancyColor;

		private Vector3 EnemyTeamPosAvg => EnemyTeam.Units.Aggregate(new Vector3(0, 0, 0),
			(compareVector, unit) => compareVector + unit.transform.position / this.EnemyTeam.Units.Count());

		public Vector3 TeamPosAvg()
		{
			Vector3 averageVector = Vector3.zero;

			int count = 0;
			foreach (var unit in myUnits)
			{
				if (unit == null) continue;
				count++;
				averageVector += unit.transform.position;
			}

			return averageVector / count;
		}

		protected override void Start()
		{
			Time.timeScale = 20f;
			base.Start();
			myUnits = GetComponentsInChildren<Unit_Gabriel_LingAndersson>().ToList();
			foreach (var node in Battlefield.Instance.Nodes)
			{
				if (node is Battlefield.Node currNode)
				{
					nodeLookup.Add(currNode.Position, currNode);
				}
			}
			StartCoroutine(UpdateData());
		}
		IEnumerator UpdateData()
		{
			while (true)
			{
				SquareMagnitude = (EnemyTeamPosAvg - TeamPosAvg()).sqrMagnitude;
				yield return null;
				data.Influence(myUnits.Cast<Unit>().ToList(), EnemyTeam.Units.ToList(), Battlefield.Instance.Nodes).FirePower(EnemyTeam.Units.ToList(), Battlefield.Instance.Nodes);
				yield return null;
			}
		}

		private Battlefield.Node FindOptimalNode()
		{
			sortNodes = new List<Battlefield.Node>();
			for (int i = 0; i < fpSteps.Length; i++)
			{
				foreach (var value in nodeLookup.Values)
				{
					if (data.GetNodeValue(value, out NodeData wrapper))
					{
						if (fpSteps != null && Equals(wrapper.EnemyFirePower, fpSteps[i]))
						{
							if (!sortNodes.Contains(value) && (value is not Game.Node_Mud))
							{
								sortNodes.Add(value);
							}
						}
					}
				}
			}

			List<Battlefield.Node> sortedByDistanceNodes = sortNodes.OrderBy(x => Vector3.Distance(x.WorldPosition, TeamPosAvg())).ToList();
			closeNodes = new List<Battlefield.Node>();

			for (int i = 0; i < sortedByDistanceNodes.Count; i++)
			{
				closeNodes.Add(sortedByDistanceNodes[i]);
				if (closeNodes.Count >= 10) break;
			}

			Battlefield.Node bestNode = null;
			float highestValue = float.MinValue;
			foreach (var closestNode in closeNodes)
			{
				if (data.GetEntry(closestNode).Influence is float currValue)
				{
					if (bestNode == null || currValue > highestValue)
					{
						bestNode = closestNode;
						highestValue = currValue;
					}
				}
			}
			return bestNode;
		}

		private Battlefield.Node FindNodeInDistance(Battlefield.Node node, int distance = MaxDistance)
		{
			HashSet<Battlefield.Node> closeNode = GraphUtils.GetNodeInDistance<Battlefield.Node>(Battlefield.Instance, node, distance == MaxDistance ? MaxDistance : distance);

			sortNodes = new List<Battlefield.Node>();
			for (int i = 0; i < fpSteps.Length; i++)
			{
				foreach (var value in closeNode)
				{
					if (data.GetNodeValue(value, out NodeData wrapper))
					{
						if (fpSteps != null && Equals(wrapper.EnemyFirePower, fpSteps[i]))
						{
							if (!sortNodes.Contains(value))
							{
								sortNodes.Add(value);
							}
						}
					}
				}
			}

			List<Battlefield.Node> sortedByDistanceNodes = sortNodes.OrderBy(x => Vector3.Distance(x.WorldPosition, TeamPosAvg())).ToList();
			closeNodes = new List<Battlefield.Node>();
			for (int i = 0; i < sortedByDistanceNodes.Count; i++)
			{
				closeNodes.Add(sortedByDistanceNodes[i]);
				if (closeNodes.Count >= 10) break;
			}

			Battlefield.Node bestNode = null;
			float highestValue = float.MinValue;
			foreach (var closestNode in closeNodes)
			{
				if (data.GetEntry(closestNode).Influence is float currValue)
				{
					if (bestNode == null || currValue > highestValue)
					{
						bestNode = closestNode;
						highestValue = currValue;
					}
				}
			}

			return bestNode;
		}

		private void Update()
		{
			foreach (var unit in myUnits)
			{
				if (unit == null) continue;

				if (unit.EnemiesInRange.Any() && EnemyTeam.Units.Count() <= myUnits.Count - 3)
				{
					var sortedList = unit.EnemiesInRange
						.OrderBy(x => Vector3.Distance(x.transform.position, unit.transform.position)).ToList();

					if (huntedUnit is null)
					{
						huntedUnit = sortedList[0];
					}

					Battlefield.Node moveNode = FindNodeInDistance(GraphUtils.GetClosestNode<Battlefield.Node>(Battlefield.Instance, huntedUnit != null ? huntedUnit.transform.position : sortedList[0].transform.position));

					if (unit.TargetNode != moveNode)
					{
						unit.TargetNode = moveNode;
					}
				}
				else if (unit.EnemiesInRange.Any())
				{
					unit.TargetNode = null;
				}
				else
				{
					Battlefield.Node moveNode = FindNodeInDistance(FindOptimalNode());
					if (unit.TargetNode != moveNode)
					{
						unit.TargetNode = moveNode;
					}
				}
			}
		}
	}
}

