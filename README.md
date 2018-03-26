# Ci.Sort
A simple solution for sort table use c#

## Concept
This is library main idea is to help aspnet mvc table with sort functionality
But still can use Ci.Sort only

## Usage
```csharp
var query = db.Bluetooths.AsQueryable();

if (!keyword.IsNullOrWhiteSpace())
{
    query = query.Where(x => x.Name.Contains(keyword) || x.Mac.Contains(keyword));
}

if (sort == null || sort.Key.IsNullOrWhiteSpace())
{
    sort = new SortOrder();
    sort.Key = nameof(Bluetooth.Mac);
    sort.Order = Order.Ascending;
}
query = query.Sort(sort);
```

cshtml
```html
@{
SortOrder sort = Viewbag.sort
}

<td>
    @Html.SortLabel("Mac", nameof(Bluetooth.Mac), new { page = Request["page"], keyword = Request["keyword"] }, sort)
</td>
<td>
    @Html.SortLabel("Name", nameof(Bluetooth.Name), new { page = Request["page"], keyword = Request["keyword"] }, sort)
</td>
```
## Detailed using for sorting with included types 
You can find extended example at [Ci.Test/Program.cs](https://github.com/lettucebo/Ci.Sort/blob/master/Ci.Test/Program.cs) file
