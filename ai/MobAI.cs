using UnityEngine;

public class MobAI : MonoBehaviour
{
	public GameObject Target; //текущая цель

	public float mobPrice = 5.0f; //цена за убийство моба
	public float mobMinSpeed = 0.5f; //минимальная скорость моба
	public float mobMaxSpeed = 2.0f; //максимальная скорость моба
	public float mobRotationSpeed = 2.5f; //скорость поворота моба
	public float attackDistance = 5.0f; //дистанция атаки
	public float damage = 5; //урон, наносимый мобом
	public float attackTimer = 0.0f; //переменная расчета задержки между ударами
	public const float coolDown = 2.0f; //константа, используется для сброса таймера атаки в начальное значение

	public float MobCurrentSpeed; //скорость моба, инициализируем позже
	private Transform mob; //переменная для трансформа моба

	private void Awake()
	{
		mob = transform; //присваиваем трансформ моба в переменную (повышает производительность)
		MobCurrentSpeed = Random.Range(mobMinSpeed, mobMaxSpeed); //посредством рандома выбираем скорость между минимально и максимально указанной
	}

	private void Update()
	{
		if (Target != null) //если у нас есть цель
		{
			mob.rotation = Quaternion.Lerp(mob.rotation, Quaternion.LookRotation(new Vector3(Target.transform.position.x, 0.0f, Target.transform.position.z) - new Vector3(mob.position.x, 0.0f, mob.position.z)), mobRotationSpeed); //избушка-избушка, повернись к пушке передом!
			mob.position += mob.forward * MobCurrentSpeed * Time.deltaTime; //двигаем в сторону, куда смотрит моб
			float squaredDistance = (Target.transform.position - mob.position).sqrMagnitude; //меряем дистанцию до цели
			Vector3 structDirection = (Target.transform.position - mob.position).normalized; //получаем вектор направления
			float attackDirection = Vector3.Dot(structDirection, mob.forward); //получаем вектор атаки
			if (squaredDistance < attackDistance * attackDistance && attackDirection > 0) //если мы на дистанции атаки и цель перед нами
			{
				if (attackTimer > 0) attackTimer -= Time.deltaTime; //если таймер атаки больше 0 - отнимаем его
				if (attackTimer <= 0) //если же он стал меньше нуля или равен ему
				{
					BaseHP bhp = Target.GetComponent<BaseHP>(); //подключаемся к компоненту ХП цели
					if (bhp != null) bhp.ChangeHP(-damage); //если цель ещё живая, наносим урон (мы можем не одни бить по цели, потому проверка необходима)
					attackTimer = coolDown; //возвращаем таймер в исходное положение
					MobHP mhp = GetComponent<MobHP>();
					mhp.curHP = 0;
				}
			}
		}
		else
		{
			GameObject baza = GameObject.FindGameObjectWithTag("Base"); //находим наш объект с базой, он всего один
			if (baza != null) Target = baza;
		}
	}
}