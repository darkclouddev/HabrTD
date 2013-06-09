using UnityEngine;

public class Graphic : MonoBehaviour
{
	public Rect buyMenu; //квадрат меню покупки
	public Rect firstTower; //квадрат кнопки покупки первой башни
	public Rect secondTower; //квадрат кнопки покупки второй башни
	public Rect thirdTower; //квадрат кнопки покупки третьей башни
	public Rect fourthTower; //квадрат кнопки покупки четвёртой башни
	public Rect fifthTower; //квадрат кнопки покупки пятой башни

	public Rect towerMenu; //квадрат сервисного меню башни (продать/обновить)
	public Rect towerMenuSellTower; //квадрат кнопки продажи башни
	public Rect towerMenuUpgradeTower; //квадрат кнопки апгрейда башни

	public Rect playerStats; //квадрат статистики игрока
	public Rect playerStatsPlayerMoney; //квадрат зоны отображения денег игрока

	public GameObject plasmaTower; //префаб первой пушки, необходимо назначить в инспекторе
	public GameObject plasmaTowerGhost; //призрак первой пушки, необходимо назначить в инспекторе
	private RaycastHit hit; //переменная для рейкаста
	public LayerMask buildRaycastLayers = 1;
	public LayerMask upgradeRaycastLayer; //слой для апгрейда
	public LayerMask sellRaycastLayer; //слой для продажи

	private GameObject ghost; //переменная для призрака устанавливаемой пушки

	private void Awake()
	{
		buyMenu = new Rect(Screen.width - 185.0f, 10.0f, 175.0f, Screen.height - 100.0f); //задаём размеры квадратов, последовательно позиция X, Y, Ширина, Высота. X и Y указывают на левый верхний угол объекта
		firstTower = new Rect(buyMenu.x + 12.5f, buyMenu.y + 30.0f, 150.0f, 50.0f);
		secondTower = new Rect(firstTower.x, buyMenu.y + 90.0f, 150.0f, 50.0f);
		thirdTower = new Rect(firstTower.x, buyMenu.y + 150.0f, 150.0f, 50.0f);
		fourthTower = new Rect(firstTower.x, buyMenu.y + 210.0f, 150.0f, 50.0f);
		fifthTower = new Rect(firstTower.x, buyMenu.y + 270.0f, 150.0f, 50.0f);

		playerStats = new Rect(10.0f, 10.0f, 150.0f, 100.0f);
		playerStatsPlayerMoney = new Rect(playerStats.x + 12.5f, playerStats.y + 30.0f, 125.0f, 25.0f);

		towerMenu = new Rect(10.0f, Screen.height - 60.0f, 400.0f, 50.0f);
		towerMenuSellTower = new Rect(towerMenu.x + 12.5f, towerMenu.y + 20.0f, 75.0f, 25.0f);
		towerMenuUpgradeTower = new Rect(towerMenuSellTower.x + 5.0f + towerMenuSellTower.width, towerMenuSellTower.y, 75.0f, 25.0f);
	}

