// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function dateChanged() {
    // IMPORTANT: domain name is being inserted automatically? deployed on server will run as expected?
    location.href = "/DailyTasks?date=" + document.getElementById("datepicker").value
}

function viewDailyTasks(event) {
    event.preventDefault()
    let date = new Date()
    let monthString = ""
    if (date.getMonth() + 1 < 10)
        monthString = "0" + (date.getMonth() + 1).toString()
    else
        monthString = (date.getMonth() + 1).toString()
    let dateString = date.getFullYear().toString() + "-" + monthString + "-" + date.getDate().toString()
    location.href = "/DailyTasks?date=" + dateString
}

function backToTasksList() {
    let date = document.getElementById("back-button").getAttribute("data-date")
    location.href = "/DailyTasks?date=" + date
}

function goToCreatePage() {
    let date = document.getElementById("back-button").getAttribute("data-date")
    location.href = "/DailyTasks/Create?date=" + date
}
