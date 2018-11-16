// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function classToggle() {
    const navs = document.querySelectorAll('.Navbar__Items')

    navs.forEach(nav => nav.classList.toggle('Navbar__ToggleShow'));
}
document.querySelector('.Navbar__Link-toggle')
    .addEventListener('click', classToggle);