	private void Update()
	{
		switch (GlobalVars.mau5tate) //свитчим состояние курсора мыши
		{
			case GlobalVars.ClickState.Placing: //если он в режиме установки башен
				{
					if (ghost == null) ghost = Instantiate(plasmaTowerGhost) as GameObject; //если переменная призрака пустая - создаём в ней объект призрака башни
					else
					{
						Ray scrRay = Camera.main.ScreenPointToRay(Input.mousePosition); //создаём луч, бьющий от координат мыши по координатам в игре
						if (Physics.Raycast(scrRay, out hit, Mathf.Infinity, buildRaycastLayers)) // бьём этим лучем в заданном выше направлении (т.е. в землю)
						{
							Quaternion normana = Quaternion.FromToRotation(Vector3.up, hit.normal); //получаем нормаль от столкновения
							ghost.transform.position = hit.point; //задаём позицию пzризрака равной позиции точки удара луча по земле
							ghost.transform.rotation = normana; //тоже самое и с вращением, только не от точки, а от нормали
							if (Input.GetMouseButtonDown(0)) //при нажатии ЛКМ
							{
								GameObject tower = Instantiate(plasmaTower, ghost.transform.position, ghost.transform.rotation) as GameObject; //Спауним башенку на позиции призрака
								if (tower != null)
								{
									GlobalVars.PlayerMoney -= tower.GetComponent<PlasmaTurretAI>().towerPrice; //отнимаем лаве за башню
									GlobalVars.TurretCount++;
								}

								Destroy(ghost); //уничтожаем призрак башни
								GlobalVars.mau5tate = GlobalVars.ClickState.Default; //меняем глобальное состояние мыши на обычное
							}
						}
					}
					break;
				}
			case GlobalVars.ClickState.Upgrading:
				{
					//Ray scrRay = Camera.main.ScreenPointToRay(Input.mousePosition); //создаём луч, бьющий от координат мыши по координатам в игре
					//if (Physics.Raycast(scrRay, out hit, Mathf.Infinity)) // бьём этим лучем в заданном выше направлении (т.е. в землю)
					//{
					//	Collider[] colls = Physics.OverlapSphere(hit.point, 10.0f);
					//	float closestMobSquaredDistance = 0;
					//	GameObject nearestObject = null;
					//	if (Input.GetMouseButtonDown(0) && GlobalVars.PlayerMoney >= 500.0f) //при нажатии ЛКМ и наличии более 500$ денег
					//	{
					//		foreach (Collider coll in colls)
					//		{
					//			float distance = (coll.transform.position - hit.point).sqrMagnitude;
					//			if (distance < 100f || closestMobSquaredDistance == 0.0f)
					//			{
					//				closestMobSquaredDistance = distance;
					//				nearestObject = coll.gameObject;
					//			}
					//		}
					//		if (nearestObject != null)
					//		{
					//			switch (nearestObject.tag)
					//			{
					//				case "Turret":
					//					{
					//						GlobalVars.mau5tate = GlobalVars.ClickState.Default;
					//						break;
					//					}
					//				case "Base":
					//					{
					//						nearestObject.GetComponent<BaseHP>().Upgrade();
					//						GlobalVars.mau5tate = GlobalVars.ClickState.Default;
					//						break;
					//					}
					//				default:
					//					GlobalVars.mau5tate = GlobalVars.ClickState.Default;
					//					break;
					//			}
					//		}
					//	}
					//}
					break;
				}
		}
	}

	private void OnGUI()
	{
		GUI.Box(buyMenu, "Buying menu"); //Делаем гуевский бокс на квадрате buyMenu с заголовком, указанным между ""
		if (GUI.Button(firstTower, "Plasma Tower\n" + (int)TowerPrices.Plasma + "$")) //если идёт нажатие на первую кнопку
		{
			if (GlobalVars.PlayerMoney >= (int)TowerPrices.Plasma) //если у игрока достаточно денег
				GlobalVars.mau5tate = GlobalVars.ClickState.Placing; //меняем глобальное состояние мыши на "Установка пушки"
		}
		if (GUI.Button(secondTower, "Pulse Tower\n" + (int)TowerPrices.Pulse + "$")) //с остальными аналогично
		{
			//action here
		}
		if (GUI.Button(thirdTower, "Beam Tower\n" + (int)TowerPrices.Beam + "$"))
		{
			//action here
		}
		if (GUI.Button(fourthTower, "Tesla Tower\n" + (int)TowerPrices.Tesla + "$"))
		{
			//action here
		}
		if (GUI.Button(fifthTower, "Artillery Tower\n" + (int)TowerPrices.Artillery + "$"))
		{
			//action here
		}

		GUI.Box(playerStats, "Player Stats");
		GUI.Label(playerStatsPlayerMoney, "Money: " + GlobalVars.PlayerMoney + "$");

		GUI.Box(towerMenu, "Tower menu");
		if (GUI.Button(towerMenuSellTower, "Sell"))
		{
			//action here
		}
		if (GUI.Button(towerMenuUpgradeTower, "Upgrade"))
		{
			//if (GlobalVars.mau5tate == GlobalVars.ClickState.Default)
			//{
			//	GlobalVars.mau5tate = GlobalVars.ClickState.Upgrading;
			//}
		}
	}

	//цены на пушки
	private enum TowerPrices
	{
		Plasma = 100,
		Pulse = 150,
		Beam = 250,
		Tesla = 300,
		Artillery = 350
	}
}