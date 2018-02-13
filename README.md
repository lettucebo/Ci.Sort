# Ci.Sort
A simple solution for sort table use c#

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
    sort.Key = nameof(Library.Bluetooth.Models.MmsFactoryBluetooth.Bluetooth.Mac);
    sort.Order = Order.Up;
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
