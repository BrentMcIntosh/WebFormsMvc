<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Web.Models.DateRange>" %>
<%@ Import Namespace="Web.Framework" %>
<%@ Import Namespace="Web.Models" %>

<script>

    $(function() {

        function changeDataRange(range, rangeValue) {

            let url = '<% = Url.Action("Index", "VendorReports", new { id = Model.Id, range = "__range__", rangeValue = "__rangeValue__" }) %>';

            window.location.href = url.replace('__range__', range).replace('__rangeValue__', rangeValue);
        }

        function rangeValueChange() {

            var rangeValue = $(this).val();

            if (rangeValue.indexOf("Please") === -1) {

                let range = $('#rangePicker').val();

                changeDataRange(range, rangeValue);
            }
        }

        $('#rangePicker').change(function () {
            
            var range = $(this).val();
            
            if (range === 'YearToDate' || range === 'PreviousTwelveMonths') {

                changeDataRange(range, '');
            }
            else {

                $('#monthPicker').hide();
                $('#quarterPicker').hide();
                $('#yearPicker').hide();

                if (range === 'Months') $('#monthPicker').show();
                else if (range == 'Quarters') $('#quarterPicker').show();
                else if (range === 'CalendarYears') $('#yearPicker').show();
            }
        });

        $('#monthPicker').change(rangeValueChange);
        $('#quarterPicker').change(rangeValueChange);
        $('#yearPicker').change(rangeValueChange);
    });

</script>

<p>
    <% = Model.Information %>
</p>

<% = Html.EnumDropDownList("rangePicker", Model.ChosenRange) %>

<% = Html.EnumDropDownList("quarterPicker", Model.ChosenQuarter, new { @style = Model.GetStyle(RangeChoices.Quarters) }) %>

<% = Html.DropDownList("yearPicker", Model.Years, new { @style = Model.GetStyle(RangeChoices.CalendarYears) }) %>

<% = Html.DropDownList("monthPicker", Model.Months, new { @style = Model.GetStyle(RangeChoices.Months) }) %>

