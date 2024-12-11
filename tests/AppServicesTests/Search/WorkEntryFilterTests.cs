using MyApp.AppServices.WorkEntries;
using MyApp.AppServices.WorkEntries.QueryDto;
using MyApp.Domain.Entities.WorkEntries;
using MyApp.TestData;

namespace AppServicesTests.Search;

public class WorkEntryFilterTests
{
    [Test]
    public void DefaultSpec_ReturnsAllNonDeleted()
    {
        // Arrange
        var spec = new WorkEntrySearchDto();
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.Where(entry => !entry.IsDeleted);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ClosedStatusSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { Status = WorkEntryStatus.Closed };
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.Where(entry => entry is { IsDeleted: false, Closed: true });

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void DeletedSpec_ReturnsFilteredList()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.Deleted };
        var expression = WorkEntryFilters.SearchPredicate(spec);
        var expected = WorkEntryData.GetData.Where(entry => entry.IsDeleted);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void NeutralDeletedSpec_ReturnsAll()
    {
        // Arrange
        var spec = new WorkEntrySearchDto { DeletedStatus = SearchDeleteStatus.All };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(WorkEntryData.GetData);
    }

    [Test]
    public void ReceivedDateSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = WorkEntryData.GetData.First();

        var spec = new WorkEntrySearchDto
        {
            ReceivedFrom = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
            ReceivedTo = DateOnly.FromDateTime(referenceItem.ReceivedDate.Date),
        };

        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData
            .Where(entry =>
                entry is { IsDeleted: false } && entry.ReceivedDate.Date == referenceItem.ReceivedDate.Date);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void ReceivedBySpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = WorkEntryData.GetData.First(entry => entry.ReceivedBy != null);
        var spec = new WorkEntrySearchDto { ReceivedBy = referenceItem.ReceivedBy!.Id };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false } && entry.ReceivedBy == referenceItem.ReceivedBy);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void EntryTypeSpec_ReturnsFilteredList()
    {
        // Arrange
        var referenceItem = WorkEntryData.GetData.First(entry => entry.EntryType != null);
        var spec = new WorkEntrySearchDto { EntryType = referenceItem.EntryType!.Id };
        var expression = WorkEntryFilters.SearchPredicate(spec);

        var expected = WorkEntryData.GetData
            .Where(entry => entry is { IsDeleted: false, EntryType: not null } &&
                            entry.EntryType.Id == referenceItem.EntryType.Id);

        // Act
        var result = WorkEntryData.GetData.Where(expression.Compile());

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
