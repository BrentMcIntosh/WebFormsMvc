<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<WebFormsMvc.Models.DateRange>" %>

<!DOCTYPE html>

<html>
<head>
<title>Date Range</title>
</head>
<body>
    <p>
        You will see information for things that occurred on or after <% = Model.Start.ToLongDateString() %> and before <% = Model.Finish.ToLongDateString() %>
    </p>

    <select>
        <option value='YearToDate' <% = Model.SelectedRange("YearToDate") %>>Year to Date</option>
        <option value='PreviousTwelveMonths' <% = Model.SelectedRange("PreviousTwelveMonths") %>>Previous 12 months</option>
        <option value='Months' <% = Model.SelectedRange("Months") %>>Months</option>
        <option value='Quarters' <% = Model.SelectedRange("Quarters") %>>Quarters</option>
        <option value='CalendarYears' <% = Model.SelectedRange("CalendarYears") %>>Calendar Years</option>
    </select>

    <select id='monthPicker' <% = Model.Visibility("Months") %>>
        <option>Please Select a Month</option>
        <option <% = Model.SelectedValue("January") %>>January</option>
        <option <% = Model.SelectedValue("February") %>>February</option>
        <option <% = Model.SelectedValue("March") %>>March</option>
        <option <% = Model.SelectedValue("April") %>>April</option>
        <option <% = Model.SelectedValue("May") %>>May</option>
        <option <% = Model.SelectedValue("June") %>>June</option>
        <option <% = Model.SelectedValue("July") %>>July</option>
        <option <% = Model.SelectedValue("August") %>>August</option>
        <option <% = Model.SelectedValue("September") %>>September</option>
        <option <% = Model.SelectedValue("October") %>>October</option>
        <option <% = Model.SelectedValue("November") %>>November</option>
        <option <% = Model.SelectedValue("December") %>>December</option>
    </select>

    <select id='quarterPicker' <% = Model.Visibility("Quarters") %>>
        <option>Please Select a Quarter</option>
        <option <% = Model.SelectedValue("First") %>>First</option>
        <option <% = Model.SelectedValue("Second") %>>Second</option>
        <option <% = Model.SelectedValue("Third") %>>Third</option>
        <option <% = Model.SelectedValue("Fourth") %>>Fourth</option>
    </select>

    <select id='yearPicker'  <% = Model.Visibility("CalendarYears") %>>
        <option>Please Select a Year</option>
        <option <% = Model.SelectedValue("2019") %>>2019</option>
        <option <% = Model.SelectedValue("2018") %>>2018</option>
        <option <% = Model.SelectedValue("2017") %>>2017</option>
        <option <% = Model.SelectedValue("2016") %>>2016</option>
    </select>

</body>
</html>
