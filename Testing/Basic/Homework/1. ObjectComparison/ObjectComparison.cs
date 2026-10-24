using NUnit.Framework;
using NUnit.Framework.Legacy;
using FluentAssertions;

namespace HomeExercise.Tasks.ObjectComparison;
public class ObjectComparison
{
    [Test]
    [Description("Проверка текущего царя")]
    [Category("Refactored")]
    public void CheckCurrentTsar()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();

        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Перепишите код на использование Fluent Assertions.
        //стало
        actualTsar.Should().BeEquivalentTo(expectedTsar,
            opt => opt.Excluding(p => p.Id)         //тест не будет проходить, если сравнивать Id, тк он задается в конструкторе автоматически
                      .Excluding(p => p.Parent.Id)); //поэтому я исключил эти два поля из сравнения

        //было (не знаю надо ли удалять)
        /**
        ClassicAssert.AreEqual(actualTsar.Name, expectedTsar.Name);
        ClassicAssert.AreEqual(actualTsar.Age, expectedTsar.Age);
        ClassicAssert.AreEqual(actualTsar.Height, expectedTsar.Height);
        ClassicAssert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

        ClassicAssert.AreEqual(expectedTsar.Parent!.Name, actualTsar.Parent!.Name);
        ClassicAssert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
        ClassicAssert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
        ClassicAssert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
        **/
    }

    [Test]
    [Description("Альтернативное решение. Какие у него недостатки?")]
    public void CheckCurrentTsar_WithCustomEquality()
    {
        var actualTsar = TsarRegistry.GetCurrentTsar();
        var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
            new Person("Vasili III of Russia", 28, 170, 60, null));

        // Какие недостатки у такого подхода? 
        //Ответ: сложно читается,
        //при изменении модели нужно будет переписывать AreEquel,
        //чтобы он проверял новое свойство, или не проверял то, которое мы убрали из модели.
        //Также, если тест не пройдет, нужно открывать дебагер, чтобы узнать, из-за какого свойства упал тест

        ClassicAssert.True(AreEqual(actualTsar, expectedTsar));
    }

    private bool AreEqual(Person? actual, Person? expected)
    {
        if (actual == expected) return true;
        if (actual == null || expected == null) return false;
        return
            actual.Name == expected.Name
            && actual.Age == expected.Age
            && actual.Height == expected.Height
            && actual.Weight == expected.Weight
            && AreEqual(actual.Parent, expected.Parent);
    }
}
