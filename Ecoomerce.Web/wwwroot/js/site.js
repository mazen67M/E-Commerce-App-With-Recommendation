// Enhanced E-Commerce Site JavaScript
// Modern UI/UX interactions and animations

document.addEventListener('DOMContentLoaded', function () {
  // Initialize all features
  initDarkMode();
  initNavbar();
  initScrollAnimations();
  initScrollToTop();
  initToastContainer();
  initLazyLoading();
  initProductCardAnimations();
});

// ===== DARK MODE TOGGLE =====
function initDarkMode() {
  // Check for saved theme preference or default to light
  const savedTheme = localStorage.getItem('theme') || 'light';
  document.documentElement.setAttribute('data-theme', savedTheme);

  // Create theme toggle button if it doesn't exist
  let themeToggle = document.querySelector('.theme-toggle');

  if (!themeToggle) {
    themeToggle = document.createElement('button');
    themeToggle.className = 'theme-toggle';
    themeToggle.setAttribute('aria-label', 'Toggle dark mode');
    themeToggle.setAttribute('title', 'Toggle dark/light mode');
    updateThemeIcon(themeToggle, savedTheme);
    document.body.appendChild(themeToggle);
  }

  // Toggle theme on click
  themeToggle.addEventListener('click', function () {
    const currentTheme = document.documentElement.getAttribute('data-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';

    document.documentElement.setAttribute('data-theme', newTheme);
    localStorage.setItem('theme', newTheme);
    updateThemeIcon(themeToggle, newTheme);

    // Show notification
    showToast(
      newTheme === 'dark' ? 'Dark mode enabled' : 'Light mode enabled',
      'info',
      2000
    );
  });
}

function updateThemeIcon(button, theme) {
  if (theme === 'dark') {
    button.innerHTML = '<i class="bi bi-sun-fill"></i>';
  } else {
    button.innerHTML = '<i class="bi bi-moon-fill"></i>';
  }
}

// ===== NAVBAR ENHANCEMENTS =====
function initNavbar() {
// Mark current navbar link as active
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

  // Navbar scroll effect
  let lastScroll = 0;
  const navbar = document.querySelector('.navbar');

  window.addEventListener('scroll', function () {
    const currentScroll = window.pageYOffset;

    if (currentScroll > 100) {
      navbar.classList.add('shadow-lg');
    } else {
      navbar.classList.remove('shadow-lg');
    }

    lastScroll = currentScroll;
  });
}

// ===== SCROLL ANIMATIONS =====
function initScrollAnimations() {
  const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
  };

  const observer = new IntersectionObserver(function (entries) {
    entries.forEach(entry => {
      if (entry.isIntersecting) {
        entry.target.classList.add('revealed');
        observer.unobserve(entry.target);
      }
    });
  }, observerOptions);

  // Observe all elements with scroll-reveal class
  document.querySelectorAll('.scroll-reveal').forEach(el => {
    observer.observe(el);
  });
}

// ===== SCROLL TO TOP BUTTON =====
function initScrollToTop() {
  // Create scroll to top button if it doesn't exist
  let scrollBtn = document.querySelector('.scroll-to-top');

  if (!scrollBtn) {
    scrollBtn = document.createElement('button');
    scrollBtn.className = 'fab scroll-to-top';
    scrollBtn.innerHTML = '<i class="bi bi-arrow-up"></i>';
    scrollBtn.setAttribute('aria-label', 'Scroll to top');
    document.body.appendChild(scrollBtn);
  }

  // Show/hide button based on scroll position
  window.addEventListener('scroll', function () {
    if (window.pageYOffset > 300) {
      scrollBtn.classList.add('show');
    } else {
      scrollBtn.classList.remove('show');
    }
  });

  // Scroll to top on click
  scrollBtn.addEventListener('click', function () {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  });
}

// ===== TOAST NOTIFICATION SYSTEM =====
function initToastContainer() {
  if (!document.querySelector('.toast-container')) {
    const container = document.createElement('div');
    container.className = 'toast-container';
    document.body.appendChild(container);
  }
}

function showToast(message, type = 'info', duration = 3000) {
  const container = document.querySelector('.toast-container');

  const toast = document.createElement('div');
  toast.className = `alert alert-${type} alert-dismissible fade show toast-custom`;
  toast.setAttribute('role', 'alert');
  toast.innerHTML = `
    ${message}
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
  `;

  container.appendChild(toast);

  // Auto-dismiss
  setTimeout(() => {
    toast.classList.remove('show');
    setTimeout(() => toast.remove(), 300);
  }, duration);
}

// Global function for showing notifications
window.showNotification = showToast;

