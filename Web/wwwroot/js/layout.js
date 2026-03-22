/**
 * RoomHub Layout JavaScript
 * Handles: MegaMenu sidebar, Location dropdown, Search form
 * Author: RoomHub Dev Team
 */

(function () {
    'use strict';

    // ============================================================
    // MEGA MENU SIDEBAR
    // ============================================================
    const megaMenuBtn  = document.getElementById('mega-menu-btn');
    const megaMenuSidebar = document.getElementById('mega-menu-sidebar');
    const megaMenuBackdrop = document.getElementById('mega-menu-backdrop');
    const megaMenuClose = document.getElementById('mega-menu-close');

    function openMegaMenu() {
        megaMenuSidebar.classList.remove('-translate-x-full');
        megaMenuBackdrop.classList.remove('opacity-0', 'pointer-events-none');
        megaMenuBackdrop.classList.add('opacity-100');
        document.body.style.overflow = 'hidden';
    }

    function closeMegaMenu() {
        megaMenuSidebar.classList.add('-translate-x-full');
        megaMenuBackdrop.classList.remove('opacity-100');
        megaMenuBackdrop.classList.add('opacity-0', 'pointer-events-none');
        document.body.style.overflow = '';
    }

    if (megaMenuBtn) megaMenuBtn.addEventListener('click', openMegaMenu);
    if (megaMenuClose) megaMenuClose.addEventListener('click', closeMegaMenu);
    if (megaMenuBackdrop) megaMenuBackdrop.addEventListener('click', closeMegaMenu);

    // ============================================================
    // LOCATION DROPDOWN (Toàn quốc)
    // ============================================================
    const locationBtn      = document.getElementById('location-btn');
    const locationDropdown = document.getElementById('location-dropdown');
    const locationLabel    = document.getElementById('location-label');
    const locationInput    = document.getElementById('location-hidden-input');
    const locationSearch   = document.getElementById('location-search-input');
    const locationOptions  = document.querySelectorAll('[data-province]');

    let locationOpen = false;

    function openLocationDropdown() {
        locationDropdown.classList.remove('opacity-0', 'invisible', 'scale-95', 'pointer-events-none');
        locationDropdown.classList.add('opacity-100', 'visible', 'scale-100');
        locationOpen = true;
        if (locationSearch) {
            locationSearch.value = '';
            filterProvinces('');
            setTimeout(() => locationSearch.focus(), 50);
        }
    }

    function closeLocationDropdown() {
        locationDropdown.classList.add('opacity-0', 'invisible', 'scale-95', 'pointer-events-none');
        locationDropdown.classList.remove('opacity-100', 'visible', 'scale-100');
        locationOpen = false;
    }

    function selectProvince(name, value) {
        if (locationLabel) locationLabel.textContent = name;
        if (locationInput) locationInput.value = value;
        closeLocationDropdown();
    }

    function filterProvinces(query) {
        const q = query.toLowerCase().trim();
        locationOptions.forEach(function (opt) {
            const text = opt.textContent.toLowerCase();
            opt.style.display = (!q || text.includes(q)) ? '' : 'none';
        });
    }

    if (locationBtn) {
        locationBtn.addEventListener('click', function (e) {
            e.stopPropagation();
            locationOpen ? closeLocationDropdown() : openLocationDropdown();
        });
    }

    if (locationSearch) {
        locationSearch.addEventListener('input', function () {
            filterProvinces(this.value);
        });
    }

    locationOptions.forEach(function (opt) {
        opt.addEventListener('click', function () {
            selectProvince(opt.textContent.trim(), opt.dataset.province);
        });
    });

    // Close dropdown when clicking outside
    document.addEventListener('click', function (e) {
        if (locationOpen && locationDropdown && !locationDropdown.contains(e.target) && e.target !== locationBtn) {
            closeLocationDropdown();
        }
    });

    // ============================================================
    // SEARCH FORM SUBMIT
    // ============================================================
    const searchForm  = document.getElementById('header-search-form');
    const searchInput = document.getElementById('header-search-input');

    if (searchForm) {
        searchForm.addEventListener('submit', function (e) {
            // Form will submit naturally via GET to RoomPosts/Index?q=...&province=...
            // Trim whitespace
            if (searchInput) searchInput.value = searchInput.value.trim();
        });
    }

    // ============================================================
    // MEGA MENU ACCORDION (mobile sub-categories)
    // ============================================================
    document.querySelectorAll('[data-accordion-trigger]').forEach(function (trigger) {
        trigger.addEventListener('click', function () {
            const target = document.getElementById(this.dataset.accordionTrigger);
            if (!target) return;
            const isOpen = !target.classList.contains('hidden');
            // close all
            document.querySelectorAll('[data-accordion-content]').forEach(el => el.classList.add('hidden'));
            document.querySelectorAll('[data-accordion-trigger]').forEach(btn => {
                const icon = btn.querySelector('[data-accordion-icon]');
                if (icon) icon.style.transform = '';
            });
            if (!isOpen) {
                target.classList.remove('hidden');
                const icon = this.querySelector('[data-accordion-icon]');
                if (icon) icon.style.transform = 'rotate(180deg)';
            }
        });
    });

})();
