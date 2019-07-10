/*! file: master.js | author: Etch | website: ? */
// Menu
$(document).ready(function () {
    $("li a").click(function () {
        $("li a").removeClass("active");
        $(this).addClass("active");
    });
});

$("#menu-icon").on('click', function () {
    $('#sidebar-menu').toggleClass("sidebar-open");
    $('#sidebar-push').toggleClass("open");
    $('main').toggleClass("open");
});

// Theme picker
$("#theme-picker").on('click', function () {
    $('#theme-picker').toggleClass("theme-picker-open");
});