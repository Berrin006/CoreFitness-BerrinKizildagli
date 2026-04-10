document.addEventListener('DOMContentLoaded', function () {
    // --- 1. Mobilmeny Element ---
    const openBtn = document.querySelector('[data-mobile-nav-open]');
    const closeBtn = document.querySelector('[data-mobile-nav-close]');
    const flyout = document.querySelector('.mobile-flyout-menu');
    const body = document.body;
    const toggles = document.querySelectorAll('.mobile-menu-group-toggle');

    // --- 2. Desktop Dropdown Element ---
    const trainingToggle = document.querySelector('.main-menu .menu-item.dropdown > a');
    const trainingItem = document.querySelector('.main-menu .menu-item.dropdown');

    // --- Mobilmeny Logik ---
    if (flyout) {
        const openMenu = () => {
            flyout.classList.add('is-open');
            flyout.setAttribute('aria-hidden', 'false');
            body.style.overflow = 'hidden';
        };

        const closeMenu = () => {
            flyout.classList.remove('is-open');
            flyout.setAttribute('aria-hidden', 'true');
            body.style.overflow = '';

            // Stäng alla öppna submenyer i mobilvyn när vi stänger huvudmenyn
            toggles.forEach(t => {
                t.setAttribute('aria-expanded', 'false');
                if (t.nextElementSibling) t.nextElementSibling.style.maxHeight = null;
            });
        };

        if (openBtn) openBtn.addEventListener('click', openMenu);
        if (closeBtn) closeBtn.addEventListener('click', closeMenu);

        // Stäng om man klickar på overlayen (bakgrunden)
        flyout.addEventListener('click', (e) => {
            if (e.target === flyout) closeMenu();
        });

        // Hantera dragspel/toggles i mobilmenyn
        toggles.forEach(toggle => {
            toggle.addEventListener('click', function () {
                const submenu = this.nextElementSibling;
                const isExpanded = this.getAttribute('aria-expanded') === 'true';

                // Stäng andra öppna flikar (Optional: ta bort loopen om du vill kunna öppna flera samtidigt)
                toggles.forEach(other => {
                    if (other !== this) {
                        other.setAttribute('aria-expanded', 'false');
                        if (other.nextElementSibling) other.nextElementSibling.style.maxHeight = null;
                    }
                });

                // Växla nuvarande
                if (!isExpanded) {
                    this.setAttribute('aria-expanded', 'true');
                    submenu.style.maxHeight = submenu.scrollHeight + "px";
                } else {
                    this.setAttribute('aria-expanded', 'false');
                    submenu.style.maxHeight = null;
                }
            });
        });

        // Stäng mobilmenyn om fönstret blir stort
        window.addEventListener('resize', () => {
            if (window.innerWidth >= 1200 && flyout.classList.contains('is-open')) {
                closeMenu();
            }
        });
    }

    // --- Desktop Dropdown Logik ---
    if (trainingToggle && trainingItem) {
        trainingToggle.addEventListener('click', function (e) {
            if (window.innerWidth >= 1200) {
                e.preventDefault();
                e.stopPropagation();
                trainingItem.classList.toggle('active');
            }
        });

        // Stäng desktop-menyn vid klick utanför
        document.addEventListener('click', (e) => {
            if (!trainingItem.contains(e.target)) {
                trainingItem.classList.remove('active');
            }
        });
    }
});