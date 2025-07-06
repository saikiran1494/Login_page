// FitnessTracker - Main JavaScript File

$(document).ready(function() {
    // Initialize all components
    initializeAnimations();
    initializeFormValidation();
    initializeTooltips();
    initializeCharts();
    initializeNotifications();
    initializeTheme();
});

// Animation Functions
function initializeAnimations() {
    // Fade in elements on scroll
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-fade-in');
            }
        });
    }, observerOptions);

    // Observe all cards and sections
    document.querySelectorAll('.card, .stat-card, .chart-container').forEach(el => {
        observer.observe(el);
    });

    // Smooth scroll for anchor links
    $('a[href^="#"]').on('click', function(e) {
        e.preventDefault();
        const target = $(this.getAttribute('href'));
        if (target.length) {
            $('html, body').animate({
                scrollTop: target.offset().top - 80
            }, 800, 'easeInOutQuart');
        }
    });
}

// Form Validation
function initializeFormValidation() {
    // Custom validation for all forms
    $('form').each(function() {
        $(this).on('submit', function(e) {
            const form = this;
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Real-time validation
    $('.form-control').on('input', function() {
        validateField($(this));
    });

    // Password strength indicator
    $('#password').on('input', function() {
        const password = $(this).val();
        const strength = calculatePasswordStrength(password);
        updatePasswordStrengthIndicator(strength);
    });
}

// Field Validation
function validateField(field) {
    const value = field.val();
    const type = field.attr('type');
    const name = field.attr('name');
    let isValid = true;
    let message = '';

    // Email validation
    if (type === 'email') {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        isValid = emailRegex.test(value);
        message = isValid ? '' : 'Please enter a valid email address.';
    }

    // Required field validation
    if (field.prop('required') && !value.trim()) {
        isValid = false;
        message = 'This field is required.';
    }

    // Update field state
    if (isValid) {
        field.removeClass('is-invalid').addClass('is-valid');
        field.siblings('.invalid-feedback').text('');
    } else {
        field.removeClass('is-valid').addClass('is-invalid');
        field.siblings('.invalid-feedback').text(message);
    }
}

// Password Strength Calculator
function calculatePasswordStrength(password) {
    let strength = 0;
    
    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;
    
    return strength;
}

// Update Password Strength Indicator
function updatePasswordStrengthIndicator(strength) {
    const strengthBar = $('.password-strength-bar');
    const strengthText = $('.password-strength-text');
    
    if (strengthBar.length === 0) {
        const strengthContainer = $('<div class="mt-2"><div class="progress password-strength-bar" style="height: 5px;"><div class="progress-bar"></div></div><small class="password-strength-text text-muted"></small></div>');
        $('#password').parent().append(strengthContainer);
    }
    
    const progressBar = $('.password-strength-bar .progress-bar');
    const textElement = $('.password-strength-text');
    
    let color, text;
    
    if (strength <= 2) {
        color = 'bg-danger';
        text = 'Weak';
    } else if (strength <= 3) {
        color = 'bg-warning';
        text = 'Fair';
    } else if (strength <= 4) {
        color = 'bg-info';
        text = 'Good';
    } else {
        color = 'bg-success';
        text = 'Strong';
    }
    
    progressBar.removeClass('bg-danger bg-warning bg-info bg-success').addClass(color);
    progressBar.css('width', (strength * 20) + '%');
    textElement.text(text);
}

// Tooltip Initialization
function initializeTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function(tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Chart Initialization
function initializeCharts() {
    // Initialize Chart.js if available
    if (typeof Chart !== 'undefined') {
        // Sample chart data
        const ctx = document.getElementById('workoutChart');
        if (ctx) {
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
                    datasets: [{
                        label: 'Workouts',
                        data: [12, 19, 3, 5, 2, 3, 7],
                        borderColor: 'rgb(75, 192, 192)',
                        tension: 0.1
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'top',
                        },
                        title: {
                            display: true,
                            text: 'Weekly Workout Progress'
                        }
                    }
                }
            });
        }
    }
}

// Notification System
function initializeNotifications() {
    // Auto-dismiss alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut(500);
    }, 5000);

    // Custom notification function
    window.showNotification = function(message, type = 'info') {
        const alertClass = `alert-${type}`;
        const icon = getNotificationIcon(type);
        
        const notification = $(`
            <div class="alert ${alertClass} alert-dismissible fade show position-fixed" 
                 style="top: 20px; right: 20px; z-index: 9999; min-width: 300px;">
                <i class="${icon} me-2"></i>${message}
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `);
        
        $('body').append(notification);
        
        // Auto-dismiss after 4 seconds
        setTimeout(function() {
            notification.fadeOut(500, function() {
                $(this).remove();
            });
        }, 4000);
    };
}

// Get Notification Icon
function getNotificationIcon(type) {
    const icons = {
        success: 'fas fa-check-circle',
        error: 'fas fa-exclamation-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-info-circle'
    };
    return icons[type] || icons.info;
}

// Theme Management
function initializeTheme() {
    // Check for saved theme preference
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        document.body.setAttribute('data-theme', savedTheme);
    }

    // Theme toggle functionality
    $('.theme-toggle').on('click', function() {
        const currentTheme = document.body.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        document.body.setAttribute('data-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        
        // Update toggle button
        $(this).find('i').toggleClass('fa-sun fa-moon');
    });
}

// AJAX Helper Functions
function makeAjaxRequest(url, method = 'GET', data = null) {
    return $.ajax({
        url: url,
        method: method,
        data: data,
        headers: {
            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
        }
    });
}

// Loading State Management
function setLoadingState(element, isLoading) {
    if (isLoading) {
        element.addClass('btn-loading').prop('disabled', true);
    } else {
        element.removeClass('btn-loading').prop('disabled', false);
    }
}

// Data Table Functions
function initializeDataTable(tableId, options = {}) {
    if ($.fn.DataTable) {
        const defaultOptions = {
            responsive: true,
            language: {
                search: "Search:",
                lengthMenu: "Show _MENU_ entries",
                info: "Showing _START_ to _END_ of _TOTAL_ entries",
                infoEmpty: "Showing 0 to 0 of 0 entries",
                infoFiltered: "(filtered from _MAX_ total entries)",
                paginate: {
                    first: "First",
                    previous: "Previous",
                    next: "Next",
                    last: "Last"
                }
            }
        };
        
        return $(tableId).DataTable({...defaultOptions, ...options});
    }
}

// Utility Functions
function formatDate(date) {
    return new Date(date).toLocaleDateString('en-US', {
        year: 'numeric',
        month: 'short',
        day: 'numeric'
    });
}

function formatTime(minutes) {
    const hours = Math.floor(minutes / 60);
    const mins = minutes % 60;
    return hours > 0 ? `${hours}h ${mins}m` : `${mins}m`;
}

function formatNumber(num) {
    return num.toLocaleString();
}

// Smooth Scrolling
$.easing.easeInOutQuart = function(x, t, b, c, d) {
    if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
    return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
};

// Export functions for global use
window.FitnessTracker = {
    showNotification,
    makeAjaxRequest,
    setLoadingState,
    formatDate,
    formatTime,
    formatNumber
}; 