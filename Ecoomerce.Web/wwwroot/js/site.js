// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Mark current navbar link as active based on URL path
document.addEventListener('DOMContentLoaded', function () {
  var path = window.location.pathname.toLowerCase();
  document.querySelectorAll('.navbar a.nav-link, .navbar .dropdown-menu a.dropdown-item').forEach(function (link) {
    try {
      var href = link.getAttribute('href');
      if (!href) return;
      var linkPath = href.split('?')[0].toLowerCase();
      if (linkPath === '/' && path === '/') {
        link.classList.add('active');
      } else if (linkPath !== '/' && path.startsWith(linkPath)) {
        link.classList.add('active');
      }
    } catch (e) { /* no-op */ }
  });
});