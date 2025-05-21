// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    document.addEventListener("DOMContentLoaded", function () {
        const toggle = document.querySelector(".user-toggle");
    const dropdown = document.querySelector(".dropdown-menu");

        toggle.addEventListener("click", () => {
        dropdown.style.display = dropdown.style.display === "block" ? "none" : "block";
        });

    // إغلاق القائمة إذا ضغطت خارجها
    document.addEventListener("click", function (event) {
            if (!toggle.contains(event.target) && !dropdown.contains(event.target)) {
        dropdown.style.display = "none";
            }
        });
    });

