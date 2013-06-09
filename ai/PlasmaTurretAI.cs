using UnityEngine;

public class PlasmaTurretAI : MonoBehaviour
{
	public GameObject curTarget;
	public float towerPrice = 100.0f;
	public float attackMaximumDistance = 50.0f; //дистанци€ атаки
	public float attackMinimumDistance = 5.0f;
	public float attackDamage = 10.0f; //урон
	public float reloadTimer = 2.5f; //задержка между выстрелами, измен€емое значение
	public float reloadCooldown = 2.5f; //задержка между выстрелами, константа
	public float rotationSpeed = 1.5f; //множитель скорости вращени€ башни
	public int FiringOrder = 1; //очередность стрельбы дл€ стволов (у нас же их 2)
	public int upgradeLevel = 0;
	public int ammoAmount = 64;
	public int ammoAmountConst = 64;
	public float ammoReloadTimer = 5.0f;
	public float ammoReloadConst = 5.0f;
	public LayerMask turretLayerMask;

	public Transform turretHead;

	//используем этот метод дл€ инициализации
	private void Start()
	{
		turretHead = transform.Find("pushka"); //находим башню в иерархии частей модели
	}

	//а этот метод вызываетс€ каждый фрейм
	private void Update()
	{
		if (curTarget != null) //если переменна€ текущей цели не пуста€
		{
			float squaredDistance = (turretHead.position - curTarget.transform.position).sqrMagnitude; //мер€ем дистанцию до нее
			if (Mathf.Pow(attackMinimumDistance, 2) < squaredDistance && squaredDistance < Mathf.Pow(attackMaximumDistance, 2)) //если дистанци€ больше мертвой зоны и меньше дистанции поражени€ пушки
			{
				turretHead.rotation = Quaternion.Lerp(turretHead.rotation, Quaternion.LookRotation(curTarget.transform.position - turretHead.position), rotationSpeed * Time.deltaTime); //вращаем башню в сторону цели
				if (reloadTimer > 0) reloadTimer -= Time.deltaTime; //если таймер перезар€дки больше нул€ - отнимаем его
				if (reloadTimer <= 0)
				{
					if (ammoAmount > 0) //пока есть порох в пороховницах
					{
						MobHP mhp = curTarget.GetComponent<MobHP>();
						switch (FiringOrder) //смотрим, из какого ствола стрел€ть
						{
							case 1:
								if (mhp != null) mhp.ChangeHP(-attackDamage); //наносим урон цели
								FiringOrder++; //переключаем ствол
								ammoAmount--; //минус патрон
								break;
							case 2:
								if (mhp != null) mhp.ChangeHP(-attackDamage);
								FiringOrder = 1;
								ammoAmount--;
								break;
						}
						reloadTimer = reloadCooldown; //возвращаем переменной таймера перезар€дки еЄ первоначальное значение из "константы"
					}
					else
					{
						if (ammoReloadTimer > 0) ammoReloadTimer -= Time.deltaTime;
						if (ammoReloadTimer <= 0)
						{
							ammoAmount = ammoAmountConst;
							ammoReloadTimer = ammoReloadConst;
						}
					}
				}
				if (squaredDistance < Mathf.Pow(attackMinimumDistance, 2)) curTarget = null;//сбрасываем с прицела текущую цель, если она вне радиуса атаки
			}
		}
		else
		{
			curTarget = SortTargets(); //сортируем цели и получаем новую
		}
	}

	//ћодифицированный алгоритм поиска ближайшей цели
	private GameObject SortTargets()
	{
		float closestMobSquaredDistance = 0; //переменна€ дл€ хранени€ квадрата рассто€ни€ ближайшего моба
		GameObject nearestmob = null; //инициализаци€ переменной ближайшего моба
		Collider[] mobColliders = Physics.OverlapSphere(transform.position, attackMaximumDistance, turretLayerMask.value); //находим коллайдеры всех мобов в радиусе максимальной дальности атаки и создаЄм массив дл€ сортировки

		foreach (var mobCollider in mobColliders) //дл€ каждого коллайдера в массиве
		{
			float distance = (mobCollider.transform.position - turretHead.position).sqrMagnitude;
			//если дистанци€ до моба меньше, чем closestMobDistance или равна нулю
			if (distance < closestMobSquaredDistance && (distance > Mathf.Pow(attackMinimumDistance, 2)) || closestMobSquaredDistance == 0)
			{
				closestMobSquaredDistance = distance; //записываем еЄ в переменную
				nearestmob = mobCollider.gameObject;//устанавливаем моба как ближайшего
			}
		}
		return nearestmob; // и возвращаем его
	}

	private void OnGUI()
	{
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position); //Ќаходим позицию объекта на экране относительно мира
		Vector3 cameraRelative = Camera.main.transform.InverseTransformPoint(transform.position); //ѕолучаем дальность объекта от камеры
		if (cameraRelative.z > 0) //если объект находитс€ впереди камеры
		{
			string ammoString;
			if (ammoAmount > 0)
			{
				ammoString = ammoAmount + "/" + ammoAmountConst;
			}
			else
			{
				ammoString = "Reloading: " + (int)ammoReloadTimer + " s left";
			}
			GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 250f, 20f), ammoString);
		}
	}
}