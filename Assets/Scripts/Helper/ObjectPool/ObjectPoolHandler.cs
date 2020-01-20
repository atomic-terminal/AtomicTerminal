using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MyTools {

	[System.Serializable]
	public class PoolInfo {
		public string poolName;
		public GameObject prefab;
        public Transform poolParent;
		public int poolSize;
		public bool fixedSize;
	}

	class Pool {
		private Stack<PoolObject> availableObjStack = new Stack<PoolObject>();

		private bool fixedSize;
		private GameObject poolObjectPrefab;
        private Transform poolObjectParent;
		private int poolSize;
		private string poolName;

		public Pool(string poolName, GameObject poolObjectPrefab,Transform poolObjectParent, int initialCount, bool fixedSize) {
			this.poolName = poolName;
			this.poolObjectPrefab = poolObjectPrefab;
            this.poolObjectParent = poolObjectParent;
            this.poolSize = initialCount;
			this.fixedSize = fixedSize;
			//初始化池
			for(int index = 0; index < initialCount; index++) {
				AddObjectToPool(NewObjectInstance());
			}
		}
        /// <summary>
        /// 入池
        /// </summary>
        /// <param name="po"></param>
		private void AddObjectToPool(PoolObject po) {
			po.gameObject.SetActive(false);
			availableObjStack.Push(po);
			po.isPooled = true;
		}
		/// <summary>
        /// 实例化
        /// </summary>
        /// <returns></returns>
		private PoolObject NewObjectInstance() {
			GameObject go = (GameObject)GameObject.Instantiate(poolObjectPrefab);
			PoolObject po = go.GetComponent<PoolObject>();
			if(po == null) {
				po = go.AddComponent<PoolObject>();
			}
			//set name
			po.poolName = poolName;
            po.transform.SetParent(poolObjectParent);
			return po;
		}

		/// <summary>
        /// 从池中取得
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
		public GameObject NextAvailableObject(Vector3 position, Quaternion rotation) {
			PoolObject po = null;
			if(availableObjStack.Count > 0) {
				po = availableObjStack.Pop();
			} else if(fixedSize == false) {
				poolSize++;
				po = NewObjectInstance();
			} else {
				Debug.LogWarning("No object available & cannot grow pool: " + poolName);
			}
			
			GameObject result = null;
			if(po != null) {
				po.isPooled = false;
				result = po.gameObject;
				result.SetActive(true);
				
				result.transform.position = position;
				result.transform.rotation = rotation;
			}
			
			return result;
		} 
		
		/// <summary>
        /// 根据名字，入对应池
        /// </summary>
        /// <param name="po"></param>
		public void ReturnObjectToPool(PoolObject po) {
			
			if(poolName.Equals(po.poolName)) {
				if(po.isPooled) {
					Debug.LogWarning(po.gameObject.name + " is already in pool. Why are you trying to return it again? Check usage.");	
				} else {
					AddObjectToPool(po);
				}
				
			} else {
				Debug.LogError(string.Format("Trying to add object to incorrect pool {0} {1}",po.poolName,poolName));
			}
		}
	}

	public class ObjectPoolHandler : MonoBehaviour {

		public static ObjectPoolHandler instance;
		public PoolInfo[] poolInfo;

		//池 字典
		private Dictionary<string, Pool> poolDictionary  = new Dictionary<string, Pool>();
		
		void Start () {
			instance = this;
			//检查空池或者重复池
			CheckForDuplicatePoolNames();
			CreatePools();
		}
		
		private void CheckForDuplicatePoolNames() {
			for (int index = 0; index < poolInfo.Length; index++) {
				string poolName = poolInfo[index].poolName;
				if(poolName.Length == 0) {
					Debug.LogError(string.Format("Pool {0} does not have a name!",index));
				}
				for (int internalIndex = index + 1; internalIndex < poolInfo.Length; internalIndex++) {
					if(poolName.Equals(poolInfo[internalIndex].poolName)) {
						Debug.LogError(string.Format("Pool {0} & {1} have the same name. Assign different names.", index, internalIndex));
					}
				}
			}
		}

		private void CreatePools() {
			foreach (PoolInfo currentPoolInfo in poolInfo) {
				
				Pool pool = new Pool(currentPoolInfo.poolName, currentPoolInfo.prefab, currentPoolInfo.poolParent,
				                     currentPoolInfo.poolSize, currentPoolInfo.fixedSize);

				
				Debug.Log("Creating pool: " + currentPoolInfo.poolName);
				//加入字典
				poolDictionary[currentPoolInfo.poolName] = pool;
			}
		}

		public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation) {
			GameObject result = null;
			
			if(poolDictionary.ContainsKey(poolName)) {
				Pool pool = poolDictionary[poolName];
				result = pool.NextAvailableObject(position,rotation);
				//池被锁住了
				if(result == null) {
					Debug.LogWarning("No object available in pool. Consider setting fixedSize to false.: " + poolName);
				}
				
			} else {
				Debug.LogError("Invalid pool name specified: " + poolName);
			}
			return result;
		}

		public void ReturnObjectToPool(GameObject go) {
			PoolObject po = go.GetComponent<PoolObject>();
			if(po == null) {
				Debug.LogWarning("Specified object is not a pooled instance: " + go.name);
			} else {
				if(poolDictionary.ContainsKey(po.poolName)) {
					Pool pool = poolDictionary[po.poolName];
					pool.ReturnObjectToPool(po);
				} else {
					Debug.LogWarning("No pool available with name: " + po.poolName);
				}
			}
		}
	}
}