// ===== LAZY LOADING IMAGES =====
function initLazyLoading() {
  if ('IntersectionObserver' in window) {
    const imageObserver = new IntersectionObserver((entries, observer) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const img = entry.target;
          img.src = img.dataset.src;
          img.classList.remove('lazy');
          imageObserver.unobserve(img);
        }
      });
    });

    document.querySelectorAll('img.lazy').forEach(img => {
      imageObserver.observe(img);
    });
  }
}

// ===== PRODUCT CARD ANIMATIONS =====
function initProductCardAnimations() {
  const cards = document.querySelectorAll('.product-card');

  cards.forEach(card => {
    card.addEventListener('mouseenter', function () {
      this.style.transform = 'translateY(-10px)';
    });

    card.addEventListener('mouseleave', function () {
      this.style.transform = 'translateY(0)';
    });
  });
}

// ===== CART FUNCTIONS =====
function updateCartCount() {
  fetch('/Cart/GetCartCount')
    .then(response => response.json())
    .then(count => {
      const cartBadge = document.getElementById('cartCount');
      if (cartBadge) {
        cartBadge.textContent = count;
        if (count > 0) {
          cartBadge.classList.remove('d-none');
        } else {
          cartBadge.classList.add('d-none');
        }
      }
    })
    .catch(error => console.error('Error updating cart count:', error));
}

// ===== WISHLIST FUNCTIONS =====
function updateWishlistCount() {
  fetch('/Profile/Wishlist/GetWishlistCount')
    .then(response => response.json())
    .then(count => {
      const wishlistBadge = document.getElementById('wishlistCount');
      if (wishlistBadge) {
        wishlistBadge.textContent = count;
        if (count > 0) {
          wishlistBadge.classList.remove('d-none');
        } else {
          wishlistBadge.classList.add('d-none');
        }
      }
    })
    .catch(error => console.error('Error updating wishlist count:', error));
}

// ===== FORM VALIDATION ENHANCEMENTS =====
function enhanceFormValidation() {
  const forms = document.querySelectorAll('.needs-validation');

  forms.forEach(form => {
    form.addEventListener('submit', function (event) {
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }
      form.classList.add('was-validated');
    }, false);
  });
}

// ===== SEARCH FUNCTIONALITY =====
function initSearchEnhancements() {
  const searchInput = document.querySelector('input[name="q"]');

  if (searchInput) {
    let searchTimeout;

    searchInput.addEventListener('input', function () {
      clearTimeout(searchTimeout);

      searchTimeout = setTimeout(() => {
        // Add search suggestions logic here
        console.log('Search for:', this.value);
      }, 300);
    });
  }
}

// ===== PRICE FORMATTING =====
function formatPrice(price) {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD'
  }).format(price);
}

// ===== IMAGE PREVIEW =====
function previewImage(input, previewElement) {
  if (input.files && input.files[0]) {
    const reader = new FileReader();

    reader.onload = function (e) {
      previewElement.src = e.target.result;
      previewElement.style.display = 'block';
    };

    reader.readAsDataURL(input.files[0]);
  }
}

// ===== COPY TO CLIPBOARD =====
function copyToClipboard(text) {
  navigator.clipboard.writeText(text).then(() => {
    showToast('Copied to clipboard!', 'success', 2000);
  }).catch(err => {
    console.error('Failed to copy:', err);
    showToast('Failed to copy', 'danger', 2000);
  });
}

// ===== LOADING OVERLAY =====
function showLoadingOverlay() {
  let overlay = document.querySelector('.spinner-overlay');

  if (!overlay) {
    overlay = document.createElement('div');
    overlay.className = 'spinner-overlay';
    overlay.innerHTML = `
      <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
        <span class="visually-hidden">Loading...</span>
      </div>
    `;
    document.body.appendChild(overlay);
  }

  overlay.style.display = 'flex';
}

function hideLoadingOverlay() {
  const overlay = document.querySelector('.spinner-overlay');
  if (overlay) {
    overlay.style.display = 'none';
  }
}

// ===== DEBOUNCE UTILITY =====
function debounce(func, wait) {
  let timeout;
  return function executedFunction(...args) {
    const later = () => {
      clearTimeout(timeout);
      func(...args);
    };
    clearTimeout(timeout);
    timeout = setTimeout(later, wait);
  };
}

// ===== INITIALIZE ON LOAD =====
window.addEventListener('load', function () {
  updateCartCount();
  updateWishlistCount();
  enhanceFormValidation();
  initSearchEnhancements();
});

// Export functions for global use
window.showLoadingOverlay = showLoadingOverlay;
window.hideLoadingOverlay = hideLoadingOverlay;
window.copyToClipboard = copyToClipboard;
window.formatPrice = formatPrice;
window.previewImage = previewImage;
window.updateCartCount = updateCartCount;
window.updateWishlistCount = updateWishlistCount;