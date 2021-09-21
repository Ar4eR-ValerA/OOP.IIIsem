# is-oop-y24/Ar4eR-ValerA

[![wakatime](https://wakatime.com/badge/github/is-oop-y24/Ar4eR-ValerA.svg)](https://wakatime.com/badge/github/is-oop-y24/Ar4eR-ValerA)
# [Лабораторная 0. Isu](Isu)

**Цель:** ознакомиться с языком C#, базовыми механизмами ООП. В шаблонном репозитории описаны базовые сущности, требуется реализовать недостающие методы и написать тесты, которые бы проверили корректность работы.

**Предметная область. С**туденты, группы, переводы (хоть где-то), поиск. Группа имеет название (соответсвует шаблону M3XYY, где X - номер курса, а YY - номер группы). Студент может находиться только в одной группе. Система должна поддерживать механизм перевода между группами, добавления в группу и удаление из группы.

Требуется реализовать предоставленный в шаблоне интерфейс:

```csharp
public interface IIsuService
{
    Group AddGroup(GroupName name);
    Student AddStudent(Group group, string name);

    Student GetStudent(int id);
    Student FindStudent(string name);
    List<Student> FindStudents(GroupName groupName);
    List<Student> FindStudents(CourseNumber courseNumber);

    Group FindGroup(GroupName groupName);
    List<Group> FindGroups(CourseNumber courseNumber);

    void ChangeStudentGroup(Student student, Group newGroup);
}
```

И протестировать написанный код:

```csharp
[Test]
public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
{
}

[Test]
public void ReachMaxStudentPerGroup_ThrowException()
{
}

[Test]
public void CreateGroupWithInvalidName_ThrowException()
{
}

[Test]
public void TransferStudentToAnotherGroup_GroupChanged()
{
}
```

# [Лабораторная 1. Shops](Shops)

**Цель:** продемонстрировать умение выделять сущности и проектировать по ним классы.

**Прикладная область**: магазин, покупатель, доставка, пополнение и покупка товаров. Магазин имеет уникальный идентификатор, название (не обязательно уникальное) и адрес. В каждом магазине установлена своя цена на товар и есть в наличии некоторое количество единиц товара (какого-то товара может и не быть вовсе). Покупатель может производить покупку. Во время покупки - он передает нужную сумму денег магазину. Поставка товаров представляет собой набор товаров, их цен и количества, которые должны быть добавлены в магазин.

Тест кейсы:

1. Поставка товаров в магазин. Создаётся магазин, добавляются в систему товары, происходит поставка товаров в магазин. После добавления товары можно купить.
2. Установка и изменение цен на какой-то товар в магазине.
3. Поиск магазина, в котором партию товаров можно купить максимально дешево. Обработать ситуации, когда товара может быть недостаточно или товаров может небыть нигде.
4. Покупка партии товаров в магазине (набор пар товар + количество). Нужно убедиться, что товаров хватает, что у пользователя достаточно денег. После покупки должны передаваться деньги, а количество товаров измениться.

NB:

- Можно не поддерживать разные цены для одного магазина. Как вариант, можно брать старую цену, если магазин уже содержит этот товар. Иначе брать цену указанную в поставке.
- Пример ожидаемого формата тестов представлен ниже. **Используемые в тестах API магазина/менеджера/etc не являются интерфейсом для реализации в данной лабораторной. Не нужно ему следовать 1 в 1, это просто пример.**

```csharp
public void SomeTest(moneyBefore, productPrice, productCount, productToBuyCount)
{
	var person = new Person("name", moneyBefore);
	var shopManager = new ShopManager();
	var shop = shopManager.Create("shop name", ...);
	var product = shopManager.RegisterProduct("product name");
	
	shop.AddProducts( ... );
	shop.Buy(person, ...);
	
	Assert.AreEquals(moneyBefore - productPrice  * productToBuyCount, person.Money);
	Assert.AreEquals(productCount - productToBuyCount , shop.GetProductInfo(product).Count);
}
```