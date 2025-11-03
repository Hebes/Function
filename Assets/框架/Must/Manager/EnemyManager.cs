// using System;
// using System.Collections.Generic;
// using UnityEngine;
//
// namespace GameWorld
// {
// 	public class EnemyManager
// 	{
// 		public EnemyType GetEnemyType(string name)
// 		{
// 			return EnemyGenerator.GetEnemyType(name);
// 		}
//
// 		public GameObject Generate(string name, Vector2? pos = null, bool withEffect = true, bool enemyPoint = true)
// 		{
// 			return this.Generator.GenerateEnemy(name, pos, withEffect, enemyPoint);
// 		}
//
// 		public GameObject Generate(EnemyType type, Vector2? pos = null, bool withEffect = true, bool enemyPoint = true)
// 		{
// 			return this.Generator.GenerateEnemy(type, pos, withEffect, enemyPoint);
// 		}
//
// 		private EnemyGenerator Generator
// 		{
// 			get
// 			{
// 				return Singleton<EnemyGenerator>.Instance;
// 			}
// 		}
//
// 		public List<EnemyAttribute> EnemyAttributes
// 		{
// 			get
// 			{
// 				return this._enemyAttributes;
// 			}
// 		}
//
// 		public int Count
// 		{
// 			get
// 			{
// 				return this._enemyAttributes.Count;
// 			}
// 		}
//
// 		public GameObject First
// 		{
// 			get
// 			{
// 				return (this._enemyAttributes.Count <= 0 || !(this._enemyAttributes[0] != null)) ? null : this._enemyAttributes[0].gameObject;
// 			}
// 		}
//
// 		private List<GameObject> GetXNearestRangeEnemy(float middle, float range)
// 		{
// 			List<GameObject> list = new List<GameObject>();
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (MathfX.isInMiddleRange(this._enemyAttributes[i].transform.position.x, middle, range))
// 				{
// 					list.Add(this._enemyAttributes[i].gameObject);
// 				}
// 			}
// 			return list;
// 		}
//
// 		public bool HasNearEnemy(float xPos, float range)
// 		{
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (MathfX.isInMiddleRange(this._enemyAttributes[i].transform.position.x, xPos, range))
// 				{
// 					return true;
// 				}
// 			}
// 			return false;
// 		}
//
// 		public bool HasEnemyInRect(Rect rect)
// 		{
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (rect.Contains(this._enemyAttributes[i].transform.position))
// 				{
// 					return true;
// 				}
// 			}
// 			return false;
// 		}
//
// 		public GameObject GetNearestRangeEnemys(Vector2 pos, float range)
// 		{
// 			GameObject result = null;
// 			List<GameObject> xnearestRangeEnemy = this.GetXNearestRangeEnemy(pos.x, range);
// 			float num = range;
// 			for (int i = 0; i < xnearestRangeEnemy.Count; i++)
// 			{
// 				float num2 = Vector2.Distance(pos, xnearestRangeEnemy[i].transform.position);
// 				if (num2 < num && num2 < range)
// 				{
// 					result = xnearestRangeEnemy[i];
// 					num = num2;
// 				}
// 			}
// 			return result;
// 		}
//
// 		public GameObject GetNearestRangeEnemyWithDir(Vector2 pos, float rectX, float rectY, int dir, bool isNeedAlive, bool isOnGround)
// 		{
// 			GameObject result = null;
// 			List<GameObject> xnearestRangeEnemy = this.GetXNearestRangeEnemy(pos.x, rectX + rectY);
// 			float num = rectX + rectY;
// 			for (int i = 0; i < xnearestRangeEnemy.Count; i++)
// 			{
// 				GameObject gameObject = xnearestRangeEnemy[i];
// 				bool flag = InputSetting.JudgeDir(pos, gameObject.transform.position) == dir;
// 				bool flag2 = MathfX.isInMiddleRange(gameObject.transform.position.x, pos.x, rectX) && MathfX.isInMiddleRange(gameObject.transform.position.y, pos.y, rectY);
// 				EnemyAttribute component = gameObject.GetComponent<EnemyAttribute>();
// 				bool flag3 = component != null && !component.isDead;
// 				bool flag4 = component != null && component.isOnGround;
// 				if (flag2 && flag && flag3 == isNeedAlive && flag4 == isOnGround)
// 				{
// 					float num2 = Vector2.Distance(pos, gameObject.transform.position);
// 					if (num2 < num)
// 					{
// 						result = gameObject;
// 						num = num2;
// 					}
// 				}
// 			}
// 			return result;
// 		}
//
// 		public bool HasXNearestRangeEnemy(float middle, float diameter)
// 		{
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (MathfX.isInMiddleRange(this._enemyAttributes[i].transform.position.x, middle, diameter))
// 				{
// 					return true;
// 				}
// 			}
// 			return false;
// 		}
//
// 		public Vector3? GetFarestEnemyPosition(Vector3 pivot, Rect? rect = null)
// 		{
// 			float num = 0f;
// 			int num2 = -1;
// 			for (int i = 0; i < this.EnemyAttributes.Count; i++)
// 			{
// 				EnemyAttribute enemyAttribute = this.EnemyAttributes[i];
// 				if (Vector3.Distance(enemyAttribute.transform.position, pivot) > num)
// 				{
// 					if (rect != null)
// 					{
// 						if (Mathf.Abs(enemyAttribute.transform.position.x - rect.Value.center.x) < rect.Value.width / 2f && Mathf.Abs(enemyAttribute.transform.position.y - rect.Value.center.y) < rect.Value.height / 2f)
// 						{
// 							num = Vector3.Distance(enemyAttribute.transform.position, pivot);
// 							num2 = i;
// 						}
// 					}
// 					else
// 					{
// 						num = Vector3.Distance(enemyAttribute.transform.position, pivot);
// 						num2 = i;
// 					}
// 				}
// 			}
// 			return (num2 != -1) ? new Vector3?(this.EnemyAttributes[num2].transform.position) : null;
// 		}
//
// 		public Vector3 GetAverageEnemyPosition(Vector3 player, float distance = 10f)
// 		{
// 			int num = 0;
// 			Vector3 vector = Vector3.zero;
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (Vector3.Distance(player, this._enemyAttributes[i].transform.position) < distance)
// 				{
// 					vector += this._enemyAttributes[i].transform.position;
// 					num++;
// 				}
// 			}
// 			if (num != 0)
// 			{
// 				vector /= (float)num;
// 			}
// 			return vector;
// 		}
//
// 		public GameObject GetEnemyById(string id)
// 		{
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (this._enemyAttributes[i].id == id)
// 				{
// 					return this._enemyAttributes[i].gameObject;
// 				}
// 			}
// 			return null;
// 		}
//
// 		public GameObject GetEnemyByType(EnemyType type)
// 		{
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (this._enemyAttributes[i].baseData.enemyType == type)
// 				{
// 					return this._enemyAttributes[i].gameObject;
// 				}
// 			}
// 			return null;
// 		}
//
// 		public List<GameObject> GetEnemysByType(EnemyType type)
// 		{
// 			List<GameObject> list = new List<GameObject>();
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (this._enemyAttributes[i].baseData.enemyType == type)
// 				{
// 					list.Add(this._enemyAttributes[i].gameObject);
// 				}
// 			}
// 			return list;
// 		}
//
// 		public int GetEnemyCountByType(EnemyType type)
// 		{
// 			int num = 0;
// 			for (int i = 0; i < this._enemyAttributes.Count; i++)
// 			{
// 				if (this._enemyAttributes[i].baseData.enemyType == type)
// 				{
// 					num++;
// 				}
// 			}
// 			return num;
// 		}
//
// 		public bool CanBeExecutedEnemyExist()
// 		{
// 			bool flag = false;
// 			for (int i = 0; i < R.Enemy.Count; i++)
// 			{
// 				flag |= R.Enemy.EnemyAttributes[i].CurrentCanBeExecute;
// 			}
// 			return flag;
// 		}
//
// 		public void AddEnemy(EnemyAttribute enemyAttribute)
// 		{
// 			if (enemyAttribute != null)
// 			{
// 				this._enemyAttributes.Add(enemyAttribute);
// 			}
// 		}
//
// 		public void RemoveEnemy(EnemyAttribute enemyAttribute)
// 		{
// 			if (enemyAttribute != null)
// 			{
// 				this._enemyAttributes.Remove(enemyAttribute);
// 			}
// 		}
//
// 		public void KillAllEnemy()
// 		{
// 			if (this._enemyAttributes != null)
// 			{
// 				for (int i = 0; i < this._enemyAttributes.Count; i++)
// 				{
// 					this._enemyAttributes[i].currentHp = 0;
// 				}
// 			}
// 		}
//
// 		public void Clear()
// 		{
// 			this.Boss = null;
// 			this._enemyAttributes.Clear();
// 		}
//
// 		private readonly List<EnemyAttribute> _enemyAttributes = new List<EnemyAttribute>();
//
// 		public GameObject Boss;
// 	}
// }
