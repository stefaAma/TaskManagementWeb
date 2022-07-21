// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function dateChanged() {
    location.href = location.hostname + "/DailyTasks?date=" + document.getElementById("datepicker").value
}

function viewDailyTasks(event) {
    event.preventDefault()
    let date = new Date()
    let dateString = date.getFullYear().toString() + "-" + (date.getMonth() + 1).toString() + "-" + date.getDate().toString()
}