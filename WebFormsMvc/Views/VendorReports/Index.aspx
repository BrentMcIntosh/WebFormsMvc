<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Web.Models.DateRange>" %>

<script src="../../Scripts/jquery-3.4.1.js"></script>
   
<% Html.RenderPartial("DateRangePicker", Model); %>

